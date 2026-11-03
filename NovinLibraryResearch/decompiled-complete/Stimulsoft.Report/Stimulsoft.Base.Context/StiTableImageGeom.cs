using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Stimulsoft.Base.Context;

public class StiTableImageGeom : StiTableGeom
{
	public Image Image { get; set; }

	public RectangleF Rect { get; set; }

	public override StiTableGeomType Type => StiTableGeomType.Image;

	public static void CheckImage(ref Image image, ref RectangleF rect)
	{
		rect = GetRect(image, rect);
		image = (Image)(object)ResizeImage(image, (int)rect.Width, (int)rect.Height);
	}

	private static RectangleF GetRect(Image image, RectangleF rect)
	{
		RectangleF result = default(RectangleF);
		float num = (float)image.Width / rect.Width;
		float num2 = (float)image.Height / num;
		if (num2 <= rect.Height)
		{
			result.Width = rect.Width;
			result.Height = num2;
			result.X = rect.X + (rect.Width - result.Width) / 2f;
			result.Y = rect.Y;
		}
		else
		{
			float num3 = (float)image.Height / rect.Height;
			result.Width = (float)image.Width / num3;
			result.Height = rect.Height;
			result.X = rect.X + (rect.Width - result.Width) / 2f;
			result.Y = rect.Y;
		}
		return result;
	}

	private static Bitmap ResizeImage(Image image, int width, int height)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(0, 0, width, height);
		Bitmap val = new Bitmap(width, height);
		val.SetResolution(image.HorizontalResolution, image.VerticalResolution);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingMode = (CompositingMode)1;
			val2.CompositingQuality = (CompositingQuality)2;
			val2.InterpolationMode = (InterpolationMode)7;
			val2.SmoothingMode = (SmoothingMode)2;
			val2.PixelOffsetMode = (PixelOffsetMode)2;
			ImageAttributes val3 = new ImageAttributes();
			try
			{
				val3.SetWrapMode((WrapMode)3);
				val2.DrawImage(image, rectangle, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val3);
				return val;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public StiTableImageGeom(Image image, RectangleF rect)
	{
		Rect = rect;
		Image = image;
	}
}
