using System.Collections.Generic;
using System.Text;

namespace ExcelDataReader.Core.NumberFormat;

internal class FractionSection
{
	public List<string> IntegerPart { get; set; }

	public List<string> Numerator { get; set; }

	public List<string> DenominatorPrefix { get; set; }

	public List<string> Denominator { get; set; }

	public int DenominatorConstant { get; set; }

	public List<string> DenominatorSuffix { get; set; }

	public List<string> FractionSuffix { get; set; }

	public static bool TryParse(List<string> tokens, out FractionSection format)
	{
		List<string> list = null;
		List<string> tokens2 = null;
		for (int i = 0; i < tokens.Count; i++)
		{
			if (tokens[i] == "/")
			{
				list = tokens.GetRange(0, i);
				i++;
				tokens2 = tokens.GetRange(i, tokens.Count - i);
				break;
			}
		}
		if (list == null)
		{
			format = null;
			return false;
		}
		GetNumerator(list, out var integerPart, out var numeratorPart);
		if (!TryGetDenominator(tokens2, out var denominatorPrefix, out var denominatorPart, out var denominatorConstant, out var denominatorSuffix, out var fractionSuffix))
		{
			format = null;
			return false;
		}
		format = new FractionSection
		{
			IntegerPart = integerPart,
			Numerator = numeratorPart,
			DenominatorPrefix = denominatorPrefix,
			Denominator = denominatorPart,
			DenominatorConstant = denominatorConstant,
			DenominatorSuffix = denominatorSuffix,
			FractionSuffix = fractionSuffix
		};
		return true;
	}

	private static void GetNumerator(List<string> tokens, out List<string> integerPart, out List<string> numeratorPart)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		int num = -1;
		for (int num2 = tokens.Count - 1; num2 >= 0; num2--)
		{
			if (Token.IsPlaceholder(tokens[num2]))
			{
				flag = true;
				if (flag2)
				{
					flag3 = true;
					break;
				}
			}
			else if (flag && !flag2)
			{
				flag2 = true;
				num = num2 + 1;
			}
		}
		if (flag3)
		{
			integerPart = tokens.GetRange(0, num);
			numeratorPart = tokens.GetRange(num, tokens.Count - num);
		}
		else
		{
			integerPart = null;
			numeratorPart = tokens;
		}
	}

	private static bool TryGetDenominator(List<string> tokens, out List<string> denominatorPrefix, out List<string> denominatorPart, out int denominatorConstant, out List<string> denominatorSuffix, out List<string> fractionSuffix)
	{
		int i = 0;
		bool flag = false;
		bool flag2 = false;
		StringBuilder stringBuilder = new StringBuilder();
		for (; i < tokens.Count; i++)
		{
			string token = tokens[i];
			if (Token.IsPlaceholder(token))
			{
				flag = true;
				break;
			}
			if (Token.IsDigit19(token))
			{
				flag2 = true;
				break;
			}
		}
		if (!flag && !flag2)
		{
			denominatorPrefix = null;
			denominatorPart = null;
			denominatorConstant = 0;
			denominatorSuffix = null;
			fractionSuffix = null;
			return false;
		}
		int num = i;
		for (; i < tokens.Count; i++)
		{
			string text = tokens[i];
			if (!flag || !Token.IsPlaceholder(text))
			{
				if (!flag2 || !Token.IsDigit09(text))
				{
					break;
				}
				stringBuilder.Append(text);
			}
		}
		int num2 = tokens.Count;
		while (num2 > i && !Token.IsPlaceholder(tokens[num2 - 1]))
		{
			num2--;
		}
		if (num > 0)
		{
			denominatorPrefix = tokens.GetRange(0, num);
		}
		else
		{
			denominatorPrefix = null;
		}
		if (flag2)
		{
			denominatorConstant = int.Parse(stringBuilder.ToString());
		}
		else
		{
			denominatorConstant = 0;
		}
		denominatorPart = tokens.GetRange(num, i - num);
		if (i < num2)
		{
			denominatorSuffix = tokens.GetRange(i, num2 - i);
		}
		else
		{
			denominatorSuffix = null;
		}
		if (num2 < tokens.Count)
		{
			fractionSuffix = tokens.GetRange(num2, tokens.Count - num2);
		}
		else
		{
			fractionSuffix = null;
		}
		return true;
	}
}
