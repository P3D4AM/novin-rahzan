using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Stimulsoft.Base.Drawing;

public class StiRotatedTextDrawing
{
	public static PointF GetStartPoint(StiRotationMode rotationMode, RectangleF textRect)
	{
		PointF pointF = new PointF(textRect.X + textRect.Width / 2f, textRect.Y + textRect.Height / 2f);
		return rotationMode switch
		{
			StiRotationMode.LeftCenter => new PointF(textRect.X, pointF.Y), 
			StiRotationMode.LeftBottom => new PointF(textRect.X, textRect.Bottom), 
			StiRotationMode.CenterTop => new PointF(pointF.X, textRect.Top), 
			StiRotationMode.CenterCenter => pointF, 
			StiRotationMode.CenterBottom => new PointF(pointF.X, textRect.Bottom), 
			StiRotationMode.RightTop => new PointF(textRect.Right, textRect.Top), 
			StiRotationMode.RightCenter => new PointF(textRect.Right, pointF.Y), 
			StiRotationMode.RightBottom => new PointF(textRect.Right, textRect.Bottom), 
			_ => textRect.Location, 
		};
	}

	private static RectangleF DrawString(Graphics g, string text, Font font, Brush brush, PointF point, StringFormat stringFormat, StiRotationMode rotationMode, float angle, bool antialiasing, bool draw, bool measure, int maximalWidth)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(text))
		{
			return new RectangleF(point.X, point.Y, 0f, 0f);
		}
		SizeF sizeF = g.MeasureString(text, font, maximalWidth, stringFormat);
		if (sizeF.Width == 0f && sizeF.Height == 0f)
		{
			return new RectangleF(point.X, point.Y, 0f, 0f);
		}
		GraphicsState val = g.Save();
		TextRenderingHint textRenderingHint = g.TextRenderingHint;
		if (antialiasing)
		{
			g.TextRenderingHint = (TextRenderingHint)4;
		}
		float x = point.X;
		float y = point.Y;
		RectangleF rectangleF = new RectangleF(0f, 0f, sizeF.Width, sizeF.Height);
		g.TranslateTransform(x, y);
		if (angle != 0f)
		{
			g.RotateTransform(angle);
		}
		PointF startPoint = GetStartPoint(rotationMode, rectangleF);
		rectangleF.X -= startPoint.X;
		rectangleF.Y -= startPoint.Y;
		RectangleF result = RectangleF.Empty;
		if (rectangleF.Width != 0f && rectangleF.Height != 0f)
		{
			if (draw)
			{
				g.DrawString(text, font, brush, rectangleF, stringFormat);
			}
			if (measure)
			{
				GraphicsPath val2 = new GraphicsPath();
				try
				{
					val2.AddRectangle(rectangleF);
					val2.Transform(g.Transform);
					result = val2.GetBounds();
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			if (antialiasing)
			{
				g.TextRenderingHint = textRenderingHint;
			}
		}
		if (angle != 0f)
		{
			g.RotateTransform(0f - angle);
		}
		g.TranslateTransform(0f - x, 0f - y);
		if (measure)
		{
			result = new RectangleF(result.X - g.Transform.OffsetX, result.Y - g.Transform.OffsetY, result.Width, result.Height);
		}
		result.Width++;
		result.Height++;
		g.Restore(val);
		return result;
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, PointF point, StringFormat stringFormat, StiRotationMode rotationMode, float angle, bool antialiasing)
	{
		DrawString(g, text, font, brush, point, stringFormat, rotationMode, angle, antialiasing, 0);
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, PointF point, StringFormat stringFormat, StiRotationMode rotationMode, float angle, bool antialiasing, int maximalWidth)
	{
		DrawString(g, text, font, brush, point, stringFormat, rotationMode, angle, antialiasing, draw: true, measure: false, maximalWidth);
	}

	public static RectangleF MeasureString(Graphics g, string text, Font font, PointF point, StringFormat stringFormat, StiRotationMode rotationMode, float angle, int maximalWidth)
	{
		return DrawString(g, text, font, null, point, stringFormat, rotationMode, angle, antialiasing: false, draw: false, measure: true, maximalWidth);
	}

	public static RectangleF MeasureString(Graphics g, string text, Font font, PointF point, StringFormat stringFormat, StiRotationMode rotationMode, float angle)
	{
		return MeasureString(g, text, font, point, stringFormat, rotationMode, angle, 0);
	}

	public static RectangleF MeasureString(Graphics g, string text, Font font, RectangleF rect, StringFormat stringFormat, StiRotationMode rotationMode, float angle, int maximalWidth)
	{
		PointF point = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
		return DrawString(g, text, font, null, point, stringFormat, rotationMode, angle, antialiasing: false, draw: false, measure: true, maximalWidth);
	}

	public static RectangleF MeasureString(Graphics g, string text, Font font, RectangleF rect, StringFormat stringFormat, StiRotationMode rotationMode, float angle)
	{
		return MeasureString(g, text, font, rect, stringFormat, rotationMode, angle, 0);
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleF rect, StringFormat stringFormat, StiRotationMode rotationMode, float angle, bool antialiasing, int maximalWidth)
	{
		PointF point = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
		DrawString(g, text, font, brush, point, stringFormat, rotationMode, angle, antialiasing, maximalWidth);
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleF rect, StringFormat stringFormat, StiRotationMode rotationMode, float angle, bool antialiasing)
	{
		DrawString(g, text, font, brush, rect, stringFormat, rotationMode, angle, antialiasing, 0);
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleF rect, StringFormat stringFormat, float angle, bool antialiasing, int maximalWidth)
	{
		DrawString(g, text, font, brush, rect, stringFormat, StiRotationMode.CenterCenter, angle, antialiasing, maximalWidth);
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleF rect, StringFormat stringFormat, float angle, bool antialiasing)
	{
		DrawString(g, text, font, brush, rect, stringFormat, angle, antialiasing, 0);
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleF rect, StringFormat stringFormat, float angle, int maximalWidth)
	{
		DrawString(g, text, font, brush, rect, stringFormat, angle, antialiasing: false, maximalWidth);
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleF rect, StringFormat stringFormat, float angle)
	{
		DrawString(g, text, font, brush, rect, stringFormat, angle, 0);
	}
}
