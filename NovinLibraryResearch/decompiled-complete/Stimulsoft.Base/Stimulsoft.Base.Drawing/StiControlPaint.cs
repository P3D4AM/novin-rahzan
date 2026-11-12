using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing;

public static class StiControlPaint
{
	public static void DrawImageBackground(Graphics g, Image image, Rectangle rect)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		Rectangle rectangle = rect;
		g.TranslateTransform((float)rect.X, (float)rect.Y);
		rectangle.X = 0;
		rectangle.Y = 0;
		TextureBrush val = new TextureBrush(image);
		try
		{
			g.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		g.TranslateTransform((float)(-rect.X), (float)(-rect.Y));
	}

	public static void DrawString(Graphics graphics, string text, Font font, Brush brush, Rectangle layoutRectangle, StringFormat format, float angle)
	{
		if (angle != 0f)
		{
			Region clip = graphics.Clip;
			graphics.SetClip(layoutRectangle, (CombineMode)1);
			GraphicsState val = graphics.Save();
			graphics.TranslateTransform((float)(layoutRectangle.Left + layoutRectangle.Width / 2), (float)(layoutRectangle.Top + layoutRectangle.Height / 2));
			graphics.RotateTransform(angle);
			layoutRectangle.X = -layoutRectangle.Width / 2;
			layoutRectangle.Y = -layoutRectangle.Height / 2;
			Rectangle rectangle = new Rectangle(layoutRectangle.X, layoutRectangle.Y, layoutRectangle.Width, layoutRectangle.Height);
			if ((angle > 45f && angle < 135f) || (angle > 225f && angle < 315f))
			{
				rectangle = new Rectangle(layoutRectangle.Y, layoutRectangle.X, layoutRectangle.Height, layoutRectangle.Width);
			}
			graphics.DrawString(text, font, brush, (RectangleF)rectangle, format);
			graphics.Restore(val);
			graphics.SetClip(clip, (CombineMode)0);
		}
		else
		{
			graphics.DrawString(text, font, brush, (RectangleF)layoutRectangle, format);
		}
	}

	public static void DrawReversibleFrame(Graphics graphics, int x, int y, int width, int height, Color backColor, FrameStyle style)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		DrawReversibleFrame(graphics, new Rectangle(x, y, width, height), backColor, style);
	}

	public static void DrawReversibleFrame(Graphics graphics, Rectangle rectangle, Color backColor, FrameStyle style)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		Win32.DrawMode nDrawMode;
		Color c;
		if ((double)backColor.GetBrightness() < 0.5)
		{
			nDrawMode = Win32.DrawMode.R2_NOTXORPEN;
			c = Color.White;
		}
		else
		{
			nDrawMode = Win32.DrawMode.R2_XORPEN;
			c = Color.Black;
		}
		IntPtr intPtr = IntPtr.Zero;
		if ((int)style != 0)
		{
			if ((int)style == 1)
			{
				intPtr = Win32.CreatePen(0, 1, ColorTranslator.ToWin32(backColor));
			}
		}
		else
		{
			intPtr = Win32.CreatePen(2, 1, ColorTranslator.ToWin32(backColor));
		}
		IntPtr hdc = graphics.GetHdc();
		int nDrawMode2 = Win32.SetROP2(hdc, (int)nDrawMode);
		IntPtr hObject = Win32.SelectObject(hdc, Win32.GetStockObject(5));
		IntPtr hObject2 = Win32.SelectObject(hdc, intPtr);
		Win32.SetBkColor(hdc, ColorTranslator.ToWin32(c));
		Win32.Rectangle(hdc, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
		Win32.SetROP2(hdc, nDrawMode2);
		Win32.SelectObject(hdc, hObject);
		Win32.SelectObject(hdc, hObject2);
		Win32.DeleteObject(intPtr);
		graphics.ReleaseHdc(hdc);
	}

	public static void DrawReversibleLine(Graphics graphics, int startX, int startY, int endX, int endY, Color backColor, FrameStyle style)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		DrawReversibleLine(graphics, new Point(startX, startY), new Point(endX, endY), backColor, style);
	}

	public static void DrawReversibleLine(Graphics graphics, Point start, Point end, Color backColor, FrameStyle style)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		Win32.DrawMode nDrawMode;
		Color c;
		if ((double)backColor.GetBrightness() < 0.5)
		{
			nDrawMode = Win32.DrawMode.R2_NOTXORPEN;
			c = Color.White;
		}
		else
		{
			nDrawMode = Win32.DrawMode.R2_XORPEN;
			c = Color.Black;
		}
		IntPtr intPtr = IntPtr.Zero;
		if ((int)style != 0)
		{
			if ((int)style == 1)
			{
				intPtr = Win32.CreatePen(0, 1, ColorTranslator.ToWin32(backColor));
			}
		}
		else
		{
			intPtr = Win32.CreatePen(2, 1, ColorTranslator.ToWin32(backColor));
		}
		IntPtr hdc = graphics.GetHdc();
		int nDrawMode2 = Win32.SetROP2(hdc, (int)nDrawMode);
		IntPtr hObject = Win32.SelectObject(hdc, Win32.GetStockObject(5));
		IntPtr hObject2 = Win32.SelectObject(hdc, intPtr);
		Win32.SetBkColor(hdc, ColorTranslator.ToWin32(c));
		Win32.MoveToEx(hdc, start.X, start.Y, IntPtr.Zero);
		Win32.LineTo(hdc, end.X, end.Y);
		Win32.SetROP2(hdc, nDrawMode2);
		Win32.SelectObject(hdc, hObject);
		Win32.SelectObject(hdc, hObject2);
		Win32.DeleteObject(intPtr);
		graphics.ReleaseHdc(hdc);
	}

	public static void FillReversibleRectangle(Graphics graphics, int x, int y, int width, int height, Color backColor)
	{
		FillReversibleRectangle(graphics, new Rectangle(x, y, width, height), backColor);
	}

	public static void FillReversibleRectangle(Graphics graphics, Rectangle bounds, Color backColor)
	{
		int colorRop = StiColorUtils.GetColorRop(backColor, 10813541, 5898313);
		int colorRop2 = StiColorUtils.GetColorRop(backColor, 6, 6);
		IntPtr hdc = graphics.GetHdc();
		IntPtr intPtr = Win32.CreateSolidBrush(ColorTranslator.ToWin32(backColor));
		int nDrawMode = Win32.SetROP2(hdc, colorRop2);
		IntPtr hObject = Win32.SelectObject(hdc, intPtr);
		Win32.PatBlt(hdc, bounds.X, bounds.Y, bounds.Width, bounds.Height, colorRop);
		Win32.SetROP2(hdc, nDrawMode);
		Win32.SelectObject(hdc, hObject);
		Win32.DeleteObject(intPtr);
		graphics.ReleaseHdc(hdc);
	}

	public static void DrawImageDisabled(Graphics graphics, Image image, int x, int y)
	{
		if (image is Bitmap)
		{
			DrawImageDisabled(graphics, (Bitmap)(object)((image is Bitmap) ? image : null), x, y);
		}
		else
		{
			ControlPaint.DrawImageDisabled(graphics, image, x, y, SystemColors.Control);
		}
	}

	public static void DrawImageDisabled(Graphics graphics, Bitmap bmp, int x, int y)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		ImageAttributes val = new ImageAttributes();
		ColorMatrix colorMatrix = new ColorMatrix(new float[6][]
		{
			new float[5] { 0.3f, 0.3f, 0.3f, 0f, 0f },
			new float[5] { 0.59f, 0.59f, 0.59f, 0f, 0f },
			new float[5] { 0.11f, 0.11f, 0.11f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 0.4f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 0.4f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 0f, 0.4f }
		});
		val.SetColorMatrix(colorMatrix);
		graphics.DrawImage((Image)(object)bmp, new Rectangle(x, y, ((Image)bmp).Width, ((Image)bmp).Height), 0, 0, ((Image)bmp).Width, ((Image)bmp).Height, (GraphicsUnit)2, val);
	}

	public static void DrawImageDisabled(Graphics graphics, Image bmp, Rectangle rect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		ImageAttributes val = new ImageAttributes();
		ColorMatrix colorMatrix = new ColorMatrix(new float[6][]
		{
			new float[5] { 0.3f, 0.3f, 0.3f, 0f, 0f },
			new float[5] { 0.59f, 0.59f, 0.59f, 0f, 0f },
			new float[5] { 0.11f, 0.11f, 0.11f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 0.4f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 0.4f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 0f, 0.4f }
		});
		val.SetColorMatrix(colorMatrix);
		graphics.DrawImage(bmp, rect, 0, 0, bmp.Width, bmp.Height, (GraphicsUnit)2, val);
	}

	public static void DrawButton(Graphics graphics, Rectangle bounds, Image image, bool isPressed, bool isFocused, bool isMouseOverButton, bool enabled, bool flat)
	{
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		if (flat)
		{
			bounds.Width++;
			bounds.Height++;
			Color color = StiColors.ControlStart;
			Color color2 = StiColors.ControlEnd;
			if (isMouseOverButton)
			{
				color = StiColors.ControlStartLight;
				color2 = StiColors.ControlEndLight;
			}
			if (isPressed)
			{
				color = StiColors.ControlStartDark;
				color2 = StiColors.ControlEndDark;
			}
			if (!enabled)
			{
				color = color2;
			}
			Brush val = (Brush)new LinearGradientBrush(bounds, color, color2, 90f);
			try
			{
				graphics.FillRectangle(val, bounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			Color color3 = SystemColors.ControlDark;
			if (isFocused)
			{
				color3 = StiColors.SelectedText;
			}
			Pen val2 = new Pen(color3);
			try
			{
				bounds.X--;
				bounds.Y--;
				bounds.Width++;
				bounds.Height++;
				graphics.DrawRectangle(val2, bounds);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		else
		{
			ButtonState val3 = (ButtonState)0;
			if (isPressed)
			{
				val3 = (ButtonState)512;
			}
			if (!enabled)
			{
				val3 = (ButtonState)256;
			}
			bounds.Width++;
			bounds.Height++;
			if (bounds.Width > 0 && bounds.Height > 0)
			{
				ControlPaint.DrawButton(graphics, bounds, val3);
			}
		}
		if (isPressed)
		{
			bounds.X++;
			bounds.Y++;
		}
		if (image != null)
		{
			if (flat)
			{
				bounds.X++;
				bounds.Y++;
			}
			if (enabled)
			{
				graphics.DrawImage(image, new Rectangle(bounds.X + (bounds.Width - image.Width) / 2, bounds.Y + (bounds.Height - image.Height - 1) / 2, image.Width, image.Height));
			}
			else
			{
				DrawImageDisabled(graphics, image, bounds.X + (bounds.Width - image.Width) / 2, bounds.Y + (bounds.Height - image.Height - 1) / 2);
			}
		}
	}

	public static void DrawBorder(Graphics graphics, Rectangle bounds, bool isFocused, bool isEnabled, bool flat)
	{
		if (flat)
		{
			Color color = ((!isEnabled) ? (StiUX.IsDark ? StiUX.InputBorder : StiColorUtils.Dark(SystemColors.Control, 30)) : (StiUX.IsDark ? StiUX.InputBorder : StiColorUtils.Dark(SystemColors.ControlDark, 10)));
			if (isFocused)
			{
				color = (StiUX.IsDark ? StiUX.Accent : StiColors.SelectedText);
			}
			StiDrawing.DrawRectangle(graphics, color, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
		}
		else
		{
			bounds.Width++;
			bounds.Height++;
			ControlPaint.DrawBorder3D(graphics, bounds, (Border3DStyle)10);
		}
	}

	public static void DrawItemBackground(Graphics g, Rectangle bounds, DrawItemState state, Color? backColor = null)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		bounds.Width++;
		Color itemBackground = GetItemBackground(state, backColor.GetValueOrDefault(StiUX.IsDark ? StiUX.InputBackground : SystemColors.Window));
		StiDrawing.FillRectangle(g, itemBackground, bounds);
	}

	private static Color GetItemBackground(DrawItemState state, Color backColor)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		bool flag = (state & 0x1000) > 0;
		if ((state & 4) != 0)
		{
			return StiUX.ItemBackgroundDisabled;
		}
		if ((state & 0x10) != 0 && !flag)
		{
			return StiUX.ItemBackgroundHover;
		}
		if ((state & 1) != 0 && !flag)
		{
			return StiUX.ItemBackgroundSelected;
		}
		return backColor;
	}

	public static void DrawItem(Graphics graphics, Rectangle bounds, DrawItemState state, string text, ImageList imageList, int imageIndex, Font font, Color backColor, Color foreColor, int textStartPos, RightToLeft rightToLeft)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		DrawItem(graphics, bounds, state, text, imageList, imageIndex, font, backColor, foreColor, textStartPos, rightToLeft, (StringAlignment)0);
	}

	public static void DrawItem(Graphics graphics, Rectangle bounds, DrawItemState state, string text, ImageList imageList, int imageIndex, Font font, Color backColor, Color foreColor, int textStartPos, RightToLeft rightToLeft, StringAlignment alignment)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Invalid comparison between Unknown and I4
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Invalid comparison between Unknown and I4
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		DrawItemBackground(graphics, bounds, state, backColor);
		int num = 0;
		if (imageList != null && imageIndex >= 0 && imageIndex < imageList.Images.Count)
		{
			Rectangle rectangle = new Rectangle(bounds.X + 1, bounds.Y + (bounds.Height - imageList.ImageSize.Height) / 2, imageList.ImageSize.Width, imageList.ImageSize.Height);
			imageList.Draw(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, imageIndex);
			num = imageList.ImageSize.Width + 2;
		}
		StringFormat val = new StringFormat();
		try
		{
			val.Alignment = alignment;
			val.LineAlignment = (StringAlignment)1;
			val.FormatFlags = (StringFormatFlags)4096;
			val.Trimming = (StringTrimming)3;
			if ((int)rightToLeft == 1)
			{
				val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 1);
			}
			bounds.X += num + textStartPos;
			bounds.Width -= num + textStartPos;
			if (text == null)
			{
				return;
			}
			bool flag = (state & 0x1000) > 0;
			if (((state & 0x10) != 0 || (state & 1) != 0) && !flag)
			{
				SolidBrush val2 = new SolidBrush(StiUX.IsDark ? StiUX.ItemForeground : SystemColors.HighlightText);
				try
				{
					graphics.DrawString(text, font, (Brush)(object)val2, (RectangleF)bounds, val);
					return;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			SolidBrush val3 = new SolidBrush(foreColor);
			try
			{
				graphics.DrawString(text, font, (Brush)(object)val3, (RectangleF)bounds, val);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawItem(Graphics graphics, Rectangle bounds, DrawItemState state, string text, ImageList imageList, int imageIndex, Font font, Color backColor, Color foreColor, RightToLeft rightToLeft)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		DrawItem(graphics, bounds, state, text, imageList, imageIndex, font, backColor, foreColor, 0, rightToLeft);
	}

	public static void DrawCheck(Graphics graphics, int x, int y, bool enabled)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		x -= StiScale.I3;
		y -= StiScale.I3;
		Point point = new Point(x, y + StiScale.I2);
		Point point2 = new Point(x + StiScale.I2, y + StiScale.I4);
		Point point3 = new Point(x + StiScale.I6, y);
		Color color = Color.Black;
		if (!enabled)
		{
			color = SystemColors.ControlDark;
		}
		Pen val = new Pen(color);
		try
		{
			graphics.DrawLine(val, point, point2);
			graphics.DrawLine(val, point2, point3);
			point.Y++;
			point2.Y++;
			point3.Y++;
			graphics.DrawLine(val, point, point2);
			graphics.DrawLine(val, point2, point3);
			point.Y++;
			point2.Y++;
			point3.Y++;
			graphics.DrawLine(val, point, point2);
			graphics.DrawLine(val, point2, point3);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawFocus(Graphics graphics, Rectangle bounds, Rectangle buttonBounds)
	{
		Rectangle rectangle = new Rectangle(bounds.X + StiScale.I2, bounds.Y + StiScale.I2, bounds.Width - buttonBounds.Width - StiScale.I4, bounds.Height - StiScale.I3);
		ControlPaint.DrawFocusRectangle(graphics, rectangle);
	}

	public static void DrawFocus(Graphics graphics, Rectangle bounds)
	{
		Rectangle rectangle = new Rectangle(bounds.X + StiScale.I2, bounds.Y + StiScale.I2, bounds.Width - StiScale.I3, bounds.Height - StiScale.I3);
		ControlPaint.DrawFocusRectangle(graphics, rectangle);
	}

	public static bool ScrollWindow(IntPtr hWnd, int xAmount, int yAmount)
	{
		return Win32.ScrollWindowEx(hWnd, xAmount, yAmount, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 10);
	}

	public static Rectangle GetButtonRect(Rectangle bounds, bool flat, RightToLeft rightToLeft)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return GetButtonRect(bounds, flat, SystemInformation.HorizontalScrollBarArrowWidth - StiScale.I1, rightToLeft);
	}

	public static Rectangle GetButtonRect(Rectangle bounds, bool flat, int buttonWidth, RightToLeft rightToLeft)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		int num = SystemInformation.Border3DSize.Width;
		int num2 = SystemInformation.Border3DSize.Height;
		if (flat)
		{
			num = (num2 = StiScale.I1);
		}
		if ((int)rightToLeft == 1)
		{
			return new Rectangle(bounds.Left + num, bounds.Top + num2, buttonWidth + StiScale.I1, bounds.Height - num2 * 2);
		}
		return new Rectangle(bounds.Right - buttonWidth - num, bounds.Top + num2 + StiScale.I1, buttonWidth, bounds.Height - num2 * 2 - StiScale.I1);
	}

	public static Rectangle GetContentRect(Rectangle bounds, bool flat, RightToLeft rightToLeft)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		Rectangle buttonRect = GetButtonRect(bounds, flat, rightToLeft);
		int num = SystemInformation.Border3DSize.Width;
		int num2 = SystemInformation.Border3DSize.Height;
		if (flat)
		{
			num = (num2 = StiScale.I1);
		}
		if ((int)rightToLeft == 1)
		{
			return new Rectangle(bounds.Left + num + buttonRect.Width, bounds.Top + num2, bounds.Width - num * 2, bounds.Height - num2 * 2);
		}
		return new Rectangle(bounds.Left + num, bounds.Top + num2, bounds.Width - num * 2 - buttonRect.Width - StiScale.I2, bounds.Height - num2 * 2);
	}
}
