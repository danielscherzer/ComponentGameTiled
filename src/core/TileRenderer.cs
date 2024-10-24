using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Diagnostics;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace Example.Core;

internal partial class TileRenderer
{
	internal TileRenderer(IResourceDirectory resourceDirectory)
	{
		_resourceDirectory = resourceDirectory;
		GL.Enable(EnableCap.Texture2D);
		//TODO: 01. Enable blending
#if SOLUTION
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		GL.Enable(EnableCap.Blend);
#endif
	}

	internal void AddAnimation(TileSet tileSet, uint tileId, Animation animation)
	{
		_animations[tileId] = animation;
		tileSet.AddAnimation(animation);
	}

	internal Animation? GetAnimation(uint tileId)
	{
		if (_animations.TryGetValue(tileId, out var animation))
		{
			return animation;
		}
		else
		{
			return null;
		}
	}

	internal TileSet? GetTileSet(string name)
	{
		if (_tileSets.TryGetValue(name, out var tileSet))
		{
			return tileSet;
		}
		return null;
	}

	internal void AddTile(TileSet tileSet, uint tileId, float x, float y)
	{
		var data = new TileData(x,y, tileSet);
		_tiles.Add(tileId, data);
	}

	internal TileSet AddTileSet(string name, string imageName, uint columns, uint rows)
	{
		var texture = _resourceDirectory.LoadTexture(imageName);
		texture.Function = TextureFunction.ClampToEdge;
		texture.MagFilter = Zenseless.OpenTK.TextureMagFilter.Nearest;
		texture.MinFilter = Zenseless.OpenTK.TextureMinFilter.Nearest;
		var tileSet = new TileSet(imageName, texture, 1f / columns, 1f / rows);
		_tileSets[name] = tileSet;
		return tileSet;
	}

	internal void Draw(uint tileId, Box2 bounds)
	{
		var tile = _tiles[tileId];
		tile.TileSet.Texture.Bind();
		Draw(bounds, tile.TexCoords);
	}

	[DebuggerDisplay("{ToString()}")]
	private readonly struct TileData(float x, float y, TileSet tileSet)
	{
		public readonly Box2 TexCoords => Box2Extensions.CreateFromMinSize(x, y, TileSet.TileWidth, TileSet.TileHeight);

		public readonly TileSet TileSet => tileSet;

		public override readonly string ToString() => $"Position={x},{y} TileSet={TileSet.ImageName}";
	}

	private readonly Dictionary<uint, Animation> _animations = [];
	private readonly Dictionary<uint, TileData> _tiles = [];
	private readonly Dictionary<string, TileSet> _tileSets = [];
	private readonly IResourceDirectory _resourceDirectory;

	private static void Draw(Box2 bounds, Box2 texCoords)
	{
		GL.Begin(PrimitiveType.Quads);
		GL.TexCoord2(texCoords.Min); GL.Vertex2(bounds.Min);
		GL.TexCoord2(texCoords.Max.X, texCoords.Min.Y); GL.Vertex2(bounds.Max.X, bounds.Min.Y);
		GL.TexCoord2(texCoords.Max); GL.Vertex2(bounds.Max);
		GL.TexCoord2(texCoords.Min.X, texCoords.Max.Y); GL.Vertex2(bounds.Min.X, bounds.Max.Y);
		GL.End();
	}

}
