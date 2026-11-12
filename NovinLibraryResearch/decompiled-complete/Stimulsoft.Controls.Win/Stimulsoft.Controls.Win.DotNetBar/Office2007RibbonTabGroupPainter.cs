using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007RibbonTabGroupPainter : RibbonTabGroupPainter, IOffice2007Painter
{
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

	protected virtual ThemeTextFormat GetThemeFormat()
	{
		return (ThemeTextFormat)32805;
	}

	protected virtual eTextFormat GetTextFormat()
	{
		return eTextFormat.VerticalCenter;
	}

	protected virtual Themes.RECT GetThemeTextBounds(Rectangle rect)
	{
		return new Themes.RECT(new Rectangle(0, 0, rect.Width, rect.Height));
	}

	protected virtual Rectangle GetTextBounds(RibbonTabGroupRendererEventArgs e)
	{
		return e.Bounds;
	}

	public override void PaintTabGroup(RibbonTabGroupRendererEventArgs e)
	{
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Office2007RibbonTabGroupColorTable colors = GetColors(e.RibbonTabItemGroup);
		if (colors == null)
		{
			return;
		}
		if (e.ItemPaintArgs.GlassEnabled)
		{
			if (e.ItemPaintArgs.CachedPaint)
			{
				return;
			}
			PaintTabGroupBackground(graphics, colors, e.Bounds, e.GroupBounds, glassEnabled: true);
			Rectangle bounds = e.Bounds;
			bounds.Height -= 2;
			IntPtr hdc = graphics.GetHdc();
			Font groupFont = e.GroupFont;
			string groupTitle = e.RibbonTabItemGroup.GroupTitle;
			ThemeTextFormat themeFormat = GetThemeFormat();
			try
			{
				IntPtr intPtr = WinApi.CreateCompatibleDC(hdc);
				try
				{
					WinApi.BITMAPINFO bITMAPINFO = new WinApi.BITMAPINFO();
					bITMAPINFO.biWidth = bounds.Width;
					bITMAPINFO.biHeight = -bounds.Height;
					bITMAPINFO.biPlanes = 1;
					bITMAPINFO.biBitCount = 32;
					bITMAPINFO.biSize = Marshal.SizeOf(bITMAPINFO);
					IntPtr hObject = WinApi.CreateDIBSection(hdc, bITMAPINFO, 0u, 0, IntPtr.Zero, 0u);
					WinApi.SelectObject(intPtr, hObject);
					IntPtr hObject2 = groupFont.ToHfont();
					WinApi.SelectObject(intPtr, hObject2);
					Themes.RECT pRect = GetThemeTextBounds(bounds);
					VisualStyleRenderer val = new VisualStyleRenderer(Caption.Active);
					Themes.DTTOPTS options = default(Themes.DTTOPTS);
					options.iGlowSize = 10;
					options.crText = new Themes.COLORREF(colors.Text);
					options.dwFlags = 8193;
					if (colors.Background == null || colors.Background.IsEmpty || colors.Background.Start.A < byte.MaxValue)
					{
						options.dwFlags |= 2048;
					}
					options.dwSize = Marshal.SizeOf(options);
					Graphics val2 = Graphics.FromHdc(intPtr);
					try
					{
						PaintTabGroupBackground(val2, colors, new Rectangle(0, 0, bounds.Width, bounds.Height + 2), new Rectangle(0, 0, bounds.Width, bounds.Height + 2), glassEnabled: true);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					Themes.DrawThemeTextEx(val.Handle, intPtr, 0, 0, groupTitle, -1, (int)themeFormat, ref pRect, ref options);
					WinApi.BitBlt(hdc, bounds.Left, bounds.Top, bounds.Width, bounds.Height, intPtr, 0, 0, 13369376u);
					WinApi.DeleteObject(hObject2);
					WinApi.DeleteObject(hObject);
					return;
				}
				finally
				{
					WinApi.DeleteDC(intPtr);
				}
			}
			finally
			{
				graphics.ReleaseHdc(hdc);
			}
		}
		PaintTabGroupBackground(graphics, colors, e.Bounds, e.GroupBounds, glassEnabled: false);
		ElementStyle style = e.RibbonTabItemGroup.Style;
		Color textColor = style.TextColor;
		Color textShadowColor = style.TextShadowColor;
		Point textShadowOffset = style.TextShadowOffset;
		style.FreezeEvents = true;
		style.TextColor = colors.Text;
		style.TextShadowColor = Color.Empty;
		style.TextShadowOffset = Point.Empty;
		style.TextAlignment = eStyleTextAlignment.Center;
		ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(style, e.Graphics, GetTextBounds(e));
		ElementStyleDisplay.PaintText(e2, e.RibbonTabItemGroup.GroupTitle, e.GroupFont, useDefaultFont: false, e.RibbonTabItemGroup.Style.TextFormat | GetTextFormat());
		style.TextColor = textColor;
		style.TextShadowColor = textShadowColor;
		style.TextShadowOffset = textShadowOffset;
		style.FreezeEvents = false;
	}

	protected virtual void PaintTabGroupBackground(Graphics g, Office2007RibbonTabGroupColorTable colorTable, Rectangle bounds, Rectangle groupBounds, bool glassEnabled)
	{
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		if (colorTable != null)
		{
			Rectangle rectangle = bounds;
			rectangle.Height -= 2;
			Rectangle rectangle2 = rectangle;
			rectangle2.Width--;
			DisplayHelp.FillRectangle(g, rectangle2, colorTable.Background.Start, colorTable.Background.End, 90, new float[3] { 0f, 0.1f, 0.9f }, new float[3]
			{
				0f,
				glassEnabled ? 0.4f : 0.7f,
				1f
			});
			if (!colorTable.BackgroundHighlight.IsEmpty && rectangle.Width > 0 && rectangle.Height > 0)
			{
				Rectangle rectangle3 = new Rectangle(rectangle.X - rectangle.Width * 3, rectangle.Y, rectangle.Width * 7, (int)((float)rectangle.Height * 4.5f));
				GraphicsPath val = new GraphicsPath();
				val.AddEllipse(rectangle3);
				PathGradientBrush val2 = new PathGradientBrush(val);
				val2.CenterColor = colorTable.BackgroundHighlight.Start;
				val2.SurroundColors = new Color[1] { colorTable.BackgroundHighlight.End };
				val2.CenterPoint = new PointF(rectangle3.X + rectangle3.Width / 2, rectangle.Bottom);
				Blend val3 = new Blend();
				val3.Factors = new float[3] { 0f, 0.05f, 1f };
				val3.Positions = new float[3] { 0f, 0.8f, 1f };
				val2.Blend = val3;
				val.Dispose();
				g.FillRectangle((Brush)(object)val2, rectangle2);
				((Brush)val2).Dispose();
				val.Dispose();
			}
			Pen val4 = new Pen(Color.FromArgb(64, ControlPaint.Dark(colorTable.BackgroundHighlight.Start)));
			try
			{
				g.DrawLine(val4, rectangle.X, rectangle.Bottom, rectangle.Right - 1, rectangle.Bottom);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			rectangle = bounds;
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)0;
			Rectangle rectangle4 = groupBounds;
			if (!colorTable.Border.IsEmpty)
			{
				DisplayHelp.FillRectangle(g, new Rectangle(rectangle4.X, rectangle.Y, 1, rectangle.Height), colorTable.Border);
				DisplayHelp.FillRectangle(g, new Rectangle(rectangle4.Right - 1, rectangle.Y, 1, rectangle.Height), colorTable.Border);
				DisplayHelp.FillRectangle(g, new Rectangle(rectangle4.X, rectangle.Bottom - 1, 1, rectangle4.Height - rectangle.Height), colorTable.Border.End, Color.Transparent, 90);
				DisplayHelp.FillRectangle(g, new Rectangle(rectangle4.Right - 1, rectangle.Bottom - 1, 1, rectangle4.Height - rectangle.Height), colorTable.Border.End, Color.Transparent, 90);
			}
			g.SmoothingMode = smoothingMode;
		}
	}

	private Office2007RibbonTabGroupColorTable GetColors(RibbonTabItemGroup group)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = null;
		if (group.CustomColorName != "")
		{
			office2007RibbonTabGroupColorTable = m_ColorTable.RibbonTabGroupColors[group.CustomColorName];
		}
		if (office2007RibbonTabGroupColorTable == null)
		{
			office2007RibbonTabGroupColorTable = m_ColorTable.RibbonTabGroupColors[Enum.GetName(typeof(eRibbonTabGroupColor), group.Color)];
		}
		if (office2007RibbonTabGroupColorTable == null && m_ColorTable.RibbonTabGroupColors.Count > 0)
		{
			office2007RibbonTabGroupColorTable = m_ColorTable.RibbonTabGroupColors[0];
		}
		return office2007RibbonTabGroupColorTable;
	}
}
