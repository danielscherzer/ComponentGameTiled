using Example.Core;
using OpenTK.Mathematics;
using System;
using Zenseless.OpenTK;

namespace Example.Model;

internal class Terrain : IComponent
{
	private readonly Box2 bounds;
	/// <summary>
	/// <seealso cref="collisionGrid"/>[0, 0] maps to the tile of the map in the lower-left corner.
	/// <seealso cref="collisionGrid"/>[gridWidth - 1, gridHeight - 1] maps to the tile of the map in the upper-right corner.
	/// </summary>
	private readonly bool[,] collisionGrid;

	public Terrain(Box2 bounds, bool[,] collisionGrid)
	{
		this.bounds = bounds;
		this.collisionGrid = collisionGrid;
		PrintToConsole(collisionGrid);
	}

	/// <summary>
	/// Checks the given object bounds for collision with the terrain and the terrain borders.
	/// </summary>
	/// <param name="objBounds">Object bounds to check.</param>
	/// <returns><c>true</c> if a collision is present.</returns>
	public bool Collides(Box2 objBounds)
	{
		//TODO: 08. Implement `Terrain.Collides`.
		//TODO: 08.1 Oject bounds that are not fully contained inside the terrains bounds create a collision. You can use Box2Extensions.Contains() to check for this.
		//TODO: 08.2 Use the collision information stored in the `collisionGrid`, which holds a `true` entry for every tile that should create a collision. 
		//TODO: 08.3 Bounds that overlap a tile with a true entry in the collision grid should create a collision.
		//TODO: 08.4 Make your collision test efficient (only test tiles which the given rectangle currently overlaps.
#if SOLUTION
		if (!Box2Extensions.Contains(bounds, objBounds)) return true;
		var gridSize = Vector2.Divide(new Vector2(collisionGrid.GetLength(0), collisionGrid.GetLength(1)), bounds.Size);
		//convert from world to grid coordinates bounds -> [0, gridLength - 1]²
		var coord_grid = Box2Extensions.CreateFromMinSize((objBounds.Min - bounds.Min) * gridSize, objBounds.Size * gridSize);

		int objLowerX = (int)coord_grid.Min.X;
		int objLowerY = (int)coord_grid.Min.Y;
		int objUpperX = (int)Math.Ceiling(coord_grid.Max.X);
		int objUpperY = (int)Math.Ceiling(coord_grid.Max.Y);
		for (var x = objLowerX; x < objUpperX; ++x)
		{
			for (var y = objLowerY; y < objUpperY; ++y)
			{
				if (collisionGrid[x, y])
				{
					return true;
				}
			}
		}
#endif
		return false;
	}

	private static void PrintToConsole(bool[,] collisionGrid)
	{
		var width = collisionGrid.GetLength(0);
		var height = collisionGrid.GetLength(1);
		Console.WriteLine(new string('X', width + 2));
		for (int y = height - 1; y >= 0; --y)
		{
			Console.Write("X");
			for (int x = 0; x < width; ++x)
			{
				Console.Write(collisionGrid[x, y] ? "8" : " ");
			}
			Console.WriteLine("X");
		}
		Console.WriteLine(new string('X', width + 2));
	}
}