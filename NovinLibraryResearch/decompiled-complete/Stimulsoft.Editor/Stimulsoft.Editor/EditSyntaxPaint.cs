using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class EditSyntaxPaint : SyntaxPaint
{
	private SyntaxEdit owner;

	private IWhiteSpace whiteSpace;

	private ISelection selection;

	private ILineStylesEx lineStyles;

	private ILineSeparator lineSeparator;

	private IBraceMatchingEx braces;

	private IOutlining outlining;

	private IHyperTextEx hyperText;

	private ISpellingEx spelling;

	private IGutter gutter;

	private IDisplayStrings displayLines;

	private Color selBackColor;

	private Color selForeColor;

	private bool drawSelection;

	private ILexStyle errorStyle;

	private ILexStyle snippetStyle;

	private Hashtable syntaxErrors;

	private ArrayList internalLineStyles;

	[Description("Represents \"SyntaxEdit\" object that owns this \"EditSyntaxPaint\".")]
	public SyntaxEdit Owner => owner;

	[Description("Gets or sets object that can make lexical analysis for the control's content.")]
	public override ILexer Lexer => owner.Lexer;

	public EditSyntaxPaint(IPainter painter, SyntaxEdit owner)
		: base(painter, (Control)(object)owner)
	{
		this.owner = owner;
		displayLines = owner.DisplayLines;
		whiteSpace = owner.WhiteSpace;
		selection = owner.Selection;
		lineStyles = owner.LineStyles;
		lineSeparator = owner.LineSeparator;
		gutter = owner.Gutter;
		braces = owner.Braces;
		outlining = owner.Outlining;
		spelling = owner.Spelling;
		hyperText = owner.HyperText;
		internalLineStyles = new ArrayList();
	}

	private void InitSyntaxErrors()
	{
		if (syntaxErrors == null)
		{
			syntaxErrors = new Hashtable();
		}
		syntaxErrors.Clear();
		TextSource textSource = (TextSource)owner.Source;
		foreach (ISyntaxError syntaxError in textSource.GetSyntaxErrors())
		{
			if (syntaxError.Position.X >= textSource.Lines.GetLength(syntaxError.Position.Y))
			{
				syntaxErrors[syntaxError.Position.Y] = true;
			}
		}
	}

	private void DrawHighlighter(int left, int top, int right, int bottom, int style, int pos, int line, string s, bool inSelection, bool afterText)
	{
		if (lineSeparator.LineColor != Color.Empty)
		{
			painter.ForeColor = lineSeparator.LineColor;
			Rectangle rect = new Rectangle(left, top, right - left, bottom - top);
			DrawState drawState = DrawState.LineHighlight;
			drawState = (afterText ? (drawState | DrawState.BeyondEol) : (drawState | DrawState.Text));
			if (inSelection)
			{
				drawState |= DrawState.Selection;
			}
			drawInfo.Init();
			drawInfo.Text = s;
			drawInfo.Line = line;
			drawInfo.Char = pos;
			drawInfo.Style = (short)style;
			if (!OnCustomDraw(painter, rect, DrawStage.Before, drawState, drawInfo))
			{
				painter.DrawLine(left, top, right, top);
				painter.DrawLine(left, bottom, right, bottom);
			}
			OnCustomDraw(painter, rect, DrawStage.After, drawState, drawInfo);
		}
	}

	private void DrawLineStyle(int left, int top, int right, int bottom, int style, int pos, int line, string s, bool inSelection, bool afterText, ILineStyle lineStyle)
	{
		if (lineStyle.PenColor != Color.Empty)
		{
			painter.ForeColor = lineStyle.PenColor;
			Rectangle rect = new Rectangle(left, top, right - left, bottom - top);
			DrawState drawState = DrawState.LineStyle;
			drawState = (afterText ? (drawState | DrawState.BeyondEol) : (drawState | DrawState.Text));
			if (inSelection)
			{
				drawState |= DrawState.Selection;
			}
			drawInfo.Init();
			drawInfo.Text = s;
			drawInfo.Line = line;
			drawInfo.Char = pos;
			drawInfo.Style = (short)style;
			if (!OnCustomDraw(painter, rect, DrawStage.Before, drawState, drawInfo))
			{
				painter.DrawLine(left, top, right, top);
				painter.DrawLine(left, bottom, right, bottom);
			}
			OnCustomDraw(painter, rect, DrawStage.After, drawState, drawInfo);
		}
	}

	private void FillRectangleAfterLineEnd(int x, int y, int w, int h, int line, bool inSelection, bool highlight, ILineStyle lineStyle)
	{
		Rectangle rect = new Rectangle(x, y, w, h);
		DrawState drawState = DrawState.BeyondEol;
		if (inSelection)
		{
			drawState |= DrawState.Selection;
		}
		drawInfo.Init();
		drawInfo.Line = line;
		if (!OnCustomDraw(painter, rect, DrawStage.Before, drawState, drawInfo))
		{
			painter.FillRectangle(x, y, w, h);
		}
		OnCustomDraw(painter, rect, DrawStage.After, drawState, drawInfo);
		if (lineStyle != null && lineStyle.PenColor != Color.Empty && (lineStyle.Options & LineStyleOptions.BeyondEol) != 0)
		{
			DrawLineStyle(x, y - 1, x + w, y + h - 1, -1, 0, line, string.Empty, inSelection, afterText: true, lineStyle);
		}
		if (highlight)
		{
			DrawHighlighter(x, y - 1, x + w, y + h - 1, -1, 0, line, string.Empty, inSelection, afterText: true);
		}
	}

	private void DrawAfterLineEnd(int w, Point position, LineStyle lineStyle, bool selLine, bool emptySel, int offset, int left, int right, int index, bool highlight, bool readOnly, bool drawColumnIndent, int start, int firstIndentChar)
	{
		if (w <= position.X)
		{
			return;
		}
		Color backColor = ((lineStyle != null && (lineStyle.Options & LineStyleOptions.BeyondEol) != 0) ? lineStyle.GetBackColor(owner.GetBackColor(readOnly)) : ((highlight && lineSeparator.HighlightBackColor != Color.Empty) ? lineSeparator.HighlightBackColor : owner.GetBackColor(readOnly)));
		bool flag = (selLine && (SelectionOptions.SelectBeyondEol & selection.Options) != 0) || selection.SelectionType == SelectionType.Block || emptySel;
		bool flag2 = (owner.Transparent || owner.InPrinting) && !highlight && lineStyle == null;
		int fontHeight = painter.FontHeight;
		if (flag)
		{
			if (emptySel)
			{
				right = left + 1;
			}
			int width = ((Gutter)gutter).GetWidth();
			int num = MeasureLine(index, 0, left);
			if (num != int.MaxValue)
			{
				num -= offset - width;
			}
			int num2 = MeasureLine(index, 0, right);
			num2 = ((num2 == int.MaxValue) ? w : (num2 - (offset - width)));
			if (num2 > position.X)
			{
				num = Math.Max(num, position.X);
				if (num > position.X && !flag2)
				{
					painter.BackColor = backColor;
					FillRectangleAfterLineEnd(position.X, position.Y, num - position.X, fontHeight, index, inSelection: false, highlight, lineStyle);
				}
				painter.BackColor = selBackColor;
				FillRectangleAfterLineEnd(num, position.Y, num2 - num, fontHeight, index, inSelection: true, highlight, lineStyle);
				if (num2 < w && !flag2)
				{
					painter.BackColor = backColor;
					FillRectangleAfterLineEnd(num2, position.Y, w - num2, fontHeight, index, inSelection: false, highlight, lineStyle);
				}
				return;
			}
		}
		if (!flag2)
		{
			painter.BackColor = backColor;
			FillRectangleAfterLineEnd(position.X, position.Y, w - position.X, painter.FontHeight, index, inSelection: false, highlight, lineStyle);
		}
		if (!drawColumnIndent)
		{
			return;
		}
		for (int i = 1; i <= firstIndentChar; i++)
		{
			if (i == owner.Lines.GetTabStop(i - 1) && (!selLine || i < left || i > right))
			{
				int x = painter.CharWidth(' ', i) + start;
				DrawColumnIndent(x, position.Y, position.Y + fontHeight);
			}
		}
	}

	private void DrawSelectionBorder(Rectangle rect, Border3DSide sides, Color borderColor)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		if (borderColor != Color.Empty && (int)sides != 0 && rect.Width >= 0)
		{
			Color foreColor = painter.ForeColor;
			painter.ForeColor = borderColor;
			if ((sides & 1) != 0)
			{
				painter.DrawLine(rect.X, rect.Y, rect.X, rect.Y + rect.Height - 1);
			}
			if ((sides & 4) != 0)
			{
				painter.DrawLine(rect.X + rect.Width - 1, rect.Y, rect.X + rect.Width - 1, rect.Y + rect.Height - 1);
			}
			if ((sides & 2) != 0)
			{
				painter.DrawLine(rect.X, rect.Y, rect.X + rect.Width - 1, rect.Y);
			}
			if ((sides & 8) != 0)
			{
				painter.DrawLine(rect.X, rect.Y + rect.Height - 1, rect.X + rect.Width - 1, rect.Y + rect.Height - 1);
			}
			painter.ForeColor = foreColor;
		}
	}

	private int GetSelectionBorder(int pos, int index)
	{
		pos = ((pos != int.MaxValue) ? owner.DisplayToScreen(pos, index).X : (((SelectionOptions.SelectBeyondEol & owner.Selection.Options) == 0 && selection.SelectionType != SelectionType.Block) ? owner.DisplayToScreen(displayLines[index].Length, index).X : owner.ClientRect.Width));
		return pos;
	}

	private bool GetSelectionBorder(int index, out int left, out int right)
	{
		left = 0;
		right = 0;
		if (index >= 0 && index < displayLines.GetCount() && selection.GetSelectionForLine(index, out left, out right) && right > left)
		{
			left = GetSelectionBorder(left, index);
			right = GetSelectionBorder(right, index);
			return right > left;
		}
		return false;
	}

	private void DrawSelectionBorder(int left, int right, int top, int index, int len)
	{
		int selectionBorder = GetSelectionBorder(left, index);
		int selectionBorder2 = GetSelectionBorder(right, index);
		if (selectionBorder2 <= selectionBorder)
		{
			return;
		}
		DrawSelectionBorder(new Rectangle(selectionBorder, top, selectionBorder2 - selectionBorder, painter.FontHeight), (Border3DSide)5, selection.BorderColor);
		if (GetSelectionBorder(index - 1, out left, out right))
		{
			if (left > selectionBorder)
			{
				left = Math.Min(left, selectionBorder2);
				DrawSelectionBorder(new Rectangle(selectionBorder, top, Math.Min(left, right) - selectionBorder, painter.FontHeight), (Border3DSide)2, selection.BorderColor);
			}
			if (right < selectionBorder2)
			{
				DrawSelectionBorder(new Rectangle(right, top, selectionBorder2 - right, painter.FontHeight), (Border3DSide)2, selection.BorderColor);
			}
		}
		else
		{
			DrawSelectionBorder(new Rectangle(selectionBorder, top, selectionBorder2 - selectionBorder, painter.FontHeight), (Border3DSide)2, selection.BorderColor);
		}
		if (GetSelectionBorder(index + 1, out left, out right))
		{
			if (left > selectionBorder)
			{
				DrawSelectionBorder(new Rectangle(selectionBorder, top, selectionBorder - left, painter.FontHeight), (Border3DSide)8, selection.BorderColor);
			}
			if (right < selectionBorder2)
			{
				right = Math.Max(right, selectionBorder);
				DrawSelectionBorder(new Rectangle(right, top, selectionBorder2 - right, painter.FontHeight), (Border3DSide)8, selection.BorderColor);
			}
		}
		else
		{
			DrawSelectionBorder(new Rectangle(selectionBorder, top, selectionBorder2 - selectionBorder, painter.FontHeight), (Border3DSide)8, selection.BorderColor);
		}
	}

	private void DrawCodeSnippets(IPainter painter)
	{
		TextSource textSource = (TextSource)owner.Source;
		if (textSource.CurrentSnippet == null || !textSource.CodeSnippets.IsFirstSnippet(textSource.CurrentSnippet))
		{
			return;
		}
		foreach (SnippetRange codeSnippet in textSource.CodeSnippets)
		{
			if (codeSnippet.Id == textSource.CurrentSnippet.Id && (codeSnippet.EndPoint.Y != codeSnippet.StartPoint.Y || codeSnippet.EndPoint.X != codeSnippet.StartPoint.X))
			{
				Point location = owner.TextToScreen(codeSnippet.StartPoint);
				Point point = owner.TextToScreen(codeSnippet.EndPoint);
				Rectangle rectangle = new Rectangle(location, new Size(point.X - location.X, point.Y - location.Y + painter.FontHeight));
				Color color = ((snippetStyle != null) ? snippetStyle.ForeColor : EditConsts.DefaultCodeSnippetForeColor);
				if (codeSnippet == textSource.CurrentSnippet)
				{
					Color backColor = painter.BackColor;
					painter.BackColor = color;
					painter.DrawRectangle(rectangle.Left, rectangle.Top, rectangle.Width - 1, rectangle.Height - 1);
					painter.BackColor = backColor;
				}
				else
				{
					painter.DrawFocusRect(rectangle.Left, rectangle.Top, rectangle.Width - 1, rectangle.Height - 1, color);
				}
			}
		}
	}

	private void DrawColumnIndent(int x, int top, int bottom)
	{
		Color foreColor = painter.ForeColor;
		painter.ForeColor = owner.ColumnsIndentForeColor;
		painter.DrawLine(x, top, x, bottom);
		painter.ForeColor = foreColor;
	}

	private int DrawTextFragment(Point position, string s, int style, LineStyle lineStyle, bool inSelection, int line, int startChar, int endChar, bool specialSymbol, bool highlight, bool readOnly, bool columnIndent)
	{
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		ColorFlags state = ColorFlags.None;
		ILexStyle lexStyle = GetLexStyle(style, ref state);
		bool inPrinting = owner.InPrinting;
		if (DisableSyntaxPaint)
		{
			lexStyle = null;
		}
		string text = ((startChar == 0 && endChar == -1) ? s : s.Substring(startChar, endChar - startChar + 1));
		if (lexStyle != null)
		{
			painter.FontStyle = GetFontStyle(lexStyle.FontStyle, state);
			if (inSelection)
			{
				painter.TextColor = GetSelectionForeColor(GetFontColor(lexStyle.ForeColor, state));
				painter.BackColor = selBackColor;
			}
			else
			{
				if (highlight && lineSeparator.HighlightForeColor != Color.Empty)
				{
					painter.TextColor = lineSeparator.HighlightForeColor;
				}
				else
				{
					painter.TextColor = GetFontColor(lexStyle.ForeColor, state);
				}
				if (highlight && lineSeparator.HighlightBackColor != Color.Empty)
				{
					painter.BackColor = lineSeparator.HighlightBackColor;
				}
				else
				{
					painter.BackColor = GetBackColor((lexStyle.BackColor != Color.Empty) ? lexStyle.BackColor : owner.GetBackColor(readOnly), state);
				}
			}
		}
		else
		{
			painter.FontStyle = GetFontStyle(((Control)owner).Font.Style, state);
			if (inSelection)
			{
				painter.TextColor = selForeColor;
				painter.BackColor = selBackColor;
			}
			else
			{
				if (highlight && lineSeparator.HighlightForeColor != Color.Empty)
				{
					painter.TextColor = lineSeparator.HighlightForeColor;
				}
				else
				{
					painter.TextColor = GetFontColor(((Control)owner).ForeColor, state);
				}
				if (highlight && lineSeparator.HighlightBackColor != Color.Empty)
				{
					painter.BackColor = lineSeparator.HighlightBackColor;
				}
				else
				{
					painter.BackColor = GetBackColor(owner.GetBackColor(readOnly), state);
				}
			}
		}
		Color color = Color.Empty;
		if (!inSelection && lineStyle != null && (lexStyle == null || lexStyle.BackColor == Color.Empty) && (state & ColorFlags.OutlineSection) == 0 && (state & ColorFlags.CodeSnippet) == 0)
		{
			painter.BackColor = lineStyle.GetBackColor(painter.BackColor);
			color = lineStyle.GetForeColor(painter.TextColor);
			painter.TextColor = color;
		}
		int num = painter.StringWidth(text);
		Rectangle rect = new Rectangle(position.X, position.Y, num, painter.FontHeight);
		int space = -1;
		if (!specialSymbol && whiteSpace.Visible)
		{
			if (whiteSpace.SpaceSymbol != 0 && (state & ColorFlags.WhiteSpace) != 0)
			{
				text = new string(whiteSpace.SpaceSymbol, text.Length);
				space = painter.CharWidth(' ', 1);
			}
			else if (whiteSpace.TabSymbol != 0 && (state & ColorFlags.Tabulation) != 0)
			{
				text = new string(whiteSpace.TabSymbol, text.Length);
				space = painter.CharWidth(' ', 1);
			}
		}
		DrawState drawState = DrawState.Text;
		if (inSelection)
		{
			drawState |= DrawState.Selection;
		}
		if (specialSymbol)
		{
			drawState |= DrawState.WhiteSpace;
		}
		if ((state & ColorFlags.CodeSnippet) != 0)
		{
			drawState |= DrawState.CodeSnippet;
		}
		bool flag = (owner.Transparent || inPrinting) && !inSelection && !highlight && lineStyle == null;
		if (flag)
		{
			painter.Opaque = false;
		}
		drawInfo.Init();
		drawInfo.Text = text;
		drawInfo.Style = (short)style;
		drawInfo.Line = line;
		drawInfo.Char = startChar;
		if (!OnCustomDraw(painter, rect, DrawStage.Before, drawState, drawInfo))
		{
			painter.TextOut(text, -1, rect, rect.Left, rect.Top, !inPrinting, !(owner.Transparent || inPrinting), space);
		}
		OnCustomDraw(painter, rect, DrawStage.After, drawState, drawInfo);
		if (flag)
		{
			painter.Opaque = true;
		}
		if (lineStyle != null && lineStyle.PenColor != Color.Empty)
		{
			DrawLineStyle(rect.Left, rect.Top - 1, rect.Right, rect.Bottom - 1, style, startChar, line, text, inSelection, afterText: false, lineStyle);
		}
		else if (highlight)
		{
			DrawHighlighter(rect.Left, rect.Top - 1, rect.Right, rect.Bottom - 1, style, startChar, line, text, inSelection, afterText: false);
		}
		if ((state & ColorFlags.OutlineSection) != 0)
		{
			Color foreColor = painter.ForeColor;
			Color backColor = painter.BackColor;
			painter.BackColor = outlining.OutlineColor;
			painter.ForeColor = outlining.OutlineColor;
			if (!OnCustomDraw(painter, rect, DrawStage.Before, drawState | DrawState.OutlineButton, drawInfo))
			{
				painter.DrawRectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
			}
			OnCustomDraw(painter, rect, DrawStage.After, drawState | DrawState.OutlineButton, drawInfo);
			painter.ForeColor = foreColor;
			painter.BackColor = backColor;
		}
		if (columnIndent)
		{
			DrawColumnIndent(rect.Left, rect.Top, rect.Bottom);
		}
		if (braces.UseRoundRect && (state & ColorFlags.Brace) != 0 && (state & ColorFlags.Tabulation) == 0 && (state & ColorFlags.WhiteSpace) == 0 && text.Trim() != string.Empty)
		{
			Color backColor2 = painter.BackColor;
			painter.BackColor = braces.BackColor;
			if (!OnCustomDraw(painter, rect, DrawStage.Before, drawState | DrawState.Brace, drawInfo))
			{
				painter.DrawRectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
			}
			OnCustomDraw(painter, rect, DrawStage.After, drawState | DrawState.Brace, drawInfo);
			painter.BackColor = backColor2;
		}
		if (spelling.CheckSpelling && (state & ColorFlags.MisSpelledWord) != 0)
		{
			if (!OnCustomDraw(painter, rect, DrawStage.Before, drawState | DrawState.Spelling, drawInfo))
			{
				painter.DrawWave(rect, (color != Color.Empty) ? color : spelling.SpellColor);
			}
			OnCustomDraw(painter, rect, DrawStage.After, drawState | DrawState.Spelling, drawInfo);
		}
		if ((state & ColorFlags.WaveLine) != 0)
		{
			if (!OnCustomDraw(painter, rect, DrawStage.Before, drawState | DrawState.SyntaxError, drawInfo))
			{
				painter.DrawWave(rect, (color != Color.Empty) ? color : ((errorStyle != null) ? errorStyle.ForeColor : EditConsts.DefaultSyntaxErrorForeColor));
			}
			OnCustomDraw(painter, rect, DrawStage.After, drawState | DrawState.SyntaxError, drawInfo);
		}
		return num;
	}

	private void DrawLine(int index, int realLine, string line, short[] colorData, Point position, Rectangle clipRect)
	{
		int right = clipRect.Right;
		int left = 0;
		int right2 = 0;
		int x = position.X;
		bool flag = (drawSelection & !selection.IsEmpty) && selection.GetSelectionForLine(index, out left, out right2);
		bool flag2 = false;
		bool flag3 = false;
		bool readOnly = owner.Source.IsLineReadonly(realLine);
		IList list = internalLineStyles;
		owner.Source.LineStyles.GetLineStyles(realLine, list);
		LineStyle lineStyle = GetLineStyleAt(index, 0, list);
		bool flag4 = lineStyle == null && owner.DrawColumnsIndent && owner.ColumnsIndentForeColor != Color.Empty;
		int length = line.Length;
		bool highlight = ((LineSeparator)lineSeparator).NeedHighlightLine(index, display: true);
		int offset = ((Gutter)gutter).GetWidth() - position.X;
		bool flag5 = false;
		if (whiteSpace.Visible || syntaxErrors.Count > 0 || flag4)
		{
			flag5 = owner.WordWrap && displayLines.DisplayPointToPoint(0, index + 1).X > 0;
		}
		if (flag5)
		{
			flag4 = false;
		}
		int num = 0;
		if (flag4)
		{
			num = line.TrimStart(new char[0]).Length;
			if (num == 0)
			{
				for (int num2 = index - 1; num2 >= 0; num2--)
				{
					short[] colorData2 = null;
					string line2 = string.Empty;
					GetString(num2, ref line2, ref colorData2);
					num = line2.TrimStart(new char[0]).Length;
					if (num != 0)
					{
						num = line2.Length - num;
						break;
					}
				}
			}
			else
			{
				num = length - num;
			}
		}
		if (line != string.Empty)
		{
			int num3 = (ushort)colorData[0];
			int num4 = num3;
			int num5 = 0;
			flag2 = flag && left == 0 && right2 > 0;
			bool flag6 = flag2;
			bool flag7 = false;
			LineStyle lineStyle2 = lineStyle;
			for (int i = 1; i < length; i++)
			{
				num3 = (ushort)colorData[i];
				flag6 = flag && left <= i && right2 > i;
				lineStyle2 = GetLineStyleAt(index, i, list);
				flag7 = flag4 && !flag2 && i < num && i == owner.Lines.GetTabStop(i - 1);
				if (i - num5 > EditConsts.MaxPaintChar || flag6 != flag2 || flag3 != flag7 || !EqualStyles(num4, num3, useColors: true) || !EqualLineStyles(lineStyle2, lineStyle, num3, num4))
				{
					position.X += DrawTextFragment(position, line, num4, lineStyle, flag2, index, num5, i - 1, specialSymbol: false, highlight, readOnly, flag3);
					if (position.X >= right)
					{
						break;
					}
					num4 = num3;
					num5 = i;
					lineStyle = lineStyle2;
					flag2 = flag6;
					flag3 = flag7;
				}
			}
			if (num5 < length && position.X < right)
			{
				position.X += DrawTextFragment(position, line, num4, lineStyle, flag2, index, num5, length - 1, specialSymbol: false, highlight, readOnly, flag3);
			}
		}
		if (position.X >= right)
		{
			return;
		}
		if (whiteSpace.Visible)
		{
			string text = string.Empty;
			if (flag5 && whiteSpace.WordWrapSymbol != 0)
			{
				text = whiteSpace.WordWrapString;
			}
			else if (!flag5)
			{
				bool flag8 = index == displayLines.GetCount() - 1;
				if (flag8 && whiteSpace.EofSymbol != 0)
				{
					text = whiteSpace.EofString;
				}
				else if (!flag8 && whiteSpace.EolSymbol != 0)
				{
					text = whiteSpace.EolString;
				}
			}
			if (text != string.Empty)
			{
				flag2 = flag && ((SelectionOptions.SelectBeyondEol & owner.Selection.Options) != 0 || selection.SelectionType == SelectionType.Block) && flag && left <= length && right2 > length;
				position.X += DrawTextFragment(position, text, 256, lineStyle, flag2, index, 0, 0, specialSymbol: true, highlight, readOnly, flag3);
			}
		}
		DrawAfterLineEnd(right, position, lineStyle, flag, flag && line == string.Empty && left == 0 && right2 == int.MaxValue && (SelectionOptions.SelectBeyondEol & selection.Options) == 0, offset, left, right2, index, highlight, readOnly, flag4, x, (line.TrimStart(new char[0]) == string.Empty) ? num : 0);
		if (flag && (selection.Options & SelectionOptions.DrawBorder) != 0 && selection.BorderColor != Color.Empty)
		{
			DrawSelectionBorder(left, right2, position.Y, index, line.Length);
		}
		if (right > position.X && !flag5 && syntaxErrors[realLine] != null)
		{
			DrawErrorAfterLineEnd(right, position, realLine, (!flag2) ? lineStyle : null);
		}
	}

	private void DrawErrorAfterLineEnd(int w, Point position, int line, LineStyle lineStyle)
	{
		Rectangle rect = new Rectangle(position.X, position.Y, painter.FontWidth, painter.FontHeight);
		drawInfo.Init();
		drawInfo.Line = line;
		if (!OnCustomDraw(painter, rect, DrawStage.Before, DrawState.BeyondEol | DrawState.SyntaxError, drawInfo))
		{
			Color color = lineStyle?.GetForeColor(painter.TextColor) ?? Color.Empty;
			painter.DrawWave(rect, (color != Color.Empty) ? color : ((errorStyle != null) ? errorStyle.ForeColor : EditConsts.DefaultSyntaxErrorForeColor));
		}
		OnCustomDraw(painter, rect, DrawStage.After, DrawState.BeyondEol | DrawState.SyntaxError, drawInfo);
	}

	private bool DrawEndLine(Rectangle rect, Point position, int endLine, bool readOnly, LineStyle lineStyle)
	{
		if (whiteSpace.Visible && whiteSpace.EofSymbol != 0)
		{
			int right = rect.Right;
			int count = owner.Lines.Count;
			bool highlight = ((LineSeparator)lineSeparator).NeedHighlightLine(count, display: false);
			position.X += DrawTextFragment(position, whiteSpace.EofString, 256, lineStyle, inSelection: false, endLine, 0, 0, specialSymbol: true, highlight, readOnly, columnIndent: false);
			DrawAfterLineEnd(right, position, lineStyle, selLine: false, emptySel: false, 0, 0, 0, count, highlight, readOnly, drawColumnIndent: false, 0, 0);
			return true;
		}
		return false;
	}

	protected LineStyle GetLineStyleAt(int line, int pos, IList styles)
	{
		if (styles.Count > 0)
		{
			Point point = displayLines.DisplayPointToPoint(pos, line);
			foreach (LStyle style in styles)
			{
				Point point2 = new Point(style.Pos, style.Line);
				Point point3 = ((style.Range != null) ? style.Range.EndPoint : new Point(int.MaxValue, style.Line));
				if ((point.Y > point2.Y || (point.Y == point2.Y && point.X >= point2.X)) && (point.Y < point3.Y || (point.Y == point3.Y && point.X < point3.X)) && style.Index >= 0 && style.Index < lineStyles.Count)
				{
					return (LineStyle)lineStyles[style.Index];
				}
			}
		}
		return null;
	}

	protected internal Color GetSelectionForeColor(Color color)
	{
		if ((SelectionOptions.UseColors & selection.Options) != 0)
		{
			return color;
		}
		return selForeColor;
	}

	protected override void GetString(int index, ref string line, ref short[] colorData)
	{
		((DisplayStrings)displayLines).GetDisplayString(index, ref line, ref colorData, needData: true);
	}

	public override FontStyle GetFontStyle(FontStyle fontStyle, ColorFlags state)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if ((state & ColorFlags.HyperText) != 0)
		{
			return (FontStyle)(fontStyle | hyperText.UrlStyle);
		}
		if ((state & ColorFlags.Brace) != 0 && !braces.UseRoundRect)
		{
			return (FontStyle)(fontStyle | braces.Style);
		}
		return fontStyle;
	}

	public override Color GetFontColor(Color color, ColorFlags state)
	{
		if (DisableColorPaint)
		{
			return ((Control)owner).ForeColor;
		}
		if ((state & ColorFlags.HyperText) != 0)
		{
			Color urlColor = hyperText.UrlColor;
			if (urlColor != Color.Empty)
			{
				return urlColor;
			}
		}
		if ((state & ColorFlags.WhiteSpace) != 0 || (state & ColorFlags.Tabulation) != 0)
		{
			Color urlColor = whiteSpace.SymbolColor;
			if (urlColor != Color.Empty)
			{
				return urlColor;
			}
		}
		if ((state & ColorFlags.OutlineSection) != 0)
		{
			Color urlColor = outlining.OutlineColor;
			if (urlColor != Color.Empty)
			{
				return urlColor;
			}
		}
		if ((state & ColorFlags.Brace) != 0 && !braces.UseRoundRect)
		{
			Color urlColor = braces.ForeColor;
			if (urlColor != Color.Empty)
			{
				return urlColor;
			}
		}
		return color;
	}

	public override Color GetBackColor(Color color, ColorFlags state)
	{
		if ((state & ColorFlags.CodeSnippet) != 0)
		{
			Color color2 = ((snippetStyle != null) ? snippetStyle.BackColor : EditConsts.DefaultCodeSnippetBackColor);
			if (color2 != Color.Empty)
			{
				return color2;
			}
		}
		if ((state & ColorFlags.Brace) != 0 && !braces.UseRoundRect)
		{
			Color backColor = braces.BackColor;
			if (backColor != Color.Empty)
			{
				return backColor;
			}
		}
		if (!(color != Color.Empty))
		{
			return painter.BackColor;
		}
		return color;
	}

	public override void DrawLine(int index, Point position, Rectangle clipRect)
	{
		string line = string.Empty;
		short[] data = null;
		int displayString = ((DisplayStrings)displayLines).GetDisplayString(index, ref line, ref data, needData: true);
		DrawLine(index, displayString, line, data, position, clipRect);
	}

	public virtual void DrawLine(int index, Point position, Rectangle clipRect, out int realLine)
	{
		string line = string.Empty;
		short[] data = null;
		realLine = ((DisplayStrings)displayLines).GetDisplayString(index, ref line, ref data, needData: true);
		DrawLine(index, realLine, line, data, position, clipRect);
	}

	public override void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint)
	{
		InitSyntaxErrors();
		internalLineStyles.Clear();
		if (owner.IsFocused())
		{
			selBackColor = selection.BackColor;
			selForeColor = selection.ForeColor;
			drawSelection = true;
		}
		else
		{
			drawSelection = (SelectionOptions.HideSelection & selection.Options) == 0;
			selBackColor = selection.InActiveBackColor;
			selForeColor = selection.InActiveForeColor;
		}
		errorStyle = ((Lexer != null) ? Lexer.Scheme.LexStyles.FindLexStyle(StringConsts.SyntaxErrorsInternalName) : null);
		snippetStyle = ((Lexer != null) ? Lexer.Scheme.LexStyles.FindLexStyle(StringConsts.CodeSnippetsInternalName) : null);
		int fontHeight = painter.FontHeight;
		int realLine = 0;
		for (int i = startLine; i < endLine; i++)
		{
			DrawLine(i, position, rect, out realLine);
			position.Y += fontHeight;
			if (position.Y > rect.Bottom || (position.Y == rect.Bottom && specialPaint))
			{
				break;
			}
			if ((owner.LineSeparator.Options & SeparatorOptions.SeparateContent) != 0 && Lexer != null && Lexer is ISyntaxParser && ((ISyntaxParser)Lexer).IsContentDivider(realLine))
			{
				painter.ForeColor = owner.LineSeparator.ContentDividerColor;
				painter.DrawLine(rect.Left, position.Y - 1, rect.Right - 1, position.Y - 1);
			}
			else if ((lineSeparator.Options & SeparatorOptions.SeparateLines) != 0 && ((lineSeparator.Options & SeparatorOptions.SeparateWrapLines) != 0 || displayLines.DisplayPointToPoint(0, i + 1).X == 0))
			{
				painter.ForeColor = lineSeparator.LineColor;
				Rectangle rect2 = new Rectangle(rect.Left, position.Y, rect.Width, painter.FontHeight);
				drawInfo.Init();
				drawInfo.Line = i;
				if (!OnCustomDraw(painter, rect2, DrawStage.Before, DrawState.LineSeparator, drawInfo))
				{
					painter.DrawLine(rect.Left, position.Y - 1, rect.Right - 1, position.Y - 1);
				}
				OnCustomDraw(painter, rect2, DrawStage.After, DrawState.LineSeparator, drawInfo);
			}
		}
		DrawCodeSnippets(painter);
		if (position.Y < rect.Bottom && endLine == startLine && DrawEndLine(rect, position, endLine, readOnly: false, null))
		{
			position.Y += fontHeight;
		}
		if (position.Y >= rect.Bottom)
		{
			return;
		}
		painter.BackColor = owner.GetBackColor(readOnly: false);
		Rectangle rect3 = new Rectangle(0, position.Y, rect.Right, rect.Bottom - position.Y);
		drawInfo.Init();
		if (!owner.Transparent && !owner.InPrinting)
		{
			if (!OnCustomDraw(painter, rect3, DrawStage.Before, DrawState.BeyondEof, drawInfo))
			{
				painter.FillRectangle(rect3);
			}
			OnCustomDraw(painter, rect3, DrawStage.After, DrawState.BeyondEof, drawInfo);
		}
		if ((lineSeparator.Options & SeparatorOptions.SeparateLines) == 0 || (lineSeparator.Options & SeparatorOptions.SeparateBeyondEof) == 0)
		{
			return;
		}
		painter.ForeColor = lineSeparator.LineColor;
		rect3 = new Rectangle(rect.Left, position.Y, rect.Width, painter.FontHeight);
		drawInfo.Init();
		drawInfo.Line = endLine;
		while (position.Y < rect.Bottom)
		{
			drawInfo.Line++;
			if (!OnCustomDraw(painter, rect3, DrawStage.Before, DrawState.LineSeparator, drawInfo))
			{
				painter.DrawLine(rect.Left, position.Y - 1, rect.Right - 1, position.Y - 1);
			}
			OnCustomDraw(painter, rect3, DrawStage.After, DrawState.LineSeparator, drawInfo);
			position.Y += fontHeight;
		}
	}

	public void PaintLineBookMarks(IPainter painter, Rectangle rect)
	{
		IBookMarks bookMarks = owner.Source.BookMarks;
		for (int i = 0; i < bookMarks.Count; i++)
		{
			PaintLineBookMark(painter, bookMarks[i], rect);
		}
	}

	public void PaintLineBookMark(IPainter painter, IBookMark bookMark, Rectangle rect)
	{
		if (outlining.AllowOutlining && !outlining.IsVisible(new Point(bookMark.Pos, bookMark.Line)))
		{
			return;
		}
		int num = 3;
		Point point = ((DisplayStrings)displayLines).PointToDisplayPoint(bookMark.Pos, bookMark.Line, lineEnd: false);
		Point point2 = owner.DisplayToScreen(point.X, point.Y);
		point2.Y += painter.FontHeight - 1;
		if (point2.X + num >= ((Gutter)gutter).GetWidth() && point2.X - num < rect.Width && point2.Y > 0 && point2.Y - num < rect.Height)
		{
			Point[] points = new Point[3]
			{
				new Point(point2.X - num, point2.Y),
				new Point(point2.X + num, point2.Y),
				new Point(point2.X, point2.Y - num)
			};
			Rectangle rect2 = new Rectangle(point2.X - num, point2.Y - num, num * 2, num * 2);
			drawInfo.Init();
			drawInfo.Line = point.Y;
			drawInfo.Char = point.X;
			if (!OnCustomDraw(painter, rect2, DrawStage.Before, DrawState.LineBookMark, drawInfo))
			{
				painter.DrawPolygon(points, gutter.LineBookmarksColor);
			}
			OnCustomDraw(painter, rect2, DrawStage.After, DrawState.LineBookMark, drawInfo);
		}
	}
}
