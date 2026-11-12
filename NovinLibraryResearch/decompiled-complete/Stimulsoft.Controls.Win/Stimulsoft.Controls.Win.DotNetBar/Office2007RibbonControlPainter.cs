using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007RibbonControlPainter : RibbonControlPainter, IOffice2007Painter
{
	private class RectangleWidthComparer : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			if (x is Rectangle && y is Rectangle)
			{
				return ((Rectangle)x).Width - ((Rectangle)y).Width;
			}
			return new CaseInsensitiveComparer().Compare(x, y);
		}
	}

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

	public override void PaintBackground(RibbonControlRendererEventArgs e)
	{
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		RibbonControl ribbonControl = e.RibbonControl;
		Office2007RibbonColorTable ribbonControl2 = m_ColorTable.RibbonControl;
		Office2007RibbonTabItemStateColorTable office2007RibbonTabItemStateColorTable = null;
		PaintCaptionBackground(e, ((Control)ribbonControl).DisplayRectangle);
		if (ribbonControl.Expanded && ribbonControl.RibbonStrip.HasVisibleTabs)
		{
			int cornerSize = ribbonControl2.CornerSize;
			Rectangle empty = Rectangle.Empty;
			if (ribbonControl.SelectedRibbonTabItem == null || !ribbonControl.Expanded)
			{
				empty = Rectangle.Empty;
			}
			else
			{
				empty = ribbonControl.SelectedRibbonTabItem.DisplayRectangle;
				office2007RibbonTabItemStateColorTable = Office2007RibbonTabItemPainter.GetStateColorTable(Office2007RibbonTabItemPainter.GetColorTable(ColorTable, ribbonControl.SelectedRibbonTabItem), ribbonControl.SelectedRibbonTabItem, ribbonControl.Expanded);
			}
			Rectangle bottomBounds = new Rectangle(((Control)ribbonControl).ClientRectangle.X, ((Control)ribbonControl).ClientRectangle.Bottom - cornerSize, ((Control)ribbonControl).ClientRectangle.Width - 1, cornerSize);
			if (!empty.IsEmpty)
			{
				bottomBounds = new Rectangle(((Control)ribbonControl).ClientRectangle.X, empty.Bottom, ((Control)ribbonControl).ClientRectangle.Width - 1, ((Control)ribbonControl).ClientRectangle.Bottom - empty.Bottom);
			}
			GraphicsPath bottomLinePath = GetBottomLinePath(bottomBounds, cornerSize);
			try
			{
				Region val = null;
				if (graphics.Clip != null)
				{
					val = graphics.Clip;
				}
				if (!empty.IsEmpty)
				{
					if (office2007RibbonTabItemStateColorTable != null)
					{
						Pen val2 = new Pen(office2007RibbonTabItemStateColorTable.Background.End.IsEmpty ? office2007RibbonTabItemStateColorTable.Background.Start : office2007RibbonTabItemStateColorTable.Background.End, 1f);
						try
						{
							graphics.DrawLine(val2, empty.X + 1, empty.Bottom, empty.Right - 1, empty.Bottom);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
					graphics.SetClip(new Rectangle(empty.X + 1, empty.Bottom, empty.Width - 1, 1), (CombineMode)4);
					if (ribbonControl.EffectiveStyle == eDotNetBarStyle.Metro && ribbonControl.GetApplicationButton() is ApplicationButton { Displayed: not false, Bounds: var bounds } applicationButton)
					{
						graphics.TranslateTransform(0f, 1f);
						applicationButton.Paint(e.ItemPaintArgs);
						graphics.TranslateTransform(0f, -1f);
						bounds.Width -= 3;
						graphics.SetClip(bounds, (CombineMode)4);
					}
				}
				Pen val3 = new Pen(ribbonControl2.OuterBorder.Start, 1f);
				try
				{
					graphics.DrawPath(val3, bottomLinePath);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				if (!empty.IsEmpty)
				{
					if (val != null)
					{
						graphics.Clip = val;
					}
					else
					{
						graphics.ResetClip();
					}
				}
				if (val != null)
				{
					val.Dispose();
				}
			}
			finally
			{
				((IDisposable)bottomLinePath)?.Dispose();
			}
			bottomBounds.Y++;
			if (ribbonControl2.InnerBorder == null)
			{
				return;
			}
			GraphicsPath bottomLinePath2 = GetBottomLinePath(bottomBounds, cornerSize);
			try
			{
				Pen val4 = new Pen(ribbonControl2.InnerBorder.Start, 1f);
				try
				{
					graphics.DrawPath(val4, bottomLinePath2);
					return;
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)bottomLinePath2)?.Dispose();
			}
		}
		int num = ((Control)ribbonControl).ClientRectangle.Bottom - 2;
		if (!ribbonControl2.TabDividerBorder.IsEmpty)
		{
			Pen val5 = new Pen(ribbonControl2.TabDividerBorder, 1f);
			try
			{
				graphics.DrawLine(val5, 0, num, ((Control)ribbonControl).ClientRectangle.Right, num);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			num++;
		}
		if (!ribbonControl2.TabDividerBorderLight.IsEmpty)
		{
			Pen val6 = new Pen(ribbonControl2.TabDividerBorderLight, 1f);
			try
			{
				graphics.DrawLine(val6, 0, num, ((Control)ribbonControl).ClientRectangle.Right, num);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
	}

	public override void PaintCaptionBackground(RibbonControlRendererEventArgs e, Rectangle displayRect)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		RibbonControl ribbonControl = e.RibbonControl;
		Office2007RibbonColorTable ribbonControl2 = m_ColorTable.RibbonControl;
		int num = GetTabYPosition(ribbonControl.Items);
		if (num == -1)
		{
			num = ribbonControl.RibbonStrip.GetAbsoluteCaptionHeight() + StiScale2.I2;
		}
		if (num > StiScale2.I2)
		{
			num = ((!e.GlassEnabled || ribbonControl.EffectiveStyle != eDotNetBarStyle.Windows7) ? (num - StiScale2.I3) : (num - StiScale2.I2));
			if (!e.GlassEnabled)
			{
				if (!ribbonControl2.TabDividerBorder.IsEmpty)
				{
					Pen val = new Pen(ribbonControl2.TabDividerBorder, 1f);
					try
					{
						graphics.DrawLine(val, 0, num, displayRect.Right + 12, num);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					num++;
				}
				if (!ribbonControl2.TabDividerBorderLight.IsEmpty)
				{
					Pen val2 = new Pen(ribbonControl2.TabDividerBorderLight, 1f);
					try
					{
						graphics.DrawLine(val2, 0, num, displayRect.Right + 12, num);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
			}
			else if (ribbonControl.EffectiveStyle != eDotNetBarStyle.Office2010 && ribbonControl.EffectiveStyle != eDotNetBarStyle.Metro)
			{
				Pen val3 = new Pen(m_ColorTable.Form.Active.BorderColors[0], 1f);
				try
				{
					graphics.DrawLine(val3, 0, num, displayRect.Right + 12, num);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		if (!ribbonControl.RibbonStrip.CaptionVisible)
		{
			return;
		}
		Office2007FormStateColorTable office2007FormStateColorTable = m_ColorTable.Form.Active;
		Form val4 = ((Control)ribbonControl).FindForm();
		if (!ribbonControl.IsDesignMode && val4 != null && ((val4 != Form.ActiveForm && val4.MdiParent == null) || (val4.MdiParent != null && val4.MdiParent.ActiveMdiChild != val4)))
		{
			office2007FormStateColorTable = m_ColorTable.Form.Inactive;
		}
		Rectangle rectangle = new Rectangle(displayRect.X, displayRect.Y, displayRect.Width, num - StiScale2.I1);
		if (rectangle.Height <= 0)
		{
			rectangle.Height = ribbonControl.RibbonStrip.GetAbsoluteCaptionHeight();
		}
		if (!e.GlassEnabled)
		{
			Rectangle rectangle2 = rectangle;
			if (val4 is RibbonForm)
			{
				rectangle2.Width = ((Control)(RibbonForm)(object)val4).Width;
			}
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)0;
			if (!office2007FormStateColorTable.CaptionTopBackground.End.IsEmpty && !office2007FormStateColorTable.CaptionBottomBackground.End.IsEmpty)
			{
				LinearGradientBrush val5 = new LinearGradientBrush(rectangle2, office2007FormStateColorTable.CaptionTopBackground.Start, office2007FormStateColorTable.CaptionBottomBackground.End, (float)office2007FormStateColorTable.CaptionTopBackground.GradientAngle);
				try
				{
					ColorBlend val6 = new ColorBlend(4);
					val6.Colors = new Color[4]
					{
						office2007FormStateColorTable.CaptionTopBackground.Start,
						office2007FormStateColorTable.CaptionTopBackground.End,
						office2007FormStateColorTable.CaptionBottomBackground.Start,
						office2007FormStateColorTable.CaptionBottomBackground.End
					};
					val6.Positions = new float[4] { 0f, 0.4f, 0.4f, 1f };
					val5.InterpolationColors = val6;
					graphics.FillRectangle((Brush)(object)val5, rectangle2);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			else
			{
				LinearGradientBrush val7 = new LinearGradientBrush(rectangle2, office2007FormStateColorTable.CaptionTopBackground.Start, office2007FormStateColorTable.CaptionBottomBackground.Start, (float)office2007FormStateColorTable.CaptionTopBackground.GradientAngle);
				try
				{
					graphics.FillRectangle((Brush)(object)val7, rectangle2);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			graphics.SmoothingMode = smoothingMode;
		}
		ribbonControl.RibbonStrip.CaptionBounds = rectangle;
	}

	private GraphicsPath GetBottomLinePath(Rectangle bottomBounds, int cornerSize)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (cornerSize > 1)
		{
			if (bottomBounds.Height > cornerSize)
			{
				val.AddLine(bottomBounds.X, bottomBounds.Bottom, bottomBounds.X, bottomBounds.Y + cornerSize);
			}
			ElementStyleDisplay.AddCornerArc(val, bottomBounds, cornerSize, eCornerArc.TopLeft);
			val.AddLine(bottomBounds.X + cornerSize, bottomBounds.Y, bottomBounds.Right - cornerSize, bottomBounds.Y);
			ElementStyleDisplay.AddCornerArc(val, bottomBounds, cornerSize, eCornerArc.TopRight);
			if (bottomBounds.Height > cornerSize)
			{
				val.AddLine(bottomBounds.Right, bottomBounds.Y + cornerSize, bottomBounds.Right, bottomBounds.Bottom);
			}
		}
		else
		{
			val.AddLine(bottomBounds.X, bottomBounds.Y, bottomBounds.Right, bottomBounds.Y);
		}
		return val;
	}

	private int GetTabYPosition(SubItemsCollection c)
	{
		foreach (BaseItem item in c)
		{
			if (item is RibbonTabItem && item.Visible)
			{
				return item.Bounds.Top;
			}
		}
		return -1;
	}

	public override void PaintCaptionText(RibbonControlRendererEventArgs e)
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Invalid comparison between Unknown and I4
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Invalid comparison between Unknown and I4
		RibbonStrip ribbonStrip = e.RibbonControl.RibbonStrip;
		if (!ribbonStrip.CaptionVisible || ribbonStrip.CaptionBounds.IsEmpty)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		bool flag = false;
		bool flag2 = true;
		Office2007FormStateColorTable office2007FormStateColorTable = m_ColorTable.Form.Active;
		Form val = ((Control)ribbonStrip).FindForm();
		if (val != null && ((val != Form.ActiveForm && val.MdiParent == null) || (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)))
		{
			office2007FormStateColorTable = m_ColorTable.Form.Inactive;
			flag2 = false;
		}
		string titleText = e.RibbonControl.TitleText;
		string text = titleText;
		bool flag3 = e.RibbonControl.RibbonStrip.TitleTextMarkupBody != null;
		if (flag3)
		{
			text = e.RibbonControl.RibbonStrip.TitleTextMarkupBody.PlainText;
		}
		if (val != null)
		{
			if (titleText == "")
			{
				titleText = ((Control)val).Text;
				text = titleText;
			}
			flag = (int)val.WindowState == 2;
		}
		Rectangle captionRect = ribbonStrip.CaptionBounds;
		if (!ribbonStrip.QuickToolbarBounds.IsEmpty)
		{
			DisplayHelp.ExcludeEdgeRect(ref captionRect, ribbonStrip.QuickToolbarBounds);
		}
		else
		{
			BaseItem applicationButton = e.RibbonControl.GetApplicationButton();
			if (applicationButton != null && applicationButton.Visible && applicationButton.Displayed)
			{
				DisplayHelp.ExcludeEdgeRect(ref captionRect, applicationButton.Bounds);
			}
			else
			{
				DisplayHelp.ExcludeEdgeRect(ref captionRect, new Rectangle(0, 0, 22, 22));
			}
		}
		if (!ribbonStrip.SystemCaptionItemBounds.IsEmpty)
		{
			DisplayHelp.ExcludeEdgeRect(ref captionRect, ribbonStrip.SystemCaptionItemBounds);
		}
		ArrayList arrayList = new ArrayList(5);
		ArrayList arrayList2 = new ArrayList(5);
		if (ribbonStrip.TabGroupsVisible)
		{
			foreach (RibbonTabItemGroup tabGroup in ribbonStrip.TabGroups)
			{
				foreach (Rectangle displayPosition in tabGroup.DisplayPositions)
				{
					if (arrayList.Count > 0)
					{
						arrayList2.Clear();
						Rectangle[] array = new Rectangle[arrayList.Count];
						arrayList.CopyTo(array);
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i].IntersectsWith(displayPosition))
							{
								arrayList2.Add(i);
								Rectangle[] c = DisplayHelp.ExcludeRectangle(array[i], displayPosition);
								arrayList.AddRange(c);
							}
						}
						foreach (int item in arrayList2)
						{
							arrayList.RemoveAt(item);
						}
					}
					else if (displayPosition.IntersectsWith(captionRect))
					{
						Rectangle[] array2 = DisplayHelp.ExcludeRectangle(captionRect, displayPosition);
						if (array2.Length != 0)
						{
							arrayList.AddRange(array2);
							break;
						}
					}
				}
			}
		}
		Font captionFont = SystemFonts.CaptionFont;
		bool flag4 = true;
		if (ribbonStrip.CaptionFont != null)
		{
			captionFont.Dispose();
			captionFont = ribbonStrip.CaptionFont;
			flag4 = false;
		}
		Size empty = Size.Empty;
		empty = ((!flag3) ? TextDrawing.MeasureString(graphics, text, captionFont) : e.RibbonControl.RibbonStrip.TitleTextMarkupBody.Bounds.Size);
		if (arrayList.Count > 0)
		{
			ribbonStrip.CaptionTextBounds = (Rectangle[])arrayList.ToArray(typeof(Rectangle));
			if ((int)((Control)e.RibbonControl).RightToLeft == 0)
			{
				arrayList.Reverse();
			}
			captionRect = Rectangle.Empty;
			foreach (Rectangle item2 in arrayList)
			{
				if (item2.Width >= empty.Width)
				{
					captionRect = item2;
					break;
				}
				if (item2.Width > captionRect.Width)
				{
					captionRect = item2;
				}
			}
		}
		else
		{
			ribbonStrip.CaptionTextBounds = new Rectangle[1] { captionRect };
		}
		if (captionRect.Width > 6 && captionRect.Height > 6)
		{
			if (e.GlassEnabled && e.ItemPaintArgs != null && e.ItemPaintArgs.ThemeWindow != null && !e.RibbonControl.IsDesignMode)
			{
				if (!e.ItemPaintArgs.CachedPaint || flag)
				{
					PaintGlassText(graphics, text, captionFont, captionRect, flag);
				}
			}
			else if (!flag3)
			{
				TextDrawing.DrawString(graphics, text, captionFont, office2007FormStateColorTable.CaptionText, captionRect, eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.NoPrefix | eTextFormat.VerticalCenter);
			}
			else
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, captionFont, office2007FormStateColorTable.CaptionText, (int)((Control)e.RibbonControl).RightToLeft == 1, captionRect, hotKeyPrefixVisible: false);
				markupDrawContext.AllowMultiLine = false;
				markupDrawContext.IgnoreFormattingColors = !flag2;
				BodyElement titleTextMarkupBody = e.RibbonControl.RibbonStrip.TitleTextMarkupBody;
				if (e.RibbonControl.RibbonStrip.TitleTextMarkupLastArrangeBounds != captionRect)
				{
					titleTextMarkupBody.Measure(captionRect.Size, markupDrawContext);
					titleTextMarkupBody.Arrange(captionRect, markupDrawContext);
					e.RibbonControl.RibbonStrip.TitleTextMarkupLastArrangeBounds = captionRect;
					Rectangle bounds = titleTextMarkupBody.Bounds;
					if (bounds.Width < captionRect.Width)
					{
						bounds.Offset((captionRect.Width - bounds.Width) / 2, 0);
					}
					if (bounds.Height < captionRect.Height)
					{
						bounds.Offset(0, (captionRect.Height - bounds.Height) / 2);
					}
					titleTextMarkupBody.Bounds = bounds;
				}
				Region clip = graphics.Clip;
				graphics.SetClip(captionRect, (CombineMode)1);
				titleTextMarkupBody.Render(markupDrawContext);
				graphics.Clip = clip;
				if (clip != null)
				{
					clip.Dispose();
				}
			}
		}
		if (flag4)
		{
			captionFont.Dispose();
		}
	}

	public static void PaintTextOnGlass(Graphics g, string text, Font font, Rectangle rect, ThemeTextFormat tf)
	{
		PaintTextOnGlass(g, text, font, rect, tf, SystemColors.ControlText, copySourceBackground: false, renderGlow: true, 16);
	}

	public static void PaintTextOnGlass(Graphics g, string text, Font font, Rectangle rect, ThemeTextFormat tf, Color textColor, bool copySourceBackground, bool renderGlow, int glowSize)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		IntPtr hdc = g.GetHdc();
		try
		{
			IntPtr intPtr = WinApi.CreateCompatibleDC(hdc);
			try
			{
				WinApi.BITMAPINFO bITMAPINFO = new WinApi.BITMAPINFO();
				bITMAPINFO.biWidth = rect.Width;
				bITMAPINFO.biHeight = -rect.Height;
				bITMAPINFO.biPlanes = 1;
				bITMAPINFO.biBitCount = 32;
				bITMAPINFO.biSize = Marshal.SizeOf(bITMAPINFO);
				IntPtr hObject = WinApi.CreateDIBSection(hdc, bITMAPINFO, 0u, 0, IntPtr.Zero, 0u);
				WinApi.SelectObject(intPtr, hObject);
				IntPtr hObject2 = font.ToHfont();
				WinApi.SelectObject(intPtr, hObject2);
				Themes.RECT pRect = new Themes.RECT(new Rectangle(0, 0, rect.Width, rect.Height));
				try
				{
					if (VisualStyleRenderer.IsElementDefined(Caption.Active))
					{
						VisualStyleRenderer val = new VisualStyleRenderer(Caption.Active);
						Themes.DTTOPTS options = default(Themes.DTTOPTS);
						options.iGlowSize = glowSize;
						options.crText = new Themes.COLORREF(textColor);
						options.dwFlags = 0x2000 | (renderGlow ? 2048 : 0) | 1;
						options.dwSize = Marshal.SizeOf(options);
						if (copySourceBackground)
						{
							WinApi.BitBlt(intPtr, 0, 0, rect.Width, rect.Height, hdc, rect.Left, rect.Top, 13369376u);
						}
						Themes.DrawThemeTextEx(val.Handle, intPtr, 0, 0, text, -1, (int)tf, ref pRect, ref options);
						WinApi.BitBlt(hdc, rect.Left, rect.Top, rect.Width, rect.Height, intPtr, 0, 0, 13369376u);
					}
				}
				catch
				{
				}
				WinApi.DeleteObject(hObject2);
				WinApi.DeleteObject(hObject);
			}
			finally
			{
				WinApi.DeleteDC(intPtr);
			}
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
	}

	public static void PaintShadowTextOnGlass(Graphics g, string text, Font font, Rectangle rect, ThemeTextFormat tf, Color textColor, Color shadowColor, bool copySourceBackground)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		try
		{
			Point point = default(Point);
			Matrix transform = g.Transform;
			try
			{
				Point[] array = new Point[1] { point };
				transform.TransformPoints(array);
				point = array[0];
			}
			finally
			{
				((IDisposable)transform)?.Dispose();
			}
			IntPtr hdc = g.GetHdc();
			try
			{
				IntPtr intPtr = WinApi.CreateCompatibleDC(hdc);
				try
				{
					WinApi.BITMAPINFO bITMAPINFO = new WinApi.BITMAPINFO();
					bITMAPINFO.biWidth = rect.Width;
					bITMAPINFO.biHeight = -rect.Height;
					bITMAPINFO.biPlanes = 1;
					bITMAPINFO.biBitCount = 32;
					bITMAPINFO.biSize = Marshal.SizeOf(bITMAPINFO);
					IntPtr hObject = WinApi.CreateDIBSection(hdc, bITMAPINFO, 0u, 0, IntPtr.Zero, 0u);
					WinApi.SelectObject(intPtr, hObject);
					IntPtr hObject2 = font.ToHfont();
					WinApi.SelectObject(intPtr, hObject2);
					Themes.RECT pRect = new Themes.RECT(new Rectangle(0, 0, rect.Width, rect.Height));
					try
					{
						if (VisualStyleRenderer.IsElementDefined(Caption.Active))
						{
							VisualStyleRenderer val = new VisualStyleRenderer(Caption.Active);
							Themes.DTTOPTS options = default(Themes.DTTOPTS);
							options.crText = new Themes.COLORREF(shadowColor);
							options.dwFlags = 8193;
							options.dwSize = Marshal.SizeOf(options);
							if (copySourceBackground)
							{
								WinApi.BitBlt(intPtr, 0, 0, rect.Width, rect.Height, hdc, rect.Left + point.X, rect.Top + point.Y, 13369376u);
							}
							pRect.Offset(1, 2);
							Themes.DrawThemeTextEx(val.Handle, intPtr, 0, 0, text, -1, (int)tf, ref pRect, ref options);
							pRect.Offset(-1, -1);
							options.crText = new Themes.COLORREF(textColor);
							Themes.DrawThemeTextEx(val.Handle, intPtr, 0, 0, text, -1, (int)tf, ref pRect, ref options);
							WinApi.BitBlt(hdc, rect.Left + point.X, rect.Top + point.Y, rect.Width, rect.Height, intPtr, 0, 0, 13369376u);
						}
					}
					catch
					{
					}
					WinApi.DeleteObject(hObject2);
					WinApi.DeleteObject(hObject);
				}
				finally
				{
					WinApi.DeleteDC(intPtr);
				}
			}
			finally
			{
				g.ReleaseHdc(hdc);
			}
		}
		catch
		{
		}
	}

	private void PaintGlassText(Graphics g, string text, Font font, Rectangle captionRect, bool isMaximized)
	{
		if (isMaximized && !BarFunctions.IsWindows7)
		{
			captionRect.Offset(0, 3);
			TextDrawing.DrawString(g, text, font, Color.White, captionRect, eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.NoPrefix | eTextFormat.VerticalCenter);
			return;
		}
		if (isMaximized && BarFunctions.IsWindows7)
		{
			captionRect.Y += 6;
			captionRect.Height -= 6;
		}
		ThemeTextFormat tf = (ThemeTextFormat)32805;
		PaintTextOnGlass(g, text, font, captionRect, tf);
	}

	public override void PaintQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Expected O, but got Unknown
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		RibbonStrip ribbonStrip = e.RibbonControl.RibbonStrip;
		RibbonControl ribbonControl = e.RibbonControl;
		if (!ribbonStrip.CaptionVisible)
		{
			return;
		}
		eDotNetBarStyle effectiveStyle = ribbonControl.EffectiveStyle;
		Graphics graphics = e.Graphics;
		bool flag = (int)((Control)ribbonStrip).RightToLeft == 1;
		bool flag2 = true;
		Office2007QuickAccessToolbarStateColorTable office2007QuickAccessToolbarStateColorTable = m_ColorTable.QuickAccessToolbar.Active;
		Form val = ((Control)ribbonStrip).FindForm();
		bool flag3 = false;
		if (val != null)
		{
			flag3 = (int)val.WindowState == 2;
		}
		if (val != null && !e.RibbonControl.IsDesignMode && ((val != Form.ActiveForm && val.MdiParent == null) || (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)))
		{
			office2007QuickAccessToolbarStateColorTable = m_ColorTable.QuickAccessToolbar.Inactive;
		}
		int num = 0;
		int num2 = 0;
		Size size = Size.Empty;
		for (int num3 = ribbonStrip.QuickToolbarItems.Count - 1; num3 >= 0; num3--)
		{
			BaseItem baseItem = ribbonStrip.QuickToolbarItems[num3];
			if (baseItem.Visible && (baseItem.Displayed || ((RibbonStripContainerItem)ribbonStrip.GetBaseItemContainer()).MetroBackstageOpen))
			{
				if (baseItem is QatCustomizeItem)
				{
					size = baseItem.DisplayRectangle.Size;
				}
				if (baseItem.ItemAlignment == eItemAlignment.Near && baseItem.Visible && num3 > 0)
				{
					num = ((!flag) ? baseItem.DisplayRectangle.Right : baseItem.DisplayRectangle.X);
					break;
				}
				if (baseItem.ItemAlignment == eItemAlignment.Far && baseItem.Visible)
				{
					num2 = ((!flag) ? baseItem.DisplayRectangle.X : baseItem.DisplayRectangle.Right);
				}
			}
		}
		if (num == 0)
		{
			flag2 = false;
		}
		if (ribbonStrip.CaptionContainerItem is CaptionItemContainer && ((CaptionItemContainer)ribbonStrip.CaptionContainerItem).MoreItems != null)
		{
			num = ((!flag) ? ((CaptionItemContainer)ribbonStrip.CaptionContainerItem).MoreItems.DisplayRectangle.Right : ((CaptionItemContainer)ribbonStrip.CaptionContainerItem).MoreItems.DisplayRectangle.X);
			size = ((CaptionItemContainer)ribbonStrip.CaptionContainerItem).MoreItems.DisplayRectangle.Size;
		}
		Rectangle captionBounds = ribbonStrip.CaptionBounds;
		if (num2 > 0)
		{
			if (flag)
			{
				ribbonStrip.SystemCaptionItemBounds = new Rectangle(captionBounds.X, captionBounds.Y, num2, captionBounds.Height);
			}
			else
			{
				ribbonStrip.SystemCaptionItemBounds = new Rectangle(num2, captionBounds.Y, captionBounds.Right - num2, captionBounds.Height);
			}
		}
		if (num == 0 || captionBounds.Height <= 0 || captionBounds.Width <= 0)
		{
			return;
		}
		if (!ribbonControl.QatPositionedBelowRibbon)
		{
			BaseItem applicationButton = ribbonControl.GetApplicationButton();
			if (applicationButton != null)
			{
				int num4 = ribbonStrip.QuickToolbarItems.IndexOf(applicationButton);
				if (ribbonStrip.QuickToolbarItems.Count - 1 > num4)
				{
					BaseItem baseItem2 = ribbonStrip.QuickToolbarItems[num4 + 1];
					if (flag)
					{
						captionBounds.Width -= captionBounds.Right - baseItem2.DisplayRectangle.Right;
					}
					else
					{
						captionBounds.Width -= baseItem2.DisplayRectangle.X - captionBounds.X;
						captionBounds.X = baseItem2.DisplayRectangle.X;
					}
				}
			}
			captionBounds.Height = ((CaptionItemContainer)ribbonControl.RibbonStrip.CaptionContainerItem).MaxItemHeight + 6;
		}
		int num5 = captionBounds.Right - num;
		if (flag)
		{
			captionBounds.Width = num5;
			if (flag)
			{
				captionBounds.X += num;
			}
		}
		else
		{
			captionBounds.Width -= num5;
		}
		if (e.GlassEnabled)
		{
			captionBounds.Inflate(0, -3);
			captionBounds.Offset(0, 2);
		}
		else
		{
			captionBounds.Inflate(0, -2);
			if (flag3)
			{
				captionBounds.Y++;
				captionBounds.Height--;
			}
		}
		GraphicsPath quickToolbarBackPath = GetQuickToolbarBackPath(captionBounds, flag);
		if (quickToolbarBackPath == null)
		{
			return;
		}
		Rectangle rectangle2 = (ribbonStrip.QuickToolbarBounds = Rectangle.Ceiling(quickToolbarBackPath.GetBounds()));
		captionBounds.Width -= size.Width + size.Height / 2;
		if (flag)
		{
			captionBounds.X += size.Width + size.Height / 2 + 3;
		}
		quickToolbarBackPath.Dispose();
		quickToolbarBackPath = GetQuickToolbarBackPath(captionBounds, flag);
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		if (!ribbonControl.QatPositionedBelowRibbon && flag2 && captionBounds.Width > 6)
		{
			bool flag4 = false;
			if (e.GlassEnabled)
			{
				graphics.SmoothingMode = (SmoothingMode)2;
				if (e.GlassEnabled && flag3 && effectiveStyle == eDotNetBarStyle.Office2007)
				{
					DisplayHelp.FillPath(graphics, quickToolbarBackPath, Color.FromArgb(72, Color.LightGray), Color.Empty);
				}
				if (!office2007QuickAccessToolbarStateColorTable.GlassBorder.IsEmpty)
				{
					DisplayHelp.DrawGradientPathBorder(graphics, quickToolbarBackPath, office2007QuickAccessToolbarStateColorTable.GlassBorder, 1);
				}
				graphics.SmoothingMode = smoothingMode;
				if (e.GlassEnabled)
				{
					flag4 = true;
				}
			}
			else if (!office2007QuickAccessToolbarStateColorTable.OutterBorderColor.IsEmpty)
			{
				graphics.SmoothingMode = (SmoothingMode)2;
				captionBounds.X++;
				captionBounds.Y--;
				captionBounds.Height += 2;
				GraphicsPath quickToolbarBackPath2 = GetQuickToolbarBackPath(captionBounds, flag);
				try
				{
					Pen val2 = new Pen(office2007QuickAccessToolbarStateColorTable.OutterBorderColor);
					try
					{
						graphics.DrawPath(val2, quickToolbarBackPath2);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)quickToolbarBackPath2)?.Dispose();
				}
				captionBounds.X--;
				captionBounds.Y++;
				captionBounds.Height -= 2;
				graphics.SmoothingMode = smoothingMode;
				if (e.GlassEnabled)
				{
					flag4 = true;
				}
			}
			Region clip = graphics.Clip;
			graphics.SetClip(quickToolbarBackPath, (CombineMode)1);
			Rectangle r = new Rectangle(rectangle2.X, rectangle2.Y, rectangle2.Width, (int)((double)captionBounds.Height * 0.2));
			Rectangle r2 = new Rectangle(rectangle2.X, r.Bottom, rectangle2.Width, rectangle2.Height - r.Height);
			graphics.SmoothingMode = (SmoothingMode)0;
			if (!e.GlassEnabled)
			{
				DisplayHelp.FillRectangle(graphics, r, office2007QuickAccessToolbarStateColorTable.TopBackground);
				DisplayHelp.FillRectangle(graphics, r2, office2007QuickAccessToolbarStateColorTable.BottomBackground);
			}
			else if (!office2007QuickAccessToolbarStateColorTable.GlassBorder.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, rectangle2, Color.FromArgb(64, Color.White));
			}
			graphics.SmoothingMode = smoothingMode;
			rectangle2.Height -= 2;
			rectangle2.Width += 2;
			rectangle2.Y++;
			rectangle2.X++;
			graphics.SetClip(rectangle2);
			graphics.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
			if (!office2007QuickAccessToolbarStateColorTable.MiddleBorderColor.IsEmpty && !(e.GlassEnabled && flag4))
			{
				graphics.SmoothingMode = (SmoothingMode)2;
				quickToolbarBackPath = GetQuickToolbarBackPath(captionBounds, flag);
				Pen val3 = new Pen(office2007QuickAccessToolbarStateColorTable.MiddleBorderColor);
				try
				{
					graphics.DrawPath(val3, quickToolbarBackPath);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				quickToolbarBackPath.Dispose();
				quickToolbarBackPath = null;
				graphics.SmoothingMode = smoothingMode;
			}
		}
		if (quickToolbarBackPath != null)
		{
			quickToolbarBackPath.Dispose();
		}
	}

	private GraphicsPath GetQuickToolbarBackPath(Rectangle r, bool rightToLeft)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		r.Offset(-1, 0);
		r.Height--;
		r.Width--;
		if (r.Width < 2 || r.Height < 2)
		{
			return null;
		}
		GraphicsPath val = new GraphicsPath();
		int num = 11;
		if (rightToLeft)
		{
			val.AddCurve(new Point[3]
			{
				new Point(r.Right + num, r.Y),
				new Point(r.Right + 2, r.Y + 10),
				new Point(r.Right, r.Bottom)
			}, 0.6f);
			val.AddLine(r.Right, r.Bottom, r.X, r.Bottom);
			val.AddArc(r.X - (num + 1), r.Y, 20, r.Height, 90f, 180f);
			val.AddLine(r.X, r.Y, r.Right + num, r.Y);
		}
		else
		{
			val.AddCurve(new Point[3]
			{
				new Point(r.X - num, r.Y),
				new Point(r.X - 2, r.Y + 10),
				new Point(r.X, r.Bottom)
			}, 0.6f);
			val.AddLine(r.X, r.Bottom, r.Right, r.Bottom);
			val.AddArc(r.Right - (num + 1), r.Y, 20, r.Height, 90f, -180f);
			val.AddLine(r.Right, r.Y, r.X - num, r.Y);
		}
		val.CloseAllFigures();
		return val;
	}
}
