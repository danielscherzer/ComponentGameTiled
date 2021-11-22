using System;

namespace Example.Core
{
	internal class PeriodicBehavior : IOnUpdate
	{
		private float coolDown;
		private readonly Func<bool> periodicActionReset;
		public float PeriodSpan { get; }

		public PeriodicBehavior(float periodSpan, Func<bool> periodicActionReset, float initialPeriodSpan = -1)
		{
			this.periodicActionReset = periodicActionReset ?? throw new ArgumentNullException(nameof(periodicActionReset));
			PeriodSpan = periodSpan;
			coolDown = initialPeriodSpan;
		}

		public void Update(float deltaTime)
		{
			if (coolDown < 0)
			{
				if (periodicActionReset()) coolDown = PeriodSpan;
			}
			coolDown -= deltaTime;
		}
	}
}
