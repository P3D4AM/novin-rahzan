using System.Drawing;
using System.IO;

namespace Stimulsoft.Editor;

public class HtmlLexer : Lexer
{
	private class HtmlItem : StrItem
	{
		public HtmlItem(string s)
			: base(s)
		{
		}

		public void Add(string text, short style)
		{
			int num = base.ColorData.Length;
			short[] array = new short[num + text.Length];
			if (num > 0)
			{
				base.ColorData.CopyTo(array, 0);
			}
			String += text;
			for (int i = 0; i < text.Length; i++)
			{
				array[num + i] = style;
			}
			base.ColorData = array;
		}
	}

	private FmtImporter importer;

	public HtmlLexer()
	{
		importer = new HtmlImporter();
		importer.ReadFormattedText += ReadFormattedText;
	}

	protected int GetStyle(Color foreColor, Color backColor, FontStyle fontStyle)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		foreach (ILexStyle lexStyle3 in Scheme.LexStyles)
		{
			if (lexStyle3.ForeColor == foreColor && lexStyle3.BackColor == backColor && lexStyle3.FontStyle == fontStyle)
			{
				return lexStyle3.Index + 1;
			}
		}
		ILexStyle lexStyle2 = Scheme.LexStyles.AddLexStyle();
		lexStyle2.ForeColor = foreColor;
		lexStyle2.BackColor = backColor;
		lexStyle2.FontStyle = fontStyle;
		return lexStyle2.Index + 1;
	}

	protected void ReadFormattedText(object sender, ReadFormattedTextEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (e.UserData is HtmlItem)
		{
			((HtmlItem)e.UserData).Add(e.Text, (short)GetStyle(e.ForeColor, e.BackColor, e.FontStyle));
		}
	}

	public int ParseHtmlText(int state, FontStyle style, ref string s, ref short[] colorData)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		StringReader reader = new StringReader(s);
		HtmlItem htmlItem = new HtmlItem(string.Empty);
		importer.DefaultStyle = style;
		importer.Read(null, reader, htmlItem);
		s = htmlItem.String;
		colorData = htmlItem.ColorData;
		return state;
	}
}
