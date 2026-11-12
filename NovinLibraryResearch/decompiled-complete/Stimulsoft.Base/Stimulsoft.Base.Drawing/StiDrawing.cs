using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Base.Drawing;

public sealed class StiDrawing
{
	public static void DrawRectangle(Graphics g, Color color, double x, double y, double width, double height)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Pen val = new Pen(color);
		try
		{
			DrawRectangle(g, val, new RectangleD(x, y, width, height));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawRectangle(Graphics g, Color color, RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Pen val = new Pen(color);
		try
		{
			DrawRectangle(g, val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawRectangle(Graphics g, Color color, RectangleF rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Pen val = new Pen(color);
		try
		{
			DrawRectangle(g, val, rect.X, rect.Y, rect.Width, rect.Height);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawRectangle(Graphics g, Color color, Rectangle rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Pen val = new Pen(color);
		try
		{
			DrawRectangle(g, val, rect.X, rect.Y, rect.Width, rect.Height);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawRectangle(Graphics g, Pen pen, double x, double y, double width, double height)
	{
		DrawRectangle(g, pen, new RectangleD(x, y, width, height));
	}

	public static void DrawRectangle(Graphics g, Pen pen, RectangleD rect)
	{
		g.DrawRectangle(pen, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
	}

	public static void FillRectangle(Graphics g, Color color, double x, double y, double width, double height)
	{
		FillRectangle(g, color, new RectangleD(x, y, width, height));
	}

	public static void FillRectangle(Graphics g, Color color, RectangleD rect)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (color == Color.Transparent)
		{
			return;
		}
		Brush val = (Brush)new SolidBrush(color);
		try
		{
			FillRectangle(g, val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void FillRectangle(Graphics g, Color color, RectangleF rect)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (color == Color.Transparent)
		{
			return;
		}
		Brush val = (Brush)new SolidBrush(color);
		try
		{
			FillRectangle(g, val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void FillRectangle(Graphics g, StiBrush brush, double x, double y, double width, double height)
	{
		FillRectangle(g, brush, new RectangleD(x, y, width, height));
	}

	public static void FillRectangle(Graphics g, StiBrush brush, RectangleD rect)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		if (brush == null)
		{
			return;
		}
		Brush brush2 = StiBrush.GetBrush(brush, rect);
		try
		{
			StiGradientBrush stiGradientBrush = brush as StiGradientBrush;
			if (StiBaseOptions.FixBandedGradients && (int)g.PageUnit != 1 && stiGradientBrush != null && stiGradientBrush.Angle != 0.0 && stiGradientBrush.Angle != 180.0)
			{
				PointF[] array = new PointF[1]
				{
					new PointF((float)rect.Width, (float)rect.Height)
				};
				g.TransformPoints((CoordinateSpace)1, (CoordinateSpace)2, array);
				Bitmap val = new Bitmap((int)(array[0].X / g.PageScale), (int)(array[0].Y / g.PageScale), g);
				try
				{
					Graphics val2 = Graphics.FromImage((Image)(object)val);
					try
					{
						val2.PageUnit = g.PageUnit;
						val2.PageScale = g.PageScale;
						val2.TranslateTransform(0f - (float)rect.Left, 0f - (float)rect.Top);
						FillRectangle(val2, brush2, new RectangleD(rect.Left, rect.Top, rect.Width, rect.Height));
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					g.DrawImage((Image)(object)val, (float)rect.Left, (float)rect.Top);
					return;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			FillRectangle(g, brush2, rect);
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public static void FillRectangle(Graphics g, StiBrush brush, RectangleF rect)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		if (brush == null)
		{
			return;
		}
		Brush brush2 = StiBrush.GetBrush(brush, rect);
		try
		{
			StiGradientBrush stiGradientBrush = brush as StiGradientBrush;
			if (StiBaseOptions.FixBandedGradients && (int)g.PageUnit != 1 && stiGradientBrush != null && stiGradientBrush.Angle != 0.0 && stiGradientBrush.Angle != 180.0)
			{
				PointF[] array = new PointF[1]
				{
					new PointF(rect.Width, rect.Height)
				};
				g.TransformPoints((CoordinateSpace)1, (CoordinateSpace)2, array);
				Bitmap val = new Bitmap((int)(array[0].X / g.PageScale), (int)(array[0].Y / g.PageScale), g);
				try
				{
					Graphics val2 = Graphics.FromImage((Image)(object)val);
					try
					{
						val2.PageUnit = g.PageUnit;
						val2.PageScale = g.PageScale;
						val2.TranslateTransform(0f - rect.Left, 0f - rect.Top);
						FillRectangle(val2, brush2, new RectangleD(rect.Left, rect.Top, rect.Width, rect.Height));
						g.DrawImage((Image)(object)val, rect.Left, rect.Top);
						return;
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			FillRectangle(g, brush2, rect);
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public static void FillRectangle(Graphics g, Brush brush, double x, double y, double width, double height)
	{
		FillRectangle(g, brush, new RectangleD(x, y, width, height));
	}

	public static void FillRectangle(Graphics g, Brush brush, RectangleD rect)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (brush != null && (!(brush is SolidBrush) || !(((SolidBrush)brush).Color == Color.Transparent)))
		{
			g.FillRectangle(brush, rect.ToRectangleF());
		}
	}

	public static void FillRectangle(Graphics g, Brush brush, RectangleF rect)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (brush != null && (!(brush is SolidBrush) || !(((SolidBrush)brush).Color == Color.Transparent)))
		{
			g.FillRectangle(brush, rect);
		}
	}

	public static void FillRectangle(Graphics g, Color color, Rectangle rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		SolidBrush val = new SolidBrush(color);
		try
		{
			g.FillRectangle((Brush)(object)val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void FillEllipse(Graphics g, Color color, RectangleD rect)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (color == Color.Transparent)
		{
			return;
		}
		Brush val = (Brush)new SolidBrush(color);
		try
		{
			g.FillEllipse(val, rect.ToRectangleF());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void FillEllipse(Graphics g, Color color, RectangleF rect)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (color == Color.Transparent)
		{
			return;
		}
		Brush val = (Brush)new SolidBrush(color);
		try
		{
			g.FillEllipse(val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void FillEllipse(Graphics g, Brush brush, RectangleF rect)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (brush != null && (!(brush is SolidBrush) || !(((SolidBrush)brush).Color == Color.Transparent)))
		{
			g.FillEllipse(brush, rect);
		}
	}

	public static void FillEllipse(Graphics g, StiBrush brush, RectangleF rect)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		if (brush == null)
		{
			return;
		}
		Brush brush2 = StiBrush.GetBrush(brush, rect);
		try
		{
			StiGradientBrush stiGradientBrush = brush as StiGradientBrush;
			if (StiBaseOptions.FixBandedGradients && (int)g.PageUnit != 1 && stiGradientBrush != null && stiGradientBrush.Angle != 0.0 && stiGradientBrush.Angle != 180.0)
			{
				PointF[] array = new PointF[1]
				{
					new PointF(rect.Width, rect.Height)
				};
				g.TransformPoints((CoordinateSpace)1, (CoordinateSpace)2, array);
				Bitmap val = new Bitmap((int)(array[0].X / g.PageScale), (int)(array[0].Y / g.PageScale), g);
				try
				{
					Graphics val2 = Graphics.FromImage((Image)(object)val);
					try
					{
						val2.PageUnit = g.PageUnit;
						val2.PageScale = g.PageScale;
						val2.TranslateTransform(0f - rect.Left, 0f - rect.Top);
						g.FillEllipse(brush2, new RectangleF(rect.Left, rect.Top, rect.Width, rect.Height));
						g.DrawImage((Image)(object)val, rect.Left, rect.Top);
						return;
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			FillEllipse(g, brush2, rect);
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public static void DrawLine(Graphics g, Pen pen, double x1, double y1, double x2, double y2)
	{
		g.DrawLine(pen, (float)x1, (float)y1, (float)x2, (float)y2);
	}

	public static void DrawLine(Graphics g, Color color, double x1, double y1, double x2, double y2)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Pen val = new Pen(color);
		try
		{
			g.DrawLine(val, (float)x1, (float)y1, (float)x2, (float)y2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawSelectedPoint(Graphics g, float size, Brush brush, double x, double y)
	{
		FillRectangle(g, brush, x - (double)size, y - (double)size, size * 2f + 1f, size * 2f + 1f);
	}

	public static void DrawSelectedRectangle(Graphics g, int size, Brush brush, RectangleD rect)
	{
		DrawSelectedPoint(g, size, brush, rect.Left, rect.Top);
		DrawSelectedPoint(g, size, brush, rect.Right, rect.Top);
		DrawSelectedPoint(g, size, brush, rect.Left, rect.Bottom);
		DrawSelectedPoint(g, size, brush, rect.Right, rect.Bottom);
		DrawSelectedPoint(g, size, brush, rect.Left + rect.Width / 2.0, rect.Bottom);
		DrawSelectedPoint(g, size, brush, rect.Left, rect.Top + rect.Height / 2.0);
		DrawSelectedPoint(g, size, brush, rect.Left + rect.Width / 2.0, rect.Top);
		DrawSelectedPoint(g, size, brush, rect.Right, rect.Top + rect.Height / 2.0);
	}

	public static void DrawString(Graphics g, string text, Font font, Color color, Rectangle rect, StringFormat sf)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (color == Color.Transparent)
		{
			return;
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			g.DrawString(text, font, (Brush)(object)val, (RectangleF)rect, sf);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawString(Graphics g, string text, Font font, StiBrush brush, RectangleD rect, StringFormat sf)
	{
		if (StiBrush.IsTransparent(brush))
		{
			return;
		}
		Brush brush2 = StiBrush.GetBrush(brush, rect);
		try
		{
			g.DrawString(text, font, brush2, rect.ToRectangleF(), sf);
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public static void DrawString(Graphics g, string text, Font font, Color color, RectangleD rect, StringFormat sf)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (color == Color.Transparent)
		{
			return;
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			g.DrawString(text, font, (Brush)(object)val, rect.ToRectangleF(), sf);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawString(Graphics g, string text, Font font, Color color, RectangleF rect, StringFormat sf)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (color == Color.Transparent)
		{
			return;
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			g.DrawString(text, font, (Brush)(object)val, rect, sf);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawString(Graphics g, string text, Font font, StiBrush brush, Rectangle rect, StringFormat sf)
	{
		if (StiBrush.IsTransparent(brush))
		{
			return;
		}
		Brush brush2 = StiBrush.GetBrush(brush, rect);
		try
		{
			g.DrawString(text, font, brush2, (RectangleF)rect, sf);
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public static void DrawString(Graphics g, string text, Font font, StiBrush brush, RectangleF rect, StringFormat sf)
	{
		if (StiBrush.IsTransparent(brush))
		{
			return;
		}
		Brush brush2 = StiBrush.GetBrush(brush, rect);
		try
		{
			g.DrawString(text, font, brush2, rect, sf);
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}
}
