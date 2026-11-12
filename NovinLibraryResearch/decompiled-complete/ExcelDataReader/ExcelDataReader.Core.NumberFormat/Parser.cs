using System.Collections.Generic;
using System.Globalization;

namespace ExcelDataReader.Core.NumberFormat;

internal static class Parser
{
	public static Section ParseSection(Tokenizer reader, out bool syntaxError)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		Condition condition = null;
		Color color = null;
		List<string> list = new List<string>();
		syntaxError = false;
		string text;
		while ((text = ReadToken(reader, out syntaxError)) != null && !(text == ";"))
		{
			if (Token.IsDatePart(text))
			{
				flag = flag || true;
				flag2 |= Token.IsDurationPart(text);
				list.Add(text);
			}
			else if (Token.IsGeneral(text))
			{
				flag3 = flag3 || true;
				list.Add(text);
			}
			else if (text == "@")
			{
				flag4 = flag4 || true;
				list.Add(text);
			}
			else if (text.StartsWith("["))
			{
				string token = text.Substring(1, text.Length - 2);
				Color color2;
				if (TryParseCondition(token, out var result))
				{
					condition = result;
				}
				else if (TryParseColor(token, out color2))
				{
					color = color2;
				}
			}
			else
			{
				list.Add(text);
			}
		}
		if (syntaxError || list.Count == 0)
		{
			return null;
		}
		if ((flag && (flag3 || flag4)) || (flag3 && (flag || flag4)) || (flag4 && (flag3 || flag)))
		{
			syntaxError = true;
			return null;
		}
		FractionSection format = null;
		ExponentialSection format2 = null;
		DecimalSection format3 = null;
		List<string> result2 = null;
		SectionType type;
		if (flag)
		{
			if (flag2)
			{
				type = SectionType.Duration;
				result2 = list;
			}
			else
			{
				type = SectionType.Date;
				ParseDate(list, out result2);
			}
		}
		else if (flag3)
		{
			type = SectionType.General;
			result2 = list;
		}
		else if (flag4)
		{
			type = SectionType.Text;
			result2 = list;
		}
		else if (FractionSection.TryParse(list, out format))
		{
			type = SectionType.Fraction;
		}
		else if (ExponentialSection.TryParse(list, out format2))
		{
			type = SectionType.Exponential;
		}
		else
		{
			if (!DecimalSection.TryParse(list, out format3))
			{
				syntaxError = true;
				return null;
			}
			type = SectionType.Number;
		}
		return new Section
		{
			Type = type,
			Color = color,
			Condition = condition,
			Fraction = format,
			Exponential = format2,
			Number = format3,
			GeneralTextDateDurationParts = result2
		};
	}

	internal static int ParseNumberTokens(List<string> tokens, int startPosition, out List<string> beforeDecimal, out bool decimalSeparator, out List<string> afterDecimal)
	{
		beforeDecimal = null;
		afterDecimal = null;
		decimalSeparator = false;
		List<string> list = new List<string>();
		int num = 0;
		for (num = 0; num < tokens.Count; num++)
		{
			string text = tokens[num];
			if (text == "." && beforeDecimal == null)
			{
				decimalSeparator = true;
				beforeDecimal = tokens.GetRange(0, num);
				list = new List<string>();
			}
			else if (Token.IsNumberLiteral(text))
			{
				list.Add(text);
			}
			else if (!text.StartsWith("["))
			{
				break;
			}
		}
		if (list.Count > 0)
		{
			if (beforeDecimal != null)
			{
				afterDecimal = list;
			}
			else
			{
				beforeDecimal = list;
			}
		}
		return num;
	}

	private static void ParseDate(List<string> tokens, out List<string> result)
	{
		result = new List<string>();
		for (int i = 0; i < tokens.Count; i++)
		{
			string text = tokens[i];
			if (text == ".")
			{
				int num = 0;
				while (i + 1 < tokens.Count && tokens[i + 1] == "0")
				{
					i++;
					num++;
				}
				if (num > 0)
				{
					result.Add("." + new string('0', num));
				}
				else
				{
					result.Add(".");
				}
			}
			else
			{
				result.Add(text);
			}
		}
	}

	private static string ReadToken(Tokenizer reader, out bool syntaxError)
	{
		int position = reader.Position;
		if (ReadLiteral(reader) || reader.ReadEnclosed('[', ']') || reader.ReadOneOf("#?,!&%+-$€£0123456789{}():;/.@ ") || reader.ReadString("e+", ignoreCase: true) || reader.ReadString("e-", ignoreCase: true) || reader.ReadString("General", ignoreCase: true) || reader.ReadString("am/pm", ignoreCase: true) || reader.ReadString("a/p", ignoreCase: true) || reader.ReadOneOrMore(121) || reader.ReadOneOrMore(89) || reader.ReadOneOrMore(109) || reader.ReadOneOrMore(77) || reader.ReadOneOrMore(100) || reader.ReadOneOrMore(68) || reader.ReadOneOrMore(104) || reader.ReadOneOrMore(72) || reader.ReadOneOrMore(115) || reader.ReadOneOrMore(83) || reader.ReadOneOrMore(103) || reader.ReadOneOrMore(71))
		{
			syntaxError = false;
			int length = reader.Position - position;
			return reader.Substring(position, length);
		}
		syntaxError = reader.Position < reader.Length;
		return null;
	}

	private static bool ReadLiteral(Tokenizer reader)
	{
		if (reader.Peek() == 92 || reader.Peek() == 42 || reader.Peek() == 95)
		{
			reader.Advance(2);
			return true;
		}
		if (reader.ReadEnclosed('"', '"'))
		{
			return true;
		}
		return false;
	}

	private static bool TryParseCondition(string token, out Condition result)
	{
		Tokenizer tokenizer = new Tokenizer(token);
		if (tokenizer.ReadString("<=") || tokenizer.ReadString("<>") || tokenizer.ReadString("<") || tokenizer.ReadString(">=") || tokenizer.ReadString(">") || tokenizer.ReadString("="))
		{
			int position = tokenizer.Position;
			string @operator = tokenizer.Substring(0, position);
			if (ReadConditionValue(tokenizer))
			{
				string s = tokenizer.Substring(position, tokenizer.Position - position);
				result = new Condition
				{
					Operator = @operator,
					Value = double.Parse(s, CultureInfo.InvariantCulture)
				};
				return true;
			}
		}
		result = null;
		return false;
	}

	private static bool ReadConditionValue(Tokenizer tokenizer)
	{
		tokenizer.ReadString("-");
		while (tokenizer.ReadOneOf("0123456789"))
		{
		}
		if (tokenizer.ReadString("."))
		{
			while (tokenizer.ReadOneOf("0123456789"))
			{
			}
		}
		if (tokenizer.ReadString("e+", ignoreCase: true) || tokenizer.ReadString("e-", ignoreCase: true))
		{
			if (!tokenizer.ReadOneOf("0123456789"))
			{
				return false;
			}
			while (tokenizer.ReadOneOf("0123456789"))
			{
			}
		}
		return true;
	}

	private static bool TryParseColor(string token, out Color color)
	{
		Tokenizer tokenizer = new Tokenizer(token);
		if (tokenizer.ReadString("black", ignoreCase: true) || tokenizer.ReadString("blue", ignoreCase: true) || tokenizer.ReadString("cyan", ignoreCase: true) || tokenizer.ReadString("green", ignoreCase: true) || tokenizer.ReadString("magenta", ignoreCase: true) || tokenizer.ReadString("red", ignoreCase: true) || tokenizer.ReadString("white", ignoreCase: true) || tokenizer.ReadString("yellow", ignoreCase: true))
		{
			color = new Color
			{
				Value = tokenizer.Substring(0, tokenizer.Position)
			};
			return true;
		}
		color = null;
		return false;
	}
}
