using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public class RunOncePolicy : IExecutionPolicy
{
	private int runCount;

	private int DefaultBackoffIntervalInSeconds;

	public bool Expired
	{
		get
		{
			return runCount > 0;
		}
		set
		{
			if (value)
			{
				runCount = 1;
			}
		}
	}

	public bool ResumeExecution()
	{
		if (Expired)
		{
			return false;
		}
		runCount++;
		return true;
	}

	public TimeSpan BackoffInterval()
	{
		return TimeSpan.FromSeconds(DefaultBackoffIntervalInSeconds);
	}
}
