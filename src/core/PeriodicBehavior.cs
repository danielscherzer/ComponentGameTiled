using System;

namespace Example.Core;

internal class PeriodicBehavior(float periodSpan, Func<bool> periodicActionReset, float initialPeriodSpan = -1) : IOnUpdate
{
	private readonly Func<bool> periodicActionReset = periodicActionReset ?? throw new ArgumentNullException(nameof(periodicActionReset));
	public float PeriodSpan { get; } = periodSpan;

	public void Update(float deltaTime)
	{
		if (initialPeriodSpan < 0)
		{
			if (periodicActionReset()) initialPeriodSpan = PeriodSpan;
		}
		initialPeriodSpan -= deltaTime;
	}
}
