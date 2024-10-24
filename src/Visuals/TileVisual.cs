using Example.Core;

namespace Example.Visuals;

internal class TileVisual(GameObject gameObject, uint tileId, TileRenderer tileRenderer) : IOnDraw
{
	public uint TileId { get; } = tileId;

	public virtual void Draw(float deltaTime)
	{
		tileRenderer.Draw(TileId, gameObject.Bounds);
	}
}