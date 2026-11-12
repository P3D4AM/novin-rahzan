using System;
using System.Globalization;

namespace Stimulsoft.Base.Helpers;

public static class DateTimeExt
{
	public static string ToMonthName(this DateTime dateTime)
	{
		return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
	}

	public static string ToShortMonthName(this DateTime dateTime)
	{
		return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
	}
}
