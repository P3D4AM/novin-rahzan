using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class TabStripRoundHeaderDisplay : TabStripBaseDisplay
{
	public override void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		base.Paint(g, tabStrip);
		TabsCollection tabs = tabStrip.Tabs;
		TabColorScheme colorScheme = tabStrip.ColorScheme;
		Rectangle clientRectangle = ((Control)tabStrip).ClientRectangle;
		Rectangle displayRectangle = ((Control)tabStrip).DisplayRectangle;
		displayRectangle.Inflate(2, 2);
		SolidBrush val = new SolidBrush(((Control)tabStrip).BackColor);
		try
		{
			g.FillRectangle((Brush)(object)val, displayRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		TabItem lastVisibleTab = GetLastVisibleTab(tabs);
		Rectangle selectedTabRect = Rectangle.Empty;
		if (tabStrip.SelectedTab != null)
		{
			selectedTabRect = tabStrip.SelectedTab.DisplayRectangle;
		}
		DrawBackground(tabStrip, clientRectangle, g, colorScheme, GetTabsRegion(tabs, lastVisibleTab), tabStrip.TabAlignment, selectedTabRect);
		bool first = true;
		foreach (TabItem item in tabs)
		{
			if (item.Visible)
			{
				if (item.DisplayRectangle.IntersectsWith(clientRectangle))
				{
					PaintTab(g, item, first, item == lastVisibleTab);
				}
				first = false;
			}
		}
	}

	protected override Region GetTabsRegion(TabsCollection tabs, TabItem lastTab)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		bool bFirst = true;
		Region val = new Region();
		val.MakeEmpty();
		foreach (TabItem tab in tabs)
		{
			if (tab.Visible)
			{
				GraphicsPath tabItemPath = GetTabItemPath(tab, bFirst, tab == lastTab);
				val.Union(tabItemPath);
				bFirst = false;
			}
		}
		return val;
	}

	protected override LinearGradientBrush CreateTabGradientBrush(Rectangle r, Color color1, Color color2, int gradientAngle)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		LinearGradientBrush val = base.CreateTabGradientBrush(r, color1, color2, gradientAngle);
		Blend val2 = new Blend(5);
		val2.Factors = new float[5] { 0f, 0.6f, 1f, 0.6f, 0f };
		val2.Positions = new float[5] { 0f, 0.3f, 0.5f, 0.7f, 1f };
		val.Blend = val2;
		return val;
	}

	protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Expected O, but got Unknown
		int num = 5;
		GraphicsPath val = new GraphicsPath();
		switch (tabAlignment)
		{
		case eTabStripAlignment.Top:
		{
			Rectangle bounds = new Rectangle(tabStripRect.X, tabStripRect.Y + tabStripRect.Height / 2, tabStripRect.Width - 1, tabStripRect.Height - tabStripRect.Height / 2);
			val.AddLine(bounds.X, bounds.Bottom, bounds.X, bounds.Top + num);
			ArcData cornerArc4 = GetCornerArc(bounds, num, eCornerArc.TopLeft);
			val.AddArc(cornerArc4.X, cornerArc4.Y, cornerArc4.Width, cornerArc4.Height, cornerArc4.StartAngle, cornerArc4.SweepAngle);
			val.AddLine(bounds.X + num, bounds.Y, bounds.Right - num, bounds.Y);
			cornerArc4 = GetCornerArc(bounds, num, eCornerArc.TopRight);
			val.AddArc(cornerArc4.X, cornerArc4.Y, cornerArc4.Width, cornerArc4.Height, cornerArc4.StartAngle, cornerArc4.SweepAngle);
			val.AddLine(bounds.Right, bounds.Y + num, bounds.Right, bounds.Bottom);
			break;
		}
		case eTabStripAlignment.Bottom:
		{
			Rectangle bounds = new Rectangle(tabStripRect.X, tabStripRect.Y, tabStripRect.Width - 1, tabStripRect.Height / 2);
			val.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Bottom - num);
			ArcData cornerArc3 = GetCornerArc(bounds, num, eCornerArc.BottomRight);
			val.AddArc(cornerArc3.X, cornerArc3.Y, cornerArc3.Width, cornerArc3.Height, cornerArc3.StartAngle, cornerArc3.SweepAngle);
			val.AddLine(bounds.Right - num, bounds.Bottom, bounds.X + num, bounds.Bottom);
			cornerArc3 = GetCornerArc(bounds, num, eCornerArc.BottomLeft);
			val.AddArc(cornerArc3.X, cornerArc3.Y, cornerArc3.Width, cornerArc3.Height, cornerArc3.StartAngle, cornerArc3.SweepAngle);
			val.AddLine(bounds.X, bounds.Bottom - num, bounds.X, bounds.Y);
			break;
		}
		case eTabStripAlignment.Left:
		{
			Rectangle bounds = new Rectangle(tabStripRect.X + tabStripRect.Width / 2, tabStripRect.Y, tabStripRect.Width / 2 + 1, tabStripRect.Height - 1);
			val.AddLine(bounds.Right, bounds.Bottom, bounds.X + num, bounds.Bottom);
			ArcData cornerArc2 = GetCornerArc(bounds, num, eCornerArc.BottomLeft);
			val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
			val.AddLine(bounds.X, bounds.Bottom - num, bounds.X, bounds.Y + num);
			cornerArc2 = GetCornerArc(bounds, num, eCornerArc.TopLeft);
			val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
			val.AddLine(bounds.X + num, bounds.Y, bounds.Right, bounds.Y);
			break;
		}
		case eTabStripAlignment.Right:
		{
			Rectangle bounds = new Rectangle(tabStripRect.X, tabStripRect.Y, tabStripRect.Width / 2, tabStripRect.Height - 1);
			val.AddLine(bounds.X, bounds.Y, bounds.Right - num, bounds.Y);
			ArcData cornerArc = GetCornerArc(bounds, num, eCornerArc.TopRight);
			val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
			val.AddLine(bounds.Right, bounds.Y + num, bounds.Right, bounds.Bottom - num);
			cornerArc = GetCornerArc(bounds, num, eCornerArc.BottomRight);
			val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
			val.AddLine(bounds.Right - num, bounds.Bottom, bounds.X, bounds.Bottom);
			break;
		}
		}
		object obj = val.Clone();
		GraphicsPath val2 = (GraphicsPath)((obj is GraphicsPath) ? obj : null);
		val2.CloseAllFigures();
		g.SetClip(val2, (CombineMode)0);
		g.SetClip(tabsRegion, (CombineMode)4);
		g.Clear(colors.TabPanelBackground);
		g.ResetClip();
		val2.Dispose();
		g.SetClip(tabsRegion, (CombineMode)4);
		Pen val3 = new Pen(colors.TabBorder, 1f);
		try
		{
			g.DrawPath(val3, val);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		val.Dispose();
		g.ResetClip();
	}

	protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		int num = 3;
		Rectangle displayRectangle = tab.DisplayRectangle;
		if (!bFirst && !bLast)
		{
			val.AddRectangle(displayRectangle);
		}
		else
		{
			if (bFirst)
			{
				if (tab.Parent.TabAlignment == eTabStripAlignment.Top || tab.Parent.TabAlignment == eTabStripAlignment.Bottom)
				{
					val.AddLine(displayRectangle.Right - (bLast ? num : 0), displayRectangle.Bottom, displayRectangle.X + num, displayRectangle.Bottom);
					ArcData cornerArc = GetCornerArc(displayRectangle, num, eCornerArc.BottomLeft);
					val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
					val.AddLine(displayRectangle.X, displayRectangle.Bottom - num, displayRectangle.X, displayRectangle.Y + num);
					cornerArc = GetCornerArc(displayRectangle, num, eCornerArc.TopLeft);
					val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
					val.AddLine(displayRectangle.X + num, displayRectangle.Y, displayRectangle.Right - (bLast ? num : 0), displayRectangle.Y);
				}
				else
				{
					ArcData cornerArc2 = GetCornerArc(displayRectangle, num, eCornerArc.TopLeft);
					val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
					val.AddLine(displayRectangle.X + num, displayRectangle.Y, displayRectangle.Right - num, displayRectangle.Y);
					cornerArc2 = GetCornerArc(displayRectangle, num, eCornerArc.TopRight);
					val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
					val.AddLine(displayRectangle.Right, displayRectangle.Y + num, displayRectangle.Right, displayRectangle.Bottom - (bLast ? num : 0));
				}
			}
			else
			{
				val.AddLine(displayRectangle.X, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
			}
			if (bLast)
			{
				if (tab.Parent.TabAlignment == eTabStripAlignment.Top || tab.Parent.TabAlignment == eTabStripAlignment.Bottom)
				{
					if (!bFirst)
					{
						val.AddLine(displayRectangle.X, displayRectangle.Y, displayRectangle.Right - num, displayRectangle.Y);
					}
					ArcData cornerArc3 = GetCornerArc(displayRectangle, num, eCornerArc.TopRight);
					val.AddArc(cornerArc3.X, cornerArc3.Y, cornerArc3.Width, cornerArc3.Height, cornerArc3.StartAngle, cornerArc3.SweepAngle);
					val.AddLine(displayRectangle.Right, displayRectangle.Y + num, displayRectangle.Right, displayRectangle.Bottom - num);
					cornerArc3 = GetCornerArc(displayRectangle, num, eCornerArc.BottomRight);
					val.AddArc(cornerArc3.X, cornerArc3.Y, cornerArc3.Width, cornerArc3.Height, cornerArc3.StartAngle, cornerArc3.SweepAngle);
					if (!bFirst)
					{
						val.AddLine(displayRectangle.Right - num, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
					}
				}
				else
				{
					if (!bFirst)
					{
						val.AddLine(displayRectangle.Right, displayRectangle.Y, displayRectangle.Right, displayRectangle.Bottom - num);
					}
					ArcData cornerArc4 = GetCornerArc(displayRectangle, num, eCornerArc.BottomRight);
					val.AddArc(cornerArc4.X, cornerArc4.Y, cornerArc4.Width, cornerArc4.Height, cornerArc4.StartAngle, cornerArc4.SweepAngle);
					val.AddLine(displayRectangle.Right - num, displayRectangle.Bottom, displayRectangle.X + num, displayRectangle.Bottom);
					cornerArc4 = GetCornerArc(displayRectangle, num, eCornerArc.BottomLeft);
					val.AddArc(cornerArc4.X, cornerArc4.Y, cornerArc4.Width, cornerArc4.Height, cornerArc4.StartAngle, cornerArc4.SweepAngle);
					if (!bFirst)
					{
						val.AddLine(displayRectangle.X, displayRectangle.Bottom - num, displayRectangle.X, displayRectangle.Y);
					}
				}
			}
			else
			{
				val.AddLine(displayRectangle.Right, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
			}
			val.CloseAllFigures();
		}
		return val;
	}
}
