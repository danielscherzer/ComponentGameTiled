using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace Example.Core
{
	internal class Scene
	{
		internal GameObject CreateGameObject(Box2 bounds, string name)
		{
			var gameObject = new GameObject(bounds, name);
			if (IsIterating)
			{
				spawnedGameObjects.Add(gameObject);
			}
			else
			{
				gameObjects.Add(gameObject);
			}
			return gameObject;
		}

		public IEnumerable<GameObject> GameObjects
		{
			get
			{
				++iterationCount; // could be called recursively, so a boolean is not enough (iterate while iterating)
				foreach (var gameObject in gameObjects)
				{
					yield return gameObject;
				}
				--iterationCount;
			}
		}

		public IEnumerable<GameObject> GetGameObjects(string name)
		{
			return GameObjects.Where(gameObject => gameObject.Name == name);
		}

		public IEnumerable<TYPE> GetAllComponents<TYPE>()
		{
			return GameObjects.SelectMany(gameObject => gameObject.Components.OfType<TYPE>());
		}

		public void Remove(GameObject gameObject)
		{
			if (IsIterating)
			{
				removeGameObjects.Add(gameObject);
			}
			else
			{
				gameObjects.Remove(gameObject);
			}
		}

		public void Update(float deltaTime)
		{
			foreach (var gameObject in spawnedGameObjects)
			{
				gameObjects.Add(gameObject);
			}
			spawnedGameObjects.Clear();
			foreach (var updateComponent in GetAllComponents<IOnUpdate>())
			{
				updateComponent.Update(deltaTime);
			}
			foreach (var gameObject in removeGameObjects)
			{
				gameObjects.Remove(gameObject);
			}
			removeGameObjects.Clear();
		}

		private bool IsIterating => 0 < iterationCount;

		private readonly HashSet<GameObject> gameObjects = new(); //fast add and removal
		private readonly List<GameObject> removeGameObjects = new();
		private readonly List<GameObject> spawnedGameObjects = new();
		private int iterationCount = 0;
	}
}
