using Example;
using Example.Core;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Zenseless.OpenTK;
using Zenseless.Resources;

var window = new GameWindow(GameWindowSettings.Default, ImmediateMode.NativeWindowSettings);
window.KeyDown += args => { if (Keys.Escape == args.Key) window.Close(); };
window.Title = "Exercise Tiled";

EmbeddedResourceDirectory resourceDirectory = new(nameof(Example) + ".Content");
Input input = new(window);
var scene = resourceDirectory.LoadSceneFromTiled("level.tmx", input);
//scene = Game.LoadSceneFromTiled(@"D:\Daten\downloads\TiledMap_CG\Zuhause.tmx", new Input(this));

window.RenderFrame += args =>
{
	var deltaTime = (float)args.Time;
	foreach (var drawComponent in scene.GetAllComponents<IOnDraw>())
	{
		drawComponent.Draw(deltaTime);
	}
};
window.RenderFrame += _ => window.SwapBuffers();
window.Resize += args =>
{
	foreach (var resizeComponent in scene.GetAllComponents<IOnResize>())
	{
		resizeComponent.Resize(args.Width, args.Height);
	}
};

window.UpdateFrame += args => scene.Update((float)args.Time);

window.Run();
