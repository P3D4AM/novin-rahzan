using System.Drawing;
using System.Globalization;

namespace Stimulsoft.Editor;

public class HtmlImporter : FmtImporter
{
	private const string htmlB = "b";

	private const string htmlI = "i";

	private const string htmlU = "u";

	private const string htmlStrike = "strike";

	private const string htmlStrong = "strong";

	private const string htmlFont = "font";

	private const string htmlColor = "color";

	private const string htmlBr = "br";

	private const string htmlP = "p";

	private const string htmlLt = "&lt;";

	private const string htmlQt = "&gt;";

	private const string htmlAmp = "&amp;";

	private const string htmlQuot = "&quot;";

	private const string htmlNbsp = "&nbsp;";

	private SyntaxParser parser;

	public HtmlImporter()
	{
		parser = new HtmlParser();
	}

	private string HtmlToString(string text)
	{
		return text.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&")
			.Replace("&quot;", "\"")
			.Replace("&nbsp;", " ");
	}

	private int StringToHex(string text, int pos, int len)
	{
		if (text.Length >= pos + len)
		{
			try
			{
				return int.Parse(text.Substring(pos, len), NumberStyles.HexNumber);
			}
			catch
			{
				return 0;
			}
		}
		return 0;
	}

	private Color HtmlToColor(string text)
	{
		if (text != string.Empty)
		{
			if (text[0] == '#')
			{
				return Color.FromArgb(StringToHex(text, 1, 2), StringToHex(text, 3, 2), StringToHex(text, 5, 2));
			}
			return Color.FromName(text);
		}
		return Color.Empty;
	}

	protected void WriteText(string text)
	{
		WriteString(HtmlToString(text));
	}

	protected void ProcessFontTag()
	{
		while (!parser.Eof)
		{
			switch ((HtmlLexerToken)parser.NextToken())
			{
			case HtmlLexerToken.ParamName:
				if (parser.TokenString.ToLower(CultureInfo.InvariantCulture) == "color" && parser.NextToken() == 7 && parser.NextToken() == 10)
				{
					WriteForeColor(HtmlToColor(parser.TokenString));
				}
				break;
			case HtmlLexerToken.Body:
			case HtmlLexerToken.OpenTag:
			case HtmlLexerToken.CloseTag:
			case HtmlLexerToken.OpenEndTag:
			case HtmlLexerToken.CloseEndTag:
				return;
			}
		}
	}

	protected void ProcessTag(bool negate)
	{
		if (parser.NextToken() != 8)
		{
			return;
		}
		string text = parser.TokenString.ToLower(CultureInfo.InvariantCulture);
		if (!negate)
		{
			Push(text);
			switch (text)
			{
			case "br":
			case "p":
				WriteString("\r\n");
				break;
			case "b":
			case "strong":
				WriteFontStyle((FontStyle)1, value: true);
				break;
			case "i":
				WriteFontStyle((FontStyle)2, value: true);
				break;
			case "strike":
				WriteFontStyle((FontStyle)8, value: true);
				break;
			case "u":
				WriteFontStyle((FontStyle)4, value: true);
				break;
			case "font":
				ProcessFontTag();
				break;
			}
		}
		else
		{
			Pop(text);
		}
	}

	protected override void ReadContent()
	{
		base.ReadContent();
		parser.Strings = new StringList(reader);
		parser.Reset();
		int num = 0;
		int num2 = 0;
		while (!parser.Eof)
		{
			switch (parser.NextToken())
			{
			case 5:
				ProcessTag(negate: true);
				break;
			case 3:
				ProcessTag(negate: false);
				break;
			case 2:
				if (num != parser.CurrentPosition.Y && num2 != 6 && num2 != 4)
				{
					WriteText(' ' + parser.TokenString);
				}
				else
				{
					WriteText(parser.TokenString);
				}
				break;
			}
			num2 = parser.Token;
			num = parser.CurrentPosition.Y;
		}
	}
}
