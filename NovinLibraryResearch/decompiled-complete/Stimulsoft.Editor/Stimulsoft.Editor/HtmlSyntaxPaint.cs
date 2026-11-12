using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class HtmlSyntaxPaint : SyntaxPaint
{
	private ICodeCompletionProvider provider;

	private HtmlLexer internalLexer;

	public ICodeCompletionProvider Provider
	{
		get
		{
			return provider;
		}
		set
		{
			if (provider == value)
			{
				return;
			}
			provider = value;
			if (provider != null && provider.UseHtmlFormatting)
			{
				if (Lexer == null)
				{
					Lexer = internalLexer;
				}
			}
			else if (Lexer == internalLexer)
			{
				Lexer = null;
			}
		}
	}

	public HtmlSyntaxPaint(IPainter painter, Control control)
		: base(painter, control)
	{
		internalLexer = new HtmlLexer();
	}

	protected bool IsDelimiter(string s, int pos)
	{
		char c = s[pos];
		if (c > ' ')
		{
			return EditConsts.DefaultDelimiters.IndexOf(c) >= 0;
		}
		return true;
	}

	protected void AddMultiLine(string s, int state, bool useLexer)
	{
		s = s.Replace("\t", new string(' ', EditConsts.DefaultSpacesInTab));
		short[] colorData = null;
		if (useLexer && PrepareData(state, (FontStyle)0, ref s, ref colorData))
		{
			AddMultiLine(s, colorData);
			return;
		}
		int num = 0;
		string[] array = StrItem.Split(s);
		foreach (string text in array)
		{
			colorData = new short[text.Length];
			if (useLexer && Lexer != null)
			{
				state = Lexer.ParseText(state, num, text, ref colorData);
			}
			num++;
			AddLine(text, colorData);
		}
	}

	protected void AddMultiLine(string s, short[] data)
	{
		int num = 0;
		int i = 0;
		int length;
		for (length = s.Length; i < length; i++)
		{
			if (s[i] == '\r' || s[i] == '\n')
			{
				AddSubstring(s, data, num, i - num);
				if (s[i] == '\r' && i < length - 1 && s[i + 1] == '\n')
				{
					i++;
				}
				i++;
				num = i;
			}
		}
		AddSubstring(s, data, num, length - num);
	}

	protected void AddSubstring(string s, short[] data, int start, int len)
	{
		short[] array = new short[len];
		Array.Copy(data, start, array, 0, len);
		AddLine(s.Substring(start, len), array);
	}

	protected virtual void AddLine(string s, short[] data)
	{
	}

	protected internal bool PrepareData(int state, FontStyle style, ref string s, ref short[] colorData)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (Lexer is HtmlLexer)
		{
			((HtmlLexer)Lexer).ParseHtmlText(state, style, ref s, ref colorData);
		}
		else
		{
			colorData = new short[s.Length];
		}
		return Lexer is HtmlLexer;
	}
}
