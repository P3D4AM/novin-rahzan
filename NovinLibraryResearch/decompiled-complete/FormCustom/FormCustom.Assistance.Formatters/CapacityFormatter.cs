using System;
using System.Text.RegularExpressions;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.Assistance.Formatters;

public class CapacityFormatter : IFormatProvider, ICustomFormatter
{
	private Regex _formatParser;

	public CapacityFormatter()
	{
		_formatParser = new Regex("c", RegexOptions.Compiled);
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
		if (arg is decimal?)
		{
			long? number = (long?)arg;
			return _formatParser.Replace(format, GetMatchEvaluator(number));
		}
		if (arg is float? num)
		{
			long? number2 = (long?)num;
			return _formatParser.Replace(format, GetMatchEvaluator(number2));
		}
		if (arg is double? num2)
		{
			long? number3 = (long?)num2;
			return _formatParser.Replace(format, GetMatchEvaluator(number3));
		}
		if (arg is int? num3)
		{
			long? number4 = num3;
			return _formatParser.Replace(format, GetMatchEvaluator(number4));
		}
		if (arg is short? num4)
		{
			long? number5 = num4;
			return _formatParser.Replace(format, GetMatchEvaluator(number5));
		}
		long? number6 = arg?.ToString()?.ParseToLong(0L);
		return _formatParser.Replace(format, GetMatchEvaluator(number6));
	}

	private MatchEvaluator GetMatchEvaluator(long? number)
	{
		return (Match m) => EvaluateMatch(m, number);
	}

	private string EvaluateMatch(Match match, long? number)
	{
		try
		{
			if (!number.HasValue)
			{
				return null;
			}
			if (match.Value == "c")
			{
				bool num = number.Value < 0;
				number = Math.Abs(number.Value);
				long? num2 = number;
				long value = number.Value;
				if (num2.HasValue)
				{
					num2.GetValueOrDefault();
				}
				if (num)
				{
					return "0";
				}
				if (number < 1024)
				{
					return number + "Bytes";
				}
				if (number < 1048576)
				{
					return Math.Round((double)number.Value / 1024.0, 2).ToString().Replace(".", "/")
						.Replace("/00", "") + "Kb";
				}
				return Math.Round((double)number.Value / 1048576.0, 2).ToString().Replace(".", "/")
					.Replace("/00", "") + "Mb";
			}
			return match.Value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EvaluateMatch", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Assistance\\Formatters\\CapacityFormatter.cs", 104);
			throw;
		}
	}
}
