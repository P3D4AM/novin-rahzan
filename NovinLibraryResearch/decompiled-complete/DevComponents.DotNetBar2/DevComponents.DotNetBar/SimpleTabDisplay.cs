using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar;

public class SimpleTabDisplay
{
	protected class Line
	{
		public int X1;

		public int Y1;

		public int X2;

		public int Y2;

		public Line(int x1, int y1, int x2, int y2)
		{
			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
		}
	}

	private int _283F = 8;

	private int _2840 = 2;

	public virtual void Paint(Graphics g, ISimpleTab[] tabs)
	{
		int num = -1;
		for (int num2 = tabs.Length - 1; num2 >= 0; num2--)
		{
			if (tabs[num2].IsSelected)
			{
				num = num2;
			}
			else
			{
				PaintTab(g, tabs[num2]);
			}
		}
		if (num != -1)
		{
			PaintTab(g, tabs[num]);
		}
	}

	protected virtual void PaintTab(Graphics g, ISimpleTab tab)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		TabColors tabColors = ᕝ(tab);
		Rectangle displayRectangle = tab.DisplayRectangle;
		displayRectangle.Width += _283F * 2;
		displayRectangle.X -= _283F;
		GraphicsPath tabPath = GetTabPath(displayRectangle, tab.TabAlignment);
		if (tabColors.BackColor2.IsEmpty)
		{
			SolidBrush val = new SolidBrush(tabColors.BackColor);
			try
			{
				g.FillPath((Brush)(object)val, tabPath);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			LinearGradientBrush val2 = BarFunctions.ॴ(tab.DisplayRectangle, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
			try
			{
				g.FillPath((Brush)(object)val2, tabPath);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (!tabColors.BorderColor.IsEmpty)
		{
			Pen val3 = new Pen(tabColors.BorderColor, 1f);
			try
			{
				Line leftLine = GetLeftLine(displayRectangle, tab.TabAlignment);
				g.DrawLine(val3, leftLine.X1, leftLine.Y1, leftLine.X2, leftLine.Y2);
				leftLine = GetRightLine(displayRectangle, tab.TabAlignment);
				g.DrawLine(val3, leftLine.X1, leftLine.Y1, leftLine.X2, leftLine.Y2);
				if (tab.TabAlignment == eTabStripAlignment.Top)
				{
					leftLine = GetTopLine(displayRectangle, tab.TabAlignment);
					g.DrawLine(val3, leftLine.X1, leftLine.Y1, leftLine.X2, leftLine.Y2);
				}
				else if (tab.TabAlignment == eTabStripAlignment.Bottom)
				{
					leftLine = GetBottomLine(displayRectangle, tab.TabAlignment);
					g.DrawLine(val3, leftLine.X1, leftLine.Y1, leftLine.X2, leftLine.Y2);
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (!tabColors.LightBorderColor.IsEmpty)
		{
			Pen val4 = new Pen(tabColors.LightBorderColor, 1f);
			try
			{
				Line leftLine2 = GetLeftLine(displayRectangle, tab.TabAlignment);
				g.DrawLine(val4, leftLine2.X1, leftLine2.Y1, leftLine2.X2, leftLine2.Y2);
				if (tab.TabAlignment == eTabStripAlignment.Top)
				{
					leftLine2 = GetTopLine(displayRectangle, tab.TabAlignment);
					g.DrawLine(val4, leftLine2.X1, leftLine2.Y1, leftLine2.X2, leftLine2.Y2);
				}
				else if (tab.TabAlignment == eTabStripAlignment.Bottom)
				{
					leftLine2 = GetBottomLine(displayRectangle, tab.TabAlignment);
					g.DrawLine(val4, leftLine2.X1, leftLine2.Y1, leftLine2.X2, leftLine2.Y2);
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		if (!tabColors.DarkBorderColor.IsEmpty)
		{
			Pen val5 = new Pen(tabColors.DarkBorderColor, 1f);
			try
			{
				Line rightLine = GetRightLine(displayRectangle, tab.TabAlignment);
				g.DrawLine(val5, rightLine.X1, rightLine.Y1, rightLine.X2, rightLine.Y2);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		DrawTabText(g, tab, GetTextRectangle(tab), tabColors);
	}

	private TabColors ᕝ(ISimpleTab ᆰ)
	{
		TabColors tabColors = new TabColors();
		tabColors.BackColor = ᆰ.BackColor;
		tabColors.BackColor2 = ᆰ.BackColor2;
		tabColors.BackColorGradientAngle = ᆰ.BackColorGradientAngle;
		tabColors.BorderColor = ᆰ.BorderColor;
		tabColors.DarkBorderColor = ᆰ.DarkBorderColor;
		tabColors.LightBorderColor = ᆰ.LightBorderColor;
		tabColors.TextColor = ᆰ.TextColor;
		if (ᆰ.IsSelected && ᆰ is BubbleBarTab && ((BubbleBarTab)ᆰ).Parent != null)
		{
			TabColors selectedTabColors = ((BubbleBarTab)ᆰ).Parent.SelectedTabColors;
			if (!selectedTabColors.BackColor.IsEmpty)
			{
				tabColors.BackColor = selectedTabColors.BackColor;
			}
			if (!selectedTabColors.BackColor2.IsEmpty)
			{
				tabColors.BackColor2 = selectedTabColors.BackColor2;
				tabColors.BackColorGradientAngle = selectedTabColors.BackColorGradientAngle;
			}
			if (!selectedTabColors.BorderColor.IsEmpty)
			{
				tabColors.BorderColor = selectedTabColors.BorderColor;
			}
			if (!selectedTabColors.DarkBorderColor.IsEmpty)
			{
				tabColors.DarkBorderColor = selectedTabColors.DarkBorderColor;
			}
			if (!selectedTabColors.LightBorderColor.IsEmpty)
			{
				tabColors.LightBorderColor = selectedTabColors.LightBorderColor;
			}
			if (!selectedTabColors.TextColor.IsEmpty)
			{
				tabColors.TextColor = selectedTabColors.TextColor;
			}
		}
		if (ᆰ.IsMouseOver && ᆰ is BubbleBarTab && ((BubbleBarTab)ᆰ).Parent != null)
		{
			TabColors mouseOverTabColors = ((BubbleBarTab)ᆰ).Parent.MouseOverTabColors;
			if (!mouseOverTabColors.BackColor.IsEmpty)
			{
				tabColors.BackColor = mouseOverTabColors.BackColor;
			}
			if (!mouseOverTabColors.BackColor2.IsEmpty)
			{
				tabColors.BackColor2 = mouseOverTabColors.BackColor2;
				tabColors.BackColorGradientAngle = mouseOverTabColors.BackColorGradientAngle;
			}
			if (!mouseOverTabColors.BorderColor.IsEmpty)
			{
				tabColors.BorderColor = mouseOverTabColors.BorderColor;
			}
			if (!mouseOverTabColors.DarkBorderColor.IsEmpty)
			{
				tabColors.DarkBorderColor = mouseOverTabColors.DarkBorderColor;
			}
			if (!mouseOverTabColors.LightBorderColor.IsEmpty)
			{
				tabColors.LightBorderColor = mouseOverTabColors.LightBorderColor;
			}
			if (!mouseOverTabColors.TextColor.IsEmpty)
			{
				tabColors.TextColor = mouseOverTabColors.TextColor;
			}
		}
		return tabColors;
	}

	protected virtual Rectangle GetTextRectangle(ISimpleTab tab)
	{
		Rectangle displayRectangle = tab.DisplayRectangle;
		displayRectangle.Y += _2840;
		displayRectangle.Height -= _2840;
		return displayRectangle;
	}

	protected virtual GraphicsPath GetTabPath(Rectangle r, eTabStripAlignment align)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Line leftLine = GetLeftLine(r, align);
		val.AddLine(leftLine.X1, leftLine.Y1, leftLine.X2, leftLine.Y2);
		leftLine = GetTopLine(r, align);
		val.AddLine(leftLine.X1, leftLine.Y1, leftLine.X2, leftLine.Y2);
		leftLine = GetRightLine(r, align);
		val.AddLine(leftLine.X1, leftLine.Y1, leftLine.X2, leftLine.Y2);
		leftLine = GetBottomLine(r, align);
		val.AddLine(leftLine.X1, leftLine.Y1, leftLine.X2, leftLine.Y2);
		val.CloseAllFigures();
		return val;
	}

	protected virtual Line GetLeftLine(Rectangle r, eTabStripAlignment align)
	{
		return align switch
		{
			eTabStripAlignment.Top => new Line(r.X, r.Bottom, r.X + _283F, r.Y), 
			eTabStripAlignment.Bottom => new Line(r.X + _283F, r.Bottom, r.X, r.Y), 
			_ => null, 
		};
	}

	protected virtual Line GetRightLine(Rectangle r, eTabStripAlignment align)
	{
		return align switch
		{
			eTabStripAlignment.Top => new Line(r.Right - _283F, r.Y, r.Right, r.Bottom), 
			eTabStripAlignment.Bottom => new Line(r.Right, r.Y, r.Right - _283F, r.Bottom), 
			_ => null, 
		};
	}

	protected virtual Line GetTopLine(Rectangle r, eTabStripAlignment align)
	{
		return align switch
		{
			eTabStripAlignment.Top => new Line(r.X + _283F, r.Y, r.Right - _283F, r.Y), 
			eTabStripAlignment.Bottom => new Line(r.X, r.Y, r.Right, r.Y), 
			_ => null, 
		};
	}

	protected virtual Line GetBottomLine(Rectangle r, eTabStripAlignment align)
	{
		return align switch
		{
			eTabStripAlignment.Top => new Line(r.X, r.Bottom, r.Right, r.Bottom), 
			eTabStripAlignment.Bottom => new Line(r.Right - _283F, r.Bottom, r.X + _283F, r.Bottom), 
			_ => null, 
		};
	}

	protected virtual void DrawTabText(Graphics g, ISimpleTab tab, Rectangle rText, TabColors c)
	{
		eTextFormat stringFormat = GetStringFormat();
		Font tabFont = tab.GetTabFont();
		if (tab.TabAlignment == eTabStripAlignment.Left || tab.TabAlignment == eTabStripAlignment.Right)
		{
			g.RotateTransform(90f);
			rText = new Rectangle(rText.Top, -rText.Right, rText.Height, rText.Width);
		}
		if (tab.TabAlignment == eTabStripAlignment.Left || tab.TabAlignment == eTabStripAlignment.Right)
		{
			TextDrawing.DrawStringLegacy(g, tab.Text, tabFont, c.TextColor, rText, stringFormat);
		}
		else
		{
			TextDrawing.DrawString(g, tab.Text, tabFont, c.TextColor, rText, stringFormat);
		}
		if (tab.TabAlignment == eTabStripAlignment.Left || tab.TabAlignment == eTabStripAlignment.Right)
		{
			g.ResetTransform();
		}
	}

	protected virtual eTextFormat GetStringFormat()
	{
		return eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
	}
}
