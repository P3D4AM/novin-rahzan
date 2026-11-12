using System;
using System.Text.RegularExpressions;
using System.Threading;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base;

public static class StringExt
{
	public static bool TryParseDateTime(this string value, out DateTime dateTime)
	{
		dateTime = DateTime.Now;
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		if (value.TryParseJsonDateTime(out dateTime))
		{
			return true;
		}
		if (DateTime.TryParse(value, out dateTime))
		{
			return true;
		}
		DateTimeRoutines.DateTimeFormat defaultFormat = ((!(Thread.CurrentThread.CurrentCulture.Name == "en-US")) ? DateTimeRoutines.DateTimeFormat.UK_DATE : DateTimeRoutines.DateTimeFormat.USA_DATE);
		if (DateTimeRoutines.TryParseDateTime(value, defaultFormat, out dateTime))
		{
			return true;
		}
		return false;
	}

	private static bool TryParseJsonDateTime(this string value, out DateTime dateTime)
	{
		dateTime = DateTime.Now;
		try
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			if (!value.Replace(" ", "").Contains("Date("))
			{
				return false;
			}
			if (value.TryParseJsonDateTimeInNewDate(out dateTime))
			{
				return true;
			}
			if (value.StartsWithInvariant("Date("))
			{
				value = "\"\\/" + value + "\\/\"";
			}
			else if (value.StartsWithInvariant("/Date("))
			{
				value = "\"\\" + value + "/\"";
			}
			else if (value.StartsWithInvariant("\\/Date("))
			{
				value = "\"" + value + "\"";
			}
			DateTime? dateTime2 = JsonConvert.DeserializeObject<DateTime?>(value);
			if (!dateTime2.HasValue)
			{
				return false;
			}
			dateTime = dateTime2.Value;
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static bool TryParseJsonDateTimeInNewDate(this string value, out DateTime dateTime)
	{
		dateTime = DateTime.Now;
		try
		{
			string text = value.Replace(" ", "").ToLowerInvariant();
			if (!text.StartsWithInvariantIgnoreCase("newdate("))
			{
				return false;
			}
			int num = text.IndexOf("(", StringComparison.Ordinal) + 1;
			if (num == -1)
			{
				return false;
			}
			int num2 = text.IndexOf(")", num, StringComparison.Ordinal);
			if (num2 == -1)
			{
				return false;
			}
			text = text.Substring(num, num2 - num);
			if (!long.TryParse(text, out var result))
			{
				return false;
			}
			dateTime = new DateTime(1970, 1, 1).AddTicks(result * 10000);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static string ToLowerNull(string str)
	{
		if (!string.IsNullOrWhiteSpace(str))
		{
			return str.ToLowerInvariant();
		}
		return str;
	}

	public static bool IsBase64String(this string s)
	{
		s = s.Trim().Replace("\r", string.Empty).Replace("\n", string.Empty);
		if (s.Length % 4 == 0)
		{
			return Regex.IsMatch(s, "^[a-zA-Z0-9\\+/]*={0,3}$", RegexOptions.None);
		}
		return false;
	}

	public static bool StartsWithInvariantIgnoreCase(this string str, string check)
	{
		return str.StartsWith(check, StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool EndsWithInvariantIgnoreCase(this string str, string check)
	{
		return str.EndsWith(check, StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool StartsWithInvariant(this string str, string check)
	{
		return str.StartsWith(check, StringComparison.InvariantCulture);
	}

	public static bool EndsWithInvariant(this string str, string check)
	{
		return str.EndsWith(check, StringComparison.InvariantCulture);
	}

	public static int IndexOfInvariant(this string str, string value)
	{
		return str.IndexOf(value, StringComparison.InvariantCulture);
	}

	public static int IndexOfInvariant(this string str, string value, int startIndex)
	{
		return str.IndexOf(value, startIndex, StringComparison.InvariantCulture);
	}

	public static string ToEmpty(this string str)
	{
		return str ?? string.Empty;
	}
}
