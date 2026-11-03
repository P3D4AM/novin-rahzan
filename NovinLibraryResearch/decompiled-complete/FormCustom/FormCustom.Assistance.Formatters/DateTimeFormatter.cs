using System;
using System.Text.RegularExpressions;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.Assistance.Formatters;

public class DateTimeFormatter : IFormatProvider, ICustomFormatter
{
	private Regex _formatParser;

	public DateTimeFormatter()
	{
		_formatParser = new Regex("d", RegexOptions.Compiled);
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
		try
		{
			if (arg is DateTime? dateTime)
			{
				return _formatParser.Replace(format, GetMatchEvaluator(dateTime));
			}
			if (arg is IFormattable formattable)
			{
				return formattable.ToString(format, formatProvider);
			}
			return (arg != null) ? arg.ToString() : string.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Format", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Assistance\\Formatters\\DateTimeFormatter.cs", 50);
			throw;
		}
	}

	private MatchEvaluator GetMatchEvaluator(DateTime? DateTime)
	{
		return (Match m) => EvaluateMatch(m, DateTime);
	}

	private string EvaluateMatch(Match match, DateTime? date)
	{
		try
		{
			if (match.Value == "d")
			{
				return date?.TimeDifferenceToString();
			}
			return date.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EvaluateMatch", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Assistance\\Formatters\\DateTimeFormatter.cs", 72);
			throw;
		}
	}
}
