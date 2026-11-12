using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class GdiPlusPainter : Painter, IPainter
{
	private Graphics graphics;

	private Graphics measureGraphics;

	private Brush brush;

	private Brush textBrush;

	private Pen pen;

	private bool transFormed;

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

	public Graphics Graphics
	{
		get
		{
			if (graphics != null)
			{
				return graphics;
			}
			return null;
		}
	}

	public Pen Pen
	{
		get
		{
			return pen;
		}
		set
		{
			pen = value;
		}
	}

	public Brush Brush
	{
		get
		{
			return brush;
		}
		set
		{
			brush = value;
		}
	}

	public GdiPlusPainter()
	{
		measureGraphics = Graphics.FromHdc(measureDC);
	}

	~GdiPlusPainter()
	{
		measureGraphics.ReleaseHdc(measureDC);
		measureGraphics.Dispose();
	}

	protected override void ClearPens()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		foreach (DictionaryEntry item in base.PenTable)
		{
			((Pen)item.Value).Dispose();
		}
		base.ClearPens();
	}

	protected override void ClearBrushes()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		foreach (DictionaryEntry item in base.BrushTable)
		{
			((Brush)item.Value).Dispose();
		}
		base.ClearBrushes();
	}

	protected override object CreatePen(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return (object)new Pen(color);
	}

	protected override object CreateBrush(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return (object)new SolidBrush(color);
	}

	protected override void SelectPen(Color color, bool select)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		pen = (Pen)SelectPen(color);
	}

	protected override void SelectBrush(Color color, bool select)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		brush = (Brush)SelectBrush(color);
	}

	protected override void SelectTextColor(Color color)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		textBrush = (Brush)SelectBrush(color);
	}

	protected virtual bool IsFontMonoSpaced()
	{
		return false;
	}

	public int StringWidth(string text)
	{
		return StringWidth(text, 0, -1);
	}

	public int StringWidth(string text, int pos, int len)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		if (len == int.MaxValue)
		{
			return int.MaxValue;
		}
		if (text == null)
		{
			return 0;
		}
		if (len == -1)
		{
			len = text.Length;
		}
		if (IsFontMonoSpaced() && base.CurrentInfo != null)
		{
			return len * base.CurrentInfo.FontWidth;
		}
		StringFormatFlags formatFlags = base.StringFormat.FormatFlags;
		string text2 = text.Substring(pos, len);
		StringFormat obj = base.StringFormat;
		obj.FormatFlags = (StringFormatFlags)(obj.FormatFlags & -16385);
		int result = (int)measureGraphics.MeasureString(text2, base.Font, new PointF(0f, 0f), base.StringFormat).Width;
		base.StringFormat.FormatFlags = formatFlags;
		return result;
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
		count = 0;
		if (text == null || width <= 0)
		{
			return 0;
		}
		for (int i = pos; i < pos + len; i++)
		{
			int num = CharWidth(text[i], 1);
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
		return StringWidth(text, pos, count);
	}

	public int CharWidth(char ch, int count)
	{
		return StringWidth(new string(ch, (count <= 0) ? 1 : count));
	}

	public int CharWidth(char ch, int width, out int count)
	{
		int num = StringWidth(new string(ch, 1));
		count = ((num != 0) ? (width / num) : 0);
		return count * num;
	}

	public void BeginPaint(Graphics graphics)
	{
		this.graphics = graphics;
	}

	public void EndPaint()
	{
		graphics = null;
	}

	public void FillRectangle(Rectangle rect)
	{
		FillRectangle(rect.X, rect.Y, rect.Width, rect.Height);
	}

	public virtual void FillRectangle(int x, int y, int width, int height)
	{
		graphics.FillRectangle(brush, x, y, width, height);
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
		try
		{
			base.BackColor = color;
			graphics.FillPolygon(brush, points);
		}
		finally
		{
			base.BackColor = color2;
		}
	}

	public virtual void FillGradient(Rectangle rect, Color beginColor, Color endColor, Point point1, Point point2)
	{
		FillGradient(rect.X, rect.Y, rect.Width, rect.Height, beginColor, endColor, point1, point2);
	}

	public virtual void FillGradient(int x, int y, int width, int height, Color beginColor, Color endColor, Point point1, Point point2)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		graphics.FillRectangle((Brush)new LinearGradientBrush(point1, point2, beginColor, endColor), x, y, width, height);
	}

	public virtual void DrawThemeBackground(IntPtr handle, int partID, int stateID, Rectangle rect)
	{
		IntPtr hdc = graphics.GetHdc();
		try
		{
			Win32.GdiRect rect2 = new Win32.GdiRect(rect);
			Win32.DrawThemeBackground(handle, hdc, partID, stateID, ref rect2, IntPtr.Zero);
		}
		finally
		{
			graphics.ReleaseHdc(hdc);
		}
	}

	public void DrawRectangle(Rectangle rect)
	{
		DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height);
	}

	public virtual void DrawRectangle(int x, int y, int width, int height)
	{
		graphics.DrawRectangle(pen, x, y, width, height);
	}

	public virtual void DrawFocusRect(int x, int y, int width, int height, Color color)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		Pen val = new Pen(color, 1f);
		val.DashStyle = (DashStyle)2;
		graphics.DrawRectangle(val, x, y, width, height);
		val.Dispose();
	}

	public void DrawFocusRect(Rectangle rect, Color color)
	{
		DrawFocusRect(rect.Left, rect.Top, rect.Width, rect.Height, color);
	}

	public virtual void DrawRoundRectangle(int left, int top, int right, int bottom, int width, int height)
	{
		graphics.DrawArc(pen, left, top, right - left, bottom - top, 0, 360);
	}

	public virtual void DrawLine(int x1, int y1, int x2, int y2)
	{
		graphics.DrawLine(pen, x1, y1, x2, y2);
	}

	public virtual void DrawLine(int x1, int y1, int x2, int y2, Color color, int width, DashStyle penStyle)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Pen val = new Pen(color, (float)width);
		val.DashStyle = penStyle;
		graphics.DrawLine(val, x1, y1, x2, y2);
		val.Dispose();
	}

	public virtual void DrawDotLine(int x1, int y1, int x2, int y2, Color color)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		Pen val = new Pen(color, 1f);
		val.DashStyle = (DashStyle)2;
		graphics.DrawLine(val, x1, y1, x2, y2);
		val.Dispose();
	}

	public virtual void DrawWave(Rectangle rect, Color color)
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
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
		graphics.DrawBeziers((Pen)SelectPen(color), array);
	}

	public virtual void DrawImage(ImageList images, int index, Rectangle rect)
	{
		if (images != null && index >= 0 && index < images.Images.Count)
		{
			graphics.DrawImage(images.Images[index], rect.Left, rect.Top);
		}
	}

	public virtual void DrawImage(ImageList images, int index, Rectangle rect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (images != null && index >= 0 && index < images.Images.Count)
		{
			graphics.DrawImage(images.Images[index], rect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr);
		}
	}

	public virtual void StretchDrawImage(Rectangle rect, Rectangle stretchRect, Rectangle imageRect, Bitmap image)
	{
		if (image != null)
		{
			int num = imageRect.Width - stretchRect.Right;
			int num2 = imageRect.Height - stretchRect.Bottom;
			graphics.DrawImage((Image)(object)image, rect.Left, rect.Top, new Rectangle(imageRect.Left, imageRect.Top, stretchRect.Left, stretchRect.Top), (GraphicsUnit)2);
			graphics.DrawImage((Image)(object)image, new Rectangle(rect.Left, rect.Top + stretchRect.Top, stretchRect.Left, rect.Height - num2 - stretchRect.Top), imageRect.Left, imageRect.Top + stretchRect.Top, stretchRect.Left, stretchRect.Height, (GraphicsUnit)2);
			graphics.DrawImage((Image)(object)image, rect.Left, rect.Bottom - num2, new Rectangle(imageRect.Left, imageRect.Top + stretchRect.Bottom, stretchRect.Left, num2), (GraphicsUnit)2);
			graphics.DrawImage((Image)(object)image, new Rectangle(rect.Left + stretchRect.Left, rect.Top, rect.Width - stretchRect.Left - num, stretchRect.Top), imageRect.Left + stretchRect.Left, imageRect.Top, stretchRect.Width, stretchRect.Top, (GraphicsUnit)2);
			graphics.DrawImage((Image)(object)image, new Rectangle(rect.Left + stretchRect.Left, rect.Top + stretchRect.Top, rect.Width - stretchRect.Left - num, rect.Height - stretchRect.Top - num2), imageRect.Left + stretchRect.Left, imageRect.Top + stretchRect.Top, stretchRect.Width, stretchRect.Height, (GraphicsUnit)2);
			graphics.DrawImage((Image)(object)image, new Rectangle(rect.Left + stretchRect.Left, rect.Bottom - num2, rect.Width - stretchRect.Left - num, num2), imageRect.Left + stretchRect.Left, imageRect.Top + stretchRect.Bottom, stretchRect.Width, num2, (GraphicsUnit)2);
			graphics.DrawImage((Image)(object)image, rect.Right - num, rect.Top, new Rectangle(imageRect.Right - num, imageRect.Top, num, stretchRect.Top), (GraphicsUnit)2);
			graphics.DrawImage((Image)(object)image, new Rectangle(rect.Right - num, rect.Top + stretchRect.Top, num, rect.Height - num2 - stretchRect.Top), imageRect.Left + stretchRect.Right, imageRect.Top + stretchRect.Top, num, stretchRect.Height, (GraphicsUnit)2);
			graphics.DrawImage((Image)(object)image, rect.Right - num, rect.Bottom - num2, new Rectangle(imageRect.Left + stretchRect.Right, imageRect.Top + stretchRect.Bottom, num, num2), (GraphicsUnit)2);
		}
	}

	public void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		DrawEdge(ref rect, border, sides, 0);
	}

	public virtual void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides, int flags)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		ControlPaint.DrawBorder3D(graphics, rect, border, sides);
	}

	public virtual void DrawPolygon(Point[] points, Color color)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		graphics.DrawPolygon((Pen)SelectPen(color), points);
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
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		if (text != null)
		{
			string text2 = ((len == -1) ? text : text.Substring(0, len));
			StringFormatFlags formatFlags = base.StringFormat.FormatFlags;
			if (clipped)
			{
				StringFormat obj = base.StringFormat;
				obj.FormatFlags = (StringFormatFlags)(obj.FormatFlags & -16385);
			}
			else
			{
				StringFormat obj2 = base.StringFormat;
				obj2.FormatFlags = (StringFormatFlags)(obj2.FormatFlags | 0x4000);
			}
			if (opaque)
			{
				graphics.FillRectangle(brush, rect);
			}
			rect.X = x;
			rect.Y = y;
			graphics.DrawString(text2, base.Font, textBrush, (RectangleF)rect, base.StringFormat);
			base.StringFormat.FormatFlags = formatFlags;
		}
	}

	public virtual void DrawText(string text, int len, Rectangle rect)
	{
		if (text != null)
		{
			string text2 = ((len == -1) ? text : text.Substring(0, len));
			graphics.DrawString(text2, base.Font, textBrush, (RectangleF)rect, base.StringFormat);
		}
	}

	public virtual void Transform(int x, int y, float scaleX, float scaleY)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		transFormed = true;
		graphics.Transform = new Matrix(scaleX, 0f, 0f, scaleY, (float)x * scaleX, (float)y * scaleY);
	}

	public virtual void EndTransform()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		if (transFormed)
		{
			graphics.Transform = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
			transFormed = false;
		}
	}

	public virtual void IntersectClipRect(int x, int y, int width, int height)
	{
		graphics.IntersectClip(new Rectangle(x, y, width, height));
	}

	public virtual void ExcludeClipRect(int x, int y, int width, int height)
	{
		graphics.ExcludeClip(new Rectangle(x, y, width, height));
	}

	public virtual void IntersectClipRect(Rectangle rect)
	{
		graphics.IntersectClip(rect);
	}

	public virtual void ExcludeClipRect(Rectangle rect)
	{
		graphics.ExcludeClip(rect);
	}

	public virtual IntPtr SaveClip(Rectangle rect)
	{
		Region clip = graphics.Clip;
		IntPtr hrgn = clip.GetHrgn(graphics);
		clip.Dispose();
		return hrgn;
	}

	public virtual void RestoreClip(IntPtr rgn)
	{
		if (rgn != IntPtr.Zero)
		{
			Region val = Region.FromHrgn(rgn);
			graphics.SetClip(val, (CombineMode)5);
			val.Dispose();
		}
	}
}
