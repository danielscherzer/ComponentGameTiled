using Example.Core;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Zenseless.OpenTK;

namespace Example.Visuals
{
	/// <summary>
	/// This class handles calculates the camera matrix and viewport.
	/// It handles the correction of the window aspect ratio, panning and zooming.
	/// It also clears the screen.
	/// Therefore this object should be drawn before all others each frame.
	/// </summary>
	internal class Camera : IOnDraw, IOnUpdate, IOnResize
	{
		public Camera(IInput input)
		{
			this.input = input;
		}

		/// <summary>
		/// The camera centers of the screen on an optional <seealso cref="FocusObject"/>.
		/// </summary>
		public IBound? FocusObject { get; set; }

		public void Update(float deltaTime)
		{
			//TODO: 02. Render with correct aspect ratio when the window changes size.
			//TODO: 05. Implement the camera zoom by updating the camera matrix with Transformation2d.Scale(). Use the `PageUp` and `PageDown` keys for zooming in/out.
			//TODO: 06. Implement panning by using the center of the optional FocusObject as a translation of the camera.
#if SOLUTION
			var axisX = input.IsButtonDown("PageDown") ? -1f : input.IsButtonDown("PageUp") ? 1f : 0f;
			Zoom *= 1 + deltaTime * axisX;
			cameraMatrix = Transformation2d.Combine(Transformation2d.Scale(Zoom), Transformation2d.Scale(invWindowAspectRatio, 1f));

			if (FocusObject != null)
			{
				var translate = Matrix4.CreateTranslation(-new Vector3(FocusObject.Bounds.Min + 0.5f * FocusObject.Bounds.Size));
				cameraMatrix = Transformation2d.Combine(translate, cameraMatrix);
			}
#endif
		}

		public void Draw(float deltaTime)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit); // clear the screen
			GL.LoadMatrix(ref cameraMatrix);
		}

		public void Resize(int width, int height)
		{
			GL.Viewport(0, 0, width, height); // tell OpenGL to use the whole window for drawing
#if SOLUTION
			invWindowAspectRatio = height / (float)width;
#endif
		}

		internal float Zoom
		{
			get => _zoom;
			set
			{
				_zoom = value;
				_zoom = MathHelper.Clamp(_zoom, 1f, 10f);
			}
		}

		private readonly IInput input;
		private float _zoom = 3f;
		/// <summary>
		/// Contains the joint tansformation of the camera and is set each frame.
		/// </summary>
		private Matrix4 cameraMatrix = Matrix4.Identity;
#if SOLUTION
		private float invWindowAspectRatio = 1f;
#endif
	}
}