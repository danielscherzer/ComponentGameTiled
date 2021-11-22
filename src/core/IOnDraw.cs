namespace Example.Core
{
	internal interface IOnDraw : IComponent
	{
		void Draw(float deltaTime);
	}
}