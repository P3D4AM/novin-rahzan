using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class HintSyntaxPaint : HtmlSyntaxPaint
{
	private class HintItem : StrItem
	{
		public int Width;

		public HintItem(string s)
			: base(s)
		{
		}
	}

	private ArrayList strings;

	private int selectedIndex;

	private string nameStr = string.Empty;

	private const int drawTop = 1;

	private const int drawBottom = 4;

	private const int drawLeft = 2;

	private const int arrowWidth = 9;

	private const int arrowHeight = 11;

	private const int arrowTop = 3;

	private const int arrowBottom = 6;

	[Description("Represents an array of strings displayed in a hint.")]
	public ArrayList Strings => strings;

	public HintSyntaxPaint(IPainter painter, Control control)
		: base(painter, control)
	{
		strings = new ArrayList();
	}

	protected override void AddLine(string s, short[] data)
	{
		bool flag = strings.Count == 0;
		if (s == string.Empty)
		{
			strings.Add(new HintItem(string.Empty));
			return;
		}
		int num = 0;
		int length = s.Length;
		int num2 = EditConsts.MaxHintWindowWidth;
		if (flag)
		{
			num2 -= MeasureArrows();
		}
		while (num < length)
		{
			int chars;
			int width = MeasureLine(s, data, num, length, num2, out chars, measureChars: true, addSpace: false, exact: true);
			if (num + chars < length)
			{
				for (int num3 = chars; num3 > 0; num3--)
				{
					if (IsDelimiter(s, num + num3 - 1))
					{
						chars = num3;
						break;
					}
				}
			}
			if (chars <= 0)
			{
				chars = 1;
			}
			chars = Math.Min(chars, length - num);
			HintItem hintItem = new HintItem(s.Substring(num, chars));
			hintItem.Width = width;
			if (data != null)
			{
				Array.Copy(data, num, hintItem.ColorData, 0, chars);
			}
			strings.Add(hintItem);
			num += chars;
			num2 = EditConsts.MaxHintWindowWidth;
		}
	}

	protected virtual void TextChanged()
	{
		strings.Clear();
		if (base.Provider != null && selectedIndex >= 0 && selectedIndex < base.Provider.Count)
		{
			AddMultiLine(GetNameStr(), (Lexer != null) ? Lexer.DefaultState : 0, useLexer: true);
			string description = ((CodeCompletionProvider)base.Provider).GetDescription(selectedIndex);
			if (description != null && description != string.Empty)
			{
				AddMultiLine(description, (Lexer != null) ? Lexer.DefaultState : 0, useLexer: true);
			}
		}
	}

	private int MeasureArrows()
	{
		if (NeedArrows())
		{
			string text = string.Format(StringConsts.NofMstr, selectedIndex + 1, base.Provider.Count);
			return painter.StringWidth(text) + 18 + 8;
		}
		return 0;
	}

	private void DrawArrows()
	{
		string text = string.Format(StringConsts.NofMstr, selectedIndex + 1, base.Provider.Count);
		int num = painter.StringWidth(text);
		painter.BackColor = EditConsts.DefaultArrowBackColor;
		painter.FillRectangle(2, 3, 9, 11);
		int num2 = 6 + num + 9;
		painter.FillRectangle(num2, 3, 9, 11);
		Point point = new Point(3, 9);
		Point point2 = new Point(6, 6);
		Point point3 = new Point(9, 9);
		Point[] points = new Point[3] { point, point2, point3 };
		painter.DrawPolygon(points, EditConsts.DefaultArrowForeColor);
		point = new Point(num2 + 1, 7);
		point2 = new Point(num2 + 4, 10);
		point3 = new Point(num2 + 9 - 2, 7);
		Point[] points2 = new Point[3] { point, point2, point3 };
		painter.DrawPolygon(points2, EditConsts.DefaultArrowForeColor);
		painter.TextOut(text, text.Length, 13, 2);
	}

	protected internal bool NeedArrows()
	{
		if (base.Provider != null)
		{
			return base.Provider.Count > 1;
		}
		return false;
	}

	protected internal Rectangle GetLeftArrowRect()
	{
		return new Rectangle(2, 3, 9, 11);
	}

	protected internal string GetNameStr()
	{
		string text = string.Empty;
		if (base.Provider != null && selectedIndex >= 0 && selectedIndex < base.Provider.Count)
		{
			if (base.Provider.ColumnCount == 0)
			{
				text = ((CodeCompletionProvider)base.Provider).GetName(selectedIndex);
			}
			else
			{
				text = string.Empty;
				for (int i = 0; i < base.Provider.ColumnCount; i++)
				{
					if (base.Provider.ColumnVisible(i))
					{
						string columnText = base.Provider.GetColumnText(selectedIndex, i);
						if (columnText != null && columnText != string.Empty)
						{
							text = ((!(text == string.Empty)) ? (text + " " + columnText) : columnText);
						}
					}
				}
			}
		}
		return text;
	}

	protected override void GetString(int index, ref string line, ref short[] colorData)
	{
		HintItem hintItem = strings[index] as HintItem;
		line = hintItem.String;
		colorData = hintItem.ColorData;
	}

	protected internal Size UpdateSize()
	{
		TextChanged();
		int num = 0;
		for (int i = 0; i < strings.Count; i++)
		{
			int num2 = ((HintItem)strings[i]).Width;
			if (i == 0)
			{
				num2 += MeasureArrows();
			}
			num = Math.Max(num, num2);
		}
		return new Size(num + 6, (painter.FontHeight + 4) * Math.Max(strings.Count, 1) + 1);
	}

	protected internal void ProviderChanged(ICodeCompletionProvider provider, int index)
	{
		base.Provider = provider;
		selectedIndex = index;
	}

	protected internal bool UpdateHint(ICodeCompletionProvider provider, int index)
	{
		string text = GetNameStr();
		if (nameStr != text)
		{
			nameStr = text;
			return true;
		}
		return false;
	}

	public override void DrawLine(int index, string line, short[] colorData, Point position, Rectangle clipRect)
	{
		if (index == 0 && NeedArrows())
		{
			DrawArrows();
		}
		base.DrawLine(index, line, colorData, position, clipRect);
	}

	public override void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint)
	{
		painter.BackColor = EditConsts.DefaultInfoBackColor;
		painter.FontStyle = (FontStyle)0;
		painter.FillRectangle(0, 0, rect.Width, rect.Height);
		if (base.Provider == null)
		{
			return;
		}
		painter.TextColor = control.ForeColor;
		painter.Opaque = false;
		int num = 0;
		int num2 = 1;
		for (int i = 0; i < strings.Count; i++)
		{
			if (i == 0 && base.Provider.Count > 1)
			{
				DrawArrows();
				num = MeasureArrows() + 2;
			}
			else
			{
				num = 2;
			}
			DrawLine(i, new Point(num, num2), rect);
			num2 += painter.FontHeight + 4;
		}
	}
}
