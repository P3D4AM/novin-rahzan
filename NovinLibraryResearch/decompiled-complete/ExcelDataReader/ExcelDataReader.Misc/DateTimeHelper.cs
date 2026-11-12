using System;

namespace ExcelDataReader.Misc;

internal static class DateTimeHelper
{
	public const double OADateMinAsDouble = -657435.0;

	public const double OADateMaxAsDouble = 2958466.0;

	private const long TicksPerMillisecond = 10000L;

	private const long TicksPerSecond = 10000000L;

	private const long TicksPerMinute = 600000000L;

	private const long TicksPerHour = 36000000000L;

	private const long TicksPerDay = 864000000000L;

	private const int MillisPerSecond = 1000;

	private const int MillisPerMinute = 60000;

	private const int MillisPerHour = 3600000;

	private const int MillisPerDay = 86400000;

	private const int DaysPerYear = 365;

	private const int DaysPer4Years = 1461;

	private const int DaysPer100Years = 36524;

	private const int DaysPer400Years = 146097;

	private const int DaysTo1899 = 693593;

	private const int DaysTo10000 = 3652059;

	private const long MaxMillis = 315537897600000L;

	private const long DoubleDateOffset = 599264352000000000L;

	public static DateTime FromOADate(double d)
	{
		return new DateTime(DoubleDateToTicks(d), DateTimeKind.Unspecified);
	}

	internal static long DoubleDateToTicks(double value)
	{
		if (value >= 2958466.0 || value <= -657435.0)
		{
			throw new ArgumentException("Invalid OA Date");
		}
		long num = (long)(value * 86400000.0 + ((value >= 0.0) ? 0.5 : (-0.5)));
		if (num < 0)
		{
			num -= num % 86400000 * 2;
		}
		num += 59926435200000L;
		if (num < 0 || num >= 315537897600000L)
		{
			throw new ArgumentException("OA Date out of range");
		}
		return num * 10000;
	}
}
