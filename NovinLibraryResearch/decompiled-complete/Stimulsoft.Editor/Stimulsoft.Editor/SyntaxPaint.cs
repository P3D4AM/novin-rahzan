using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class SyntaxPaint : ISyntaxPaint
{
	private bool disableColorPaint;

	private bool disableSyntaxPaint;

	private ILexer lexer;

	protected Control control;

	protected CustomDrawEventArgs customDrawEventArgs;

	protected DrawInfo drawInfo;

	protected IPainter painter;

	[Description("Gets or sets object that can make lexical analysis for the control's content.")]
	public virtual ILexer Lexer
	{
		get
		{
			return lexer;
		}
		set
		{
			lexer = value;
		}
	}

	[Description("Gets or sets a value indicating whether control uses colors to paint its content.")]
	public virtual bool DisableColorPaint
	{
		get
		{
			return disableColorPaint;
		}
		set
		{
			disableColorPaint = value;
		}
	}

	[Description("Gets or sets a value indicating whether Edit control uses lexical colors/styles to paint its content.")]
	public virtual bool DisableSyntaxPaint
	{
		get
		{
			return disableSyntaxPaint;
		}
		set
		{
			disableSyntaxPaint = value;
		}
	}

	[Description("Occurs when control draws its content.")]
	public event CustomDrawEvent CustomDraw;

	public SyntaxPaint(IPainter painter, Control control)
	{
		this.painter = painter;
		this.control = control;
		customDrawEventArgs = new CustomDrawEventArgs();
		drawInfo = default(DrawInfo);
	}

	private int MeasureTextFragment(string line, int style, int startChar, int endChar, int width, out int chars, bool measureChars, bool exact)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		ColorFlags state = ColorFlags.None;
		ILexStyle lexStyle = GetLexStyle(style, ref state);
		chars = 0;
		if (lexStyle != null && !disableSyntaxPaint)
		{
			painter.FontStyle = GetFontStyle(lexStyle.FontStyle, state);
		}
		else if (control != null)
		{
			painter.FontStyle = GetFontStyle(control.Font.Style, state);
		}
		if (measureChars)
		{
			return painter.StringWidth(line, startChar, endChar - startChar + 1, width, out chars, exact);
		}
		return painter.StringWidth(line, startChar, endChar - startChar + 1);
	}

	private int DrawTextFragment(Point position, string line, int style, int index, int startChar, int endChar)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		ColorFlags state = ColorFlags.None;
		ILexStyle lexStyle = GetLexStyle(style, ref state);
		if (disableSyntaxPaint)
		{
			lexStyle = null;
		}
		string text = ((startChar == 0 && endChar == -1) ? line : line.Substring(startChar, endChar - startChar + 1));
		if (lexStyle != null)
		{
			painter.FontStyle = GetFontStyle(lexStyle.FontStyle, state);
			painter.TextColor = GetFontColor(lexStyle.ForeColor, state);
			if (lexStyle.BackColor != Color.Empty)
			{
				painter.BackColor = lexStyle.BackColor;
			}
			else if (control != null)
			{
				painter.BackColor = control.BackColor;
			}
		}
		else if (control != null)
		{
			painter.FontStyle = GetFontStyle(control.Font.Style, state);
			painter.TextColor = GetFontColor(control.ForeColor, state);
			painter.BackColor = control.BackColor;
		}
		int num = painter.StringWidth(text);
		Rectangle rect = new Rectangle(position.X, position.Y, num, painter.FontHeight);
		DrawState state2 = DrawState.Text;
		drawInfo.Init();
		drawInfo.Text = text;
		drawInfo.Style = (short)style;
		drawInfo.Line = index;
		drawInfo.Char = startChar;
		if (!OnCustomDraw(painter, rect, DrawStage.Before, state2, drawInfo))
		{
			painter.TextOut(text, -1, rect, clipped: true, opaque: false);
		}
		OnCustomDraw(painter, rect, DrawStage.After, state2, drawInfo);
		return num;
	}

	protected internal virtual ILexStyle GetLexStyle(int style, ref ColorFlags state)
	{
		if (style < 0)
		{
			return null;
		}
		byte b = (byte)style;
		state = (ColorFlags)(style >> 8);
		if (b == 0 || Lexer == null)
		{
			return null;
		}
		if (b <= 0 || b - 1 >= Lexer.Scheme.LexStyles.Count)
		{
			return null;
		}
		return Lexer.Scheme.LexStyles[b - 1];
	}

	protected virtual void GetString(int index, ref string line, ref short[] colorData)
	{
	}

	protected virtual bool EqualStates(ColorFlags state1, ColorFlags state2)
	{
		return state1 == state2;
	}

	protected virtual bool EqualLineStyles(LineStyle style1, LineStyle style2, int style, int oldStyle)
	{
		if (style1 == style2)
		{
			return true;
		}
		ColorFlags state = ColorFlags.None;
		ColorFlags state2 = ColorFlags.None;
		GetLexStyle(style, ref state);
		GetLexStyle(style, ref state2);
		if ((state & ColorFlags.OutlineSection) == 0 && (state & ColorFlags.CodeSnippet) == 0 && (state2 & ColorFlags.OutlineSection) == 0)
		{
			return (state2 & ColorFlags.CodeSnippet) != 0;
		}
		return true;
	}

	public virtual FontStyle GetFontStyle(FontStyle fontStyle, ColorFlags state)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return fontStyle;
	}

	public virtual Color GetFontColor(Color color, ColorFlags state)
	{
		if (!disableColorPaint || control == null)
		{
			if (!(color != Color.Empty))
			{
				return painter.TextColor;
			}
			return color;
		}
		return control.ForeColor;
	}

	public virtual Color GetBackColor(Color color, ColorFlags state)
	{
		if (!(color != Color.Empty))
		{
			return painter.BackColor;
		}
		return color;
	}

	public bool EqualStyles(int style1, int style2, bool useColors)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		ColorFlags state = ColorFlags.None;
		ColorFlags state2 = ColorFlags.None;
		ILexStyle lexStyle = GetLexStyle(style1, ref state);
		ILexStyle lexStyle2 = GetLexStyle(style2, ref state2);
		if (disableSyntaxPaint)
		{
			lexStyle = null;
			lexStyle2 = null;
		}
		if (EqualStates(state, state2))
		{
			if (lexStyle != lexStyle2)
			{
				if (lexStyle != null && lexStyle2 != null && lexStyle.FontStyle == lexStyle2.FontStyle)
				{
					if (useColors && !disableColorPaint)
					{
						if (lexStyle.ForeColor == lexStyle2.ForeColor)
						{
							return lexStyle.BackColor == lexStyle2.BackColor;
						}
						return false;
					}
					return true;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public virtual void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint)
	{
		int fontHeight = painter.FontHeight;
		for (int i = startLine; i < endLine; i++)
		{
			DrawLine(i, position, rect);
			position.Y += fontHeight;
			if (position.Y > rect.Bottom || (position.Y == rect.Bottom && specialPaint))
			{
				break;
			}
		}
	}

	public virtual void DrawLine(int index, Point position, Rectangle clipRect)
	{
		string line = string.Empty;
		short[] colorData = null;
		GetString(index, ref line, ref colorData);
		DrawLine(index, line, colorData, position, clipRect);
	}

	public virtual void DrawLine(int index, string line, short[] colorData, Point position, Rectangle clipRect)
	{
		int right = clipRect.Right;
		int length = line.Length;
		if (!(line != string.Empty))
		{
			return;
		}
		int num = (ushort)colorData[0];
		int num2 = num;
		int num3 = 0;
		for (int i = 1; i < length; i++)
		{
			num = (ushort)colorData[i];
			if (!EqualStyles(num2, num, useColors: true))
			{
				position.X += DrawTextFragment(position, line, num2, index, num3, i - 1);
				if (position.X >= right)
				{
					break;
				}
				num2 = num;
				num3 = i;
			}
		}
		if (num3 < length && position.X < right)
		{
			position.X += DrawTextFragment(position, line, num2, index, num3, length - 1);
		}
	}

	public virtual int MeasureLine(string line, short[] colorData, int pos, int len, int width, out int chars, bool measureChars, bool addSpace, bool exact)
	{
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		chars = 0;
		if (measureChars)
		{
			if (width == int.MaxValue)
			{
				chars = int.MaxValue;
				return int.MaxValue;
			}
			if (len == int.MaxValue)
			{
				return int.MaxValue;
			}
		}
		else if (len == int.MaxValue)
		{
			return int.MaxValue;
		}
		if (line == string.Empty)
		{
			if (!measureChars)
			{
				return painter.CharWidth(' ', len);
			}
			return painter.CharWidth(' ', width, out chars);
		}
		int num = 0;
		int chars2 = 0;
		int num2 = ((len < 0) ? (line.Length - pos) : Math.Min(len, line.Length - pos));
		if (Lexer == null || painter.IsMonoSpaced)
		{
			if (!measureChars)
			{
				num = ((num2 < 0) ? painter.StringWidth(line) : painter.StringWidth(line, 0, num2));
				if (len > num2)
				{
					num += painter.CharWidth(' ', len - num2);
				}
				return num;
			}
			num = painter.StringWidth(line, width, out chars, exact);
		}
		else
		{
			int num3 = (ushort)colorData[0];
			int num4 = num3;
			int num5 = pos;
			for (int i = pos + 1; i < pos + num2; i++)
			{
				num3 = (ushort)colorData[i];
				if (num3 != num4 && !EqualStyles(num4, num3, useColors: false))
				{
					num += MeasureTextFragment(line, num4, num5, i - 1, width - num, out chars2, measureChars, exact);
					chars += chars2;
					num4 = num3;
					if (measureChars && (width < num || chars2 < i - num5))
					{
						width = -1;
						break;
					}
					num5 = i;
				}
			}
			if (num5 < pos + num2 && ((measureChars && width > num) || !measureChars))
			{
				num += MeasureTextFragment(line, num4, num5, pos + num2 - 1, width - num, out chars2, measureChars, exact);
				chars += chars2;
				if (measureChars && chars2 < pos + num2 - num5)
				{
					width = -1;
				}
			}
			if (!measureChars && len > num2)
			{
				ColorFlags state = ColorFlags.None;
				ILexStyle lexStyle = GetLexStyle(num4, ref state);
				if (lexStyle != null)
				{
					painter.FontStyle = GetFontStyle(lexStyle.FontStyle, state);
					num += painter.CharWidth(' ', len - num2);
				}
				else
				{
					if (control != null)
					{
						painter.FontStyle = GetFontStyle(control.Font.Style, state);
					}
					num += painter.CharWidth(' ', len - num2);
				}
			}
		}
		if (measureChars && addSpace && width > num)
		{
			num += painter.CharWidth(' ', width - num, out chars2);
			chars += chars2;
		}
		return num;
	}

	public virtual int MeasureLine(string line, short[] colorData, int pos, int len, int width, out int chars, bool measureChars)
	{
		return MeasureLine(line, colorData, pos, len, width, out chars, measureChars, addSpace: true, exact: true);
	}

	public virtual int MeasureLine(int index, int pos, int len)
	{
		string line = string.Empty;
		short[] colorData = null;
		GetString(index, ref line, ref colorData);
		int chars;
		return MeasureLine(line, colorData, pos, len, -1, out chars, measureChars: false);
	}

	public virtual int MeasureLine(int index, int pos, int len, int width, out int chars, bool exact)
	{
		string line = string.Empty;
		short[] colorData = null;
		GetString(index, ref line, ref colorData);
		return MeasureLine(line, colorData, pos, len, width, out chars, measureChars: true, addSpace: true, exact);
	}

	public virtual int MeasureLine(string line, short[] colorData, int pos, int len)
	{
		int chars;
		return MeasureLine(line, colorData, pos, len, -1, out chars, measureChars: false);
	}

	public virtual bool OnCustomDraw(IPainter painter, Rectangle rect, DrawStage stage, DrawState state, DrawInfo info)
	{
		if (this.CustomDraw != null)
		{
			customDrawEventArgs.DrawStage = stage;
			customDrawEventArgs.DrawState = state;
			customDrawEventArgs.DrawInfo = info;
			customDrawEventArgs.Painter = painter;
			customDrawEventArgs.Rect = rect;
			customDrawEventArgs.Handled = false;
			this.CustomDraw(this, customDrawEventArgs);
			return customDrawEventArgs.Handled;
		}
		return false;
	}

	public virtual void ResetDisableColorPaint()
	{
		DisableColorPaint = false;
	}

	public virtual void ResetDisableSyntaxPaint()
	{
		DisableSyntaxPaint = false;
	}
}
