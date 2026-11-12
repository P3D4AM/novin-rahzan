using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Base.Drawing;

public static class StiSimpleShadowPainter
{
	public static void PaintShadow(Graphics g, RectangleD rect, StiSimpleShadow shadow, StiCornerRadius cornerRadius, bool isCircle, float scale)
	{
		Point shadowLocation = GetShadowLocation(rect, shadow, isCircle, scale);
		Bitmap shadowBitmap = GetShadowBitmap(rect, shadow, cornerRadius, isCircle, scale);
		try
		{
			g.DrawImage((Image)(object)shadowBitmap, shadowLocation);
		}
		finally
		{
			((IDisposable)shadowBitmap)?.Dispose();
		}
	}

	public static Point GetShadowLocation(RectangleD rect, StiSimpleShadow shadow, bool isCircle, float scale)
	{
		if (isCircle)
		{
			rect = FitRectangleInCircle(rect);
		}
		float num = (float)StiScale.I15 * scale;
		rect.Inflate(num, num);
		return new Point((int)rect.X + StiScale.I(shadow.Location.X), (int)rect.Y + StiScale.I(shadow.Location.Y));
	}

	public static Bitmap GetShadowBitmap(RectangleD rect, StiSimpleShadow shadow, StiCornerRadius cornerRadius, bool isCircle, float scale)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		if (shadow == null || !shadow.Visible)
		{
			return null;
		}
		if (isCircle)
		{
			rect = FitRectangleInCircle(rect);
		}
		RectangleD rectangleD = rect;
		float num = (float)StiScale.I15 * scale;
		rectangleD.Inflate(num, num);
		Bitmap val = new Bitmap((int)rectangleD.Width, (int)rectangleD.Height);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			val2.Clear(Color.Transparent);
			SolidBrush val3 = new SolidBrush(shadow.Color);
			try
			{
				Rectangle rectangle = new Rectangle((int)num, (int)num, (int)rect.Width, (int)rect.Height);
				if (isCircle)
				{
					val2.FillEllipse((Brush)(object)val3, rectangle);
				}
				else if (cornerRadius == null || cornerRadius.IsEmpty)
				{
					val2.FillRectangle((Brush)(object)val3, rectangle);
				}
				else
				{
					GraphicsPath val4 = StiRoundedRectangleCreator.Create(rectangle, cornerRadius, scale);
					try
					{
						val2.FillPath((Brush)(object)val3, val4);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			StiGaussianBlur stiGaussianBlur = new StiGaussianBlur(val);
			return stiGaussianBlur.Process((int)(StiScale.Factor * (double)(float)shadow.Size / 2.0 * (double)scale));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static RectangleD FitRectangleInCircle(RectangleD rect)
	{
		if (rect.Width > rect.Height)
		{
			return new RectangleD(rect.X + (rect.Width - rect.Height) / 2.0, rect.Y, rect.Height, rect.Height);
		}
		return new RectangleD(rect.X, rect.Y + (rect.Height - rect.Width) / 2.0, rect.Width, rect.Width);
	}
}
