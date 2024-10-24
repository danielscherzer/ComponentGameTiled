using Example.Core;

namespace Example.Visuals;

internal class AnimatedTileVisual(GameObject gameObject, Animation animation, TileRenderer tileRenderer) : IOnDraw
{
	public virtual void Draw(float deltaTime)
	{
		time += deltaTime;
		var tileId = animation.GetFrame(time);
		tileRenderer.Draw(tileId, gameObject.Bounds);
	}

	private float time;
}