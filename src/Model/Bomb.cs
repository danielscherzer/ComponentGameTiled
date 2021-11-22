using Example.Core;
using System;
using System.Linq;
using Zenseless.OpenTK;

namespace Example.Model
{
	internal class Bomb : IOnUpdate
	{
		private readonly GameObject goBomb;
		private readonly Player player;
		private readonly Scene scene;
		public delegate void ExplosionHandler();
		public event ExplosionHandler? ExplosionStarts;

		public Bomb(GameObject goBomb, GameObject goPlayer, Scene scene)
		{
			this.goBomb = goBomb;
			player = goPlayer.Components.OfType<Player>().FirstOrDefault() ?? throw new ArgumentException($"GameObject {nameof(goPlayer)} has no component of type Player");
			this.scene = scene;
		}

		private void StartExplosion()
		{
			ExplosionStarts?.Invoke();
		}

		public void Update(float deltaTime)
		{
			//TODO: 10. Only when the player has a torch he can explode a bomb.
			//TODO: 10.1 Check if player intersects the bomb and call `Bomb.StartExplosion` and remove the bomb from the scene.
#if SOLUTION
			if (goBomb.Bounds.Overlaps(player.Bounds) && player.HasTorch)
			{
				StartExplosion();
				scene.Remove(goBomb);
			}
#endif
		}
	}
}