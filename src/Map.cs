using Example.Core;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiledLib.Layer;
using TiledLib.Objects;
using Zenseless.OpenTK;

namespace Example
{
	/// <summary>
	/// Class that handles loading of Tiled map and tile set files
	/// </summary>
	public class Map
	{
		private readonly TiledLib.Map map;

		public Map(Stream stream, Func<string, Stream> loadDependentNamedStream)
		{
			map = TiledLib.Map.FromStream(stream, ts => loadDependentNamedStream(ts.Source));
			Aspect = map.Width / (float)map.Height;
		}

		public float Aspect { get; }

		internal void LoadGraphics(TileRenderer tileRenderer)
		{
			// load all tiles
			foreach (var tileSet in map.Tilesets)
			{
				var rezImageWidth = 1f / tileSet.ImageWidth;
				var rezImageHeight = 1f / tileSet.ImageHeight;
				var graphicsTileSet = tileRenderer.AddTileSet(tileSet.Name, tileSet.ImagePath, (uint)tileSet.Columns, (uint)tileSet.Rows);
				for (int gid = tileSet.FirstGid; gid < tileSet.FirstGid + tileSet.TileCount; ++gid)
				{
					var tile = tileSet[gid];
					tileRenderer.AddTile(graphicsTileSet, (uint)gid, tile.Left * rezImageWidth,
						(tileSet.ImageHeight - tileSet.TileHeight - tile.Top) * rezImageHeight);
				}
				foreach (var animation in tileSet.TileAnimations)
				{
					var tileId = animation.Key + tileSet.FirstGid;
					var duration_ms = animation.Value.Sum(inputFrame => inputFrame.Duration_ms);
					var duration = duration_ms * 0.001f;
					var shortestDuration = (float)animation.Value.Min(inputFrame => inputFrame.Duration_ms);
					IEnumerable<uint> CalcDublicates(TiledLib.Frame inputFrame)
					{
						//the shortest input frame will produce one output frame, all others produce multiple output frames
						return Enumerable.Repeat((uint)(inputFrame.TileId + tileSet.FirstGid), (int)MathF.Round(inputFrame.Duration_ms / shortestDuration));
					}
					var outputFrames = animation.Value.Select(inputFrame => CalcDublicates(inputFrame)).SelectMany(frameGroups => frameGroups);
					tileRenderer.AddAnimation(graphicsTileSet, (uint)tileId, new Animation(duration, outputFrames));
				}
			}
		}

		/// <summary>
		/// Returns the names and bounds of all object layer objects
		/// </summary>
		/// <returns></returns>
		public IEnumerable<(string name, uint gid, Box2 bounds)> GetObjects()
		{
			foreach (var obj in GetBaseObjects())
			{
				var bounds = ConvertBounds(obj);
				var gid = 0u;
				if (obj is TileObject tileObject)
				{
					gid = (uint)tileObject.Gid;
				}
				yield return (obj.Name, gid, bounds);
			}
		}

		private Box2 ConvertBounds(BaseObject obj)
		{
			int mapPixelWidth = map.Width * map.CellWidth;
			int mapPixelHeight = map.Height * map.CellHeight;
			var x = (float)obj.X / mapPixelWidth * Aspect;
			var y = 1f - (float)obj.Y / mapPixelHeight;
			var width = (float)obj.Width / mapPixelWidth * Aspect;
			var height = (float)obj.Height / mapPixelHeight;
			var bounds = Box2Extensions.CreateFromMinSize(2f * new Vector2(x, y) - Vector2.One, 2f * new Vector2(width, height));
			return bounds;
		}

		/// <summary>
		/// Returns a boolean grid that is true for each cell that is notWalkable (in any layer) and false for all other cells.
		/// </summary>
		/// <returns></returns>
		public bool[,] ExtractCollisionGrid()
		{
			var notWalkableGids = new HashSet<int>();
			// look through all tile sets and save GID of tiles that have a property "notWalkable" in notWalkableGids
			foreach (var tileSet in map.Tilesets)
			{
				foreach (var tileProperties in tileSet.TileProperties)
				{
					var gid = tileProperties.Key + tileSet.FirstGid;
					if (tileProperties.Value.ContainsKey("notWalkable")) notWalkableGids.Add(gid);
				}
			}
			var grid = new bool[map.Width, map.Height]; // grid defaults to false
														//search through all layers if any contains a notWalkable GID
			foreach (var layer in map.Layers.OfType<TileLayer>())
			{
				for (int y = 0, i = 0; y < layer.Height; y++)
				{
					for (int x = 0; x < layer.Width; x++, i++)
					{
						var gid = layer.Data[i];
						if (notWalkableGids.Contains(gid))
						{
							grid[x, map.Height - y - 1] = true;
						}
					}
				}
			}
			return grid;
		}

		/// <summary>
		/// Returns a list of tile layers. Each layer consists of an 2 dimensional array of tile ids.
		/// </summary>
		/// <returns></returns>
		public List<uint[,]> ExtractTileLayers()
		{
			var layers = new List<uint[,]>();
			foreach (var layer in map.Layers.OfType<TileLayer>())
			{
				uint[,] grid = Extract(layer);
				layers.Add(grid);
			}
			return layers;
		}

		private static uint[,] Extract(TileLayer layer)
		{
			var grid = new uint[layer.Width, layer.Height];
			for (int y = 0, i = 0; y < layer.Height; y++)
			{
				for (int x = 0; x < layer.Width; x++, i++)
				{
					var gid = (uint)layer.Data[i];
					grid[x, layer.Height - y - 1] = gid;
				}
			}
			return grid;
		}

		public BaseLayer[] Layers => map.Layers;

		private IEnumerable<BaseObject> GetBaseObjects()
		{
			var objectLayers = map.Layers.OfType<ObjectLayer>();
			return objectLayers.SelectMany(l => l.Objects);
		}
	}
}
