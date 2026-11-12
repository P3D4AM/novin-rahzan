using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class TabStripBaseDisplay
{
	private bool m_AntiAlias = true;

	private bool m_HorizontalText;

	private bool m_CloseButtonOnTabs;

	private Size m_TabCloseButtonSize = new Size(11, 11);

	private int m_TabCloseButtonSpacing = 3;

	public virtual bool AntiAlias
	{
		get
		{
			return m_AntiAlias;
		}
		set
		{
			m_AntiAlias = value;
		}
	}

	public bool HorizontalText
	{
		get
		{
			return m_HorizontalText;
		}
		set
		{
			m_HorizontalText = value;
		}
	}

	public bool CloseButtonOnTabs
	{
		get
		{
			return m_CloseButtonOnTabs;
		}
		set
		{
			m_CloseButtonOnTabs = value;
		}
	}

	public int TabCloseButtonSpacing
	{
		get
		{
			return m_TabCloseButtonSpacing;
		}
		set
		{
			m_TabCloseButtonSpacing = value;
		}
	}

	public virtual void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (AntiAlias)
		{
			g.SmoothingMode = (SmoothingMode)4;
			g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
	}

	protected virtual void PaintTab(Graphics g, TabItem tab, bool first, bool last)
	{
		if (!tab.Visible)
		{
			return;
		}
		if (tab.Parent != null && tab.Parent.HasPreRenderTabItem)
		{
			RenderTabItemEventArgs renderTabItemEventArgs = new RenderTabItemEventArgs(tab, g);
			tab.Parent.InvokePreRenderTabItem(renderTabItemEventArgs);
			if (renderTabItemEventArgs.Cancel)
			{
				return;
			}
		}
		GraphicsPath tabItemPath = GetTabItemPath(tab, first, last);
		try
		{
			TabColors tabColors = tab.Parent.GetTabColors(tab);
			DrawTabItemBackground(tab, tabItemPath, tabColors, g);
			DrawTabText(tab, tabColors, g);
		}
		finally
		{
			((IDisposable)tabItemPath)?.Dispose();
		}
		if (tab.Parent != null && tab.Parent.HasPostRenderTabItem)
		{
			RenderTabItemEventArgs e = new RenderTabItemEventArgs(tab, g);
			tab.Parent.InvokePostRenderTabItem(e);
		}
	}

	protected virtual Region GetTabsRegion(TabsCollection tabs, TabItem lastTab)
	{
		return null;
	}

	protected virtual void DrawTabItemBackground(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		RectangleF bounds = path.GetBounds();
		Rectangle r = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		eBackgroundColorBlendType eBackgroundColorBlendType2 = colors.BackgroundColorBlend.GetBlendType();
		int num = colors.BackColorGradientAngle;
		if (tab.TabAlignment == eTabStripAlignment.Left || tab.TabAlignment == eTabStripAlignment.Right)
		{
			num -= 90;
		}
		switch (eBackgroundColorBlendType2)
		{
		case eBackgroundColorBlendType.Relative:
			if (colors.BackColor.IsEmpty && colors.BackColor2.IsEmpty)
			{
				break;
			}
			try
			{
				Rectangle r2 = Rectangle.Ceiling(bounds);
				r2.Inflate(1, 1);
				LinearGradientBrush val2 = DisplayHelp.CreateLinearGradientBrush(r2, colors.BackColor, colors.BackColor2, num);
				try
				{
					val2.InterpolationColors = colors.BackgroundColorBlend.GetColorBlend();
					g.FillPath((Brush)(object)val2, path);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			catch
			{
				eBackgroundColorBlendType2 = eBackgroundColorBlendType.Invalid;
			}
			break;
		default:
		{
			Rectangle rectangle = Rectangle.Ceiling(bounds);
			BackgroundColorBlendCollection backgroundColorBlend = colors.BackgroundColorBlend;
			Region clip = g.Clip;
			g.SetClip(path, (CombineMode)1);
			for (int i = 0; i < backgroundColorBlend.Count; i += 2)
			{
				BackgroundColorBlend backgroundColorBlend2 = backgroundColorBlend[i];
				BackgroundColorBlend backgroundColorBlend3 = null;
				if (i < backgroundColorBlend.Count)
				{
					backgroundColorBlend3 = backgroundColorBlend[i + 1];
				}
				if (backgroundColorBlend2 != null && backgroundColorBlend3 != null)
				{
					Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y + (int)backgroundColorBlend2.Position, rectangle.Width, ((backgroundColorBlend3.Position == 1f) ? rectangle.Height : ((int)backgroundColorBlend3.Position)) - (int)backgroundColorBlend2.Position);
					LinearGradientBrush val = DisplayHelp.CreateLinearGradientBrush(rectangle2, backgroundColorBlend2.Color, backgroundColorBlend3.Color, num);
					try
					{
						g.FillRectangle((Brush)(object)val, rectangle2);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			g.Clip = clip;
			break;
		}
		case eBackgroundColorBlendType.Invalid:
			break;
		}
		if (eBackgroundColorBlendType2 == eBackgroundColorBlendType.Invalid)
		{
			if (colors.BackColor2.IsEmpty)
			{
				if (!colors.BackColor.IsEmpty)
				{
					SolidBrush val3 = new SolidBrush(colors.BackColor);
					try
					{
						g.FillPath((Brush)(object)val3, path);
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
					g.FillPath((Brush)(object)val4, path);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				LinearGradientBrush val5 = CreateTabGradientBrush(r, colors.BackColor, colors.BackColor2, num);
				try
				{
					g.FillPath((Brush)(object)val5, path);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
		}
		g.SmoothingMode = smoothingMode;
		DrawTabBorder(tab, path, colors, g);
	}

	protected virtual void DrawTabBorder(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		if (!colors.BorderColor.IsEmpty)
		{
			Pen val = new Pen(colors.BorderColor, 1f);
			try
			{
				g.DrawPath(val, path);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	protected virtual LinearGradientBrush CreateTabGradientBrush(Rectangle r, Color color1, Color color2, int gradientAngle)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (r.Width <= 0)
		{
			r.Width = 1;
		}
		if (r.Height <= 0)
		{
			r.Height = 1;
		}
		return new LinearGradientBrush(new Rectangle(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, (float)gradientAngle);
	}

	private Rectangle DrawCloseButton(Graphics g, bool vertical, bool mouseOver, ref Rectangle tabRect, TabStrip tabStrip)
	{
		return DrawCloseButton(g, vertical, mouseOver, ref tabRect, tabStrip, performPaint: true);
	}

	private Rectangle DrawCloseButton(Graphics g, bool vertical, bool mouseOver, ref Rectangle tabRect, TabStrip tabStrip, bool performPaint)
	{
		Size tabCloseButtonSize = m_TabCloseButtonSize;
		int tabCloseButtonSpacing = m_TabCloseButtonSpacing;
		bool flag = tabStrip.CloseButtonPosition == eTabCloseButtonPosition.Left;
		Rectangle empty = Rectangle.Empty;
		empty = (flag ? ((!vertical) ? new Rectangle(tabRect.X + tabCloseButtonSpacing, tabRect.Y + (tabRect.Height - tabCloseButtonSize.Height) / 2, tabCloseButtonSize.Width, tabCloseButtonSize.Height) : new Rectangle(tabRect.X + (tabRect.Width - tabCloseButtonSize.Width) / 2, tabRect.Y + tabCloseButtonSpacing, tabCloseButtonSize.Width, tabCloseButtonSize.Height)) : ((!vertical) ? new Rectangle(tabRect.Right - tabCloseButtonSpacing - tabCloseButtonSize.Width, tabRect.Y + (tabRect.Height - tabCloseButtonSize.Height) / 2, tabCloseButtonSize.Width, tabCloseButtonSize.Height) : new Rectangle(tabRect.X + (tabRect.Width - tabCloseButtonSize.Width) / 2, tabRect.Bottom - tabCloseButtonSpacing - tabCloseButtonSize.Height, tabCloseButtonSize.Width, tabCloseButtonSize.Height)));
		AdjustCloseButtonRectangle(ref empty, flag, vertical);
		if (performPaint)
		{
			PaintTabItemCloseButton(g, empty, mouseOver, tabStrip);
		}
		if (vertical)
		{
			if (flag)
			{
				tabRect.Y += empty.Height + tabCloseButtonSpacing * 2;
			}
			tabRect.Height -= empty.Height + tabCloseButtonSpacing * 2;
		}
		else
		{
			if (flag)
			{
				tabRect.X += empty.Width + tabCloseButtonSpacing * 2;
			}
			tabRect.Width -= empty.Width + tabCloseButtonSpacing * 2;
		}
		return empty;
	}

	protected virtual void AdjustCloseButtonRectangle(ref Rectangle close, bool closeOnLeftSide, bool vertical)
	{
	}

	protected virtual void DrawTabText(TabItem tab, TabColors colors, Graphics g)
	{
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		int num = 4;
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		eTabStripAlignment tabAlignment = tab.Parent.TabAlignment;
		Rectangle tabRect = tab.DisplayRectangle;
		bool flag = (tabAlignment == eTabStripAlignment.Left || tabAlignment == eTabStripAlignment.Right) && !m_HorizontalText;
		if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
		{
			if (flag)
			{
				tabRect.Y++;
				tabRect.Height--;
			}
			else
			{
				tabRect.X += 2;
				tabRect.Width -= 2;
			}
			bool performPaint = tab.IsSelected || tab.IsMouseOver || tab.Parent.CloseButtonOnTabsAlwaysDisplayed;
			tab.CloseButtonBounds = DrawCloseButton(g, flag, tab.CloseButtonMouseOver, ref tabRect, tab.Parent, performPaint);
		}
		CompositeImage tabImage = GetTabImage(tab);
		if (tabImage != null && tabImage.Width + 4 <= tabRect.Width)
		{
			if (tabAlignment == eTabStripAlignment.Top || tabAlignment == eTabStripAlignment.Bottom || m_HorizontalText)
			{
				tabImage.DrawImage(g, new Rectangle(tabRect.X + 3, tabRect.Y + (tabRect.Height - tabImage.Height) / 2, tabImage.Width, tabImage.Height));
				int num2 = tabImage.Width + 2;
				tabRect.X += num2;
				tabRect.Width -= num2;
			}
			else
			{
				tabImage.DrawImage(g, new Rectangle(tabRect.X + (tabRect.Width - tabImage.Width) / 2, tabRect.Y + 3, tabImage.Width, tabImage.Height));
				int num3 = tabImage.Height + 2;
				tabRect.Y += num3;
				tabRect.Height -= num3;
			}
		}
		tabImage?.Dispose();
		bool flag2 = tab == tab.Parent.SelectedTab;
		if (!(!tab.Parent.DisplaySelectedTextOnly || flag2))
		{
			return;
		}
		if (!tab.Parent.AntiAlias)
		{
			g.TextRenderingHint = (TextRenderingHint)0;
		}
		Font font = ((Control)tab.Parent).Font;
		if (flag2 && tab.Parent.SelectedTabFont != null)
		{
			font = tab.Parent.SelectedTabFont;
		}
		AdjustTextRectangle(ref tabRect, tabAlignment);
		if (flag)
		{
			g.RotateTransform(90f);
			tabRect = new Rectangle(tabRect.Top, -tabRect.Right, tabRect.Height, tabRect.Width);
		}
		if (tabRect.Width > num)
		{
			if ((tabAlignment == eTabStripAlignment.Left || tabAlignment == eTabStripAlignment.Right) && !m_HorizontalText)
			{
				TextDrawing.DrawStringLegacy(g, tab.Text, font, colors.TextColor, tabRect, format);
			}
			else
			{
				TextDrawing.DrawString(g, tab.Text, font, colors.TextColor, tabRect, format);
			}
		}
		if (flag)
		{
			g.ResetTransform();
		}
		if (!tab.Parent.AntiAlias)
		{
			g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		if (tab.Parent.ShowFocusRectangle && ((Control)tab.Parent).Focused && flag2)
		{
			ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(tab.DisplayRectangle));
		}
	}

	protected virtual void AdjustTextRectangle(ref Rectangle rText, eTabStripAlignment tabAlignment)
	{
	}

	protected virtual CompositeImage GetTabImage(TabItem tab)
	{
		Image image = tab.GetImage();
		if (image != null)
		{
			return new CompositeImage(image, dispose: false);
		}
		Icon icon = tab.Icon;
		if (icon != null)
		{
			return new CompositeImage(icon, dispose: false, tab.IconSize);
		}
		return null;
	}

	protected virtual Rectangle GetFocusRectangle(Rectangle rText)
	{
		rText.Inflate(-1, -1);
		return rText;
	}

	protected virtual TabItem GetLastVisibleTab(TabsCollection tabs)
	{
		int num = tabs.Count - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			if (tabs[num2].Visible)
			{
				return tabs[num2];
			}
		}
		return null;
	}

	protected virtual void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		if (colors.TabBackground2.IsEmpty)
		{
			if (!colors.TabBackground.IsEmpty)
			{
				SolidBrush val = new SolidBrush(colors.TabBackground);
				try
				{
					g.FillRegion((Brush)(object)val, tabsRegion);
					return;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			return;
		}
		LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(tabsRegion.GetBounds(g), colors.TabBackground, colors.TabBackground2, colors.TabBackgroundGradientAngle);
		try
		{
			g.FillRegion((Brush)(object)val2, tabsRegion);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	protected virtual GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		return null;
	}

	protected virtual ArcData GetCornerArc(Rectangle bounds, int cornerDiameter, eCornerArc corner)
	{
		int num = cornerDiameter * 2;
		return corner switch
		{
			eCornerArc.TopLeft => new ArcData(bounds.X, bounds.Y, num, num, 180f, 90f), 
			eCornerArc.TopRight => new ArcData(bounds.Right - num, bounds.Y, num, num, 270f, 90f), 
			eCornerArc.BottomLeft => new ArcData(bounds.X, bounds.Bottom - num, num, num, 90f, 90f), 
			_ => new ArcData(bounds.Right - num, bounds.Bottom - num, num, num, 0f, 90f), 
		};
	}

	public static void PaintTabItemCloseButton(Graphics g, Rectangle r, bool mouseOver, TabStrip tabStrip)
	{
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		if (tabStrip.TabCloseButtonNormal != null)
		{
			Image val = tabStrip.TabCloseButtonNormal;
			if (mouseOver && tabStrip.TabCloseButtonHot != null)
			{
				val = tabStrip.TabCloseButtonHot;
			}
			g.DrawImageUnscaled(val, r);
			return;
		}
		Color color = tabStrip.ColorScheme.TabItemBorder;
		Color color2 = tabStrip.ColorScheme.TabItemBorderLight;
		if (!color.IsEmpty && (double)Math.Abs(color.GetBrightness() - color2.GetBrightness()) <= 0.38)
		{
			color2 = BarFunctions.Ligten(color2, 60);
			color = BarFunctions.Darken(color, 40);
		}
		if (color.IsEmpty)
		{
			if (!tabStrip.ColorScheme.TabItemSelectedBorder.IsEmpty)
			{
				color = tabStrip.ColorScheme.TabItemSelectedBorder;
				if (!tabStrip.ColorScheme.TabItemSelectedBorderLight.IsEmpty)
				{
					color2 = tabStrip.ColorScheme.TabItemSelectedBorderLight;
				}
			}
			else
			{
				color = SystemColors.ControlDark;
				color2 = SystemColors.ControlLightLight;
			}
		}
		if (color2.IsEmpty)
		{
			color2 = SystemColors.ControlLightLight;
		}
		color = Color.FromArgb(mouseOver ? 255 : 200, color);
		if (tabStrip.Style == eTabStripStyle.Metro && !mouseOver)
		{
			color2 = color;
			color = Color.Empty;
		}
		if (!color.IsEmpty)
		{
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)4;
			Pen val2 = new Pen(Color.White, 1f);
			try
			{
				g.DrawEllipse(val2, r);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			Pen val3 = new Pen(color, 1f);
			try
			{
				g.DrawEllipse(val3, r);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			SolidBrush val4 = new SolidBrush(Color.White);
			try
			{
				g.FillEllipse((Brush)(object)val4, r);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			SolidBrush val5 = new SolidBrush(color);
			try
			{
				g.FillEllipse((Brush)(object)val5, r);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			g.SmoothingMode = smoothingMode;
		}
		Pen val6 = new Pen(color2, 1f);
		try
		{
			Rectangle rectangle = r;
			rectangle.Inflate(-3, -3);
			rectangle.Width--;
			g.DrawLine(val6, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
			g.DrawLine(val6, rectangle.X, rectangle.Bottom, rectangle.Right, rectangle.Y);
			rectangle.Offset(1, 0);
			g.DrawLine(val6, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
			g.DrawLine(val6, rectangle.X, rectangle.Bottom, rectangle.Right, rectangle.Y);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
	}
}
