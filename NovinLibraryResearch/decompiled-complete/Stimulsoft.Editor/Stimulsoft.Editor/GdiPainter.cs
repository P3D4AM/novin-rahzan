using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class GdiPainter : Painter, IPainter
{
	private Graphics graphics;

	private IntPtr hdc;

	private IntPtr brush;

	private IntPtr pen;

	private int oldTextColor;

	private int oldBackColor;

	private bool oldOpaque;

	private IntPtr oldPen;

	private IntPtr oldBrush;

	private IntPtr oldFont;

	private int formatFlags;

	private int oldMode = 1;

	private bool transFormed;

	private bool safeTransformed;

	private Win32.XFORM xForm;

	private Win32.XFORM safexForm;

	private int bufferSize;

	private int[] buffer;

	public Matrix Transformation
	{
		get
		{
			if (graphics != null)
			{
				return graphics.Transform;
			}
			return null;
		}
	}

	public Graphics Graphics => graphics;

	public IntPtr Handle => hdc;

	private void InitDC(IntPtr hFont)
	{
		if (hdc != IntPtr.Zero)
		{
			Win32.SelectObject(hdc, hFont);
		}
	}

	private int CharWidth(FontInfo info, char ch, int count)
	{
		if (count == int.MaxValue)
		{
			return int.MaxValue;
		}
		if (count > 0)
		{
			if (IsFontMonoSpaced())
			{
				return count * info.FontWidth;
			}
			return count * ((GdiFontInfo)info).CharWidth(ch);
		}
		return 0;
	}

	private int CharWidth(FontInfo info, char ch, int width, out int count)
	{
		if (width == int.MaxValue)
		{
			count = int.MaxValue;
			return int.MaxValue;
		}
		int num = 0;
		count = 0;
		if (width > 0)
		{
			num = ((!IsFontMonoSpaced()) ? ((GdiFontInfo)info).CharWidth(ch) : info.FontWidth);
			if (num > 0)
			{
				count = width / num;
				return count * num;
			}
		}
		return 0;
	}

	private void FreeBuffer()
	{
		buffer = null;
		bufferSize = 0;
	}

	protected virtual int StringWidth(FontInfo info, string text, int pos, int len)
	{
		if (len == int.MaxValue)
		{
			return int.MaxValue;
		}
		if (text == null || info == null)
		{
			return 0;
		}
		if (len == -1)
		{
			len = text.Length;
		}
		if (IsFontMonoSpaced())
		{
			return len * info.FontWidth;
		}
		int num = 0;
		for (int i = pos; i < pos + len; i++)
		{
			num += ((GdiFontInfo)info).CharWidth(text[i]);
		}
		return num;
	}

	protected virtual int StringWidth(FontInfo info, string text, int pos, int len, int width, out int count, bool exact)
	{
		count = 0;
		if (text == null)
		{
			return 0;
		}
		if (len == -1)
		{
			len = text.Length - pos;
		}
		if (width == int.MaxValue)
		{
			count = len;
			return int.MaxValue;
		}
		if (IsFontMonoSpaced())
		{
			int fontWidth = info.FontWidth;
			if (fontWidth == 0)
			{
				count = len;
			}
			else
			{
				if (exact)
				{
					count = width / fontWidth;
				}
				else
				{
					count = (int)Math.Round((double)(width / fontWidth));
				}
				count = Math.Min(count, len);
			}
			return count * fontWidth;
		}
		for (int i = pos; i < pos + len; i++)
		{
			int num = ((GdiFontInfo)info).CharWidth(text[i]);
			width -= num;
			if (width < 0)
			{
				if (!exact && width > -num / 2)
				{
					count++;
				}
				break;
			}
			count++;
		}
		return StringWidth(info, text, pos, count);
	}

	protected virtual bool IsFontMonoSpaced()
	{
		return false;
	}

	protected override FontInfos CreateFontInfos(Font font)
	{
		return new GdiFontInfos(font, measureDC);
	}

	protected override void ClearPens()
	{
		foreach (DictionaryEntry item in base.PenTable)
		{
			Win32.DeleteObject((IntPtr)item.Value);
		}
		base.ClearPens();
	}

	protected override void ClearBrushes()
	{
		foreach (DictionaryEntry item in base.BrushTable)
		{
			Win32.DeleteObject((IntPtr)item.Value);
		}
		base.ClearBrushes();
	}

	protected override object CreatePen(Color color)
	{
		return Win32.CreatePen(0, 1, Win32.ColorToGdiColor(color));
	}

	protected override object CreateBrush(Color color)
	{
		return Win32.CreateSolidBrush(Win32.ColorToGdiColor(color));
	}

	protected override void SelectFont(Font font, FontStyle fontStyle)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		base.SelectFont(font, fontStyle);
		if (base.CurrentInfo != null)
		{
			InitDC(base.CurrentInfo.HFont);
		}
	}

	protected override void SelectFont(FontStyle fontStyle)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		base.SelectFont(fontStyle);
		if (base.CurrentInfo != null)
		{
			InitDC(base.CurrentInfo.HFont);
		}
	}

	protected override void SelectTextColor(Color color)
	{
		if (hdc != IntPtr.Zero)
		{
			Win32.SetTextColor(hdc, Win32.ColorToGdiColor(color));
		}
	}

	protected override void SelectStringFormat(StringFormat format)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected I4, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected I4, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Invalid comparison between Unknown and I4
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected I4, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		formatFlags = 0;
		StringAlignment alignment = format.Alignment;
		switch ((int)alignment)
		{
		case 1:
			formatFlags |= 1;
			break;
		case 2:
			formatFlags |= 2;
			break;
		case 0:
			formatFlags |= 0;
			break;
		}
		StringAlignment lineAlignment = format.LineAlignment;
		switch ((int)lineAlignment)
		{
		case 1:
			formatFlags |= 36;
			break;
		case 2:
			formatFlags |= 40;
			break;
		case 0:
			formatFlags |= 0;
			break;
		}
		HotkeyPrefix hotkeyPrefix = format.HotkeyPrefix;
		if ((int)hotkeyPrefix != 0)
		{
			if ((int)hotkeyPrefix == 2)
			{
				formatFlags |= 1048576;
			}
		}
		else
		{
			formatFlags |= 2048;
		}
		StringTrimming trimming = format.Trimming;
		switch (trimming - 3)
		{
		case 0:
			formatFlags |= 32768;
			break;
		case 2:
			formatFlags |= 16384;
			break;
		case 1:
			formatFlags |= 262144;
			break;
		}
		if ((format.FormatFlags & 1) != 0)
		{
			formatFlags |= 131072;
		}
		if ((format.FormatFlags & 0x4000) != 0)
		{
			formatFlags |= 256;
		}
	}

	protected override void SelectPen(Color color, bool select)
	{
		pen = (IntPtr)SelectPen(color);
		if (select && hdc != IntPtr.Zero)
		{
			Win32.SelectObject(hdc, pen);
		}
	}

	protected override void SelectBrush(Color color, bool select)
	{
		brush = (IntPtr)SelectBrush(color);
		if (select && hdc != IntPtr.Zero)
		{
			Win32.SetBkColor(hdc, Win32.ColorToGdiColor(color));
		}
	}

	protected override void SelectOpaque(bool value)
	{
		if (hdc != IntPtr.Zero)
		{
			Win32.SetBkMode(hdc, (!value) ? 1 : 2);
		}
	}

	public override void Clear()
	{
		FreeBuffer();
		pen = IntPtr.Zero;
		brush = IntPtr.Zero;
		base.Clear();
	}

	public void Transform(int x, int y, float scaleX, float scaleY)
	{
		transFormed = true;
		oldMode = Win32.SetGraphicsMode(hdc, 2);
		xForm = new Win32.XFORM(scaleX, 0f, 0f, scaleY, (float)x * scaleX, (float)y * scaleY);
		Win32.SetWorldTransform(hdc, ref xForm);
	}

	public void EndTransform()
	{
		if (transFormed)
		{
			xForm = new Win32.XFORM(1f, 0f, 0f, 1f, 0f, 0f);
			Win32.SetWorldTransform(hdc, ref xForm);
			transFormed = false;
		}
	}

	public void BeginPaint(Graphics graphics)
	{
		this.graphics = graphics;
		hdc = graphics.GetHdc();
		oldFont = ((base.CurrentInfo != null) ? Win32.SelectObject(hdc, base.CurrentInfo.HFont) : IntPtr.Zero);
		oldTextColor = Win32.SetTextColor(hdc, Win32.ColorToGdiColor(base.TextColor));
		oldBackColor = Win32.SetBkColor(hdc, Win32.ColorToGdiColor(base.BackColor));
		oldOpaque = Win32.SetBkMode(hdc, (!base.Opaque) ? 1 : 2) == 2;
		oldMode = Win32.GetGraphicsMode(hdc);
		SelectBrush(base.BackColor, select: true);
		SelectPen(base.ForeColor, select: false);
		oldPen = Win32.SelectObject(hdc, pen);
		oldBrush = Win32.SelectObject(hdc, brush);
	}

	public void EndPaint()
	{
		if (oldFont != IntPtr.Zero)
		{
			Win32.SelectObject(hdc, oldFont);
		}
		Win32.SetTextColor(hdc, oldTextColor);
		Win32.SetBkColor(hdc, oldBackColor);
		Win32.SetBkMode(hdc, (!oldOpaque) ? 1 : 2);
		Win32.SelectObject(hdc, oldPen);
		Win32.SelectObject(hdc, oldBrush);
		EndTransform();
		graphics.ReleaseHdc(hdc);
		graphics = null;
		hdc = IntPtr.Zero;
	}

	protected internal Graphics SafeEndPaint()
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		safexForm = xForm;
		safeTransformed = transFormed;
		Graphics val = graphics;
		EndPaint();
		if (safeTransformed)
		{
			val.Transform = new Matrix(safexForm.eM11, safexForm.eM12, safexForm.eM21, safexForm.eM22, safexForm.eDx, safexForm.eDy);
		}
		return val;
	}

	protected internal void SafeBeginPaint(Graphics gr)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		if (safeTransformed)
		{
			gr.Transform = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
		}
		BeginPaint(gr);
		if (safeTransformed)
		{
			xForm = safexForm;
			Win32.SetWorldTransform(hdc, ref xForm);
			transFormed = true;
		}
	}

	protected internal int[] GetBuffer(int len, int space)
	{
		if (IsFontMonoSpaced() || space >= 0)
		{
			if (bufferSize < len)
			{
				FreeBuffer();
				bufferSize = len;
				buffer = new int[bufferSize];
				for (int i = 0; i < bufferSize; i++)
				{
					buffer[i] = ((space >= 0) ? space : base.FontWidth);
				}
			}
			return buffer;
		}
		return null;
	}

	protected internal int[] GetBuffer(int len)
	{
		return GetBuffer(len, -1);
	}

	internal int DashStyleToPenStyle(DashStyle style)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected I4, but got Unknown
		return (style - 1) switch
		{
			0 => 1, 
			1 => 2, 
			2 => 3, 
			3 => 4, 
			_ => 0, 
		};
	}

	private int Border3dStyleToBorder(Border3DStyle border)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected I4, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		switch (border - 1)
		{
		default:
			if ((int)border != 16394)
			{
				break;
			}
			return 16384;
		case 8:
			return 9;
		case 5:
			return 6;
		case 4:
			return 5;
		case 3:
			return 4;
		case 0:
			return 1;
		case 9:
			return 10;
		case 7:
			return 8;
		case 1:
			return 2;
		case 2:
		case 6:
			break;
		}
		return 0;
	}

	private int Border3DSideToSide(Border3DSide sides)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		if ((int)sides == 2063)
		{
			num |= 0x80F;
		}
		if ((sides & 8) != 0)
		{
			num |= 8;
		}
		if ((sides & 1) != 0)
		{
			num |= 1;
		}
		if ((sides & 0x800) != 0)
		{
			num |= 0x800;
		}
		if ((sides & 4) != 0)
		{
			num |= 4;
		}
		if ((sides & 2) != 0)
		{
			num |= 2;
		}
		return num;
	}

	public int StringWidth(string text)
	{
		return StringWidth(text, 0, -1);
	}

	public int StringWidth(string text, int pos, int len)
	{
		return StringWidth(base.CurrentInfo, text, pos, len);
	}

	public int StringWidth(string text, int width, out int count, bool exact)
	{
		return StringWidth(text, 0, -1, width, out count, exact);
	}

	public int StringWidth(string text, int pos, int len, int width, out int count)
	{
		return StringWidth(text, pos, len, width, out count, exact: true);
	}

	public int StringWidth(string text, int pos, int len, int width, out int count, bool exact)
	{
		return StringWidth(base.CurrentInfo, text, pos, len, width, out count, exact);
	}

	public int CharWidth(char ch, int count)
	{
		return CharWidth(base.CurrentInfo, ch, count);
	}

	public int CharWidth(char ch, int width, out int count)
	{
		return CharWidth(base.CurrentInfo, ch, width, out count);
	}

	public void FillRectangle(Rectangle rect)
	{
		FillRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
	}

	public virtual void FillRectangle(int x, int y, int width, int height)
	{
		Win32.FillRect(hdc, x, y, width, height, brush);
	}

	public void FillRectangle(Color color, Rectangle rect)
	{
		Color color2 = base.BackColor;
		try
		{
			base.BackColor = color;
			FillRectangle(rect);
		}
		finally
		{
			base.BackColor = color2;
		}
	}

	public void FillRectangle(Color color, int x, int y, int width, int height)
	{
		Color color2 = base.BackColor;
		try
		{
			base.BackColor = color;
			FillRectangle(x, y, width, height);
		}
		finally
		{
			base.BackColor = color2;
		}
	}

	public virtual void FillPolygon(Color color, Point[] points)
	{
		Color color2 = base.BackColor;
		Color color3 = base.ForeColor;
		try
		{
			base.BackColor = color;
			base.ForeColor = color;
			Win32.Polygon(hdc, points, points.Length);
		}
		finally
		{
			base.BackColor = color2;
			base.ForeColor = color3;
		}
	}

	public void FillGradient(Rectangle rect, Color beginColor, Color endColor, Point point1, Point point2)
	{
		FillGradient(rect.Left, rect.Top, rect.Width, rect.Height, beginColor, endColor, point1, point2);
	}

	public virtual void FillGradient(int x, int y, int width, int height, Color beginColor, Color endColor, Point point1, Point point2)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		Graphics val = SafeEndPaint();
		try
		{
			Brush val2 = (Brush)new LinearGradientBrush(point1, point2, beginColor, endColor);
			val.FillRectangle(val2, x, y, width, height);
			val2.Dispose();
		}
		finally
		{
			SafeBeginPaint(val);
		}
	}

	public virtual void DrawThemeBackground(IntPtr handle, int partID, int stateID, Rectangle rect)
	{
		Win32.GdiRect rect2 = new Win32.GdiRect(rect);
		Win32.DrawThemeBackground(handle, hdc, partID, stateID, ref rect2, IntPtr.Zero);
	}

	public void DrawRectangle(Rectangle rect)
	{
		DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
	}

	public virtual void DrawRectangle(int x, int y, int width, int height)
	{
		Win32.FrameRect(hdc, x, y, width + 1, height + 1, brush);
	}

	public virtual void DrawRoundRectangle(int left, int top, int right, int bottom, int width, int height)
	{
		Win32.RoundRect(hdc, left, top, right + 1, bottom + 1, width + 1, height + 1);
	}

	public virtual void DrawFocusRect(int x, int y, int width, int height, Color color)
	{
		DrawDotLine(x, y, x + width, y, color);
		DrawDotLine(x, y, x, y + height, color);
		DrawDotLine(x + width, y, x + width, y + height, color);
		DrawDotLine(x, y + height, x + width, y + height, color);
	}

	public void DrawFocusRect(Rectangle rect, Color color)
	{
		DrawFocusRect(rect.Left, rect.Top, rect.Width, rect.Height, color);
	}

	public void StretchDrawImage(Rectangle rect, Rectangle stretchRect, Rectangle imageRect, Bitmap image)
	{
	}

	public virtual void DrawImage(ImageList images, int index, Rectangle rect)
	{
		if (images != null)
		{
			uint num = uint.MaxValue;
			Win32.ImageList_DrawEx(images.Handle, index, hdc, rect.Left, rect.Top, rect.Width, rect.Height, (int)num, (int)num, 1);
		}
	}

	public void DrawImage(ImageList images, int index, Rectangle rect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
	{
	}

	public virtual void DrawLine(int x1, int y1, int x2, int y2)
	{
		Win32.MoveToEx(hdc, x1, y1, IntPtr.Zero);
		if (x1 == x2)
		{
			y2++;
		}
		if (y1 == y2)
		{
			x2++;
		}
		Win32.LineTo(hdc, x2, y2);
	}

	public virtual void DrawLine(int x1, int y1, int x2, int y2, Color color, int width, DashStyle style)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		IntPtr intPtr = Win32.CreatePen(DashStyleToPenStyle(style), width, Win32.ColorToGdiColor(color));
		IntPtr obj = Win32.SelectObject(hdc, intPtr);
		DrawLine(x1, y1, x2, y2);
		Win32.SelectObject(hdc, obj);
		Win32.DeleteObject(intPtr);
	}

	public virtual void DrawDotLine(int x1, int y1, int x2, int y2, Color color)
	{
		int i = 0;
		if (x1 == x2)
		{
			for (; i < y2 - y1; i += 2)
			{
				Win32.SetPixel(hdc, x1, i + y1, color);
			}
		}
		else if (y1 == y2)
		{
			for (; i < x2 - x1; i += 2)
			{
				Win32.SetPixel(hdc, i + x1, y1, color);
			}
		}
	}

	public virtual void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides, int flags)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		Win32.DrawEdge(hdc, ref rect, Border3dStyleToBorder(border), Border3DSideToSide(sides) | flags);
	}

	public void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		DrawEdge(ref rect, border, sides, 0);
	}

	public virtual void DrawWave(Rectangle rect, Color color)
	{
		int num = rect.Left - rect.Left % 6;
		int num2 = rect.Right % 6;
		int num3 = ((num2 != 0) ? (rect.Right + (6 - num2)) : rect.Right);
		int num4 = num3 - num >> 1;
		if (num4 < 4)
		{
			num4 = 4;
		}
		else
		{
			num2 = (num4 - 4) / 3;
			if ((num4 - 4) % 3 != 0)
			{
				num2++;
			}
			num4 = 4 + num2 * 3;
		}
		Point[] array = new Point[num4];
		for (int i = 0; i < num4; i++)
		{
			array[i].X = num + i * 2;
			array[i].Y = rect.Bottom - 1;
			switch (i % 3)
			{
			case 0:
				array[i].Y -= 2;
				break;
			case 2:
				array[i].Y += 2;
				break;
			}
		}
		IntPtr intPtr = Win32.CreatePen(0, 1, Win32.ColorToGdiColor(color));
		IntPtr obj = Win32.SelectObject(hdc, intPtr);
		Win32.PolyBezier(hdc, array, num4);
		Win32.SelectObject(hdc, obj);
		Win32.DeleteObject(intPtr);
	}

	public virtual void DrawPolygon(Point[] points, Color color)
	{
		if (points != null)
		{
			base.BackColor = color;
			IntPtr obj = Win32.SelectObject(hdc, brush);
			IntPtr intPtr = Win32.CreatePen(0, 1, Win32.ColorToGdiColor(color));
			IntPtr obj2 = Win32.SelectObject(hdc, intPtr);
			Win32.Polygon(hdc, points, points.Length);
			Win32.SelectObject(hdc, obj);
			Win32.SelectObject(hdc, obj2);
			Win32.DeleteObject(intPtr);
		}
	}

	public void TextOut(string text, int len, int x, int y)
	{
		TextOut(text, len, new Rectangle(x, y, 0, 0), x, y, clipped: false, opaque: false, -1);
	}

	public void TextOut(string text, int len, int x, int y, bool clipped, bool opaque)
	{
		TextOut(text, len, new Rectangle(x, y, 0, 0), x, y, clipped, opaque, -1);
	}

	public void TextOut(string text, int len, Rectangle rect)
	{
		TextOut(text, len, rect, rect.Left, rect.Top, clipped: false, opaque: false, -1);
	}

	public void TextOut(string text, int len, Rectangle rect, bool clipped, bool opaque)
	{
		TextOut(text, len, rect, rect.Left, rect.Top, clipped, opaque, -1);
	}

	public void TextOut(string text, int len, Rectangle rect, bool clipped, bool opaque, int space)
	{
		TextOut(text, len, rect, rect.Left, rect.Top, clipped, opaque, space);
	}

	public void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque)
	{
		TextOut(text, len, rect, x, y, clipped, opaque, -1);
	}

	public virtual void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque, int space)
	{
		if (text != null)
		{
			Win32.ExtTextOut(hdc, rect, x, y, (clipped ? 4 : 0) | (opaque ? 2 : 0), text, len, (space > 0) ? GetBuffer((len < 0) ? text.Length : 0, space) : null);
		}
	}

	public virtual void DrawText(string text, int len, Rectangle rect)
	{
		if (text != null)
		{
			string text2 = ((len == -1) ? text : text.Substring(0, len));
			Win32.DrawText(hdc, text2, text2.Length, ref rect, formatFlags);
		}
	}

	public virtual void IntersectClipRect(int x, int y, int width, int height)
	{
		Win32.IntersectClipRect(hdc, x, y, x + width, y + height);
	}

	public virtual void ExcludeClipRect(int x, int y, int width, int height)
	{
		Win32.ExcludeClipRect(hdc, x, y, x + width, y + height);
	}

	public void IntersectClipRect(Rectangle rect)
	{
		IntersectClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
	}

	public void ExcludeClipRect(Rectangle rect)
	{
		ExcludeClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
	}

	public virtual IntPtr SaveClip(Rectangle rect)
	{
		IntPtr intPtr = Win32.CreateRectRgnIndirect(rect);
		Win32.GetClipRgn(hdc, intPtr);
		return intPtr;
	}

	public virtual void RestoreClip(IntPtr rgn)
	{
		Win32.SelectClipRgn(hdc, rgn);
		Win32.DeleteObject(rgn);
	}
}
