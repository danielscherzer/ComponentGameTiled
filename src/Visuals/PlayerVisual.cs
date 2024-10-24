using Example.Core;
using Example.Model;
using System;

namespace Example.Visuals;

internal class PlayerVisual : IOnDraw
{
	public PlayerVisual(Player player, TileRenderer tileRenderer)
	{
		this.player = player;
		this.tileRenderer = tileRenderer;
		var animations = this.tileRenderer.GetTileSet("goblin_0")?.Animations ?? throw new ArgumentException("tile set goblin_0 not found");
		animationWalkDown = animations[0];
		animationWalkRight = animations[1];
		animationWalkUp = animations[2];
		animationWalkLeft = animations[3];
		animationIdle = animations[4];
	}

	public void Draw(float deltaTime)
	{
		var newAnimation = animationIdle;
		// TODO: 04. Select the animation for the current movement direction out of the prepared animations (animationIdle, ...).
#if SOLUTION
		if (player.Velocity.X < 0f) newAnimation = animationWalkLeft;
		else if (player.Velocity.X > 0f) newAnimation = animationWalkRight;
		else if (player.Velocity.Y < 0f) newAnimation = animationWalkDown;
		else if (player.Velocity.Y > 0f) newAnimation = animationWalkUp;

		if (ReferenceEquals(lastAnimation, newAnimation))
		{
			time += deltaTime;
		}
		else
		{
			time = 0f;
			lastAnimation = newAnimation;
		}
#endif
		tileRenderer.Draw(newAnimation.GetFrame(time), player.Bounds);
	}

	private readonly TileRenderer tileRenderer;

	private readonly Animation animationIdle, animationWalkLeft, animationWalkRight, animationWalkDown, animationWalkUp;
	private readonly Player player;
	private float time = 0;
	private Animation? lastAnimation;
}
