using System;

namespace Serilog.Sinks.File;

internal static class Clock
{
	private static Func<DateTime> _dateTimeNow = () => DateTime.Now;

	[ThreadStatic]
	private static DateTime _testDateTimeNow;

	public static DateTime DateTimeNow => _dateTimeNow();

	public static void SetTestDateTimeNow(DateTime now)
	{
		_testDateTimeNow = now;
		_dateTimeNow = () => _testDateTimeNow;
	}
}
