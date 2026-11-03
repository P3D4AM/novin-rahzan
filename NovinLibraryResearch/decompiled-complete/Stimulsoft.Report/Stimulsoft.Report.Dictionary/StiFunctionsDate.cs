using System;
using System.Globalization;
using System.Threading;
using Stimulsoft.Data.Functions;

namespace Stimulsoft.Report.Dictionary;

public class StiFunctionsDate
{
	public static TimeSpan DateDiff(DateTime date1, DateTime date2)
	{
		return date1.Subtract(date2);
	}

	public static TimeSpan? DateDiff(DateTime? date1, DateTime? date2)
	{
		if (date1.HasValue && date2.HasValue)
		{
			return date1.Value.Subtract(date2.Value);
		}
		return null;
	}

	public static long Year(DateTime date)
	{
		return date.Year;
	}

	public static long Year(DateTime? date)
	{
		if (date.HasValue)
		{
			return date.Value.Year;
		}
		return 0L;
	}

	public static long Month(DateTime date)
	{
		return date.Month;
	}

	public static long Month(DateTime? date)
	{
		if (date.HasValue)
		{
			return date.Value.Month;
		}
		return 0L;
	}

	public static object MonthIdent(object value)
	{
		return Funcs.MonthIdentObject(value);
	}

	public static long Hour(DateTime date)
	{
		return date.Hour;
	}

	public static long Hour(DateTime? date)
	{
		if (date.HasValue)
		{
			return date.Value.Hour;
		}
		return 0L;
	}

	public static long Minute(DateTime date)
	{
		return date.Minute;
	}

	public static long Minute(DateTime? date)
	{
		if (date.HasValue)
		{
			return date.Value.Minute;
		}
		return 0L;
	}

	public static long Second(DateTime date)
	{
		return date.Second;
	}

	public static long Second(DateTime? date)
	{
		_ = date.HasValue;
		return date.Value.Second;
	}

	public static long Day(DateTime date)
	{
		return date.Day;
	}

	public static long Day(DateTime? date)
	{
		if (date.HasValue)
		{
			return date.Value.Day;
		}
		return 0L;
	}

	public static string DayOfWeek(DateTime date)
	{
		return Func.DayOfWeekToStr.DayOfWeek(date);
	}

	public static string DayOfWeek(DateTime? date)
	{
		if (date.HasValue)
		{
			return Func.DayOfWeekToStr.DayOfWeek(date.Value);
		}
		return string.Empty;
	}

	public static string DayOfWeek(DateTime date, bool localized)
	{
		return Func.DayOfWeekToStr.DayOfWeek(date, localized);
	}

	public static string DayOfWeek(DateTime? date, bool localized)
	{
		if (date.HasValue)
		{
			return Func.DayOfWeekToStr.DayOfWeek(date.Value, localized);
		}
		return string.Empty;
	}

	public static string DayOfWeek(DateTime date, string culture)
	{
		return Func.DayOfWeekToStr.DayOfWeek(date, culture);
	}

	public static string DayOfWeek(DateTime? date, string culture)
	{
		if (date.HasValue)
		{
			return Func.DayOfWeekToStr.DayOfWeek(date.Value, culture);
		}
		return string.Empty;
	}

	public static string DayOfWeek(DateTime date, string culture, bool upperCase)
	{
		return Func.DayOfWeekToStr.DayOfWeek(date, culture, upperCase);
	}

	public static string DayOfWeek(DateTime? date, string culture, bool upperCase)
	{
		if (date.HasValue)
		{
			return Func.DayOfWeekToStr.DayOfWeek(date.Value, culture, upperCase);
		}
		return string.Empty;
	}

	public static string MonthName(DateTime date)
	{
		return Func.MonthToStr.MonthName(date);
	}

	public static string MonthName(DateTime? date)
	{
		if (date.HasValue)
		{
			return Func.MonthToStr.MonthName(date.Value);
		}
		return string.Empty;
	}

	public static string MonthName(DateTime date, bool localized)
	{
		return Func.MonthToStr.MonthName(date, localized);
	}

	public static string MonthName(DateTime? date, bool localized)
	{
		if (date.HasValue)
		{
			return Func.MonthToStr.MonthName(date.Value, localized);
		}
		return string.Empty;
	}

	public static string MonthName(DateTime date, string culture)
	{
		return Func.MonthToStr.MonthName(date, culture);
	}

	public static string MonthName(DateTime? date, string culture)
	{
		if (date.HasValue)
		{
			return Func.MonthToStr.MonthName(date.Value, culture);
		}
		return string.Empty;
	}

	public static string MonthName(DateTime date, string culture, bool upperCase)
	{
		return Func.MonthToStr.MonthName(date, culture, upperCase);
	}

	public static string MonthName(DateTime? date, string culture, bool upperCase)
	{
		if (date.HasValue)
		{
			return Func.MonthToStr.MonthName(date.Value, culture, upperCase);
		}
		return string.Empty;
	}

	public static long DayOfYear(DateTime date)
	{
		return date.DayOfYear;
	}

	public static long DayOfYear(DateTime? date)
	{
		if (date.HasValue)
		{
			return date.Value.DayOfYear;
		}
		return 0L;
	}

	public static DateTime DateSerial(long year, long month, long day)
	{
		return new DateTime((int)year, (int)month, (int)day);
	}

	public static DateTime DateSerial(long ticks)
	{
		return new DateTime(ticks);
	}

	public static TimeSpan TimeSerial(long hours, long minutes, long seconds)
	{
		return new TimeSpan((int)hours, (int)minutes, (int)seconds);
	}

	public static long DaysInMonth(long year, long month)
	{
		return DateTime.DaysInMonth((int)year, (int)month);
	}

	public static long DaysInMonth(DateTime date)
	{
		return DateTime.DaysInMonth(date.Year, date.Month);
	}

	public static long DaysInMonth(DateTime? date)
	{
		if (date.HasValue)
		{
			return DateTime.DaysInMonth(date.Value.Year, date.Value.Month);
		}
		return 0L;
	}

	public static long DaysInYear(long year)
	{
		return DateTime.IsLeapYear((int)year) ? 366 : 365;
	}

	public static long DaysInYear(DateTime date)
	{
		return DateTime.IsLeapYear(date.Year) ? 366 : 365;
	}

	public static long DaysInYear(DateTime? date)
	{
		if (date.HasValue)
		{
			return DateTime.IsLeapYear(date.Value.Year) ? 366 : 365;
		}
		return 0L;
	}

	public static long WeekOfYear(DateTime date)
	{
		DateTimeFormatInfo dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
		return dateTimeFormat.Calendar.GetWeekOfYear(date, dateTimeFormat.CalendarWeekRule, dateTimeFormat.FirstDayOfWeek);
	}

	public static long WeekOfYear(DateTime? date)
	{
		if (!date.HasValue)
		{
			return 0L;
		}
		DateTimeFormatInfo dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
		return dateTimeFormat.Calendar.GetWeekOfYear(date.Value, dateTimeFormat.CalendarWeekRule, dateTimeFormat.FirstDayOfWeek);
	}

	public static long WeekOfYear(DateTime date, DayOfWeek firstDayOfWeek)
	{
		DateTimeFormatInfo dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
		return dateTimeFormat.Calendar.GetWeekOfYear(date, dateTimeFormat.CalendarWeekRule, firstDayOfWeek);
	}

	public static long WeekOfYear(DateTime? date, DayOfWeek firstDayOfWeek)
	{
		if (!date.HasValue)
		{
			return 0L;
		}
		DateTimeFormatInfo dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
		return dateTimeFormat.Calendar.GetWeekOfYear(date.Value, dateTimeFormat.CalendarWeekRule, firstDayOfWeek);
	}

	public static long WeekOfYear(DateTime date, DayOfWeek firstDayOfWeek, CalendarWeekRule calendarWeekRule)
	{
		DateTimeFormatInfo dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
		return dateTimeFormat.Calendar.GetWeekOfYear(date, calendarWeekRule, firstDayOfWeek);
	}

	public static long WeekOfYear(DateTime? date, DayOfWeek firstDayOfWeek, CalendarWeekRule calendarWeekRule)
	{
		if (!date.HasValue)
		{
			return 0L;
		}
		DateTimeFormatInfo dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
		return dateTimeFormat.Calendar.GetWeekOfYear(date.Value, calendarWeekRule, firstDayOfWeek);
	}

	public static long WeekOfMonth(DateTime date)
	{
		return WeekOfYear(date) - WeekOfYear(new DateTime(date.Year, date.Month, 1)) + 1;
	}

	public static long WeekOfMonth(DateTime? date)
	{
		if (!date.HasValue)
		{
			return 0L;
		}
		return WeekOfMonth(date.Value);
	}

	public static long WeekOfMonth(DateTime date, DayOfWeek firstDayOfWeek)
	{
		return WeekOfYear(date, firstDayOfWeek) - WeekOfYear(new DateTime(date.Year, date.Month, 1), firstDayOfWeek) + 1;
	}

	public static long WeekOfMonth(DateTime? date, DayOfWeek firstDayOfWeek)
	{
		if (!date.HasValue)
		{
			return 0L;
		}
		return WeekOfMonth(date.Value, firstDayOfWeek);
	}

	public static long WeekOfMonth(DateTime date, DayOfWeek firstDayOfWeek, CalendarWeekRule calendarWeekRule)
	{
		long num = WeekOfYear(new DateTime(date.Year, date.Month, 1), firstDayOfWeek, calendarWeekRule);
		long num2 = WeekOfYear(date, firstDayOfWeek, calendarWeekRule);
		if (date.Month == 1 && num > 10)
		{
			num = ((num > num2) ? 1 : WeekOfYear(new DateTime(date.Year - 1, 12, 1), firstDayOfWeek, calendarWeekRule));
		}
		return num2 - num + 1;
	}

	public static long WeekOfMonth(DateTime? date, DayOfWeek firstDayOfWeek, CalendarWeekRule calendarWeekRule)
	{
		if (!date.HasValue)
		{
			return 0L;
		}
		return WeekOfMonth(date.Value, firstDayOfWeek, calendarWeekRule);
	}

	public static DateTime FromOADate(double value)
	{
		return DateTime.FromOADate(value);
	}

	public static double ToOADate(DateTime value)
	{
		return value.ToOADate();
	}
}
