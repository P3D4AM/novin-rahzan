using System;

namespace Azure.Core;

public class RetryOptions
{
	public int MaxRetries { get; set; } = 3;


	public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(0.8);


	public TimeSpan MaxDelay { get; set; } = TimeSpan.FromMinutes(1.0);


	public RetryMode Mode { get; set; } = RetryMode.Exponential;


	public TimeSpan NetworkTimeout { get; set; } = TimeSpan.FromSeconds(100.0);


	internal RetryOptions()
	{
	}
}
