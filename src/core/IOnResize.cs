namespace Example.Core
{
	internal interface IOnResize : IComponent
	{
		void Resize(int width, int height);
	}
}