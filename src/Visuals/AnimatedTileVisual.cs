using Example.Core;

namespace Example.Visuals
{
	internal class AnimatedTileVisual : IOnDraw
	{
		public AnimatedTileVisual(GameObject gameObject, Animation animation, TileRenderer tileRenderer)
		{
			this.gameObject = gameObject;
			this.animation = animation;
			this.tileRenderer = tileRenderer;
		}

		public virtual void Draw(float deltaTime)
		{
			time += deltaTime;
			var tileId = animation.GetFrame(time);
			tileRenderer.Draw(tileId, gameObject.Bounds);
		}

		private readonly GameObject gameObject;
		private readonly Animation animation;
		private readonly TileRenderer tileRenderer;
		private float time;
	}
}