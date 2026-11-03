using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Stimulsoft.Report.Export;

internal class StiBmpHelper
{
	private static void dwByte(byte[] bfData, int position, byte dataValue)
	{
		bfData[position] = dataValue;
	}

	private static void dwByte(byte[] bfData, int position, byte dv1, byte dv2)
	{
		dwByte(bfData, position, dv1);
		dwByte(bfData, position + 1, dv2);
	}

	private static void dwWord(byte[] bfData, int position, short dataValue)
	{
		bfData[position] = (byte)((uint)dataValue & 0xFFu);
		bfData[position + 1] = (byte)((uint)(dataValue >> 8) & 0xFFu);
	}

	private static void dwDWord(byte[] bfData, int position, int dataValue)
	{
		bfData[position] = (byte)((uint)dataValue & 0xFFu);
		bfData[position + 1] = (byte)((uint)(dataValue >> 8) & 0xFFu);
		bfData[position + 2] = (byte)((uint)(dataValue >> 16) & 0xFFu);
		bfData[position + 3] = (byte)((uint)(dataValue >> 24) & 0xFFu);
	}

	public static void SaveToStreamMonochrome(Image imageForExport, StiMonochromeDitheringType ditheringType, Stream stream)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		Bitmap val;
		if (imageForExport is Bitmap)
		{
			val = (Bitmap)(object)((imageForExport is Bitmap) ? imageForExport : null);
		}
		else
		{
			val = new Bitmap(imageForExport.Width, imageForExport.Height, (PixelFormat)2498570);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				val2.DrawImage(imageForExport, 0, 0, imageForExport.Width, imageForExport.Height);
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
					b = (byte)((num4 >= 128) ? 255u : 0u);
				}
				if (ditheringType == StiMonochromeDitheringType.FloydSteinberg)
				{
					num4 += array2[k, l] - 128;
					b = (byte)((num4 >= 128) ? 255u : 0u);
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
					b = (byte)(((float)num4 >= 15f * (float)array3[l & 3, k & 3]) ? 255u : 0u);
				}
				array2[k, l] = b;
			}
		}
		int num6 = ((width - 1) / 32 + 1) * 4;
		int num7 = num6 * height;
		byte[] array4 = new byte[num7];
		for (int m = 0; m < height; m++)
		{
			byte b2 = 128;
			int num8 = (height - 1 - m) * num6;
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
					array4[num8] = b3;
					b3 = 0;
					b2 = 128;
					num8++;
				}
			}
			if (b2 != 128)
			{
				array4[num8] = b3;
			}
		}
		int num9 = 62;
		int dataValue = num9 + num7;
		byte[] array5 = new byte[num9];
		dwByte(array5, 0, 66, 77);
		dwDWord(array5, 2, dataValue);
		dwDWord(array5, 6, 0);
		dwDWord(array5, 10, 62);
		dwDWord(array5, 14, 40);
		dwDWord(array5, 18, width);
		dwDWord(array5, 22, height);
		dwWord(array5, 26, 1);
		dwWord(array5, 28, 1);
		dwDWord(array5, 30, 0);
		dwDWord(array5, 34, num7);
		dwDWord(array5, 38, 3937);
		dwDWord(array5, 42, 3937);
		dwDWord(array5, 46, 0);
		dwDWord(array5, 50, 0);
		dwDWord(array5, 54, 0);
		dwDWord(array5, 58, 16777215);
		stream.Write(array5, 0, num9);
		stream.Write(array4, 0, num7);
		stream.Flush();
	}
}
