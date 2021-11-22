using Zenseless.OpenTK;
using Zenseless.Resources;

namespace Example
{
	internal static class ResourceExtensions
	{
		internal static Texture2D LoadTexture(this IResourceDirectory resourceDirectory, string name)
		{
			using var stream = resourceDirectory.Resource(name).Open();
			return Texture2DLoader.Load(stream);
		}
	}
}
