using System;
using System.Globalization;
using System.Threading;

namespace Stimulsoft.Data.Extensions;

public static class DataTimeExt
{
	public static DateTime FirstDayOfMonth(this DateTime value)
	{
		return new DateTime(value.Year, value.Month, 1);
	}

	public static int DaysInMonth(this DateTime value)
	{
		return DateTime.DaysInMonth(value.Year, value.Month);
	}

	public static DateTime LastDayOfMonth(this DateTime value)
	{
		return new DateTime(value.Year, value.Month, value.DaysInMonth());
	}

	public static DateTime FirstDayOfQuarter(this DateTime value)
	{
		return new DateTime(value.Year, 1, 1).AddQuarters(value.GetQuarter() - 1);
	}

	public static DateTime LastDayOfQuarter(this DateTime value)
	{
		return new DateTime(value.Year, 1, 1).AddQuarters(value.GetQuarter()).AddDays(-1.0);
	}

	public static DateTime LastDateTimeOfDay(this DateTime dateTime)
	{
		return dateTime.Date.AddDays(1.0).AddTicks(-1L);
	}

	public static DateTime AddQuarters(this DateTime value, int quarters)
	{
		return value.AddMonths(quarters * 3);
	}

	public static int GetQuarter(this DateTime value)
	{
		int num = value.Month - 1;
		return Math.Abs(num / 3) + 1;
	}

	public static DateTime FirstDayOfWeek(this DateTime value)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		int num = value.DayOfWeek - currentCulture.DateTimeFormat.FirstDayOfWeek;
		if (num < 0)
		{
			num += 7;
		}
		return value.AddDays(-num).Date;
	}

	public static DateTime LastDayOfWeek(this DateTime value)
	{
		return value.FirstDayOfWeek().AddDays(6.0);
	}

	public static DateTime FirstDayOfYear(this DateTime value)
	{
		return new DateTime(value.Year, 1, 1);
	}

	public static DateTime LastDayOfYear(this DateTime value)
	{
		return new DateTime(value.Year, 12, 31);
	}

	public static DateTime FirstDayOfFirthQuarter(this DateTime value)
	{
		return new DateTime(value.Year, 1, 1).FirstDayOfQuarter();
	}

	public static DateTime LastDayOfFirthQuarter(this DateTime value)
	{
		return new DateTime(value.Year, 3, 31).LastDayOfQuarter();
	}

	public static DateTime FirstDayOfSecondQuarter(this DateTime value)
	{
		return new DateTime(value.Year, 4, 1).FirstDayOfQuarter();
	}

	public static DateTime LastDayOfSecondQuarter(this DateTime value)
	{
		return new DateTime(value.Year, 6, 30).LastDayOfQuarter();
	}

	public static DateTime FirstDayOfThirdQuarter(this DateTime value)
	{
		return new DateTime(value.Year, 7, 1).FirstDayOfQuarter();
	}

	public static DateTime LastDayOfThirdQuarter(this DateTime value)
	{
		return new DateTime(value.Year, 9, 30).LastDayOfQuarter();
	}

	public static DateTime FirstDayOfFourthQuarter(this DateTime value)
	{
		return new DateTime(value.Year, 10, 1).FirstDayOfQuarter();
	}

	public static DateTime LastDayOfFourthQuarter(this DateTime value)
	{
		return new DateTime(value.Year, 12, 31).LastDayOfQuarter();
	}
}
