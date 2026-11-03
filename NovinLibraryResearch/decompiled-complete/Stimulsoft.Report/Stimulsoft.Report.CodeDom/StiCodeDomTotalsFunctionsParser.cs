using System;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomTotalsFunctionsParser
{
	private static bool ScanString(ref string argument, string text, ref int position)
	{
		argument += text[position++];
		while (position != text.Length && text[position] != '"')
		{
			argument += text[position++];
		}
		if (position == text.Length)
		{
			return false;
		}
		argument += text[position++];
		return true;
	}

	private static string GetArgument(ref int position, string text)
	{
		string argument = string.Empty;
		int num = 0;
		while (position < text.Length && (text[position] != ',' || num > 0))
		{
			char c = text[position];
			if (c == '(')
			{
				num++;
			}
			else if (c == ')')
			{
				if (num == 0)
				{
					break;
				}
				num--;
			}
			else if (c == '"')
			{
				if (!ScanString(ref argument, text, ref position))
				{
					return null;
				}
				continue;
			}
			argument += c;
			position++;
		}
		if (position == text.Length)
		{
			return null;
		}
		return argument;
	}

	public static string ProcessTotals(StiCodeDomSerializator serializator, string script, string parent)
	{
		try
		{
			int num = script.IndexOf("Totals.", StringComparison.InvariantCulture);
			if (num >= 0)
			{
				if (num > 0 && (char.IsLetterOrDigit(script[num - 1]) || script[num - 1] == '_'))
				{
					return script;
				}
				bool flag = true;
				while (flag)
				{
					if (num > 1 && script[num - 1] == '.' && !script.Substring(0, num).EndsWith("Stimulsoft.Report."))
					{
						num = script.IndexOf("Totals.", num + 1, StringComparison.InvariantCulture);
						if (num == -1)
						{
							break;
						}
						continue;
					}
					int num2 = num;
					num = script.IndexOf("(", num, StringComparison.InvariantCulture);
					if (num == -1)
					{
						return script;
					}
					int num3 = num2 + 7;
					string text = script.Substring(num3, num - num2 - 7);
					if (!text.Contains("Count") || text.Contains("CountDistinct"))
					{
						int i;
						for (i = num + 1; i < script.Length && char.IsWhiteSpace(script, i); i++)
						{
						}
						if (i >= script.Length)
						{
							return script;
						}
						if (script[i] == ')')
						{
							return script;
						}
						num++;
						int startIndex = num;
						string argument = GetArgument(ref num, script);
						if (argument == null)
						{
							return script;
						}
						if (num < script.Length && script[num] == ')' && text != "CountAllLevels")
						{
							string text2 = null;
							StiComponent componentByName = serializator.report.GetComponentByName(parent);
							if (componentByName != null)
							{
								StiComponent groupHeaderBand = componentByName.GetGroupHeaderBand();
								if (groupHeaderBand != null)
								{
									text2 = groupHeaderBand.Name;
								}
							}
							if (text2 == null)
							{
								text2 = ((serializator.report.ScriptLanguage == StiReportLanguageType.CSharp || serializator.report.ScriptLanguage == StiReportLanguageType.JS) ? "null" : "nothing");
							}
							script = script.Insert(startIndex, text2 + ",");
							num = num - argument.Length + text2.Length;
						}
						int num4 = ++num;
						argument = GetArgument(ref num, script);
						if (argument == null || argument.Trim().Length == 0)
						{
							return script;
						}
						num3 = num;
						string text3 = $"GetTotal{serializator.TotalIndex}";
						serializator.TotalIndex++;
						serializator.GenEventMethod(text3, "e.Value = " + argument, new StiGetArgumentEvent(), parent);
						StiReportLanguageType scriptLanguage = serializator.report.ScriptLanguage;
						argument = ((scriptLanguage != 0 && scriptLanguage != StiReportLanguageType.JS) ? " Me, " : " this, ");
						argument += $"\"{text3}\"";
						script = script.Remove(num4, num3 - num4);
						script = script.Insert(num4, argument);
						int num5 = num3 - num4 - argument.Length;
						num3 -= num5;
						if (text.EndsWith("SumDistinct"))
						{
							num = num3 + 1;
							num4 = num;
							argument = GetArgument(ref num, script);
							if (!string.IsNullOrWhiteSpace(argument))
							{
								num3 = num;
								text3 = "GetTotal" + serializator.TotalIndex;
								serializator.TotalIndex++;
								serializator.GenEventMethod(text3, "e.Value = " + argument, new StiGetArgumentEvent(), parent);
								argument = $"\"{text3}\"";
								script = script.Remove(num4, num3 - num4);
								script = script.Insert(num4, argument);
								num5 = num3 - num4 - argument.Length;
								num3 -= num5;
							}
						}
					}
					num = script.IndexOf("Totals.", num3, StringComparison.InvariantCulture);
					if (num == -1)
					{
						break;
					}
					flag = script.IndexOf("Totals.", num, StringComparison.InvariantCulture) > 0;
				}
				return script;
			}
			return script;
		}
		catch
		{
			return script;
		}
	}
}
