using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class TabStripSimulatedThemeDisplay : TabStripBaseDisplay
{
	public TabStripSimulatedThemeDisplay()
	{
		base.HorizontalText = true;
	}

	public override void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		base.Paint(g, tabStrip);
		TabColorScheme colorScheme = tabStrip.ColorScheme;
		Rectangle clientRectangle = ((Control)tabStrip).ClientRectangle;
		if (((Control)tabStrip).BackColor != Color.Transparent || colorScheme.TabBackground != Color.Transparent)
		{
			if (colorScheme.TabPanelBackground2.IsEmpty)
			{
				if (!colorScheme.TabPanelBackground.IsEmpty)
				{
					SolidBrush val = new SolidBrush(colorScheme.TabPanelBackground);
					try
					{
						g.FillRectangle((Brush)(object)val, clientRectangle);
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
					g.FillRectangle((Brush)(object)val2, clientRectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				LinearGradientBrush val3 = CreateTabGradientBrush(clientRectangle, colorScheme.TabPanelBackground, colorScheme.TabPanelBackground2, colorScheme.TabPanelBackgroundGradientAngle);
				try
				{
					g.FillRectangle((Brush)(object)val3, clientRectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		tabStrip.ClipExcludeSystemBox(g);
		DrawBackgroundInternal(clientRectangle, g, colorScheme, new Region(((Control)tabStrip).DisplayRectangle), tabStrip.TabAlignment, tabStrip.TabItemsBounds);
		for (int num = tabStrip.Tabs.Count - 1; num >= 0; num--)
		{
			TabItem tabItem = tabStrip.Tabs[num];
			if (tabItem.Visible && !tabItem.IsSelected && tabItem.DisplayRectangle.IntersectsWith(clientRectangle))
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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		Rectangle displayRectangle = tab.DisplayRectangle;
		GraphicsPath val = new GraphicsPath();
		if (!tab.IsSelected && !tab.IsMouseOver)
		{
			if (tab.TabAlignment == eTabStripAlignment.Bottom)
			{
				displayRectangle.Height--;
			}
			else if (tab.TabAlignment == eTabStripAlignment.Top)
			{
				displayRectangle.Height--;
				displayRectangle.Y++;
			}
			else if (tab.TabAlignment == eTabStripAlignment.Left)
			{
				displayRectangle.Width--;
				displayRectangle.X++;
			}
			else if (tab.TabAlignment == eTabStripAlignment.Right)
			{
				displayRectangle.Width--;
			}
			val.AddRectangle(displayRectangle);
		}
		else
		{
			if (tab.TabAlignment == eTabStripAlignment.Bottom)
			{
				val.AddLine(displayRectangle.X + 2, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom - 2);
				val.AddLine(displayRectangle.X, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
				val.AddLine(displayRectangle.Right, displayRectangle.Bottom - 2, displayRectangle.Right - 2, displayRectangle.Bottom);
				val.AddLine(displayRectangle.Right - 2, displayRectangle.Bottom, displayRectangle.X + 2, displayRectangle.Bottom);
			}
			else if (tab.TabAlignment == eTabStripAlignment.Top)
			{
				val.AddLine(displayRectangle.X, displayRectangle.Y + 2, displayRectangle.X + 2, displayRectangle.Y);
				val.AddLine(displayRectangle.Right - 2, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y + 2);
				val.AddLine(displayRectangle.Right, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
			}
			else if (tab.TabAlignment == eTabStripAlignment.Left)
			{
				val.AddLine(displayRectangle.Right, displayRectangle.Bottom, displayRectangle.X + 2, displayRectangle.Bottom);
				val.AddLine(displayRectangle.X + 2, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom - 2);
				val.AddLine(displayRectangle.X, displayRectangle.Bottom - 2, displayRectangle.X, displayRectangle.Y + 2);
				val.AddLine(displayRectangle.X, displayRectangle.Y + 2, displayRectangle.X + 2, displayRectangle.Y);
				val.AddLine(displayRectangle.Right - 1, displayRectangle.Y, displayRectangle.Right - 1, displayRectangle.Bottom);
			}
			else if (tab.TabAlignment == eTabStripAlignment.Right)
			{
				val.AddLine(displayRectangle.X, displayRectangle.Y, displayRectangle.Right - 2, displayRectangle.Y);
				val.AddLine(displayRectangle.Right - 2, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y + 2);
				val.AddLine(displayRectangle.Right, displayRectangle.Y + 2, displayRectangle.Right, displayRectangle.Bottom - 2);
				val.AddLine(displayRectangle.Right, displayRectangle.Bottom - 2, displayRectangle.Right - 2, displayRectangle.Bottom);
				val.AddLine(displayRectangle.X, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Y);
			}
			val.CloseAllFigures();
		}
		return val;
	}

	private GraphicsPath GetTabEdge(TabItem tab)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle displayRectangle = tab.DisplayRectangle;
		if (tab.TabAlignment == eTabStripAlignment.Bottom)
		{
			displayRectangle.Height++;
			val.AddLine(displayRectangle.Right, displayRectangle.Bottom - 2, displayRectangle.Right - 2, displayRectangle.Bottom);
			val.AddLine(displayRectangle.Right - 2, displayRectangle.Bottom, displayRectangle.X + 2, displayRectangle.Bottom);
			val.AddLine(displayRectangle.X + 2, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom - 2);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Top)
		{
			displayRectangle.Height++;
			displayRectangle.Y--;
			val.AddLine(displayRectangle.X, displayRectangle.Y + 2, displayRectangle.X + 2, displayRectangle.Y);
			val.AddLine(displayRectangle.X + 2, displayRectangle.Y, displayRectangle.Right - 2, displayRectangle.Y);
			val.AddLine(displayRectangle.Right - 2, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y + 2);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Left)
		{
			displayRectangle.Width++;
			displayRectangle.X--;
			val.AddLine(displayRectangle.X + 2, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom - 2);
			val.AddLine(displayRectangle.X, displayRectangle.Bottom - 2, displayRectangle.X, displayRectangle.Y + 2);
			val.AddLine(displayRectangle.X, displayRectangle.Y + 2, displayRectangle.X + 2, displayRectangle.Y);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Right)
		{
			displayRectangle.Width++;
			val.AddLine(displayRectangle.Right - 2, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y + 2);
			val.AddLine(displayRectangle.Right, displayRectangle.Y + 2, displayRectangle.Right, displayRectangle.Bottom - 2);
			val.AddLine(displayRectangle.Right, displayRectangle.Bottom - 2, displayRectangle.Right - 2, displayRectangle.Bottom);
		}
		return val;
	}

	private void DrawBackgroundInternal(Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle tabItemsBounds)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Expected O, but got Unknown
		int num = 3;
		GraphicsPath val = new GraphicsPath();
		GraphicsPath val2 = null;
		if (!tabItemsBounds.IsEmpty)
		{
			Rectangle bounds;
			switch (tabAlignment)
			{
			case eTabStripAlignment.Top:
			{
				tabItemsBounds.Width += 3;
				bounds = new Rectangle(tabStripRect.X, tabItemsBounds.Y + 1, (tabItemsBounds.Right > tabStripRect.Right) ? tabStripRect.Width : tabItemsBounds.Right, tabItemsBounds.Height - 1);
				val.AddLine(bounds.X, bounds.Bottom, bounds.X, bounds.Top + num);
				ArcData cornerArc3 = GetCornerArc(bounds, num, eCornerArc.TopLeft);
				val.AddArc(cornerArc3.X, cornerArc3.Y, cornerArc3.Width, cornerArc3.Height, cornerArc3.StartAngle, cornerArc3.SweepAngle);
				val.AddLine(bounds.X + num, bounds.Y, bounds.Right - num, bounds.Y);
				cornerArc3 = GetCornerArc(bounds, num, eCornerArc.TopRight);
				val.AddArc(cornerArc3.X, cornerArc3.Y, cornerArc3.Width, cornerArc3.Height, cornerArc3.StartAngle, cornerArc3.SweepAngle);
				val.AddLine(bounds.Right, bounds.Y + num, bounds.Right, bounds.Bottom);
				object obj3 = val.Clone();
				val2 = (GraphicsPath)((obj3 is GraphicsPath) ? obj3 : null);
				val2.CloseAllFigures();
				if (tabItemsBounds.Right < tabStripRect.Right)
				{
					val.AddLine(bounds.Right, bounds.Bottom, tabStripRect.Right, bounds.Bottom);
				}
				break;
			}
			case eTabStripAlignment.Bottom:
			{
				tabItemsBounds.Width += 3;
				bounds = new Rectangle(tabStripRect.X, tabStripRect.Y, (tabItemsBounds.Right > tabStripRect.Right) ? tabStripRect.Width : tabItemsBounds.Right, tabItemsBounds.Height - 1);
				val.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Bottom - num);
				ArcData cornerArc4 = GetCornerArc(bounds, num, eCornerArc.BottomRight);
				val.AddArc(cornerArc4.X, cornerArc4.Y, cornerArc4.Width, cornerArc4.Height, cornerArc4.StartAngle, cornerArc4.SweepAngle);
				val.AddLine(bounds.Right - num, bounds.Bottom, bounds.X + num, bounds.Bottom);
				cornerArc4 = GetCornerArc(bounds, num, eCornerArc.BottomLeft);
				val.AddArc(cornerArc4.X, cornerArc4.Y, cornerArc4.Width, cornerArc4.Height, cornerArc4.StartAngle, cornerArc4.SweepAngle);
				val.AddLine(bounds.X, bounds.Bottom - num, bounds.X, bounds.Y);
				object obj4 = val.Clone();
				val2 = (GraphicsPath)((obj4 is GraphicsPath) ? obj4 : null);
				val2.CloseAllFigures();
				if (tabItemsBounds.Right < tabStripRect.Right)
				{
					val.AddLine(bounds.Right, bounds.Y, tabStripRect.Right, bounds.Y);
				}
				break;
			}
			case eTabStripAlignment.Left:
			{
				tabItemsBounds.Height += 3;
				bounds = new Rectangle(tabItemsBounds.X + 1, tabStripRect.Y, tabItemsBounds.Width - 1, (tabItemsBounds.Bottom > tabStripRect.Bottom) ? tabStripRect.Height : tabItemsBounds.Bottom);
				val.AddLine(bounds.Right - 1, bounds.Bottom, bounds.X + num, bounds.Bottom);
				ArcData cornerArc2 = GetCornerArc(bounds, num, eCornerArc.BottomLeft);
				val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
				val.AddLine(bounds.X, bounds.Bottom - num, bounds.X, bounds.Y + num);
				cornerArc2 = GetCornerArc(bounds, num, eCornerArc.TopLeft);
				val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
				val.AddLine(bounds.X + num, bounds.Y, bounds.Right - 1, bounds.Y);
				object obj2 = val.Clone();
				val2 = (GraphicsPath)((obj2 is GraphicsPath) ? obj2 : null);
				val2.CloseAllFigures();
				if (tabItemsBounds.Bottom < tabStripRect.Bottom)
				{
					val.AddLine(bounds.Right - 1, bounds.Bottom, bounds.Right - 1, tabStripRect.Bottom);
				}
				break;
			}
			case eTabStripAlignment.Right:
			{
				tabItemsBounds.Height += 3;
				bounds = new Rectangle(tabStripRect.X, tabStripRect.Y, tabItemsBounds.Width - 1, (tabItemsBounds.Bottom > tabStripRect.Bottom) ? tabStripRect.Height : tabItemsBounds.Bottom);
				val.AddLine(bounds.X, bounds.Y, bounds.Right - num, bounds.Y);
				ArcData cornerArc = GetCornerArc(bounds, num, eCornerArc.TopRight);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
				val.AddLine(bounds.Right, bounds.Y + num, bounds.Right, bounds.Bottom - num);
				cornerArc = GetCornerArc(bounds, num, eCornerArc.BottomRight);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
				val.AddLine(bounds.Right - num, bounds.Bottom, bounds.X, bounds.Bottom);
				object obj = val.Clone();
				val2 = (GraphicsPath)((obj is GraphicsPath) ? obj : null);
				val2.CloseAllFigures();
				if (tabItemsBounds.Bottom < tabStripRect.Bottom)
				{
					val.AddLine(bounds.X, bounds.Bottom, bounds.X, tabStripRect.Bottom);
				}
				break;
			}
			}
		}
		else
		{
			val.AddRectangle(tabStripRect);
		}
		if (colors.TabPanelBackground2.IsEmpty)
		{
			if (!colors.TabPanelBackground.IsEmpty)
			{
				SolidBrush val3 = new SolidBrush(colors.TabPanelBackground);
				try
				{
					g.FillPath((Brush)(object)val3, val);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		else
		{
			SolidBrush val4 = new SolidBrush(Color.White);
			try
			{
				g.FillPath((Brush)(object)val4, val);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			LinearGradientBrush val5 = CreateTabGradientBrush(tabStripRect, colors.TabPanelBackground, colors.TabPanelBackground2, colors.TabPanelBackgroundGradientAngle);
			try
			{
				g.FillPath((Brush)(object)val5, val);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		if (!colors.TabBorder.IsEmpty)
		{
			val.CloseAllFigures();
			Pen val6 = new Pen(colors.TabBorder, 1f);
			try
			{
				g.DrawPath(val6, val);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
		if (val != null)
		{
			val.Dispose();
		}
	}

	protected override void DrawTabItemBackground(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Expected O, but got Unknown
		RectangleF bounds = path.GetBounds();
		Rectangle r = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
		if (colors.BackColor2.IsEmpty)
		{
			if (!colors.BackColor.IsEmpty)
			{
				SolidBrush val = new SolidBrush(colors.BackColor);
				try
				{
					g.FillPath((Brush)(object)val, path);
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
				g.FillPath((Brush)(object)val2, path);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			LinearGradientBrush val3 = CreateTabGradientBrush(r, colors.BackColor, colors.BackColor2, colors.BackColorGradientAngle);
			try
			{
				g.FillPath((Brush)(object)val3, path);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Pen val4 = new Pen(colors.BorderColor, 1f);
		try
		{
			if (!colors.BorderColor.IsEmpty && (tab.IsSelected || tab.IsMouseOver))
			{
				g.DrawPath(val4, path);
			}
			else if (tab.TabAlignment == eTabStripAlignment.Top || tab.TabAlignment == eTabStripAlignment.Bottom)
			{
				if (!colors.LightBorderColor.IsEmpty)
				{
					Pen val5 = new Pen(colors.LightBorderColor, 1f);
					try
					{
						g.DrawLine(val5, r.Right - 1, r.Y + 4, r.Right - 1, r.Bottom - 4);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				if (!colors.DarkBorderColor.IsEmpty)
				{
					Pen val6 = new Pen(colors.DarkBorderColor, 1f);
					try
					{
						g.DrawLine(val6, r.Right, r.Y + 4, r.Right, r.Bottom - 4);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				g.DrawLine(val4, r.X, r.Y, r.Right, r.Y);
				g.DrawLine(val4, r.X, r.Bottom, r.Right, r.Bottom);
			}
			else if (tab.TabAlignment == eTabStripAlignment.Left || tab.TabAlignment == eTabStripAlignment.Right)
			{
				if (!colors.LightBorderColor.IsEmpty)
				{
					Pen val7 = new Pen(colors.LightBorderColor, 1f);
					try
					{
						g.DrawLine(val7, r.X + 4, r.Bottom - 1, r.Right - 4, r.Bottom - 1);
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
				}
				if (!colors.DarkBorderColor.IsEmpty)
				{
					Pen val8 = new Pen(colors.DarkBorderColor, 1f);
					try
					{
						g.DrawLine(val8, r.X + 4, r.Bottom, r.Right - 4, r.Bottom);
					}
					finally
					{
						((IDisposable)val8)?.Dispose();
					}
				}
				g.DrawLine(val4, r.X, r.Y, r.X, r.Bottom);
				if (tab.TabAlignment == eTabStripAlignment.Left)
				{
					g.DrawLine(val4, r.Right - 1, r.Y, r.Right - 1, r.Bottom);
				}
				else
				{
					g.DrawLine(val4, r.Right, r.Y, r.Right, r.Bottom);
				}
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		if (!tab.IsSelected && !tab.IsMouseOver)
		{
			return;
		}
		GraphicsPath tabEdge = GetTabEdge(tab);
		if (!colors.LightBorderColor.IsEmpty)
		{
			object obj = tabEdge.Clone();
			GraphicsPath val9 = (GraphicsPath)((obj is GraphicsPath) ? obj : null);
			val9.CloseAllFigures();
			SolidBrush val10 = new SolidBrush(colors.LightBorderColor);
			try
			{
				g.FillPath((Brush)(object)val10, val9);
			}
			finally
			{
				((IDisposable)val10)?.Dispose();
			}
			val9.Dispose();
		}
		if (!colors.DarkBorderColor.IsEmpty)
		{
			Pen val11 = new Pen(colors.DarkBorderColor, 1f);
			try
			{
				g.DrawPath(val11, tabEdge);
			}
			finally
			{
				((IDisposable)val11)?.Dispose();
			}
		}
		tabEdge.Dispose();
	}
}
