using System.Collections.Generic;

namespace ExcelDataReader.Core.NumberFormat;

internal class DecimalSection
{
	public bool ThousandSeparator { get; set; }

	public double ThousandDivisor { get; set; }

	public double PercentMultiplier { get; set; }

	public List<string> BeforeDecimal { get; set; }

	public bool DecimalSeparator { get; set; }

	public List<string> AfterDecimal { get; set; }

	public static bool TryParse(List<string> tokens, out DecimalSection format)
	{
		if (Parser.ParseNumberTokens(tokens, 0, out var beforeDecimal, out var decimalSeparator, out var afterDecimal) == tokens.Count)
		{
			bool thousandSeparator;
			double trailingCommasDivisor = GetTrailingCommasDivisor(tokens, out thousandSeparator);
			double percentMultiplier = GetPercentMultiplier(tokens);
			format = new DecimalSection
			{
				BeforeDecimal = beforeDecimal,
				DecimalSeparator = decimalSeparator,
				AfterDecimal = afterDecimal,
				PercentMultiplier = percentMultiplier,
				ThousandDivisor = trailingCommasDivisor,
				ThousandSeparator = thousandSeparator
			};
			return true;
		}
		format = null;
		return false;
	}

	private static double GetPercentMultiplier(List<string> tokens)
	{
		foreach (string token in tokens)
		{
			if (token == "%")
			{
				return 100.0;
			}
		}
		return 1.0;
	}

	private static double GetTrailingCommasDivisor(List<string> tokens, out bool thousandSeparator)
	{
		bool flag = false;
		double num = 1.0;
		for (int i = 0; i < tokens.Count; i++)
		{
			int num2 = tokens.Count - 1 - i;
			string text = tokens[num2];
			if (!flag)
			{
				if (!Token.IsPlaceholder(text))
				{
					continue;
				}
				for (int j = num2 + 1; j < tokens.Count; j++)
				{
					text = tokens[j];
					if (!(text == ","))
					{
						break;
					}
					num *= 1000.0;
				}
				flag = true;
			}
			else if (text == ",")
			{
				thousandSeparator = true;
				return num;
			}
		}
		thousandSeparator = false;
		return num;
	}
}
