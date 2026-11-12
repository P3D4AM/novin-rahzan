using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007BarBackgroundPainter : BarBackgroundPainter, IOffice2007Painter
{
	private float m_TopSplit = 0.4f;

	private Office2007ColorTable m_ColorTable;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			m_ColorTable = value;
		}
	}

	public override void PaintDockedBackground(ToolbarRendererEventArgs e)
	{
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Expected O, but got Unknown
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Expected O, but got Unknown
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Bar bar = e.Bar;
		ItemPaintArgs itemPaintArgs = e.ItemPaintArgs;
		Rectangle bounds = e.Bounds;
		ColorScheme legacyColors = m_ColorTable.LegacyColors;
		if (bar.LayoutType == eLayoutType.DockContainer || bar.LayoutType == eLayoutType.TaskList)
		{
			if (bar.HasFocus)
			{
				DisplayHelp.FillRectangle(graphics, bounds, legacyColors.BarBackground, legacyColors.BarBackground2, legacyColors.BarBackgroundGradientAngle);
			}
			else
			{
				DisplayHelp.FillRectangle(graphics, bounds, legacyColors.BarCaptionInactiveBackground, legacyColors.BarCaptionInactiveBackground2, legacyColors.BarCaptionInactiveBackgroundGAngle);
			}
		}
		else if (bar.MenuBar)
		{
			DisplayHelp.FillRectangle(graphics, bounds, legacyColors.MenuBarBackground, legacyColors.MenuBarBackground2, legacyColors.MenuBarBackgroundGradientAngle);
		}
		else if (bar.GrabHandleStyle != eGrabHandleStyle.ResizeHandle && bar.BarType != eBarType.StatusBar)
		{
			if (bar.ItemsContainer.BackgroundColor.IsEmpty && ((Control)bar).BackColor != Color.Transparent)
			{
				if (bar.IsThemed)
				{
					Rectangle r = new Rectangle(-bar.Location.X, -bar.Location.Y, ((Control)bar).Parent.Width, ((Control)bar).Parent.Height);
					ThemeRebar themeRebar = ((IThemeCache)bar).ThemeRebar;
					themeRebar.DrawBackground(graphics, ThemeRebarParts.Background, ThemeRebarStates.Normal, r);
				}
				else if (IsGradientBackground(bar))
				{
					DisplayHelp.FillRectangle(graphics, bounds, legacyColors.BarBackground, legacyColors.BarBackground2, legacyColors.BarBackgroundGradientAngle, new float[3] { 0f, 0.12f, 1f }, new float[3] { 0f, 0.5f, 1f });
				}
				else
				{
					DisplayHelp.FillRectangle(graphics, bounds, legacyColors.BarBackground);
				}
			}
			else if (!bar.ItemsContainer.BackColor.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, bounds, bar.ItemsContainer.BackColor);
			}
			if (((Control)bar).Parent != null && ((Control)bar).Parent.BackgroundImage != null && ((Control)bar).Parent is DockSite)
			{
				Rectangle targetRect = new Rectangle(-bar.Location.X, -bar.Location.Y, ((Control)bar).Parent.Width, ((Control)bar).Parent.Height);
				DockSite dockSite = ((Control)bar).Parent as DockSite;
				BarFunctions.PaintBackgroundImage(graphics, targetRect, ((Control)dockSite).BackgroundImage, dockSite.BackgroundImagePosition, dockSite.BackgroundImageAlpha);
			}
			else if (((Control)bar).BackgroundImage != null)
			{
				BarFunctions.PaintBackgroundImage(graphics, bounds, ((Control)bar).BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);
			}
			if (!bar.IsThemed && bar.LayoutType == eLayoutType.Toolbar && ((Control)bar).BackColor != Color.Transparent && itemPaintArgs != null)
			{
				Pen val = new Pen(itemPaintArgs.Colors.BarDockedBorder, 1f);
				try
				{
					graphics.DrawLine(val, 0, ((Control)bar).Height - 1, ((Control)bar).Width, ((Control)bar).Height - 1);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				Rectangle r2 = bounds;
				r2.Inflate(-2, -2);
				BarFunctions.DrawBorder(graphics, bar.DockedBorderStyle, r2, bar.SingleLineColor);
			}
		}
		else if (!((Control)bar).BackColor.IsEmpty && bar.ShouldSerializeBackColor())
		{
			DisplayHelp.FillRectangle(graphics, bounds, ((Control)bar).BackColor);
		}
		else
		{
			Office2007BarColorTable bar2 = m_ColorTable.Bar;
			Rectangle r3 = bounds;
			if (!itemPaintArgs.CachedPaint)
			{
				r3.Inflate(1, 1);
			}
			if (bar2.ToolbarBottomBackground == null || bar2.ToolbarBottomBackground.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, r3, bar2.ToolbarTopBackground);
			}
			else
			{
				r3.Height = (int)((float)r3.Height * m_TopSplit);
				r3.Height++;
				DisplayHelp.FillRectangle(graphics, r3, bar2.ToolbarTopBackground);
				r3.Height--;
				r3.Y += r3.Height;
				r3.Height = bounds.Height - r3.Height + 1;
				DisplayHelp.FillRectangle(graphics, r3, bar2.ToolbarBottomBackground);
			}
			if (bar.BarType == eBarType.StatusBar && bar.Items.Count > 0 && bar.Items[bar.Items.Count - 1] is ItemContainer && e.ItemPaintArgs != null && e.ItemPaintArgs.CachedPaint)
			{
				ItemContainer itemContainer = bar.Items[bar.Items.Count - 1] as ItemContainer;
				if (itemContainer.Visible && itemContainer.BackgroundStyle.Class == ElementStyleClassKeys.Office2007StatusBarBackground2Key)
				{
					Rectangle bounds2 = new Rectangle(itemContainer.Bounds.X, bounds.Y, itemContainer.Bounds.Width, ((Control)e.Bar).Height + 1);
					if (e.ItemPaintArgs.RightToLeft)
					{
						bounds2.Width += bounds2.X;
						bounds2.X = 0;
					}
					else
					{
						bounds2.X += e.ItemPaintArgs.ContainerControl.Left;
						bounds2.Width += bounds.Right - bounds2.Right;
					}
					ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(itemContainer.BackgroundStyle, graphics, bounds2));
				}
			}
			if (((Control)bar).Parent != null && ((Control)bar).Parent.BackgroundImage != null && ((Control)bar).Parent is DockSite)
			{
				Rectangle targetRect2 = new Rectangle(-bar.Location.X, -bar.Location.Y, ((Control)bar).Parent.Width, ((Control)bar).Parent.Height);
				DockSite dockSite2 = ((Control)bar).Parent as DockSite;
				BarFunctions.PaintBackgroundImage(graphics, targetRect2, ((Control)dockSite2).BackgroundImage, dockSite2.BackgroundImagePosition, dockSite2.BackgroundImageAlpha);
			}
			else if (((Control)bar).BackgroundImage != null)
			{
				BarFunctions.PaintBackgroundImage(graphics, ((Control)bar).ClientRectangle, ((Control)bar).BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);
			}
			if ((!bar2.ToolbarBottomBorder.IsEmpty && bar.BarType != eBarType.StatusBar) || (!bar2.StatusBarTopBorder.IsEmpty && bar.BarType == eBarType.StatusBar))
			{
				if (bar.BarType == eBarType.StatusBar)
				{
					Pen val2 = new Pen(bar2.StatusBarTopBorder, 1f);
					try
					{
						graphics.DrawLine(val2, bounds.X, bounds.Y, bounds.Right, bounds.Y);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					if (!bar2.StatusBarTopBorderLight.IsEmpty)
					{
						Pen val3 = new Pen(bar2.StatusBarTopBorderLight, 1f);
						try
						{
							graphics.DrawLine(val3, bounds.X, bounds.Y + 1, bounds.Right, bounds.Y + 1);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
				}
				else
				{
					Pen val4 = new Pen(bar2.ToolbarBottomBorder, 1f);
					try
					{
						graphics.DrawLine(val4, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
			}
		}
		if (itemPaintArgs != null && !itemPaintArgs.CachedPaint)
		{
			bar.PaintGrabHandle(itemPaintArgs);
		}
	}

	private bool IsGradientBackground(Bar bar)
	{
		if (bar.Style == eDotNetBarStyle.VS2005 && bar.LayoutType == eLayoutType.DockContainer)
		{
			return false;
		}
		return true;
	}

	public override void PaintFloatingBackground(ToolbarRendererEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Bar bar = e.Bar;
		ItemPaintArgs itemPaintArgs = e.ItemPaintArgs;
		Rectangle bounds = e.Bounds;
		ColorScheme legacyColors = m_ColorTable.LegacyColors;
		DisplayHelp.FillRectangle(graphics, bounds, legacyColors.BarBackground, legacyColors.BarBackground2, legacyColors.BarBackgroundGradientAngle, new float[3] { 0f, 0.12f, 1f }, new float[3] { 0f, 0.5f, 1f });
		if (((Control)bar).BackgroundImage != null)
		{
			BarFunctions.PaintBackgroundImage(graphics, ((Control)bar).ClientRectangle, ((Control)bar).BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);
		}
	}

	public override void PaintPopupBackground(ToolbarRendererEventArgs e)
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Bar bar = e.Bar;
		ItemPaintArgs itemPaintArgs = e.ItemPaintArgs;
		Office2007BarColorTable bar2 = m_ColorTable.Bar;
		int cornerSize = bar.CornerSize;
		Rectangle bounds = e.Bounds;
		DisplayHelp.FillRectangle(graphics, bounds, bar2.PopupToolbarBackground.Start, bar2.PopupToolbarBackground.End, bar2.PopupToolbarBackground.GradientAngle);
		if (((Control)bar).BackgroundImage != null)
		{
			BarFunctions.PaintBackgroundImage(graphics, ((Control)bar).ClientRectangle, ((Control)bar).BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);
		}
		bar.PaintSideBar(graphics);
		Rectangle bounds2 = bounds;
		if (bar.DisplayShadow && !bar.AlphaShadow)
		{
			bounds2 = new Rectangle(0, 0, ((Control)bar).ClientSize.Width - 2, ((Control)bar).ClientSize.Height - 2);
		}
		Pen val = new Pen(bar2.PopupToolbarBorder, 1f);
		try
		{
			DisplayHelp.DrawRoundedRectangle(graphics, val, bounds2, cornerSize);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (bar.DisplayShadow && !bar.AlphaShadow)
		{
			Point[] array = new Point[3];
			array[0].X = 2;
			array[0].Y = bounds.Height - 1;
			array[1].X = bounds.Width - 1;
			array[1].Y = bounds.Height - 1;
			array[2].X = bounds.Width - 1;
			array[2].Y = 2;
			Pen val2 = new Pen(SystemColors.ControlDark, 2f);
			try
			{
				graphics.DrawLines(val2, array);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}
}
