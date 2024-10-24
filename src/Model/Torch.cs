using Example.Core;
using System;
using System.Linq;
using Zenseless.OpenTK;

namespace Example.Model;

internal class Torch(GameObject torch, GameObject goPlayer, Scene scene) : IOnUpdate
{
	private readonly Player player = goPlayer.Components.OfType<Player>().FirstOrDefault() ?? throw new ArgumentException($"GameObject {nameof(goPlayer)} has no component of type Player");

	public void Update(float deltaTime)
	{
		//TODO: 9. The player should be able to pick-up a torch.
		//TODO: 9.1 The torch `GameObject` is removed from the scene via Scene.Remove() when the player overlaps it and is added to the player via Player.HasTorch.
#if SOLUTION
		if (torch.Bounds.Overlaps(player.Bounds))
		{
			scene.Remove(torch);
			player.HasTorch = true;
		}
#endif
	}
}