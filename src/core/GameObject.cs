using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Example.Core;

public class GameObject(Box2 bounds, string name = "") : IBound
{
	public Box2 Bounds { get; set; } = bounds;
	Box2 IBound.Bounds => Bounds;

	public string Name { get; } = name;

	public List<IComponent> Components { get; } = new List<IComponent>();

	public override string ToString() => $"{Name} {Bounds}";
}
