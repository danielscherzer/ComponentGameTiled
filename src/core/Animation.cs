using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.Core
{
	/// <summary>
	/// Class that encapsulates an animation consisting of a series of animation frame numbers and an animation duration
	/// </summary>
	public class Animation
	{
		public Animation(float duration, IEnumerable<uint> animationFrames)
		{
			Duration = duration;
			frames = animationFrames.ToArray();
		}

		public Animation(float duration, IEnumerable<int> animationFrames) : this(duration, animationFrames.Select(input => (uint)input)) { }

		/// <summary>
		/// Gets the duration of the animation in seconds.
		/// </summary>
		/// <value>
		/// The duration in seconds.
		/// </value>
		public float Duration { get; set; }

		/// <summary>
		/// Gets the frame number for the given time. The animation is looped.
		/// </summary>
		/// <param name="time">The time in seconds.</param>
		/// <returns>A frame number</returns>
		public uint GetFrame(float time)
		{
			var normalizedTime = (time / Duration) % 1f;
			var idFloat = normalizedTime * (frames.Length - 1);
			uint id = (uint)MathF.Round(idFloat);
			return frames[id];
		}

		private readonly uint[] frames;
	}
}
