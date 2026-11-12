using System;

namespace Stimulsoft.Data.Functions;

public sealed class StiExtValueConverter
{
	public static bool IsEligable(object value, string culture)
	{
		if (string.IsNullOrWhiteSpace(culture))
		{
			return false;
		}
		if (!(value is StiMonth))
		{
			return value is DayOfWeek;
		}
		return true;
	}

	public static string Convert(object value, string culture)
	{
		if (string.IsNullOrWhiteSpace(culture))
		{
			return null;
		}
		if (value is StiMonth)
		{
			return Funcs.ToProperCase(StiMonthToStrHelper.MonthName((int)value, culture));
		}
		if (value is DayOfWeek)
		{
			return Funcs.ToProperCase(StiDayOfWeekToStrHelper.DayOfWeek((int)value, culture));
		}
		return null;
	}
}
