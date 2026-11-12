using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Stimulsoft.Controls.Win.DotNetBar;

public static class ImageHelper
{
	public unsafe static Bitmap CreateReflectionImage(Image inputImage)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		if (inputImage == null)
		{
			return null;
		}
		Bitmap val = new Bitmap(inputImage.Width, inputImage.Height, (PixelFormat)2498570);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.DrawImage(inputImage, 0, 0, inputImage.Width, inputImage.Height);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		Bitmap val3 = val;
		((Image)val3).RotateFlip((RotateFlipType)6);
		Bitmap val4 = new Bitmap(((Image)val3).Width, ((Image)val3).Height, (PixelFormat)2498570);
		BitmapData val5 = val3.LockBits(new Rectangle(0, 0, ((Image)val3).Width, ((Image)val3).Height), (ImageLockMode)1, (PixelFormat)2498570);
		BitmapData val6 = val4.LockBits(new Rectangle(0, 0, ((Image)val4).Width, ((Image)val4).Height), (ImageLockMode)1, (PixelFormat)2498570);
		byte* ptr = (byte*)(void*)val5.Scan0;
		byte* ptr2 = (byte*)(void*)val6.Scan0;
		int num = val5.Stride - val5.Width * 4;
		int height = val5.Height;
		int width = val5.Width;
		for (int i = 0; i < height; i++)
		{
			int num2 = Math.Max(195, (int)(265.0 * (((double)i + (double)val5.Height * 0.4) / (double)(float)val5.Height)));
			for (int j = 0; j < width; j++)
			{
				*ptr2 = *ptr;
				ptr2[1] = ptr[1];
				ptr2[2] = ptr[2];
				ptr2[3] = (byte)Math.Max(0, ptr[3] - num2);
				ptr += 4;
				ptr2 += 4;
			}
			ptr += num;
			ptr2 += num;
		}
		val4.UnlockBits(val6);
		val3.UnlockBits(val5);
		return val4;
	}

	public static Bitmap CreateGrayScaleImage(Image inputImage)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		if (inputImage == null)
		{
			return null;
		}
		Bitmap val = new Bitmap(inputImage.Width, inputImage.Height, (PixelFormat)2498570);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.DrawImage(inputImage, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), (GraphicsUnit)2);
			val2.Flush();
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		ColorMatrix colorMatrix = new ColorMatrix(new float[6][]
		{
			new float[5] { 0.3f, 0.3f, 0.3f, 0f, 0f },
			new float[5] { 0.59f, 0.59f, 0.59f, 0f, 0f },
			new float[5] { 0.11f, 0.11f, 0.11f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 0.4f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 0.4f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 0f, 0.4f }
		});
		Bitmap val3 = new Bitmap(((Image)val).Width, ((Image)val).Height, (PixelFormat)2498570);
		Graphics val4 = Graphics.FromImage((Image)(object)val3);
		try
		{
			ImageAttributes val5 = new ImageAttributes();
			val5.SetColorMatrix(colorMatrix);
			val4.DrawImage((Image)(object)val, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), 0, 0, ((Image)val).Width, ((Image)val).Height, (GraphicsUnit)2, val5);
			val5.Dispose();
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		((Image)val).Dispose();
		return val3;
	}
}
