using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Stimulsoft.Report.Export;

internal class StiPcxHelper
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

	private static void dwByte(byte[] bfData, int position, byte dv1, byte dv2, byte dv3)
	{
		dwByte(bfData, position, dv1, dv2);
		dwByte(bfData, position + 2, dv3);
	}

	private static void dwWord(byte[] bfData, int position, short dataValue)
	{
		bfData[position] = (byte)((uint)dataValue & 0xFFu);
		bfData[position + 1] = (byte)((uint)(dataValue >> 8) & 0xFFu);
	}

	private static void dwEncodedData(Stream stream, byte byt, int runLen)
	{
		if (runLen == 1 && (byt & 0xC0) != 192)
		{
			stream.WriteByte(byt);
			return;
		}
		stream.WriteByte((byte)((uint)runLen | 0xC0u));
		stream.WriteByte(byt);
	}

	public static void SaveToStream(Image imageForExport, StiPcxPaletteType paletteType, StiMonochromeDitheringType ditheringType, Stream stream)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		Bitmap val = null;
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
		int num3 = 0;
		byte[] array2 = null;
		if (paletteType == StiPcxPaletteType.Monochrome)
		{
			byte[,] array3 = new byte[height + 1, width + 1];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					array3[i, j] = 128;
				}
			}
			int[,] array4 = new int[4, 4]
			{
				{ 1, 9, 3, 11 },
				{ 13, 5, 15, 7 },
				{ 4, 12, 2, 10 },
				{ 16, 8, 14, 6 }
			};
			for (int k = 0; k < height; k++)
			{
				int num4 = k * num;
				for (int l = 0; l < width; l++)
				{
					int num5 = (int)((float)(int)array[num4 + 2] * 0.3f + (float)(int)array[num4 + 1] * 0.59f + (float)(int)array[num4] * 0.11f);
					num4 += 4;
					byte b = 0;
					if (ditheringType == StiMonochromeDitheringType.None)
					{
						b = (byte)((num5 >= 128) ? 255u : 0u);
					}
					if (ditheringType == StiMonochromeDitheringType.FloydSteinberg)
					{
						num5 += array3[k, l] - 128;
						b = (byte)((num5 >= 128) ? 255u : 0u);
						int num6 = num5 - b;
						array3[k, l + 1] += (byte)(0.4375f * (float)num6);
						if (l != 0)
						{
							array3[k + 1, l - 1] += (byte)(0.1875f * (float)num6);
						}
						array3[k + 1, l] += (byte)(0.3125f * (float)num6);
						array3[k + 1, l + 1] += (byte)(0.0625f * (float)num6);
					}
					if (ditheringType == StiMonochromeDitheringType.Ordered)
					{
						b = (byte)(((float)num5 >= 15f * (float)array4[l & 3, k & 3]) ? 255u : 0u);
					}
					array3[k, l] = b;
				}
			}
			num3 = ((width - 1) / 16 + 1) * 2;
			array2 = new byte[num3 * height];
			for (int m = 0; m < height; m++)
			{
				byte b2 = 128;
				int num7 = m * num3;
				byte b3 = 0;
				for (int n = 0; n < width; n++)
				{
					if (array3[m, n] != 0)
					{
						b3 |= b2;
					}
					b2 >>= 1;
					if (b2 == 0)
					{
						array2[num7] = b3;
						b3 = 0;
						b2 = 128;
						num7++;
					}
				}
				if (b2 != 128)
				{
					array2[num7] = b3;
				}
			}
		}
		if (paletteType == StiPcxPaletteType.Color)
		{
			num3 = ((width - 1) / 2 + 1) * 2;
			array2 = new byte[num3 * 3 * height];
			for (int num8 = 0; num8 < height; num8++)
			{
				int num9 = num8 * num;
				int num10 = num3 * 3 * num8;
				for (int num11 = 0; num11 < width; num11++)
				{
					array2[num10] = array[num9 + 2];
					array2[num10 + num3] = array[num9 + 1];
					array2[num10 + num3 * 2] = array[num9];
					num9 += 4;
					num10++;
				}
			}
		}
		byte[] array5 = new byte[128];
		dwByte(array5, 0, 10);
		dwByte(array5, 1, 5);
		dwByte(array5, 2, 1);
		if (paletteType == StiPcxPaletteType.Monochrome)
		{
			dwByte(array5, 3, 1);
		}
		if (paletteType == StiPcxPaletteType.Color)
		{
			dwByte(array5, 3, 8);
		}
		dwWord(array5, 4, 0);
		dwWord(array5, 6, 0);
		dwWord(array5, 8, (short)(width - 1));
		dwWord(array5, 10, (short)(height - 1));
		dwWord(array5, 12, 72);
		dwWord(array5, 14, 72);
		dwByte(array5, 16, 0, 0, 0);
		dwByte(array5, 19, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		dwByte(array5, 64, 0);
		if (paletteType == StiPcxPaletteType.Monochrome)
		{
			dwByte(array5, 65, 1);
		}
		if (paletteType == StiPcxPaletteType.Color)
		{
			dwByte(array5, 65, 3);
		}
		dwWord(array5, 66, (short)num3);
		dwWord(array5, 68, 1);
		stream.Write(array5, 0, 128);
		int num12 = 1;
		byte b4 = array2[0];
		for (int num13 = 1; num13 < array2.Length; num13++)
		{
			byte b5 = array2[num13];
			if (b4 == b5)
			{
				num12++;
				if (num12 == 63 || (num13 + 1) % num3 == 0)
				{
					dwEncodedData(stream, b4, num12);
					num12 = 0;
				}
			}
			else
			{
				if (num12 != 0)
				{
					dwEncodedData(stream, b4, num12);
				}
				b4 = b5;
				num12 = 1;
			}
		}
		if (num12 != 0)
		{
			dwEncodedData(stream, b4, num12);
		}
		stream.Flush();
	}
}
