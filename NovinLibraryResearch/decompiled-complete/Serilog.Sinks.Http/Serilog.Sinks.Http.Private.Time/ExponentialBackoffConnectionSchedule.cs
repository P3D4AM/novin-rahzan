using System;

namespace Serilog.Sinks.Http.Private.Time;

public class ExponentialBackoffConnectionSchedule
{
	public static readonly TimeSpan MinimumBackoffPeriod = TimeSpan.FromSeconds(5.0);

	public static readonly TimeSpan MaximumBackoffInterval = TimeSpan.FromMinutes(10.0);

	private readonly TimeSpan period;

	private int failuresSinceSuccessfulConnection;

	public TimeSpan NextInterval
	{
		get
		{
			try
			{
				if (failuresSinceSuccessfulConnection <= 1)
				{
					return period;
				}
				double num = Math.Pow(2.0, failuresSinceSuccessfulConnection - 1);
				TimeSpan timeSpan = period;
				long ticks = timeSpan.Ticks;
				timeSpan = MinimumBackoffPeriod;
				long val = checked((long)((double)Math.Max(ticks, timeSpan.Ticks) * num));
				timeSpan = MaximumBackoffInterval;
				long val2 = Math.Min(timeSpan.Ticks, val);
				timeSpan = period;
				return TimeSpan.FromTicks(Math.Max(timeSpan.Ticks, val2));
			}
			catch (OverflowException)
			{
				return MaximumBackoffInterval;
			}
		}
	}

	public ExponentialBackoffConnectionSchedule(TimeSpan period)
	{
		if (period < TimeSpan.Zero)
		{
			throw new ArgumentOutOfRangeException("period", "The connection retry period must be a positive timespan");
		}
		this.period = period;
	}

	public void MarkSuccess()
	{
		failuresSinceSuccessfulConnection = 0;
	}

	public void MarkFailure()
	{
		failuresSinceSuccessfulConnection++;
	}
}
