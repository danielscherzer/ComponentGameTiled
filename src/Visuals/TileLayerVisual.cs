using Example.Core;
using Zenseless.OpenTK;

namespace Example.Visuals;

internal class TileLayerVisual(IBound terrain, uint[,] layer, TileRenderer tileRenderer) : IOnDraw
{
	private float time = 0f;

	public void Draw(float deltaTime)
	{
		time += deltaTime;
		var bounds = terrain.Bounds;
		var cellWidth = bounds.Size.X / layer.GetLength(0);
		var cellHeight = bounds.Size.Y / layer.GetLength(1);
		for (int ux = 0; ux < layer.GetLength(0); ++ux)
		{
			var x = bounds.Min.X + ux * cellWidth;
			for (int uy = 0; uy < layer.GetLength(1); ++uy)
			{
				var y = bounds.Min.Y + uy * cellHeight;
				var tileId = layer[ux, uy];
				if (0 == tileId) continue;
				var coords = Box2Extensions.CreateFromMinSize(x, y, cellWidth, cellHeight);
				var animation = tileRenderer.GetAnimation(tileId);
				if (animation != null)
				{
					tileId = animation.GetFrame(time);
				}
				tileRenderer.Draw(tileId, coords);
			}
		}
	}
}
