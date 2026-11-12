using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public class FixedTimeoutRetryPolicy : IExecutionPolicy
{
	private TimeSpan timeout;

	private TimeSpan interval;

	private DateTime startTime;

	public bool Expired
	{
		get
		{
			if (startTime == DateTime.MaxValue)
			{
				return false;
			}
			return startTime - DateTime.Now > timeout;
		}
		set
		{
			if (value)
			{
				timeout = TimeSpan.Zero;
			}
		}
	}

	public FixedTimeoutRetryPolicy(TimeSpan timeout, TimeSpan interval)
	{
		this.timeout = timeout;
		this.interval = interval;
		startTime = DateTime.MaxValue;
	}

	public bool ResumeExecution()
	{
		if (startTime == DateTime.MaxValue)
		{
			startTime = DateTime.Now;
		}
		if (startTime - DateTime.Now < timeout)
		{
			return true;
		}
		return false;
	}

	public TimeSpan BackoffInterval()
	{
		return interval;
	}
}
