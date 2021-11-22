using Example.Core;

namespace Example.Visuals
{
	internal class TileVisual : IOnDraw
	{
		public TileVisual(GameObject gameObject, uint tileId, TileRenderer tileRenderer)
		{
			this.gameObject = gameObject;
			TileId = tileId;
			this.tileRenderer = tileRenderer;
		}

		public uint TileId { get; }

		public virtual void Draw(float deltaTime)
		{
			tileRenderer.Draw(TileId, gameObject.Bounds);
		}

		private readonly GameObject gameObject;
		private readonly TileRenderer tileRenderer;
	}
}