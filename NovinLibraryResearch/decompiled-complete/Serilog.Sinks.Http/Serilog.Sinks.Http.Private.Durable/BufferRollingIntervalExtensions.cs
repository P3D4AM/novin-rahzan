using System;

namespace Serilog.Sinks.Http.Private.Durable;

public static class BufferRollingIntervalExtensions
{
	public static RollingInterval ToRollingInterval(this BufferRollingInterval self)
	{
		return self switch
		{
			BufferRollingInterval.Minute => RollingInterval.Minute, 
			BufferRollingInterval.Hour => RollingInterval.Hour, 
			BufferRollingInterval.Day => RollingInterval.Day, 
			BufferRollingInterval.Month => RollingInterval.Month, 
			BufferRollingInterval.Year => RollingInterval.Year, 
			_ => throw new ArgumentException($"Buffer rolling interval {self} is not supported."), 
		};
	}
}
