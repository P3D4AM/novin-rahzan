using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Stimulsoft.Base.Helpers;

public static class DateTimeRoutines
{
	public enum DateTimeFormat
	{
		USA_DATE,
		UK_DATE
	}

	public class ParsedDateTime
	{
		public readonly int IndexOfDate;

		public readonly int LengthOfDate;

		public readonly int IndexOfTime;

		public readonly int LengthOfTime;

		public readonly DateTime DateTime;

		public readonly bool IsDateFound;

		public readonly bool IsTimeFound;

		public readonly TimeSpan UtcOffset;

		public readonly bool IsUtcOffsetFound;

		public DateTime UtcDateTime;

		internal ParsedDateTime(int indexOfDate, int lengthOfDate, int indexOfTime, int lengthOfTime, DateTime dateTime)
		{
			IndexOfDate = indexOfDate;
			LengthOfDate = lengthOfDate;
			IndexOfTime = indexOfTime;
			LengthOfTime = lengthOfTime;
			DateTime = dateTime;
			IsDateFound = indexOfDate > -1;
			IsTimeFound = indexOfTime > -1;
			UtcOffset = new TimeSpan(25, 0, 0);
			IsUtcOffsetFound = false;
			UtcDateTime = new DateTime(1, 1, 1);
		}

		internal ParsedDateTime(int indexOfDate, int lengthOfDate, int indexOfTime, int lengthOfTime, DateTime dateTime, TimeSpan utcOffset)
		{
			IndexOfDate = indexOfDate;
			LengthOfDate = lengthOfDate;
			IndexOfTime = indexOfTime;
			LengthOfTime = lengthOfTime;
			DateTime = dateTime;
			IsDateFound = indexOfDate > -1;
			IsTimeFound = indexOfTime > -1;
			UtcOffset = utcOffset;
			IsUtcOffsetFound = Math.Abs(utcOffset.TotalHours) < 12.0;
			if (!IsUtcOffsetFound)
			{
				UtcDateTime = new DateTime(1, 1, 1);
			}
			else if (indexOfDate < 0)
			{
				TimeSpan timeSpan = dateTime.TimeOfDay + utcOffset;
				UtcDateTime = ((timeSpan < new TimeSpan(0L)) ? (new DateTime(1, 1, 2) + timeSpan) : (new DateTime(1, 1, 1) + timeSpan));
			}
			else
			{
				UtcDateTime = dateTime + utcOffset;
			}
		}
	}

	private static DateTime defaultDate = DateTime.Now;

	public static DateTime DefaultDate
	{
		get
		{
			if (!DefaultDateIsNow)
			{
				return defaultDate;
			}
			return DateTime.Now;
		}
		set
		{
			defaultDate = value;
			DefaultDateIsNow = false;
		}
	}

	public static bool DefaultDateIsNow { get; set; } = true;


	public static bool TryParseDateTime(string str, DateTimeFormat defaultFormat, out DateTime dateTime)
	{
		if (TryParseDateTime(str, defaultFormat, out ParsedDateTime parsedDateTime))
		{
			dateTime = parsedDateTime.DateTime;
			return true;
		}
		dateTime = new DateTime(1, 1, 1);
		return false;
	}

	public static bool TryParseDateOrTime(string str, DateTimeFormat defaultFormat, out DateTime dateTime)
	{
		if (TryParseDateOrTime(str, defaultFormat, out ParsedDateTime parsedDateTime))
		{
			dateTime = parsedDateTime.DateTime;
			return true;
		}
		dateTime = new DateTime(1, 1, 1);
		return false;
	}

	public static bool TryParseTime(string str, DateTimeFormat defaultFormat, out DateTime time)
	{
		if (TryParseTime(str, defaultFormat, out var parsedTime, null))
		{
			time = parsedTime.DateTime;
			return true;
		}
		time = new DateTime(1, 1, 1);
		return false;
	}

	public static bool TryParseDate(string str, DateTimeFormat defaultFormat, out DateTime date)
	{
		if (TryParseDate(str, defaultFormat, out ParsedDateTime parsedDate))
		{
			date = parsedDate.DateTime;
			return true;
		}
		date = new DateTime(1, 1, 1);
		return false;
	}

	public static bool TryParseDateTime(string str, DateTimeFormat defaultFormat, out ParsedDateTime parsedDateTime)
	{
		if (TryParseDateOrTime(str, defaultFormat, out parsedDateTime))
		{
			return true;
		}
		parsedDateTime = null;
		return false;
	}

	public static bool TryParseTime(string str, DateTimeFormat defaultFormat, out ParsedDateTime parsedTime)
	{
		return TryParseTime(str, defaultFormat, out parsedTime, null);
	}

	public static bool TryParseDateOrTime(string str, DateTimeFormat defaultFormat, out ParsedDateTime parsedDateTime)
	{
		parsedDateTime = null;
		ParsedDateTime parsedTime;
		if (TryParseDate(str, defaultFormat, out ParsedDateTime parsedDate))
		{
			if (!TryParseTime(str, defaultFormat, out parsedTime, parsedDate))
			{
				DateTime dateTime = parsedDate.DateTime;
				int year = dateTime.Year;
				dateTime = parsedDate.DateTime;
				int month = dateTime.Month;
				dateTime = parsedDate.DateTime;
				DateTime dateTime2 = new DateTime(year, month, dateTime.Day, 0, 0, 0);
				parsedDateTime = new ParsedDateTime(parsedDate.IndexOfDate, parsedDate.LengthOfDate, -1, -1, dateTime2);
			}
			else
			{
				DateTime dateTime = parsedDate.DateTime;
				int year2 = dateTime.Year;
				dateTime = parsedDate.DateTime;
				int month2 = dateTime.Month;
				dateTime = parsedDate.DateTime;
				int day = dateTime.Day;
				dateTime = parsedTime.DateTime;
				int hour = dateTime.Hour;
				dateTime = parsedTime.DateTime;
				int minute = dateTime.Minute;
				dateTime = parsedTime.DateTime;
				DateTime dateTime3 = new DateTime(year2, month2, day, hour, minute, dateTime.Second);
				parsedDateTime = new ParsedDateTime(parsedDate.IndexOfDate, parsedDate.LengthOfDate, parsedTime.IndexOfTime, parsedTime.LengthOfTime, dateTime3, parsedTime.UtcOffset);
			}
		}
		else
		{
			if (!TryParseTime(str, defaultFormat, out parsedTime, null))
			{
				return false;
			}
			int year3 = DefaultDate.Year;
			int month3 = DefaultDate.Month;
			int day2 = DefaultDate.Day;
			DateTime dateTime = parsedTime.DateTime;
			int hour2 = dateTime.Hour;
			dateTime = parsedTime.DateTime;
			int minute2 = dateTime.Minute;
			dateTime = parsedTime.DateTime;
			DateTime dateTime4 = new DateTime(year3, month3, day2, hour2, minute2, dateTime.Second);
			parsedDateTime = new ParsedDateTime(-1, -1, parsedTime.IndexOfTime, parsedTime.LengthOfTime, dateTime4, parsedTime.UtcOffset);
		}
		return true;
	}

	public static bool TryParseTime(string str, DateTimeFormat defaultFormat, out ParsedDateTime parsedTime, ParsedDateTime parsedDate)
	{
		parsedTime = null;
		string text = ((defaultFormat == DateTimeFormat.USA_DATE) ? "(?:\\s*(?'time_zone'UTC|GMT|CST|EST))?" : "(?:\\s*(?'time_zone'UTC|GMT))?");
		Match match;
		if (parsedDate != null && parsedDate.IndexOfDate > -1)
		{
			match = Regex.Match(str.Substring(parsedDate.IndexOfDate + parsedDate.LengthOfDate), "(?<=^\\s*,?\\s+|^\\s*at\\s*|^\\s*[T\\-]\\s*)(?'hour'\\d{2})\\s*:\\s*(?'minute'\\d{2})\\s*:\\s*(?'second'\\d{2})\\s+(?'offset_sign'[\\+\\-])(?'offset_hh'\\d{2}):?(?'offset_mm'\\d{2})(?=$|[^\\d\\w])", RegexOptions.Compiled);
			if (!match.Success)
			{
				match = Regex.Match(str.Substring(parsedDate.IndexOfDate + parsedDate.LengthOfDate), "(?<=^\\s*,?\\s+|^\\s*at\\s*|^\\s*[T\\-]\\s*)(?'hour'\\d{1,2})\\s*:\\s*(?'minute'\\d{2})\\s*(?::\\s*(?'second'\\d{2}))?(?:\\s*(?'ampm'AM|am|PM|pm))?" + text + "(?=$|[^\\d\\w])", RegexOptions.Compiled);
			}
			if (!match.Success)
			{
				match = Regex.Match(str.Substring(0, parsedDate.IndexOfDate), "(?<=^|[^\\d])(?'hour'\\d{1,2})\\s*:\\s*(?'minute'\\d{2})\\s*(?::\\s*(?'second'\\d{2}))?(?:\\s*(?'ampm'AM|am|PM|pm))?" + text + "(?=$|[\\s,]+)", RegexOptions.Compiled);
			}
			if (!match.Success)
			{
				match = Regex.Match(str.Substring(parsedDate.IndexOfDate, parsedDate.LengthOfDate), "(?<=^|[^\\d])(?'hour'\\d{1,2})\\s*:\\s*(?'minute'\\d{2})\\s*(?::\\s*(?'second'\\d{2}))?(?:\\s*(?'ampm'AM|am|PM|pm))?" + text + "(?=$|[\\s,]+)", RegexOptions.Compiled);
			}
		}
		else
		{
			match = Regex.Match(str, "(?<=^|\\s+|\\s*T\\s*)(?'hour'\\d{2})\\s*:\\s*(?'minute'\\d{2})\\s*:\\s*(?'second'\\d{2})\\s+(?'offset_sign'[\\+\\-])(?'offset_hh'\\d{2}):?(?'offset_mm'\\d{2})?(?=$|[^\\d\\w])", RegexOptions.Compiled);
			if (!match.Success)
			{
				match = Regex.Match(str, "(?<=^|\\s+|\\s*T\\s*)(?'hour'\\d{1,2})\\s*:\\s*(?'minute'\\d{2})\\s*(?::\\s*(?'second'\\d{2}))?(?:\\s*(?'ampm'AM|am|PM|pm))?" + text + "(?=$|[^\\d\\w])", RegexOptions.Compiled);
			}
		}
		if (!match.Success)
		{
			return false;
		}
		int num = int.Parse(match.Groups["hour"].Value);
		if (num < 0 || num > 23)
		{
			return false;
		}
		int num2 = int.Parse(match.Groups["minute"].Value);
		if (num2 < 0 || num2 > 59)
		{
			return false;
		}
		int num3 = 0;
		if (!string.IsNullOrEmpty(match.Groups["second"].Value))
		{
			num3 = int.Parse(match.Groups["second"].Value);
			if (num3 < 0 || num3 > 59)
			{
				return false;
			}
		}
		if (string.Compare(match.Groups["ampm"].Value, "PM", StringComparison.OrdinalIgnoreCase) == 0 && num < 12)
		{
			num += 12;
		}
		else if (string.Compare(match.Groups["ampm"].Value, "AM", StringComparison.OrdinalIgnoreCase) == 0 && num == 12)
		{
			num -= 12;
		}
		DateTime dateTime = new DateTime(1, 1, 1, num, num2, num3);
		if (match.Groups["offset_hh"].Success)
		{
			int hours = int.Parse(match.Groups["offset_hh"].Value);
			int minutes = 0;
			if (match.Groups["offset_mm"].Success)
			{
				minutes = int.Parse(match.Groups["offset_mm"].Value);
			}
			TimeSpan timeSpan = new TimeSpan(hours, minutes, 0);
			if (match.Groups["offset_sign"].Value == "-")
			{
				timeSpan = -timeSpan;
			}
			parsedTime = new ParsedDateTime(-1, -1, match.Index, match.Length, dateTime, timeSpan);
			return true;
		}
		if (match.Groups["time_zone"].Success)
		{
			TimeSpan utcOffset;
			switch (match.Groups["time_zone"].Value)
			{
			case "UTC":
			case "GMT":
				utcOffset = new TimeSpan(0, 0, 0);
				break;
			case "CST":
				utcOffset = new TimeSpan(-6, 0, 0);
				break;
			case "EST":
				utcOffset = new TimeSpan(-5, 0, 0);
				break;
			default:
				throw new Exception("Time zone: " + match.Groups["time_zone"].Value + " is not defined.");
			}
			parsedTime = new ParsedDateTime(-1, -1, match.Index, match.Length, dateTime, utcOffset);
			return true;
		}
		parsedTime = new ParsedDateTime(-1, -1, match.Index, match.Length, dateTime);
		return true;
	}

	public static bool TryParseDate(string str, DateTimeFormat defaultFormat, out ParsedDateTime parsedDate)
	{
		parsedDate = null;
		if (string.IsNullOrEmpty(str))
		{
			return false;
		}
		Match match = Regex.Match(str, "(?<=^|[^\\d])(?'day'\\d{1,2})\\s*(?'separator'[\\\\/\\.])+\\s*(?'month'\\d{1,2})\\s*\\'separator'+\\s*(?'year'\\d{2}|\\d{4})(?=$|[^\\d])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		if (match.Success)
		{
			DateTime date;
			if ((defaultFormat ^ DateTimeFormat.USA_DATE) == DateTimeFormat.USA_DATE)
			{
				if (!ConvertToDate(int.Parse(match.Groups["year"].Value), int.Parse(match.Groups["day"].Value), int.Parse(match.Groups["month"].Value), out date))
				{
					return false;
				}
			}
			else if (!ConvertToDate(int.Parse(match.Groups["year"].Value), int.Parse(match.Groups["month"].Value), int.Parse(match.Groups["day"].Value), out date))
			{
				return false;
			}
			parsedDate = new ParsedDateTime(match.Index, match.Length, -1, -1, date);
			return true;
		}
		match = Regex.Match(str, "(?<=^|[^\\d])(?'year'\\d{2}|\\d{4})\\s*(?'separator'[\\-])\\s*(?'month'\\d{1,2})\\s*\\'separator'+\\s*(?'day'\\d{1,2})(?=$|[^\\d])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		if (match.Success)
		{
			if (!ConvertToDate(int.Parse(match.Groups["year"].Value), int.Parse(match.Groups["month"].Value), int.Parse(match.Groups["day"].Value), out var date2))
			{
				return false;
			}
			parsedDate = new ParsedDateTime(match.Index, match.Length, -1, -1, date2);
			return true;
		}
		string[] monthList = (from n in Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames.ToList()
			where !string.IsNullOrWhiteSpace(n)
			select (n.Length <= 3) ? n : n.Substring(0, 3) into n
			select n.Substring(0, 1).ToUpperInvariant() + n.Substring(1, n.Length - 1).ToLowerInvariant()).ToArray();
		if (TryParseDateInternal(str, ref parsedDate, monthList))
		{
			return true;
		}
		monthList = (from n in Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthNames.ToList()
			where !string.IsNullOrWhiteSpace(n)
			select n.Substring(0, 1).ToUpperInvariant() + n.Substring(1, n.Length - 1).ToLowerInvariant()).ToArray();
		return TryParseDateInternal(str, ref parsedDate, monthList);
	}

	private static bool TryParseDateInternal(string str, ref ParsedDateTime parsedDate, string[] monthList)
	{
		string text = string.Join("|", monthList);
		Match match = Regex.Match(str, "(?:^|[^\\d\\w])(?'month'" + text + ")[uarychilestmbro]*\\s+(?'day'\\d{1,2})(?:-?st|-?th|-?rd|-?nd)?\\s*,?\\s*(?'year'\\d{4})(?=$|[^\\d\\w])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		if (!match.Success)
		{
			match = Regex.Match(str, "(?:^|[^\\d\\w:])(?'day'\\d{1,2})(?:-?st\\s+|-?th\\s+|-?rd\\s+|-?nd\\s+|-|\\s+)(?'month'" + text + ")[uarychilestmbro]*(?:\\s*,?\\s*|-)'?(?'year'\\d{2}|\\d{4})(?=$|[^\\d\\w])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
		if (!match.Success)
		{
			match = Regex.Match(str, "(?:^|[^\\d\\w])(?'year'\\d{4})\\s+(?'month'" + text + ")[uarychilestmbro]*\\s+(?'day'\\d{1,2})(?:-?st|-?th|-?rd|-?nd)?(?=$|[^\\d\\w])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
		if (!match.Success)
		{
			match = Regex.Match(str, "(?:^|[^\\d\\w])(?'month'" + text + ")[uarychilestmbro]*\\s+(?'day'\\d{1,2})\\s+\\d{2}\\:\\d{2}\\:\\d{2}\\s+(?:MDT|UTC)\\s+(?'year'\\d{4})(?=$|[^\\d\\w])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
		if (!match.Success)
		{
			match = Regex.Match(str, "(?:^|[^\\d\\w])(?'month'" + text + ")[uarychilestmbro]*\\s+(?'day'\\d{1,2})(?:-?st|-?th|-?rd|-?nd)?(?:\\s*,?\\s*(?'year'\\d{4}))?(?=$|[^\\d\\w])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
		if (match.Success)
		{
			int index = match.Index;
			int length = match.Length;
			string monthStr = match.Groups["month"].Value.ToLowerInvariant();
			string text2 = monthList.FirstOrDefault((string mm) => mm.ToLowerInvariant() == monthStr);
			if (text2 == null)
			{
				return false;
			}
			int month = Array.IndexOf(monthList, text2) + 1;
			int year = ((!string.IsNullOrEmpty(match.Groups["year"].Value)) ? int.Parse(match.Groups["year"].Value) : DefaultDate.Year);
			if (!ConvertToDate(year, month, int.Parse(match.Groups["day"].Value), out var date))
			{
				return false;
			}
			parsedDate = new ParsedDateTime(index, length, -1, -1, date);
			return true;
		}
		return false;
	}

	private static bool ConvertToDate(int year, int month, int day, out DateTime date)
	{
		if (year < 100)
		{
			year = ((year <= 30) ? (year + 2000) : (year + 1900));
		}
		else if (year < 1000)
		{
			date = new DateTime(1, 1, 1);
			return false;
		}
		try
		{
			if (month < 1 || month > 12 || day < 1 || day > 31)
			{
				date = new DateTime(1, 1, 1);
				return false;
			}
			date = new DateTime(year, month, day);
		}
		catch
		{
			date = new DateTime(1, 1, 1);
			return false;
		}
		return true;
	}
}
