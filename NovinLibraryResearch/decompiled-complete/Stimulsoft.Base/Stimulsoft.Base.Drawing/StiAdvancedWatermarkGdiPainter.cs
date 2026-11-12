using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Base.Drawing;

public static class StiAdvancedWatermarkGdiPainter
{
	public static void PaintWatermark(Graphics g, StiAdvancedWatermark watermark, StiCornerRadius cornerRadius, RectangleD rect, double scale)
	{
		GraphicsState val = g.Save();
		GraphicsPath val2 = null;
		try
		{
			if (cornerRadius == null || cornerRadius.IsEmpty)
			{
				g.SetClip(rect.ToRectangleF());
			}
			else
			{
				val2 = StiRoundedRectangleCreator.Create(rect.ToRectangleF(), cornerRadius, (float)scale);
				g.SetClip(val2);
			}
			PaintWatermarkImage(g, watermark, rect, scale);
			PaintWatermarkText(g, watermark, rect, scale);
			PaintWatermarkWeave(g, watermark, rect, scale);
		}
		finally
		{
			if (val2 != null)
			{
				val2.Dispose();
			}
			g.Restore(val);
		}
	}

	private static void PaintWatermarkText(Graphics g, StiAdvancedWatermark watermark, RectangleD rect, double scale)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(watermark.Text) || !watermark.TextEnabled)
		{
			return;
		}
		float num = (float)((double)watermark.TextFont.Size * scale / StiScale.Factor);
		SolidBrush val = new SolidBrush(watermark.TextColor);
		try
		{
			Font val2 = new Font(watermark.TextFont.FontFamily, num, watermark.TextFont.Style, watermark.TextFont.Unit, watermark.TextFont.GdiCharSet, watermark.TextFont.GdiVerticalFont);
			try
			{
				TextRenderingHint textRenderingHint = g.TextRenderingHint;
				try
				{
					g.TextRenderingHint = (TextRenderingHint)4;
					StiTextOptions textOptions = new StiTextOptions
					{
						Angle = watermark.TextAngle
					};
					StiTextDrawing.DrawString(g, watermark.Text, val2, (Brush)(object)val, rect, textOptions, StiTextHorAlignment.Center, StiVertAlignment.Center, antialiasing: true, watermark.TextAngle);
				}
				finally
				{
					g.TextRenderingHint = textRenderingHint;
				}
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

	private static void PaintWatermarkImage(Graphics g, StiAdvancedWatermark watermark, RectangleD rect, double scale)
	{
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		if (!watermark.ImageEnabled)
		{
			return;
		}
		byte[] image = GetImage(watermark);
		if (image == null)
		{
			return;
		}
		if (watermark.ImageStretch)
		{
			RectangleF rectangleF = rect.ToRectangleF();
			Image val = StiImageConverter.BytesToImage(image, (int)rectangleF.Width, (int)rectangleF.Height);
			try
			{
				if (watermark.ImageAspectRatio)
				{
					float num = rectangleF.Width / (float)val.Width;
					float num2 = rectangleF.Height / (float)val.Height;
					if (num > num2)
					{
						rectangleF.X = (rectangleF.Width - (float)val.Width * num2) / 2f;
						rectangleF.Width = (float)val.Width * num2;
					}
					else
					{
						rectangleF.Y = (rectangleF.Height - (float)val.Height * num) / 2f;
						rectangleF.Height = (float)val.Height * num;
					}
					float num3 = Math.Max(num, num2);
					SizeD size = new SizeD((float)val.Size.Width * num3, (float)val.Height * num3);
					rectangleF = StiRectangleUtils.AlignSizeInRect(rect, size, watermark.ImageAlignment).ToRectangleF();
				}
				g.DrawImage(val, rectangleF);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		Image val2 = StiImageConverter.BytesToImage(image);
		try
		{
			scale *= watermark.ImageMultipleFactor;
			SizeD sizeD = new SizeD((double)val2.Size.Width * scale, (double)val2.Height * scale);
			if (watermark.ImageTiling)
			{
				PaintWatermarkTileImage(val2, g, rect, sizeD);
			}
			else
			{
				g.DrawImage(val2, StiRectangleUtils.AlignSizeInRect(rect, sizeD, watermark.ImageAlignment).ToRectangleF());
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private static void PaintWatermarkWeave(Graphics g, StiAdvancedWatermark watermark, RectangleD rect, double scale)
	{
		if (!watermark.WeaveEnabled || (!watermark.WeaveMajorIcon.HasValue && !watermark.WeaveMinorIcon.HasValue))
		{
			return;
		}
		int num = (int)Math.Ceiling((double)(5f * (float)watermark.WeaveMajorSize) * scale);
		int num2 = (int)Math.Ceiling((double)(5f * (float)watermark.WeaveMinorSize) * scale);
		int num3 = (int)Math.Ceiling((double)watermark.WeaveDistance * scale);
		Image val = (watermark.WeaveMajorIcon.HasValue ? StiFontIconsHelper.ConvertFontIconToImage(watermark.WeaveMajorIcon.Value, watermark.WeaveMajorColor, num, num) : null);
		Image val2 = (watermark.WeaveMinorIcon.HasValue ? StiFontIconsHelper.ConvertFontIconToImage(watermark.WeaveMinorIcon.Value, watermark.WeaveMinorColor, num2, num2) : null);
		double num4 = rect.X + rect.Width / 2.0;
		double num5 = rect.Y + rect.Height / 2.0;
		double num6 = num4;
		double num7 = num5;
		for (int i = 0; i < 30; i++)
		{
			double num8 = (double)(watermark.WeaveAngle + 90) * (Math.PI / 180.0);
			double num9 = num6 + (double)(float)((double)(num3 * i) * Math.Cos(num8));
			double num10 = num7 + (double)(float)((double)(num3 * i) * Math.Sin(num8));
			if (!DrawWeaveLine(g, watermark, rect.ToRectangleF(), num3, val, val2, (float)num9, (float)num10, i))
			{
				break;
			}
		}
		num6 = num4;
		num7 = num5;
		for (int j = 1; j < 30; j++)
		{
			double num11 = (double)(watermark.WeaveAngle - 90) * (Math.PI / 180.0);
			double num12 = num6 + (double)(float)((double)(num3 * j) * Math.Cos(num11));
			double num13 = num7 + (double)(float)((double)(num3 * j) * Math.Sin(num11));
			if (!DrawWeaveLine(g, watermark, rect.ToRectangleF(), num3, val, val2, (float)num12, (float)num13, -j))
			{
				break;
			}
		}
		if (val != null)
		{
			val.Dispose();
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
	}

	private static bool DrawWeaveLine(Graphics g, StiAdvancedWatermark watermark, RectangleF rect, int dist, Image imageMajor, Image imageMinor, float posX, float posY, int shift)
	{
		bool result = false;
		bool flag = false;
		double num = (double)watermark.WeaveAngle * (Math.PI / 180.0);
		for (int i = 0; i < 30; i++)
		{
			float num2 = posX + (float)((double)(dist * i) * Math.Cos(num));
			float num3 = posY + (float)((double)(dist * i) * Math.Sin(num));
			Image val = ((((i + shift) & 1) == 0) ? imageMajor : imageMinor);
			if (val != null)
			{
				if (ContainsWeaveImage(g, watermark, val, rect, num2, num3))
				{
					DrawWeaveImage(g, watermark, val, num2, num3);
					result = true;
					flag = true;
				}
				else if (flag)
				{
					break;
				}
			}
		}
		for (int j = 1; j < 30; j++)
		{
			float num4 = posX - (float)((double)(dist * j) * Math.Cos(num));
			float num5 = posY - (float)((double)(dist * j) * Math.Sin(num));
			Image val2 = ((((-j + shift) & 1) == 0) ? imageMajor : imageMinor);
			if (val2 != null)
			{
				if (ContainsWeaveImage(g, watermark, val2, rect, num4, num5))
				{
					DrawWeaveImage(g, watermark, val2, num4, num5);
					result = true;
					flag = true;
				}
				else if (flag)
				{
					break;
				}
			}
		}
		return result;
	}

	private static bool ContainsWeaveImage(Graphics g, StiAdvancedWatermark watermark, Image image, RectangleF rect, float x, float y)
	{
		if (image == null)
		{
			return false;
		}
		int num = image.Width / 2;
		int num2 = image.Height / 2;
		double num3 = (double)watermark.WeaveAngle * (Math.PI / 180.0);
		PointF pt = new PointF((float)(-num) * (float)Math.Cos(num3) + (float)(-num2) * (float)Math.Sin(num3) + x, (float)(-num) * (float)Math.Sin(num3) - (float)(-num2) * (float)Math.Cos(num3) + y);
		PointF pt2 = new PointF((float)num * (float)Math.Cos(num3) + (float)(-num2) * (float)Math.Sin(num3) + x, (float)num * (float)Math.Sin(num3) - (float)(-num2) * (float)Math.Cos(num3) + y);
		PointF pt3 = new PointF((float)(-num) * (float)Math.Cos(num3) + (float)num2 * (float)Math.Sin(num3) + x, (float)(-num) * (float)Math.Sin(num3) - (float)num2 * (float)Math.Cos(num3) + y);
		PointF pt4 = new PointF((float)num * (float)Math.Cos(num3) + (float)num2 * (float)Math.Sin(num3) + x, (float)num * (float)Math.Sin(num3) - (float)num2 * (float)Math.Cos(num3) + y);
		if (!rect.Contains(pt) && !rect.Contains(pt2) && !rect.Contains(pt3))
		{
			return rect.Contains(pt4);
		}
		return true;
	}

	private static void DrawWeaveImage(Graphics g, StiAdvancedWatermark watermark, Image image, float posX, float posY)
	{
		GraphicsState val = g.Save();
		try
		{
			g.TranslateTransform(posX, posY);
			g.RotateTransform((float)watermark.WeaveAngle);
			posX = -image.Width / 2;
			posY = -image.Height / 2;
			RectangleF rectangleF = new RectangleF(posX, posY, image.Width, image.Height);
			g.DrawImage(image, rectangleF);
		}
		finally
		{
			g.Restore(val);
		}
	}

	public static void PaintWatermarkTileImage(Image image, Graphics g, RectangleD rect, SizeD imageSize)
	{
		for (float num = (float)rect.Y; (double)num < rect.Bottom; num += (float)imageSize.Height)
		{
			for (float num2 = (float)rect.X; (double)num2 < rect.Right; num2 += (float)imageSize.Width)
			{
				g.DrawImage(image, new RectangleF(num2, num, (float)imageSize.Width, (float)imageSize.Height));
			}
		}
	}

	public static byte[] GetImage(StiAdvancedWatermark watermark)
	{
		byte[] array = watermark.TakeImage();
		if (watermark.ImageTransparency == 0)
		{
			return array;
		}
		if (array == null)
		{
			return null;
		}
		if (watermark.GetCachedImage() == null)
		{
			Image source = StiImageConverter.BytesToImage(array);
			Bitmap transparentedImage = StiImageTransparenceHelper.GetTransparentedImage(source, 1f - (float)watermark.ImageTransparency / 255f);
			watermark.PutCachedImage(StiImageConverter.ImageToBytes((Image)(object)transparentedImage));
		}
		return watermark.GetCachedImage();
	}
}
