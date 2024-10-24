using System.Collections.Generic;
using Zenseless.OpenTK;

namespace Example.Core;

public class TileSet(string imageName, Texture2D texture, float tileWidth, float tileHeight)
{
	public IReadOnlyList<Animation> Animations => animations;

	public string ImageName { get; } = imageName;
	public Texture2D Texture { get; } = texture;
	public float TileWidth { get; } = tileWidth;
	public float TileHeight { get; } = tileHeight;

	internal void AddAnimation(Animation animation)
	{
		animations.Add(animation);
	}

	private readonly List<Animation> animations = [];
}
