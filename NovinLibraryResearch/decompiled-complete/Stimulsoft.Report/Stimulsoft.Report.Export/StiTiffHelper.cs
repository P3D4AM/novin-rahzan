using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Stimulsoft.Report.Export;

public class StiTiffHelper
{
	public static Image MakeMonochromeImage(Image inputImage, StiMonochromeDitheringType ditheringType, int diffusionPower)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		Bitmap val = null;
		if (inputImage is Bitmap)
		{
			val = (Bitmap)(object)((inputImage is Bitmap) ? inputImage : null);
		}
		else
		{
			val = new Bitmap(inputImage.Width, inputImage.Height, (PixelFormat)2498570);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				val2.DrawImage(inputImage, 0, 0, inputImage.Width, inputImage.Height);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		int width = ((Image)val).Width;
		int height = ((Image)val).Height;
		Rectangle rectangle = new Rectangle(0, 0, width, height);
		BitmapData val3 = val.LockBits(rectangle, (ImageLockMode)1, (PixelFormat)2498570);
		int num = Math.Abs(val3.Stride);
		int num2 = num * val3.Height;
		byte[] array = new byte[num2];
		Marshal.Copy(val3.Scan0, array, 0, num2);
		val.UnlockBits(val3);
		byte[,] array2 = new byte[height + 1, width + 1];
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				array2[i, j] = 128;
			}
		}
		int[,] array3 = new int[4, 4]
		{
			{ 1, 9, 3, 11 },
			{ 13, 5, 15, 7 },
			{ 4, 12, 2, 10 },
			{ 16, 8, 14, 6 }
		};
		for (int k = 0; k < height; k++)
		{
			int num3 = k * num;
			for (int l = 0; l < width; l++)
			{
				int num4 = (int)((float)(int)array[num3 + 2] * 0.3f + (float)(int)array[num3 + 1] * 0.59f + (float)(int)array[num3] * 0.11f);
				num3 += 4;
				byte b = 0;
				if (ditheringType == StiMonochromeDitheringType.None)
				{
					b = (byte)((num4 >= diffusionPower) ? 255u : 0u);
				}
				if (ditheringType == StiMonochromeDitheringType.FloydSteinberg)
				{
					num4 += array2[k, l] - 128;
					b = (byte)((num4 >= diffusionPower) ? 255u : 0u);
					int num5 = num4 - b;
					array2[k, l + 1] += (byte)(0.4375f * (float)num5);
					if (l != 0)
					{
						array2[k + 1, l - 1] += (byte)(0.1875f * (float)num5);
					}
					array2[k + 1, l] += (byte)(0.3125f * (float)num5);
					array2[k + 1, l + 1] += (byte)(0.0625f * (float)num5);
				}
				if (ditheringType == StiMonochromeDitheringType.Ordered)
				{
					b = (byte)(((float)num4 >= (float)(diffusionPower * 2) / 17f * (float)array3[l & 3, k & 3]) ? 255u : 0u);
				}
				array2[k, l] = b;
			}
		}
		Bitmap val4 = new Bitmap(width, height, (PixelFormat)196865);
		BitmapData val5 = val4.LockBits(rectangle, (ImageLockMode)2, (PixelFormat)196865);
		int num6 = Math.Abs(val5.Stride);
		byte[] array4 = new byte[num6 * height];
		for (int m = 0; m < height; m++)
		{
			byte b2 = 128;
			int num7 = m * num6;
			byte b3 = 0;
			for (int n = 0; n < width; n++)
			{
				if (array2[m, n] != 0)
				{
					b3 |= b2;
				}
				b2 >>= 1;
				if (b2 == 0)
				{
					array4[num7] = b3;
					b3 = 0;
					b2 = 128;
					num7++;
				}
			}
			if (b2 != 128)
			{
				array4[num7] = b3;
			}
		}
		Marshal.Copy(array4, 0, val5.Scan0, array4.Length);
		val4.UnlockBits(val5);
		return (Image)(object)val4;
	}
}
