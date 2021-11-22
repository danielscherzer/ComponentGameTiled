using System.Collections.Generic;
using Zenseless.OpenTK;

namespace Example.Core
{
	public class TileSet
	{
		public TileSet(string imageName, Texture2D texture, float tileWidth, float tileHeight)
		{
			ImageName = imageName;
			Texture = texture;
			TileWidth = tileWidth;
			TileHeight = tileHeight;
		}

		public IReadOnlyList<Animation> Animations => animations;

		public string ImageName { get; }
		public Texture2D Texture { get; }
		public float TileWidth { get; }
		public float TileHeight { get; }

		internal void AddAnimation(Animation animation)
		{
			animations.Add(animation);
		}

		private readonly List<Animation> animations = new();
	}
}
