using OpenTK.Mathematics;

namespace Example.Core;

internal interface IBound
{
	Box2 Bounds { get; }
}