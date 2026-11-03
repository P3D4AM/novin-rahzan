using System;
using System.Text.RegularExpressions;
using PacketParser;

namespace FormCustom.Assistance.Formatters;

public class FloatFormatter : IFormatProvider, ICustomFormatter
{
	private Regex _formatParser;

	public FloatFormatter()
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
		if (arg is decimal? number)
		{
			return _formatParser.Replace(format, GetMatchEvaluator(number));
		}
		if (arg is float? num)
		{
			decimal? number2 = (decimal?)num;
			return _formatParser.Replace(format, GetMatchEvaluator(number2));
		}
		if (arg is double? num2)
		{
			decimal? number3 = (decimal?)num2;
			return _formatParser.Replace(format, GetMatchEvaluator(number3));
		}
		if (arg is int? num3)
		{
			decimal? number4 = num3;
			return _formatParser.Replace(format, GetMatchEvaluator(number4));
		}
		if (arg is short? num4)
		{
			decimal? number5 = num4;
			return _formatParser.Replace(format, GetMatchEvaluator(number5));
		}
		decimal? number6 = arg?.ToString()?.ParseToDecimal(0m, 10);
		return _formatParser.Replace(format, GetMatchEvaluator(number6));
	}

	private MatchEvaluator GetMatchEvaluator(decimal? number)
	{
		return (Match m) => EvaluateMatch(m, number);
	}

	private string EvaluateMatch(Match match, decimal? number)
	{
		if (!number.HasValue)
		{
			return null;
		}
		if (match.Value == "d")
		{
			bool num = number.Value < 0m;
			number = Math.Abs(number.Value);
			decimal? num2 = number - (decimal?)Math.Floor(number.Value);
			decimal? num3;
			decimal num4;
			if (num)
			{
				num3 = num2;
				num4 = 0.00000001m;
				if ((num3.GetValueOrDefault() > num4) & num3.HasValue)
				{
					return $"-{Math.Floor(number.Value):N0}/" + num2.ToString().Replace("0.", "");
				}
				return $"-{Math.Floor(number.Value):N0}";
			}
			num3 = num2;
			num4 = 0.00000001m;
			if ((num3.GetValueOrDefault() > num4) & num3.HasValue)
			{
				return $"{Math.Floor(number.Value):N0}/" + num2.ToString().Replace("0.", "");
			}
			return $"{Math.Floor(number.Value):N0}";
		}
		return match.Value;
	}
}
