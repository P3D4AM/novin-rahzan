using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class TabStripOffice2007DocumentDisplay : TabStripBaseDisplay
{
	private int m_TopLeftCornerSize = 3;

	public override void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		base.Paint(g, tabStrip);
		TabColorScheme colorScheme = tabStrip.ColorScheme;
		Rectangle displayRectangle = ((Control)tabStrip).DisplayRectangle;
		if (colorScheme.TabBackgroundImage != null)
		{
			BarFunctions.PaintBackgroundImage(g, displayRectangle, colorScheme.TabBackgroundImage, eBackgroundImagePosition.Tile, 255);
		}
		if (colorScheme.TabBackground != Color.Transparent && !colorScheme.TabBackground.IsEmpty)
		{
			if (colorScheme.TabBackground2.IsEmpty)
			{
				if (!colorScheme.TabBackground.IsEmpty)
				{
					SolidBrush val = new SolidBrush(colorScheme.TabBackground);
					try
					{
						g.FillRectangle((Brush)(object)val, displayRectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val2 = new SolidBrush(Color.White);
				try
				{
					g.FillRectangle((Brush)(object)val2, displayRectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				LinearGradientBrush val3 = CreateTabGradientBrush(displayRectangle, colorScheme.TabBackground, colorScheme.TabBackground2, colorScheme.TabBackgroundGradientAngle);
				try
				{
					g.FillRectangle((Brush)(object)val3, displayRectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		Rectangle selectedTabRect = Rectangle.Empty;
		if (tabStrip.SelectedTab != null)
		{
			selectedTabRect = tabStrip.SelectedTab.DisplayRectangle;
		}
		Region val4 = new Region(((Control)tabStrip).DisplayRectangle);
		try
		{
			DrawBackground(tabStrip, displayRectangle, g, colorScheme, val4, tabStrip.TabAlignment, selectedTabRect);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		Rectangle systemBoxRectangle = tabStrip.GetSystemBoxRectangle();
		systemBoxRectangle.Inflate(-2, -2);
		for (int num = tabStrip.Tabs.Count - 1; num >= 0; num--)
		{
			TabItem tabItem = tabStrip.Tabs[num];
			if (tabItem.Visible && tabItem != tabStrip.SelectedTab && !tabItem.DisplayRectangle.IntersectsWith(systemBoxRectangle) && tabItem.DisplayRectangle.IntersectsWith(displayRectangle))
			{
				PaintTab(g, tabItem, first: false, last: false);
			}
		}
		if (tabStrip.SelectedTab != null && tabStrip.Tabs.Contains(tabStrip.SelectedTab) && !GetAdjustedRect(tabStrip.SelectedTab).IntersectsWith(systemBoxRectangle))
		{
			PaintTab(g, tabStrip.SelectedTab, first: false, last: false);
		}
		g.ResetClip();
		tabStrip.PaintTabSystemBox(g);
	}

	private Rectangle GetAdjustedRect(TabItem item)
	{
		Rectangle displayRectangle = item.DisplayRectangle;
		if (item.TabAlignment == eTabStripAlignment.Top || item.TabAlignment == eTabStripAlignment.Bottom)
		{
			displayRectangle.Width -= displayRectangle.Height - 6;
		}
		else
		{
			displayRectangle.Height -= displayRectangle.Width - 6;
		}
		return displayRectangle;
	}

	protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		return GetTabPath(tab.DisplayRectangle, tab.TabAlignment, closePath: true);
	}

	private GraphicsPath GetTabPath(Rectangle tabDisplayRectangle, eTabStripAlignment align, bool closePath)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		Rectangle rectangle = tabDisplayRectangle;
		switch (align)
		{
		case eTabStripAlignment.Right:
			rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Height, rectangle.Width);
			break;
		case eTabStripAlignment.Left:
			rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Height, rectangle.Width);
			break;
		}
		if (align == eTabStripAlignment.Bottom || align == eTabStripAlignment.Top)
		{
			rectangle.Offset(0, 1);
		}
		else
		{
			rectangle.Offset(1, 0);
		}
		GraphicsPath val = new GraphicsPath();
		if (align == eTabStripAlignment.Bottom || align == eTabStripAlignment.Left)
		{
			val.AddLine(rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom - m_TopLeftCornerSize);
			val.AddLine(rectangle.X, rectangle.Bottom - m_TopLeftCornerSize, rectangle.X + m_TopLeftCornerSize, rectangle.Bottom);
			val.AddLine(rectangle.X + m_TopLeftCornerSize, rectangle.Bottom, rectangle.Right - rectangle.Height / 2, rectangle.Bottom);
			Point[] array = new Point[4];
			array[0].X = rectangle.Right - rectangle.Height / 2;
			array[0].Y = rectangle.Bottom;
			array[1].X = array[0].X + 5;
			array[1].Y = array[0].Y - 3;
			array[2].X = array[1].X + rectangle.Height / 2;
			array[2].Y = rectangle.Y + 3;
			array[3].X = array[2].X + 4;
			array[3].Y = rectangle.Y;
			val.AddCurve(array, 0, 3, 0.1f);
			if (closePath)
			{
				val.AddLine(rectangle.Right, rectangle.Y, rectangle.X, rectangle.Y);
			}
		}
		else
		{
			val.AddLine(rectangle.X, rectangle.Bottom, rectangle.X, rectangle.Y + m_TopLeftCornerSize);
			val.AddLine(rectangle.X, rectangle.Y + m_TopLeftCornerSize, rectangle.X + m_TopLeftCornerSize, rectangle.Y);
			val.AddLine(rectangle.X + m_TopLeftCornerSize, rectangle.Y, rectangle.Right - rectangle.Height / 2, rectangle.Y);
			Point[] array2 = new Point[4];
			array2[0].X = rectangle.Right - rectangle.Height / 2;
			array2[0].Y = rectangle.Y;
			array2[1].X = array2[0].X + 5;
			array2[1].Y = array2[0].Y + 3;
			array2[2].X = array2[1].X + rectangle.Height / 2;
			array2[2].Y = rectangle.Bottom - 3;
			array2[3].X = array2[2].X + 4;
			array2[3].Y = rectangle.Bottom;
			val.AddCurve(array2, 0, 3, 0.1f);
			if (closePath)
			{
				val.AddLine(rectangle.Right, rectangle.Bottom, rectangle.X, rectangle.Bottom);
			}
		}
		switch (align)
		{
		case eTabStripAlignment.Left:
		{
			Matrix val3 = new Matrix();
			val3.RotateAt(90f, new PointF(rectangle.Right, rectangle.Bottom));
			val3.Translate((float)(-rectangle.Width - 2), (float)(rectangle.Width - rectangle.Height), (MatrixOrder)1);
			val.Transform(val3);
			break;
		}
		case eTabStripAlignment.Right:
		{
			Matrix val2 = new Matrix();
			val2.RotateAt(90f, new PointF(rectangle.Right, rectangle.Bottom));
			val2.Translate((float)(-rectangle.Width), (float)(rectangle.Width - rectangle.Height), (MatrixOrder)1);
			val.Transform(val2);
			break;
		}
		}
		if (closePath)
		{
			val.CloseAllFigures();
		}
		return val;
	}

	protected override void DrawTabBorder(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		Rectangle displayRectangle = tab.DisplayRectangle;
		eTabStripAlignment tabAlignment = tab.TabAlignment;
		Region clip = g.Clip;
		Rectangle clip2 = Rectangle.Ceiling(path.GetBounds());
		if (tab.TabAlignment == eTabStripAlignment.Right || tab.TabAlignment == eTabStripAlignment.Bottom)
		{
			clip2.Inflate(1, 1);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Left && !tab.IsSelected)
		{
			clip2.Width--;
		}
		g.SetClip(clip2);
		if (!colors.BorderColor.IsEmpty)
		{
			GraphicsPath tabPath = GetTabPath(displayRectangle, tabAlignment, closePath: false);
			try
			{
				Pen val = new Pen(colors.BorderColor);
				try
				{
					g.DrawPath(val, tabPath);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)tabPath)?.Dispose();
			}
		}
		switch (tabAlignment)
		{
		case eTabStripAlignment.Top:
			displayRectangle.Offset(1, 1);
			displayRectangle.Width--;
			break;
		case eTabStripAlignment.Bottom:
			displayRectangle.Offset(1, -1);
			displayRectangle.Width--;
			break;
		case eTabStripAlignment.Left:
			displayRectangle.Offset(1, 1);
			displayRectangle.Height--;
			break;
		case eTabStripAlignment.Right:
			displayRectangle.Offset(-1, 1);
			break;
		}
		if (!colors.LightBorderColor.IsEmpty)
		{
			GraphicsPath tabPath2 = GetTabPath(displayRectangle, tabAlignment, closePath: false);
			try
			{
				clip2 = Rectangle.Ceiling(path.GetBounds());
				if (tabAlignment == eTabStripAlignment.Left)
				{
					clip2.Width--;
				}
				g.SetClip(clip2);
				Pen val2 = new Pen(colors.LightBorderColor);
				try
				{
					g.DrawPath(val2, tabPath2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)tabPath2)?.Dispose();
			}
		}
		g.Clip = clip;
	}

	protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		base.DrawBackground(tabStrip, tabStripRect, g, colors, tabsRegion, tabAlignment, selectedTabRect);
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Pen val = new Pen(colors.TabItemBorder, 1f);
		try
		{
			int num = 9;
			if (selectedTabRect.IsEmpty)
			{
				num = 0;
			}
			Color color = colors.TabItemSelectedBackground2;
			if (tabStrip.SelectedTab != null)
			{
				TabColors tabColors = tabStrip.GetTabColors(tabStrip.SelectedTab);
				color = tabColors.BackColor2;
			}
			switch (tabAlignment)
			{
			case eTabStripAlignment.Top:
			{
				if (selectedTabRect.X > tabStripRect.Right || selectedTabRect.Bottom < tabStripRect.Bottom - 4)
				{
					selectedTabRect = Rectangle.Empty;
					num = 0;
				}
				Rectangle bounds3 = new Rectangle(tabStripRect.X, tabStripRect.Bottom - 1, tabStripRect.Width, 1);
				DisplayHelp.FillRectangle(g, bounds3, color, Color.Empty);
				g.DrawLine(val, bounds3.X, bounds3.Y, selectedTabRect.X, bounds3.Y);
				g.DrawLine(val, selectedTabRect.Right + num, bounds3.Y, bounds3.Right, bounds3.Y);
				g.DrawLine(val, bounds3.X, bounds3.Y, bounds3.X, bounds3.Bottom);
				g.DrawLine(val, bounds3.Right - 1, bounds3.Y, bounds3.Right - 1, bounds3.Bottom);
				break;
			}
			case eTabStripAlignment.Bottom:
			{
				if (selectedTabRect.X > tabStripRect.Right || selectedTabRect.Y > 4)
				{
					selectedTabRect = Rectangle.Empty;
					num = 0;
				}
				Rectangle bounds2 = new Rectangle(tabStripRect.X, tabStripRect.Y, tabStripRect.Width, 1);
				DisplayHelp.FillRectangle(g, bounds2, color, Color.Empty);
				g.DrawLine(val, bounds2.X, bounds2.Bottom - 1, selectedTabRect.X, bounds2.Bottom - 1);
				g.DrawLine(val, selectedTabRect.Right + num, bounds2.Bottom - 1, bounds2.Right, bounds2.Bottom - 1);
				g.DrawLine(val, bounds2.X, bounds2.Y, bounds2.X, bounds2.Bottom - 1);
				g.DrawLine(val, bounds2.Right - 1, bounds2.Y, bounds2.Right - 1, bounds2.Bottom - 1);
				break;
			}
			case eTabStripAlignment.Left:
			{
				if (selectedTabRect.Y > tabStripRect.Bottom || selectedTabRect.Right < tabStripRect.Right - 4)
				{
					selectedTabRect = Rectangle.Empty;
					num = 0;
				}
				Rectangle bounds4 = new Rectangle(tabStripRect.Right - 1, tabStripRect.Y, 1, tabStripRect.Height);
				DisplayHelp.FillRectangle(g, bounds4, color, Color.Empty);
				g.DrawLine(val, bounds4.X, bounds4.Y, bounds4.X, selectedTabRect.Y);
				g.DrawLine(val, bounds4.X, selectedTabRect.Bottom + num, bounds4.X, bounds4.Bottom);
				g.DrawLine(val, bounds4.X, bounds4.Y, bounds4.Right, bounds4.Y);
				g.DrawLine(val, bounds4.X, bounds4.Bottom - 1, bounds4.Right, bounds4.Bottom - 1);
				break;
			}
			case eTabStripAlignment.Right:
			{
				if (selectedTabRect.Y > tabStripRect.Bottom || selectedTabRect.X > 4)
				{
					selectedTabRect = Rectangle.Empty;
					num = 0;
				}
				Rectangle bounds = new Rectangle(tabStripRect.X, tabStripRect.Y, 1, tabStripRect.Height);
				DisplayHelp.FillRectangle(g, bounds, color, Color.Empty);
				g.DrawLine(val, bounds.Right - 1, bounds.Y, bounds.Right - 1, selectedTabRect.Y);
				g.DrawLine(val, bounds.Right - 1, selectedTabRect.Bottom + num, bounds.Right - 1, bounds.Bottom);
				g.DrawLine(val, bounds.X, bounds.Y, bounds.Right, bounds.Y);
				g.DrawLine(val, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
				break;
			}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		g.SmoothingMode = smoothingMode;
	}

	protected override void AdjustTextRectangle(ref Rectangle rText, eTabStripAlignment tabAlignment)
	{
		if (tabAlignment == eTabStripAlignment.Top || tabAlignment == eTabStripAlignment.Bottom)
		{
			rText.Width -= 3;
		}
		base.AdjustTextRectangle(ref rText, tabAlignment);
	}

	protected override void AdjustCloseButtonRectangle(ref Rectangle close, bool closeOnLeftSide, bool vertical)
	{
		if (!closeOnLeftSide)
		{
			if (!vertical)
			{
				close.X -= base.TabCloseButtonSpacing + close.Height / 6;
			}
			else
			{
				close.Y -= base.TabCloseButtonSpacing + close.Height / 6;
			}
		}
	}
}
