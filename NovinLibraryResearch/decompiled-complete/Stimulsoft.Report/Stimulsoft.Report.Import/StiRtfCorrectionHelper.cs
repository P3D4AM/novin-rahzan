using System;
using System.IO;
using System.Text;

namespace Stimulsoft.Report.Import;

public class StiRtfCorrectionHelper
{
	public enum StiRtfTokenType
	{
		None,
		Keyword,
		Control,
		Text,
		Eof,
		GroupStart,
		GroupEnd
	}

	public class StiRtfToken
	{
		public StiRtfTokenType Type { get; set; }

		public string Key { get; set; }

		public bool HasParameter { get; set; }

		public int Parameter { get; set; }

		public int Position { get; set; }

		public override string ToString()
		{
			return $"{Type} {Key} {(HasParameter ? Parameter.ToString() : string.Empty)}";
		}

		public StiRtfToken()
		{
			Type = StiRtfTokenType.None;
			Key = string.Empty;
		}
	}

	private TextReader rtf;

	private const int Eof = -1;

	private int position;

	public StiRtfToken NextToken(bool addText = true)
	{
		StiRtfToken stiRtfToken = new StiRtfToken();
		int num = rtf.Read();
		position++;
		while (num == 13 || num == 10 || num == 9 || num == 0)
		{
			num = rtf.Read();
			position++;
		}
		stiRtfToken.Position = position - 1;
		switch (num)
		{
		case 123:
			stiRtfToken.Type = StiRtfTokenType.GroupStart;
			break;
		case 125:
			stiRtfToken.Type = StiRtfTokenType.GroupEnd;
			break;
		case 92:
			ParseKeyword(stiRtfToken);
			break;
		default:
			stiRtfToken.Type = StiRtfTokenType.Text;
			ParseText((char)num, stiRtfToken, addText);
			break;
		case -1:
			stiRtfToken.Type = StiRtfTokenType.Eof;
			break;
		}
		return stiRtfToken;
	}

	private void ParseKeyword(StiRtfToken token)
	{
		int num = rtf.Peek();
		if (!char.IsLetter((char)num))
		{
			rtf.Read();
			position++;
			if (num == 92 || num == 123 || num == 125)
			{
				token.Type = StiRtfTokenType.Text;
				token.Key = ((char)num).ToString();
				return;
			}
			token.Type = StiRtfTokenType.Control;
			token.Key = ((char)num).ToString();
			if (token.Key == "'")
			{
				string value = $"{(char)rtf.Read()}{(char)rtf.Read()}";
				position += 2;
				token.HasParameter = true;
				token.Parameter = Convert.ToInt32(value, 16);
			}
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		num = rtf.Peek();
		while (char.IsLetter((char)num))
		{
			rtf.Read();
			position++;
			stringBuilder.Append((char)num);
			num = rtf.Peek();
		}
		token.Type = StiRtfTokenType.Keyword;
		token.Key = stringBuilder.ToString();
		if (char.IsDigit((char)num) || num == 45)
		{
			token.HasParameter = true;
			bool flag = false;
			if (num == 45)
			{
				flag = true;
				rtf.Read();
				position++;
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			num = rtf.Peek();
			while (char.IsDigit((char)num))
			{
				rtf.Read();
				position++;
				stringBuilder2.Append((char)num);
				num = rtf.Peek();
			}
			int num2 = Convert.ToInt32(stringBuilder2.ToString());
			if (flag)
			{
				num2 = -num2;
			}
			token.Parameter = num2;
		}
		if (num == 32)
		{
			rtf.Read();
			position++;
		}
	}

	private void ParseText(char ch, StiRtfToken token, bool addText)
	{
		StringBuilder stringBuilder = new StringBuilder(ch.ToString());
		int num = rtf.Peek();
		while (num == 13 || num == 10 || num == 9 || num == 0)
		{
			rtf.Read();
			position++;
			num = rtf.Peek();
		}
		while (num != 92 && num != 125 && num != 123 && num != -1)
		{
			rtf.Read();
			position++;
			if (addText)
			{
				stringBuilder.Append((char)num);
			}
			num = rtf.Peek();
			while (num == 13 || num == 10 || num == 9 || num == 0)
			{
				rtf.Read();
				position++;
				num = rtf.Peek();
			}
		}
		if (addText)
		{
			token.Key = stringBuilder.ToString();
		}
	}

	public StiRtfCorrectionHelper(TextReader rtfReader)
	{
		rtf = rtfReader;
	}
}
