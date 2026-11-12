using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class HtmlParser : XmlParser
{
	protected LexerProc lexParamColorProc;

	protected override void InitLanguage()
	{
		Scheme.FileType = "html";
	}

	protected virtual int LexParamColor()
	{
		currentPos++;
		int length = source.Length;
		while (currentPos < length)
		{
			char c = source[currentPos];
			if ((c < '0' || c > '9') && (c < 'a' || c > 'f') && (c < 'A' || c > 'F'))
			{
				break;
			}
			currentPos++;
		}
		return 10;
	}

	protected override void InitLexer()
	{
		base.InitLexer();
		lexParamColorProc = LexParamColor;
		RegisterLexerProc(2, '#', lexParamColorProc);
	}

	protected override void InitStyles()
	{
		AddStyle("idents", Color.Maroon);
		AddStyle("numbers", Color.Green);
		AddStyle("reswords", Color.Red);
		AddStyle("comments", Color.Silver, (FontStyle)2, plainText: true);
		AddStyle("xml_comments", Color.Gray, (FontStyle)0, plainText: true);
		AddStyle("symbol", SystemColors.ControlText);
		AddStyle("whitespace", Color.Empty);
		AddStyle("strings", Color.Blue);
		AddStyle("directives", Color.Blue);
	}

	protected override int GetLexerStyle(int token)
	{
		if (token == 11)
		{
			return 3;
		}
		return base.GetLexerStyle(token);
	}

	public override void ResetAutoIndentChars()
	{
		base.AutoIndentChars = SyntaxConsts.DefaultCsAutoIndentChars.ToCharArray();
	}
}
