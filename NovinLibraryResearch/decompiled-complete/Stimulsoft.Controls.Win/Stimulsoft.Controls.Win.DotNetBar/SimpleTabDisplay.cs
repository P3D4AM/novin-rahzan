using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

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

	private int m_SideWidth = 8;

	private int m_TextTopOffset = 2;

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
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		TabColors tabColors = GetTabColors(tab);
		Rectangle displayRectangle = tab.DisplayRectangle;
		displayRectangle.Width += m_SideWidth * 2;
		displayRectangle.X -= m_SideWidth;
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
			LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(tab.DisplayRectangle, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
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

	private TabColors GetTabColors(ISimpleTab tab)
	{
		TabColors tabColors = new TabColors();
		tabColors.BackColor = tab.BackColor;
		tabColors.BackColor2 = tab.BackColor2;
		tabColors.BackColorGradientAngle = tab.BackColorGradientAngle;
		tabColors.BorderColor = tab.BorderColor;
		tabColors.DarkBorderColor = tab.DarkBorderColor;
		tabColors.LightBorderColor = tab.LightBorderColor;
		tabColors.TextColor = tab.TextColor;
		return tabColors;
	}

	protected virtual Rectangle GetTextRectangle(ISimpleTab tab)
	{
		Rectangle displayRectangle = tab.DisplayRectangle;
		displayRectangle.Y += m_TextTopOffset;
		displayRectangle.Height -= m_TextTopOffset;
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
			eTabStripAlignment.Top => new Line(r.X, r.Bottom, r.X + m_SideWidth, r.Y), 
			eTabStripAlignment.Bottom => new Line(r.X + m_SideWidth, r.Bottom, r.X, r.Y), 
			_ => null, 
		};
	}

	protected virtual Line GetRightLine(Rectangle r, eTabStripAlignment align)
	{
		return align switch
		{
			eTabStripAlignment.Top => new Line(r.Right - m_SideWidth, r.Y, r.Right, r.Bottom), 
			eTabStripAlignment.Bottom => new Line(r.Right, r.Y, r.Right - m_SideWidth, r.Bottom), 
			_ => null, 
		};
	}

	protected virtual Line GetTopLine(Rectangle r, eTabStripAlignment align)
	{
		return align switch
		{
			eTabStripAlignment.Top => new Line(r.X + m_SideWidth, r.Y, r.Right - m_SideWidth, r.Y), 
			eTabStripAlignment.Bottom => new Line(r.X, r.Y, r.Right, r.Y), 
			_ => null, 
		};
	}

	protected virtual Line GetBottomLine(Rectangle r, eTabStripAlignment align)
	{
		return align switch
		{
			eTabStripAlignment.Top => new Line(r.X, r.Bottom, r.Right, r.Bottom), 
			eTabStripAlignment.Bottom => new Line(r.Right - m_SideWidth, r.Bottom, r.X + m_SideWidth, r.Bottom), 
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
