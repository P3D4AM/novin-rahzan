using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Net.Http.Headers;

internal static class DateTimeFormatter
{
	private static readonly DateTimeFormatInfo FormatInfo = CultureInfo.InvariantCulture.DateTimeFormat;

	private static readonly string[] MonthNames = FormatInfo.AbbreviatedMonthNames;

	private static readonly string[] DayNames = FormatInfo.AbbreviatedDayNames;

	private static readonly int Rfc1123DateLength = "ddd, dd MMM yyyy HH:mm:ss GMT".Length;

	private static readonly int QuotedRfc1123DateLength = Rfc1123DateLength + 2;

	private const int AsciiNumberOffset = 48;

	private const string Gmt = "GMT";

	private const char Comma = ',';

	private const char Space = ' ';

	private const char Colon = ':';

	private const char Quote = '"';

	public static string ToRfc1123String(this DateTimeOffset dateTime)
	{
		return dateTime.ToRfc1123String(quoted: false);
	}

	public static string ToRfc1123String(this DateTimeOffset dateTime, bool quoted)
	{
		DateTime utcDateTime = dateTime.UtcDateTime;
		int num = (quoted ? QuotedRfc1123DateLength : Rfc1123DateLength);
		InplaceStringBuilder target = default(InplaceStringBuilder);
		((InplaceStringBuilder)(ref target))._002Ector(num);
		if (quoted)
		{
			((InplaceStringBuilder)(ref target)).Append('"');
		}
		((InplaceStringBuilder)(ref target)).Append(DayNames[(int)utcDateTime.DayOfWeek]);
		((InplaceStringBuilder)(ref target)).Append(',');
		((InplaceStringBuilder)(ref target)).Append(' ');
		AppendNumber(ref target, utcDateTime.Day);
		((InplaceStringBuilder)(ref target)).Append(' ');
		((InplaceStringBuilder)(ref target)).Append(MonthNames[utcDateTime.Month - 1]);
		((InplaceStringBuilder)(ref target)).Append(' ');
		AppendYear(ref target, utcDateTime.Year);
		((InplaceStringBuilder)(ref target)).Append(' ');
		AppendTimeOfDay(ref target, utcDateTime.TimeOfDay);
		((InplaceStringBuilder)(ref target)).Append(' ');
		((InplaceStringBuilder)(ref target)).Append("GMT");
		if (quoted)
		{
			((InplaceStringBuilder)(ref target)).Append('"');
		}
		return ((object)(InplaceStringBuilder)(ref target)).ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AppendYear(ref InplaceStringBuilder target, int year)
	{
		((InplaceStringBuilder)(ref target)).Append(GetAsciiChar(year / 1000));
		((InplaceStringBuilder)(ref target)).Append(GetAsciiChar(year % 1000 / 100));
		((InplaceStringBuilder)(ref target)).Append(GetAsciiChar(year % 100 / 10));
		((InplaceStringBuilder)(ref target)).Append(GetAsciiChar(year % 10));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AppendTimeOfDay(ref InplaceStringBuilder target, TimeSpan timeOfDay)
	{
		AppendNumber(ref target, timeOfDay.Hours);
		((InplaceStringBuilder)(ref target)).Append(':');
		AppendNumber(ref target, timeOfDay.Minutes);
		((InplaceStringBuilder)(ref target)).Append(':');
		AppendNumber(ref target, timeOfDay.Seconds);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AppendNumber(ref InplaceStringBuilder target, int number)
	{
		((InplaceStringBuilder)(ref target)).Append(GetAsciiChar(number / 10));
		((InplaceStringBuilder)(ref target)).Append(GetAsciiChar(number % 10));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static char GetAsciiChar(int value)
	{
		return (char)(48 + value);
	}
}
