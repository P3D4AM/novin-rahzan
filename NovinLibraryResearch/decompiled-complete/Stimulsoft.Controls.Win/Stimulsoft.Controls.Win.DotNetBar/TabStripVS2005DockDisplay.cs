using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class TabStripVS2005DockDisplay : TabStripBaseDisplay
{
	private int m_xTabOffset = 8;

	public override void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		base.Paint(g, tabStrip);
		TabColorScheme colorScheme = tabStrip.ColorScheme;
		Rectangle displayRectangle = ((Control)tabStrip).DisplayRectangle;
		if (((Control)tabStrip).BackColor != Color.Transparent || colorScheme.TabBackground != Color.Transparent)
		{
			if (colorScheme.TabPanelBackground2.IsEmpty)
			{
				if (!colorScheme.TabPanelBackground.IsEmpty)
				{
					SolidBrush val = new SolidBrush(colorScheme.TabPanelBackground);
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
				LinearGradientBrush val3 = CreateTabGradientBrush(displayRectangle, colorScheme.TabPanelBackground, colorScheme.TabPanelBackground2, colorScheme.TabPanelBackgroundGradientAngle);
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
		DrawBackground(tabStrip, displayRectangle, g, colorScheme, new Region(((Control)tabStrip).DisplayRectangle), tabStrip.TabAlignment, selectedTabRect);
		tabStrip.ClipExcludeSystemBox(g);
		for (int num = tabStrip.Tabs.Count - 1; num >= 0; num--)
		{
			TabItem tabItem = tabStrip.Tabs[num];
			if (tabItem.Visible && tabItem != tabStrip.SelectedTab && tabItem.DisplayRectangle.IntersectsWith(displayRectangle))
			{
				PaintTab(g, tabItem, first: false, last: false);
			}
		}
		if (tabStrip.SelectedTab != null && tabStrip.Tabs.Contains(tabStrip.SelectedTab))
		{
			PaintTab(g, tabStrip.SelectedTab, first: false, last: false);
		}
		g.ResetClip();
		tabStrip.PaintTabSystemBox(g);
	}

	protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		Rectangle r = tab.DisplayRectangle;
		if (tab.TabAlignment == eTabStripAlignment.Right)
		{
			r = new Rectangle(r.X, r.Y, r.Height, r.Width);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Left)
		{
			r = new Rectangle(r.X, r.Y, r.Height, r.Width);
		}
		r.Offset(0, 1);
		GraphicsPath val = new GraphicsPath();
		val.AddPath(GetLeftLine(r), true);
		val.AddLine(r.X + 6, r.Y, r.Right - 5, r.Y);
		val.AddPath(GetRightLine(r), true);
		val.AddLine(r.Right + m_xTabOffset, r.Bottom, r.X - m_xTabOffset, r.Bottom);
		val.CloseAllFigures();
		if (tab.TabAlignment == eTabStripAlignment.Bottom)
		{
			Matrix val2 = new Matrix();
			val2.RotateAt(180f, new PointF(r.X + r.Width / 2, r.Y + r.Height / 2));
			val.Transform(val2);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Left)
		{
			Matrix val3 = new Matrix();
			val3.RotateAt(-90f, new PointF(r.X, r.Bottom));
			val3.Translate((float)r.Height, (float)(r.Width - r.Height), (MatrixOrder)1);
			val.Transform(val3);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Right)
		{
			Matrix val4 = new Matrix();
			val4.RotateAt(90f, new PointF(r.Right, r.Bottom));
			val4.Translate((float)(-r.Width), (float)(r.Width - (r.Height - 1)), (MatrixOrder)1);
			val.Transform(val4);
		}
		return val;
	}

	private GraphicsPath GetLeftLine(Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddLine(r.X - m_xTabOffset, r.Bottom, r.X, r.Y + 5);
		val.AddCurve(new Point[3]
		{
			new Point(r.X, r.Y + 5),
			new Point(r.X + 2, r.Y + 2),
			new Point(r.X + 5, r.Y)
		}, 0.9f);
		return val;
	}

	private GraphicsPath GetRightLine(Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddCurve(new Point[3]
		{
			new Point(r.Right - 5, r.Y),
			new Point(r.Right - 2, r.Y + 2),
			new Point(r.Right, r.Y + 5)
		}, 0.9f);
		val.AddLine(r.Right, r.Y + 5, r.Right + m_xTabOffset, r.Bottom);
		return val;
	}

	protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		base.DrawBackground(tabStrip, tabStripRect, g, colors, tabsRegion, tabAlignment, selectedTabRect);
		if (colors.TabItemBorder.IsEmpty)
		{
			return;
		}
		Pen val = new Pen(colors.TabItemBorder, 1f);
		try
		{
			switch (tabAlignment)
			{
			case eTabStripAlignment.Bottom:
				g.DrawLine(val, tabStripRect.X, tabStripRect.Y + 1, tabStripRect.Right, tabStripRect.Y + 1);
				break;
			case eTabStripAlignment.Left:
				g.DrawLine(val, tabStripRect.Right - 1, tabStripRect.Y, tabStripRect.Right - 1, tabStripRect.Bottom);
				break;
			case eTabStripAlignment.Right:
				g.DrawLine(val, tabStripRect.X, tabStripRect.Y, tabStripRect.X, tabStripRect.Bottom);
				break;
			case eTabStripAlignment.Top:
				g.DrawLine(val, tabStripRect.X, tabStripRect.Bottom - 1, tabStripRect.Right, tabStripRect.Bottom - 1);
				break;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
