using Example.Core;
using OpenTK.Mathematics;
using Zenseless.OpenTK;

namespace Example.Model
{
	/// <summary>
	/// This class handles the logic of the player, like movement, inventory and collision.
	/// </summary>
	internal class Player : IOnUpdate, IBound
	{
		private readonly GameObject gameObject;
		private readonly IInput input;
		private readonly Terrain terrain;

		public Player(GameObject gameObject, IInput input, Terrain terrain)
		{
			this.gameObject = gameObject;
			this.input = input;
			this.terrain = terrain;
		}

		public Vector2 Velocity { get; private set; } = Vector2.Zero;

		public Box2 Bounds => gameObject.Bounds;

		public bool HasTorch { get; internal set; }

		public void Update(float deltaTime)
		{
			//TODO: 03. Implement player movement with the arrow keys.
			//TODO: 07. Check if the terrain collides with the player. Use Terrain.Collides().
#if SOLUTION
			var movementXAxis = input.IsButtonDown("Left") ? -1 : input.IsButtonDown("Right") ? 1 : 0;
			var movementYAxis = input.IsButtonDown("Down") ? -1 : input.IsButtonDown("Up") ? 1 : 0;
			var movement = new Vector2(movementXAxis, movementYAxis);

			var oldBounds = gameObject.Bounds;
			Velocity = 0.3f * movement;
			gameObject.Bounds = gameObject.Bounds.Translated(deltaTime * Velocity);

			var scale = new Vector2(0.5f, 0.8f);
			var bounds = Box2Extensions.CreateFromCenterSize(gameObject.Bounds.Center, Vector2.Multiply(gameObject.Bounds.Size, scale));
			if (terrain.Collides(bounds))
			{
				gameObject.Bounds = oldBounds;
			}
#endif
		}
	}
}
