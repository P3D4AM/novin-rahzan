using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class TabStripOffice2007DockDisplay : TabStripBaseDisplay
{
	private int m_CornerSize = 1;

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
		for (int num = tabStrip.Tabs.Count - 1; num >= 0; num--)
		{
			TabItem tabItem = tabStrip.Tabs[num];
			if (tabItem.Visible && tabItem != tabStrip.SelectedTab)
			{
				if (!tabItem.DisplayRectangle.IntersectsWith(systemBoxRectangle))
				{
					if (tabItem.DisplayRectangle.IntersectsWith(displayRectangle))
					{
						PaintTab(g, tabItem, first: false, last: false);
					}
				}
				else
				{
					Region clip = g.Clip;
					g.SetClip(systemBoxRectangle, (CombineMode)4);
					PaintTab(g, tabItem, first: false, last: false);
					g.Clip = clip;
				}
			}
		}
		if (tabStrip.SelectedTab != null && tabStrip.Tabs.Contains(tabStrip.SelectedTab))
		{
			if (!tabStrip.SelectedTab.DisplayRectangle.IntersectsWith(systemBoxRectangle))
			{
				PaintTab(g, tabStrip.SelectedTab, first: false, last: false);
			}
			else
			{
				Region clip2 = g.Clip;
				g.SetClip(systemBoxRectangle, (CombineMode)4);
				PaintTab(g, tabStrip.SelectedTab, first: false, last: false);
				g.Clip = clip2;
			}
		}
		g.ResetClip();
		tabStrip.PaintTabSystemBox(g);
	}

	protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		return GetTabItemPath(tab.DisplayRectangle, tab.TabAlignment, closePath: true);
	}

	private GraphicsPath GetTabItemPath(Rectangle tabRect, eTabStripAlignment align, bool closePath)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		switch (align)
		{
		case eTabStripAlignment.Top:
			if (closePath)
			{
				tabRect.Height += 2;
			}
			val.AddLine(tabRect.X, tabRect.Bottom, tabRect.X, tabRect.Y + m_CornerSize);
			val.AddLine(tabRect.X, tabRect.Y + m_CornerSize, tabRect.X + m_CornerSize, tabRect.Y);
			val.AddLine(tabRect.X + m_CornerSize, tabRect.Y, tabRect.Right - m_CornerSize, tabRect.Y);
			val.AddLine(tabRect.Right - m_CornerSize, tabRect.Y, tabRect.Right, tabRect.Y + m_CornerSize);
			val.AddLine(tabRect.Right, tabRect.Y + m_CornerSize, tabRect.Right, tabRect.Bottom);
			break;
		case eTabStripAlignment.Bottom:
			if (!closePath)
			{
				tabRect.Y++;
			}
			tabRect.Height--;
			val.AddLine(tabRect.X, tabRect.Y, tabRect.X, tabRect.Bottom - m_CornerSize);
			val.AddLine(tabRect.X, tabRect.Bottom - m_CornerSize, tabRect.X + m_CornerSize, tabRect.Bottom);
			val.AddLine(tabRect.X + m_CornerSize, tabRect.Bottom, tabRect.Right - m_CornerSize, tabRect.Bottom);
			val.AddLine(tabRect.Right - m_CornerSize, tabRect.Bottom, tabRect.Right, tabRect.Bottom - m_CornerSize);
			val.AddLine(tabRect.Right, tabRect.Bottom - m_CornerSize, tabRect.Right, tabRect.Y);
			break;
		case eTabStripAlignment.Left:
			tabRect.X--;
			tabRect.Height--;
			if (closePath)
			{
				tabRect.Width++;
			}
			val.AddLine(tabRect.Right, tabRect.Y, tabRect.X + m_CornerSize, tabRect.Y);
			val.AddLine(tabRect.X + m_CornerSize, tabRect.Y, tabRect.X, tabRect.Y + m_CornerSize);
			val.AddLine(tabRect.X, tabRect.Y + m_CornerSize, tabRect.X, tabRect.Bottom - m_CornerSize);
			val.AddLine(tabRect.X, tabRect.Bottom - m_CornerSize, tabRect.X + m_CornerSize, tabRect.Bottom);
			val.AddLine(tabRect.X + m_CornerSize, tabRect.Bottom, tabRect.Right, tabRect.Bottom);
			break;
		case eTabStripAlignment.Right:
			if (!closePath)
			{
				tabRect.X++;
			}
			val.AddLine(tabRect.X, tabRect.Y, tabRect.Right - m_CornerSize, tabRect.Y);
			val.AddLine(tabRect.Right - m_CornerSize, tabRect.Y, tabRect.Right, tabRect.Y + m_CornerSize);
			val.AddLine(tabRect.Right, tabRect.Y + m_CornerSize, tabRect.Right, tabRect.Bottom - m_CornerSize);
			val.AddLine(tabRect.Right, tabRect.Bottom - m_CornerSize, tabRect.Right - m_CornerSize, tabRect.Bottom);
			val.AddLine(tabRect.Right - m_CornerSize, tabRect.Bottom, tabRect.X, tabRect.Bottom);
			break;
		}
		if (closePath)
		{
			val.CloseAllFigures();
		}
		return val;
	}

	protected override void DrawTabBorder(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		Rectangle displayRectangle = tab.DisplayRectangle;
		eTabStripAlignment tabAlignment = tab.TabAlignment;
		if (!colors.BorderColor.IsEmpty)
		{
			GraphicsPath tabItemPath = GetTabItemPath(displayRectangle, tabAlignment, closePath: false);
			try
			{
				Pen val = new Pen(colors.BorderColor);
				try
				{
					g.DrawPath(val, tabItemPath);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)tabItemPath)?.Dispose();
			}
		}
		switch (tabAlignment)
		{
		case eTabStripAlignment.Top:
			displayRectangle.Offset(1, 1);
			displayRectangle.Width -= 2;
			displayRectangle.Height--;
			break;
		case eTabStripAlignment.Bottom:
			displayRectangle.Offset(1, 0);
			displayRectangle.Height--;
			displayRectangle.Width -= 2;
			break;
		case eTabStripAlignment.Left:
			displayRectangle.Offset(1, 1);
			displayRectangle.Height -= 2;
			displayRectangle.Width -= 2;
			break;
		case eTabStripAlignment.Right:
			displayRectangle.Offset(1, 1);
			displayRectangle.Height -= 2;
			displayRectangle.Width -= 2;
			break;
		}
		if (colors.LightBorderColor.IsEmpty)
		{
			return;
		}
		GraphicsPath tabItemPath2 = GetTabItemPath(displayRectangle, tabAlignment, closePath: false);
		try
		{
			Pen val2 = new Pen(colors.LightBorderColor);
			try
			{
				g.DrawPath(val2, tabItemPath2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)tabItemPath2)?.Dispose();
		}
	}

	protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		base.DrawBackground(tabStrip, tabStripRect, g, colors, tabsRegion, tabAlignment, selectedTabRect);
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		if (!colors.TabItemBorder.IsEmpty)
		{
			Pen val = new Pen(colors.TabItemBorder, 1f);
			try
			{
				switch (tabAlignment)
				{
				case eTabStripAlignment.Top:
				{
					if (selectedTabRect.X > tabStripRect.Right)
					{
						selectedTabRect = Rectangle.Empty;
					}
					Rectangle bounds3 = new Rectangle(tabStripRect.X, tabStripRect.Bottom - 1, tabStripRect.Width, 2);
					DisplayHelp.FillRectangle(g, bounds3, colors.TabItemBackground, Color.Empty);
					g.DrawLine(val, bounds3.X, bounds3.Y, selectedTabRect.X - 1, bounds3.Y);
					g.DrawLine(val, selectedTabRect.Right, bounds3.Y, bounds3.Right, bounds3.Y);
					g.DrawLine(val, bounds3.X, bounds3.Y, bounds3.X, bounds3.Bottom);
					g.DrawLine(val, bounds3.Right - 1, bounds3.Y, bounds3.Right - 1, bounds3.Bottom);
					break;
				}
				case eTabStripAlignment.Bottom:
				{
					if (selectedTabRect.X > tabStripRect.Right)
					{
						selectedTabRect = Rectangle.Empty;
					}
					Rectangle bounds2 = new Rectangle(tabStripRect.X, tabStripRect.Y, tabStripRect.Width, 1);
					DisplayHelp.FillRectangle(g, bounds2, colors.TabItemBackground, Color.Empty);
					g.DrawLine(val, bounds2.X, bounds2.Bottom - 1, selectedTabRect.X - 1, bounds2.Bottom - 1);
					g.DrawLine(val, selectedTabRect.Right, bounds2.Bottom - 1, bounds2.Right, bounds2.Bottom - 1);
					g.DrawLine(val, bounds2.X, bounds2.Y, bounds2.X, bounds2.Bottom - 1);
					g.DrawLine(val, bounds2.Right - 1, bounds2.Y, bounds2.Right - 1, bounds2.Bottom - 1);
					break;
				}
				case eTabStripAlignment.Left:
				{
					if (selectedTabRect.Y > tabStripRect.Bottom)
					{
						selectedTabRect = Rectangle.Empty;
					}
					Rectangle bounds4 = new Rectangle(tabStripRect.Right - 1, tabStripRect.Y, 1, tabStripRect.Height);
					DisplayHelp.FillRectangle(g, bounds4, colors.TabItemBackground, Color.Empty);
					g.DrawLine(val, bounds4.X, bounds4.Y, bounds4.X, selectedTabRect.Y - 1);
					g.DrawLine(val, bounds4.X, selectedTabRect.Bottom, bounds4.X, bounds4.Bottom);
					g.DrawLine(val, bounds4.X, bounds4.Y, bounds4.Right, bounds4.Y);
					g.DrawLine(val, bounds4.X, bounds4.Bottom - 1, bounds4.Right, bounds4.Bottom - 1);
					break;
				}
				case eTabStripAlignment.Right:
				{
					if (selectedTabRect.Y > tabStripRect.Bottom)
					{
						selectedTabRect = Rectangle.Empty;
					}
					Rectangle bounds = new Rectangle(tabStripRect.X, tabStripRect.Y, 1, tabStripRect.Height);
					DisplayHelp.FillRectangle(g, bounds, colors.TabItemBackground, Color.Empty);
					g.DrawLine(val, bounds.Right - 1, bounds.Y, bounds.Right - 1, selectedTabRect.Y - 1);
					g.DrawLine(val, bounds.Right - 1, selectedTabRect.Bottom, bounds.Right - 1, bounds.Bottom);
					g.DrawLine(val, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
					g.DrawLine(val, bounds.X, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
					break;
				}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		g.SmoothingMode = smoothingMode;
	}
}
