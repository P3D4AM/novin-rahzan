using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;

namespace Stimulsoft.Report.CodeDom;

public class StiCodeDomExpressionHelper
{
	private static StringBuilder ParseRtf(StringBuilder str, StiRichText richText)
	{
		if (richText != null && !richText.FullConvertExpression)
		{
			string text = str.ToString();
			bool flag = false;
			int num = text.LastIndexOf("__LP__", StringComparison.InvariantCulture);
			if (num == -1)
			{
				num = 0;
			}
			for (int num2 = str.Length - 1; num2 > num; num2--)
			{
				if (str[num2] == ' ')
				{
					flag = true;
					break;
				}
				if (str[num2] == '\\')
				{
					break;
				}
			}
			if (!flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append('"');
				stringBuilder.Append((object?)str);
				stringBuilder.Append(' ');
				stringBuilder.Append('"');
				return stringBuilder;
			}
		}
		if (str.Length > 0)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append('"');
			stringBuilder2.Append((object?)str);
			stringBuilder2.Append('"');
			return stringBuilder2;
		}
		return new StringBuilder();
	}

	public static void ReadString(StiCodeGenerator codeGenerator, ref int pos, ref StringBuilder lexem, string script, ref List<string> al, bool isRichText, bool fullRtf)
	{
		lexem = lexem.Append(script[pos++]);
		StringBuilder stringBuilder = new StringBuilder();
		while (pos != script.Length && script[pos] != '"')
		{
			if (script[pos] == '\\' && pos + 1 != script.Length)
			{
				if (script[pos + 1] == '\\')
				{
					stringBuilder.Append("\\\\");
					pos += 2;
					continue;
				}
				if (script[pos + 1] == '"')
				{
					pos++;
				}
			}
			stringBuilder.Append(script[pos++]);
		}
		string text;
		if (codeGenerator != null)
		{
			stringBuilder = ReplaceBackslash(stringBuilder, isRichText, fullRtf);
			text = codeGenerator.QuoteSnippetString(stringBuilder.ToString());
		}
		else
		{
			text = stringBuilder.ToString();
		}
		if (text.StartsWith("\"", StringComparison.InvariantCulture) && text.EndsWith("\"", StringComparison.InvariantCulture) && text.Length > 1)
		{
			lexem = lexem.Append(text.Substring(1, text.Length - 2));
		}
		else
		{
			lexem = lexem.Append(text);
		}
		if (pos != script.Length)
		{
			lexem = lexem.Append(script[pos]);
			pos++;
		}
		if (pos == script.Length)
		{
			if (lexem.Length > 0)
			{
				al.Add(lexem.ToString());
			}
			lexem = new StringBuilder();
		}
		pos--;
	}

	private static void ReadChar(ref int pos, ref StringBuilder lexem, string script)
	{
		lexem = lexem.Append(script[pos++]);
		if (pos != script.Length)
		{
			lexem = lexem.Append(script[pos++]);
		}
		if (pos != script.Length)
		{
			lexem = lexem.Append(script[pos]);
		}
	}

	private static void AddLexem(int pos, int startPos, StiRichText richText, RichTextBox rich, string script, List<string> al, StiCodeGenerator codeGenerator)
	{
		if (pos > startPos)
		{
			string text;
			if (richText != null)
			{
				((TextBoxBase)rich).SelectionStart = startPos;
				((TextBoxBase)rich).SelectionLength = pos - startPos;
				text = rich.SelectedRtf;
			}
			else
			{
				text = script.Substring(startPos, pos - startPos);
			}
			if (codeGenerator != null)
			{
				text = codeGenerator.QuoteSnippetString(text);
			}
			al.Add(text);
		}
	}

	public static List<string> GetLexemSimple(StiCodeGenerator codeGenerator, string script, StiRichText richText)
	{
		List<string> al = new List<string>();
		StringBuilder lexem = new StringBuilder();
		bool flag = false;
		for (int i = 0; i < script.Length; i++)
		{
			if (!flag)
			{
				if (script[i] == '{')
				{
					flag = true;
					lexem = ParseRtf(lexem, richText);
					if (lexem.Length > 0)
					{
						string text = lexem.ToString().Substring(1, lexem.Length - 2);
						if (codeGenerator != null)
						{
							text = codeGenerator.QuoteSnippetString(text);
						}
						al.Add(text);
					}
					lexem = new StringBuilder();
				}
				else
				{
					lexem = lexem.Append(script[i]);
				}
			}
			else if (script[i] == '"')
			{
				ReadString(codeGenerator, ref i, ref lexem, script, ref al, richText != null, fullRtf: false);
			}
			else if (script[i] == '\'')
			{
				ReadChar(ref i, ref lexem, script);
			}
			else if (script[i] == '}')
			{
				flag = false;
				if (codeGenerator == null)
				{
					string value = lexem.ToString();
					lexem = new StringBuilder();
					lexem = lexem.Append("{");
					lexem = lexem.Append(value);
					lexem = lexem.Append("}");
				}
				if (lexem.Length > 0)
				{
					al.Add(lexem.ToString());
				}
				lexem = new StringBuilder();
			}
			else
			{
				lexem = lexem.Append(script[i]);
			}
		}
		if (!flag)
		{
			lexem = ParseRtf(lexem, richText);
			if (lexem.Length > 0)
			{
				string text2 = lexem.ToString().Substring(1, lexem.Length - 2);
				if (codeGenerator != null)
				{
					text2 = codeGenerator.QuoteSnippetString(text2);
				}
				al.Add(text2);
			}
		}
		else
		{
			al.Clear();
		}
		return al;
	}

	public static List<string> GetLexemFullRtf(StiCodeGenerator codeGenerator, string script, StiRichText richText)
	{
		List<string> al = new List<string>();
		RichTextBox val = null;
		if (richText != null)
		{
			val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
			val.Rtf = StiRichText.UnpackRtf(script);
			script = ((Control)val).Text;
		}
		StringBuilder lexem = new StringBuilder();
		bool flag = false;
		int i = 0;
		int num = 0;
		for (; i != script.Length; i++)
		{
			if (!flag)
			{
				if (script[i] != '{')
				{
					continue;
				}
				flag = true;
				AddLexem(i, num, richText, val, script, al, codeGenerator);
				lexem = new StringBuilder();
				num = i;
				if (val != null)
				{
					((TextBoxBase)val).SelectionStart = num;
					((TextBoxBase)val).SelectionLength = 1;
					string selectedText = ((TextBoxBase)val).SelectedText;
					if (codeGenerator != null)
					{
						al.Add(codeGenerator.QuoteSnippetString(val.SelectedRtf));
					}
					else
					{
						al.Add(val.SelectedRtf);
					}
				}
			}
			else if (script[i] == '"')
			{
				ReadString(codeGenerator, ref i, ref lexem, script, ref al, richText != null, fullRtf: true);
			}
			else if (script[i] == '\'')
			{
				ReadChar(ref i, ref lexem, script);
			}
			else if (script[i] == '}')
			{
				flag = false;
				num = i + 1;
				if (codeGenerator == null)
				{
					string value = lexem.ToString();
					lexem = new StringBuilder();
					lexem = lexem.Append("{");
					lexem = lexem.Append(value);
					lexem = lexem.Append("}");
				}
				if (lexem.Length > 0)
				{
					al.Add(lexem.ToString());
				}
				lexem = new StringBuilder();
			}
			else
			{
				lexem = lexem.Append(script[i]);
			}
		}
		if (!flag)
		{
			AddLexem(i, num, richText, val, script, al, codeGenerator);
		}
		else
		{
			al.Clear();
		}
		if (al.Count > 0)
		{
			string text = al[al.Count - 1];
			string text2 = ((text.Length > 30) ? text.Substring(text.Length - 30) : text);
			int num2 = text.LastIndexOf('}');
			if (num2 != -1 && text.StartsWith("\"{\\") && text2.IndexOf("\\\\par") == -1)
			{
				text = text.Insert(num2, "\\\\par");
				al[al.Count - 1] = text;
			}
		}
		((Component)(object)val)?.Dispose();
		return al;
	}

	public static List<string> GetLexem(string script)
	{
		return GetLexemSimple(null, script, null);
	}

	public static string ConvertToString(StiCodeGenerator codeGenerator, string script, bool fullConvert, bool onlyText, StiComponent component, bool isTextExpression)
	{
		StiRichText stiRichText = component as StiRichText;
		if (stiRichText != null)
		{
			script = script.Replace('\0', ' ');
		}
		if (onlyText)
		{
			return codeGenerator.QuoteSnippetString(script);
		}
		try
		{
			bool flag = true;
			List<string> list = ((!((stiRichText?.FullConvertExpression ?? false) && isTextExpression)) ? GetLexemSimple(codeGenerator, script, stiRichText) : GetLexemFullRtf(codeGenerator, script, stiRichText));
			StringBuilder stringBuilder = new StringBuilder(script.Length * 2);
			if (list.Count == 0)
			{
				return codeGenerator.QuoteSnippetString(script);
			}
			foreach (string item in list)
			{
				if (!flag)
				{
					stringBuilder = ((stiRichText == null || !stiRichText.FullConvertExpression) ? stringBuilder.Append(" + ") : stringBuilder.Append(" , "));
				}
				stringBuilder = ((item[0] != '"') ? ((!fullConvert) ? stringBuilder.Append(item) : ((component is StiText stiText && string.IsNullOrEmpty(stiText.ExcelValue.Value)) ? stringBuilder.Append(string.Format("ToString(sender, {0}, {1})", item, isTextExpression ? "true" : "false")) : stringBuilder.Append("ToString(" + item + ")"))) : stringBuilder.Append(item));
				flag = false;
			}
			if (stiRichText != null)
			{
				string text = stringBuilder.Replace('\0', ' ').ToString();
				if (!stiRichText.FullConvertExpression)
				{
					return text;
				}
				return $"ConvertRtf({text})";
			}
			return stringBuilder.ToString();
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(StiExpression), "ConvertToString...ERROR");
			StiLogService.Write(typeof(StiExpression), ex.Message);
			return null;
		}
	}

	private static StringBuilder ReplaceBackslash(StringBuilder input, bool isRichText, bool fullRtf)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < input.Length; i++)
		{
			if (input[i] == '\\' && i < input.Length - 1)
			{
				i++;
				char c = input[i];
				switch (c)
				{
				case '\\':
					if (isRichText && !fullRtf && i + 1 < input.Length && input[i + 1] == 'n')
					{
						stringBuilder.Append("\n");
						i++;
					}
					else
					{
						stringBuilder.Append("\\");
					}
					break;
				case '\'':
					if (isRichText)
					{
						stringBuilder.Append("\\'");
					}
					else
					{
						stringBuilder.Append('\'');
					}
					break;
				case '0':
					stringBuilder.Append('\0');
					break;
				case 'n':
					stringBuilder.Append('\n');
					break;
				case 'r':
					stringBuilder.Append('\r');
					break;
				case 't':
					stringBuilder.Append('\t');
					break;
				case 'x':
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					int num = 0;
					while (i < input.Length - 1 && num < 4 && "0123456789abcdefABCDEF".IndexOf(input[i + 1]) != -1)
					{
						stringBuilder2.Append(input[i + 1]);
						i++;
						num++;
					}
					int result = 0;
					bool flag = int.TryParse(stringBuilder2.ToString(), NumberStyles.HexNumber, null, out result);
					stringBuilder.Append((char)result);
					break;
				}
				default:
					stringBuilder.Append("\\" + c);
					break;
				}
			}
			else
			{
				stringBuilder.Append(input[i]);
			}
		}
		return stringBuilder;
	}
}
