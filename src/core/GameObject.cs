using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Example.Core
{
	public class GameObject : IBound
	{
		public GameObject(Box2 bounds, string name = "")
		{
			Bounds = bounds;
			Name = name;
		}

		public Box2 Bounds { get; set; }
		Box2 IBound.Bounds => Bounds;

		public string Name { get; }

		public List<IComponent> Components { get; } = new List<IComponent>();

		public override string ToString() => $"{Name} {Bounds}";
	}
}
