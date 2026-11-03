using System;
using System.Text.RegularExpressions;

namespace FormCustom.Assistance.Formatters;

public class TimeSpanFormatter : IFormatProvider, ICustomFormatter
{
	private Regex _formatParser;

	public TimeSpanFormatter()
	{
		_formatParser = new Regex("hhmm|d{1,2}|h{1,2}|m{1,2}|s{1,2}|f{1,7}|npc", RegexOptions.Compiled);
	}

	public object GetFormat(Type formatType)
	{
		if (typeof(ICustomFormatter).Equals(formatType))
		{
			return this;
		}
		return null;
	}

	public string Format(string format, object arg, IFormatProvider formatProvider)
	{
		if (arg is TimeSpan? timeSpan)
		{
			return _formatParser.Replace(format, GetMatchEvaluator(timeSpan));
		}
		if (arg is IFormattable formattable)
		{
			return formattable.ToString(format, formatProvider);
		}
		if (arg == null)
		{
			return string.Empty;
		}
		return arg.ToString();
	}

	private MatchEvaluator GetMatchEvaluator(TimeSpan? timeSpan)
	{
		return (Match m) => EvaluateMatch(m, timeSpan);
	}

	private string replacePersianNumbers(string input)
	{
		return input.Replace("0", "٠").Replace("1", "۱").Replace("2", "۲")
			.Replace("3", "٣")
			.Replace("4", "۴")
			.Replace("5", "٥")
			.Replace("6", "۶")
			.Replace("7", "٧")
			.Replace("8", "۸")
			.Replace("9", "۹");
	}

	private string EvaluateMatch(Match match, TimeSpan? timeSpan)
	{
		switch (match.Value)
		{
		case "npc":
			if ((timeSpan?.TotalSeconds ?? 0.0) < 1.0)
			{
				return "";
			}
			if (timeSpan.Value.TotalSeconds < 60.0)
			{
				return $"{timeSpan.Value.Seconds} ثانیه";
			}
			if (timeSpan.Value.TotalMinutes < 60.0)
			{
				return $"{timeSpan.Value.Minutes} دقیقه";
			}
			if (timeSpan.Value.TotalHours > 0.0)
			{
				return $"{(int)timeSpan.Value.TotalHours} ساعت و {timeSpan.Value.Minutes} دقیقه";
			}
			return match.Value;
		case "dd":
			return timeSpan.Value.Days.ToString("00");
		case "hhmm":
			return $"{timeSpan.Value.Hours:00}:{timeSpan.Value.Minutes:00}";
		case "d":
			return timeSpan.Value.Days.ToString("0");
		case "hh":
			return timeSpan.Value.Hours.ToString("00");
		case "h":
			return timeSpan.Value.Hours.ToString("0");
		case "mm":
			return timeSpan.Value.Minutes.ToString("00");
		case "m":
			return timeSpan.Value.Minutes.ToString("0");
		case "ss":
			return timeSpan.Value.Seconds.ToString("00");
		case "s":
			return timeSpan.Value.Seconds.ToString("0");
		case "fffffff":
			return (timeSpan.Value.Milliseconds * 10000).ToString("0000000");
		case "ffffff":
			return (timeSpan.Value.Milliseconds * 1000).ToString("000000");
		case "fffff":
			return (timeSpan.Value.Milliseconds * 100).ToString("00000");
		case "ffff":
			return (timeSpan.Value.Milliseconds * 10).ToString("0000");
		case "fff":
			return timeSpan.Value.Milliseconds.ToString("000");
		case "ff":
			return (timeSpan.Value.Milliseconds / 10).ToString("00");
		case "f":
			return (timeSpan.Value.Milliseconds / 100).ToString("0");
		default:
			return match.Value;
		}
	}
}
