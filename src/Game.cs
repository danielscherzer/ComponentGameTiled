using Example.Core;
using Example.Model;
using Example.Visuals;
using System.Linq;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace Example;

internal static class Game
{
	public static Scene LoadSceneFromTiled(this IResourceDirectory resourceDirectory, string levelName, IInput input)
	{
		var scene = new Scene();
		// camera
		var cameraGameObject = scene.CreateGameObject(Box2Extensions.CreateFromMinSize(-1, -1, 2, 2), "camera");
		var camera = new Camera(input);
		cameraGameObject.Components.Add(camera);

		// load map
		using (var mapStream = resourceDirectory.Open(levelName))
		{
			var map = new Map(mapStream, resourceDirectory.Open);

			// create the tile renderer and load all tile sets and tile information from Tiled
			var tileRenderer = new TileRenderer(resourceDirectory);
			map.LoadGraphics(tileRenderer);

			// terrain
			var goTerrain = scene.CreateGameObject(Box2Extensions.CreateFromMinSize(-1, -1, 2 * map.Aspect, 2), "terrain");
			// extract collision information from Tiled and pass it to the terrain
			var terrain = new Terrain(goTerrain.Bounds, map.ExtractCollisionGrid());
			goTerrain.Components.Add(terrain);
			foreach (var layer in map.ExtractTileLayers())
			{
				goTerrain.Components.Add(new TileLayerVisual(goTerrain, layer, tileRenderer));
			}

			// create for each tiled object layer object a game object
			foreach (var (name, tileId, bounds) in map.GetObjects())
			{
				var gameObject = scene.CreateGameObject(bounds, name);
				if (0 != tileId)
				{
					// if the tiled object has a tile as visual representation create a tile visual to render it
					if (tileRenderer.GetAnimation(tileId) is Animation animation)
					{
						gameObject.Components.Add(new AnimatedTileVisual(gameObject, animation, tileRenderer));
					}
					else
					{
						gameObject.Components.Add(new TileVisual(gameObject, tileId, tileRenderer));
					}
				}
			}

			// create player logic
			var goPlayer = scene.GetGameObjects("player").FirstOrDefault();
			if (goPlayer is null) return scene;

			var player = new Player(goPlayer, input, terrain);
			var playerVisual = new PlayerVisual(player, tileRenderer);
			goPlayer.Components.Clear(); //HACK to remove the TileVisual because we want to replace it with a PlayerVisual
			goPlayer.Components.Add(new PlayerVisual(player, tileRenderer));
			goPlayer.Components.Add(player);

			//TODO: 06.1 Confirm: the player is always in the center of the screen.
			camera.FocusObject = goPlayer;

			// create torch logic
			foreach (var goTorch in scene.GetGameObjects("torch"))
			{
				var torch = new Torch(goTorch, goPlayer, scene);
				goTorch.Components.Add(torch);
			}

			// create bombs logic
			foreach (var goBomb in scene.GetGameObjects("bomb"))
			{
				var bomb = new Bomb(goBomb, goPlayer, scene);
				bomb.ExplosionStarts += () =>
				{
					if (tileRenderer.GetTileSet("BombExploding")?.Animations[0] is Animation animBombExploding)
					{
						var bombExploding = scene.CreateGameObject(goBomb.Bounds, "bombExploding");
						var expiringBehavior = new PeriodicBehavior(animBombExploding.Duration, () => { scene.Remove(bombExploding); return true; }, animBombExploding.Duration);
						bombExploding.Components.Add(expiringBehavior);
						bombExploding.Components.Add(new AnimatedTileVisual(bombExploding, animBombExploding, tileRenderer));
					}
				};
				goBomb.Components.Add(bomb);
			}
		}
		return scene;
	}
}