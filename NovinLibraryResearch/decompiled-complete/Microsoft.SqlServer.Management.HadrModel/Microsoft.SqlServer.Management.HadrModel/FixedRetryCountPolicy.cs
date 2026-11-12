using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public class FixedRetryCountPolicy : IExecutionPolicy
{
	private int retryCount;

	private int maxRetry;

	private TimeSpan interval;

	public bool Expired
	{
		get
		{
			if (retryCount >= maxRetry)
			{
				return true;
			}
			return false;
		}
		set
		{
			if (value)
			{
				maxRetry = 0;
			}
		}
	}

	public FixedRetryCountPolicy(TimeSpan interval, int maxRetry)
	{
		retryCount = 0;
		this.interval = interval;
		this.maxRetry = maxRetry;
	}

	public bool ResumeExecution()
	{
		if (Expired)
		{
			return false;
		}
		retryCount++;
		return true;
	}

	public TimeSpan BackoffInterval()
	{
		return interval;
	}
}
