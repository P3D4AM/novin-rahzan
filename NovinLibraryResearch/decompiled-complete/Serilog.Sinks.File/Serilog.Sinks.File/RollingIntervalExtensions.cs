using System;

namespace Serilog.Sinks.File;

internal static class RollingIntervalExtensions
{
	public static string GetFormat(this RollingInterval interval)
	{
		return interval switch
		{
			RollingInterval.Infinite => "", 
			RollingInterval.Year => "yyyy", 
			RollingInterval.Month => "yyyyMM", 
			RollingInterval.Day => "yyyyMMdd", 
			RollingInterval.Hour => "yyyyMMddHH", 
			RollingInterval.Minute => "yyyyMMddHHmm", 
			_ => throw new ArgumentException("Invalid rolling interval"), 
		};
	}

	public static DateTime? GetCurrentCheckpoint(this RollingInterval interval, DateTime instant)
	{
		return interval switch
		{
			RollingInterval.Infinite => null, 
			RollingInterval.Year => new DateTime(instant.Year, 1, 1, 0, 0, 0, instant.Kind), 
			RollingInterval.Month => new DateTime(instant.Year, instant.Month, 1, 0, 0, 0, instant.Kind), 
			RollingInterval.Day => new DateTime(instant.Year, instant.Month, instant.Day, 0, 0, 0, instant.Kind), 
			RollingInterval.Hour => new DateTime(instant.Year, instant.Month, instant.Day, instant.Hour, 0, 0, instant.Kind), 
			RollingInterval.Minute => new DateTime(instant.Year, instant.Month, instant.Day, instant.Hour, instant.Minute, 0, instant.Kind), 
			_ => throw new ArgumentException("Invalid rolling interval"), 
		};
	}

	public static DateTime? GetNextCheckpoint(this RollingInterval interval, DateTime instant)
	{
		DateTime? currentCheckpoint = interval.GetCurrentCheckpoint(instant);
		if (!currentCheckpoint.HasValue)
		{
			return null;
		}
		return interval switch
		{
			RollingInterval.Year => currentCheckpoint.Value.AddYears(1), 
			RollingInterval.Month => currentCheckpoint.Value.AddMonths(1), 
			RollingInterval.Day => currentCheckpoint.Value.AddDays(1.0), 
			RollingInterval.Hour => currentCheckpoint.Value.AddHours(1.0), 
			RollingInterval.Minute => currentCheckpoint.Value.AddMinutes(1.0), 
			_ => throw new ArgumentException("Invalid rolling interval"), 
		};
	}
}
