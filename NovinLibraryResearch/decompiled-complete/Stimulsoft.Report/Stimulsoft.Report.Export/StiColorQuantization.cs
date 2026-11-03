using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Stimulsoft.Report.Export;

internal class StiColorQuantization
{
	private class Pip
	{
		public int X1;

		public int X2;

		public int Y1;

		public int Y2;

		public int Z1;

		public int Z2;

		public int XW => X2 - X1;

		public int YW => Y2 - Y1;

		public int ZW => Z2 - Z1;
	}

	private static int[] CreateColorMap(Bitmap bitmap, ref byte[] pixelsData, ref long usedMemoryCounter)
	{
		if (StiOptions.Engine.FullTrust)
		{
			return CreateColorMapFullTrust(bitmap, ref pixelsData, ref usedMemoryCounter);
		}
		int[] array = new int[262144];
		for (int i = 0; i < ((Image)bitmap).Height; i++)
		{
			for (int j = 0; j < ((Image)bitmap).Width; j++)
			{
				Color pixel = bitmap.GetPixel(j, i);
				int num = (pixel.R >> 2 << 12) | (pixel.G >> 2 << 6) | (pixel.B >> 2);
				if (array[num] < 65535)
				{
					array[num]++;
				}
			}
		}
		return array;
	}

	private static int[] CreateColorMapFullTrust(Bitmap bitmap, ref byte[] pixelsData, ref long usedMemoryCounter)
	{
		int width = ((Image)bitmap).Width;
		int height = ((Image)bitmap).Height;
		Rectangle rectangle = new Rectangle(0, 0, width, height);
		BitmapData val = bitmap.LockBits(rectangle, (ImageLockMode)1, (PixelFormat)2498570);
		int num = Math.Abs(val.Stride);
		int num2 = num * val.Height;
		pixelsData = new byte[num2];
		Marshal.Copy(val.Scan0, pixelsData, 0, num2);
		bitmap.UnlockBits(val);
		int[] array = new int[262144];
		for (int i = 0; i < height; i++)
		{
			int num3 = i * num;
			for (int num4 = width; num4 > 0; num4--)
			{
				int num5 = (pixelsData[num3 + 2] >> 2 << 12) | (pixelsData[num3 + 1] >> 2 << 6) | (pixelsData[num3] >> 2);
				if (array[num5] < 65535)
				{
					array[num5]++;
				}
				num3 += 4;
			}
		}
		usedMemoryCounter += pixelsData.Length;
		return array;
	}

	private static Color[] GetPaletteV1(int[] colorMap, int count)
	{
		int[] array = new int[65536];
		for (int i = 0; i < colorMap.Length; i++)
		{
			array[colorMap[i]]++;
		}
		int num = 65535;
		int num2 = 0;
		for (int num3 = 65535; num3 >= 0; num3--)
		{
			int num4 = array[num3];
			if (num4 > 0)
			{
				num = num3;
				num2 += num4;
				if (num2 > count)
				{
					break;
				}
			}
		}
		Color[] array2 = new Color[num2];
		num2 = 0;
		for (int j = 0; j < colorMap.Length; j++)
		{
			if (colorMap[j] >= num)
			{
				array2[num2] = Color.FromArgb((j >> 10) & 0xFC, (j >> 4) & 0xFC, (j << 2) & 0xFC);
				num2++;
			}
		}
		return array2;
	}

	private static Color[] GetPaletteV2(int[] colorMap, int count)
	{
		Color[] array = new Color[count];
		int num = 3 + 96 / count;
		for (int i = 0; i < count; i++)
		{
			int num2 = 0;
			int num3 = 0;
			for (int j = 0; j < colorMap.Length; j++)
			{
				if (colorMap[j] > num2)
				{
					num2 = colorMap[j];
					num3 = j;
				}
			}
			int num4 = (num3 >> 10) & 0xFC;
			int num5 = (num3 >> 4) & 0xFC;
			int num6 = (num3 << 2) & 0xFC;
			array[i] = Color.FromArgb(num4, num5, num6);
			num4 >>= 2;
			num5 >>= 2;
			num6 >>= 2;
			for (int k = Math.Max(num4 - num, 0); k < Math.Min(num4 + num, 64); k++)
			{
				for (int l = Math.Max(num5 - num, 0); l < Math.Min(num5 + num, 64); l++)
				{
					for (int m = Math.Max(num6 - num, 0); m < Math.Min(num6 + num, 64); m++)
					{
						colorMap[(k << 12) + (l << 6) + m] = 1;
					}
				}
			}
		}
		return array;
	}

	private static Color[] GetPaletteV3(int[] colorMap, int count)
	{
		long[,] array = new long[3, 64];
		int num = 0;
		for (int i = 0; i < 64; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				for (int k = 0; k < 64; k++)
				{
					int num2 = colorMap[num++];
					array[0, i] += num2;
					array[1, j] += num2;
					array[2, k] += num2;
				}
			}
		}
		Pip pip = new Pip
		{
			X2 = 63,
			Y2 = 63,
			Z2 = 63
		};
		while (array[0, pip.X1] == 0L)
		{
			pip.X1++;
		}
		while (array[0, pip.X2] == 0L)
		{
			pip.X2--;
		}
		while (array[1, pip.Y1] == 0L)
		{
			pip.Y1++;
		}
		while (array[1, pip.Y2] == 0L)
		{
			pip.Y2--;
		}
		while (array[2, pip.Z1] == 0L)
		{
			pip.Z1++;
		}
		while (array[2, pip.Z2] == 0L)
		{
			pip.Z2--;
		}
		List<Pip> list = new List<Pip>();
		list.Add(pip);
		DivPip(list, count, array);
		Color[] array2 = new Color[count];
		for (int l = 0; l < count; l++)
		{
			Pip pip2 = list[l];
			int red = GetMax(pip2.X1, pip2.X2, 0, array) * 4;
			int green = GetMax(pip2.Y1, pip2.Y2, 1, array) * 4;
			int blue = GetMax(pip2.Z1, pip2.Z2, 2, array) * 4;
			array2[l] = Color.FromArgb(red, green, blue);
		}
		return array2;
	}

	private static void DivPip(List<Pip> pips, int count, long[,] colorsByAxes)
	{
		while (true)
		{
			int num = pips.Count - 1;
			while (num >= 0)
			{
				if (pips.Count >= count)
				{
					return;
				}
				Pip pip = pips[num];
				int num2 = 4;
				if (pip.XW < num2 || pip.YW < num2 || pip.ZW < num2)
				{
					num--;
					continue;
				}
				Pip pip2 = null;
				Pip pip3 = null;
				if (pip.XW > pip.YW)
				{
					if (pip.XW >= pip.ZW)
					{
						int median = GetMedian(pip.X1, pip.X2, 0, colorsByAxes);
						pip2 = new Pip
						{
							X1 = pip.X1,
							X2 = median,
							Y1 = pip.Y1,
							Y2 = pip.Y2,
							Z1 = pip.Z1,
							Z2 = pip.Z2
						};
						pip3 = new Pip
						{
							X1 = median + 1,
							X2 = pip.X2,
							Y1 = pip.Y1,
							Y2 = pip.Y2,
							Z1 = pip.Z1,
							Z2 = pip.Z2
						};
					}
					else
					{
						int median2 = GetMedian(pip.Z1, pip.Z2, 2, colorsByAxes);
						pip2 = new Pip
						{
							X1 = pip.X1,
							X2 = pip.X2,
							Y1 = pip.Y1,
							Y2 = pip.Y2,
							Z1 = pip.Z1,
							Z2 = median2
						};
						pip3 = new Pip
						{
							X1 = pip.X1,
							X2 = pip.X2,
							Y1 = pip.Y1,
							Y2 = pip.Y2,
							Z1 = median2 + 1,
							Z2 = pip.Z2
						};
					}
				}
				else if (pip.YW >= pip.ZW)
				{
					int median3 = GetMedian(pip.Y1, pip.Y2, 1, colorsByAxes);
					pip2 = new Pip
					{
						X1 = pip.X1,
						X2 = pip.X2,
						Y1 = pip.Y1,
						Y2 = median3,
						Z1 = pip.Z1,
						Z2 = pip.Z2
					};
					pip3 = new Pip
					{
						X1 = pip.X1,
						X2 = pip.X2,
						Y1 = median3 + 1,
						Y2 = pip.Y2,
						Z1 = pip.Z1,
						Z2 = pip.Z2
					};
				}
				else
				{
					int median4 = GetMedian(pip.Z1, pip.Z2, 2, colorsByAxes);
					pip2 = new Pip
					{
						X1 = pip.X1,
						X2 = pip.X2,
						Y1 = pip.Y1,
						Y2 = pip.Y2,
						Z1 = pip.Z1,
						Z2 = median4
					};
					pip3 = new Pip
					{
						X1 = pip.X1,
						X2 = pip.X2,
						Y1 = pip.Y1,
						Y2 = pip.Y2,
						Z1 = median4 + 1,
						Z2 = pip.Z2
					};
				}
				pips.RemoveAt(num);
				pips.Add(pip2);
				pips.Add(pip3);
				num--;
			}
		}
	}

	private static int GetMedian(int a, int b, int axis, long[,] colorsByAxes)
	{
		long num = 0L;
		for (int i = a; i <= b; i++)
		{
			num += colorsByAxes[axis, i];
		}
		long num2 = 0L;
		int num3 = b;
		while (num3 > a && num2 < num)
		{
			long num4 = colorsByAxes[axis, num3];
			num -= num4;
			num2 += num4;
			num3--;
		}
		if (num3 == b)
		{
			return a;
		}
		return num3;
	}

	private static int GetMax(int a, int b, int axis, long[,] colorsByAxes)
	{
		long num = 0L;
		int result = a;
		for (int i = a; i <= b; i++)
		{
			long num2 = colorsByAxes[axis, i];
			if (num < num2)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	private static void PrepareColorMapForFastConversion(int[] colorMap, Color[] palette, StiMonochromeDitheringType ditheringType)
	{
		for (int i = 0; i < colorMap.Length; i++)
		{
			if (colorMap[i] == 0 && ditheringType == StiMonochromeDitheringType.None)
			{
				continue;
			}
			int num = (i >> 10) & 0xFC;
			int num2 = (i >> 4) & 0xFC;
			int num3 = (i << 2) & 0xFC;
			int num4 = int.MaxValue;
			int num5 = 0;
			for (int j = 0; j < palette.Length; j++)
			{
				Color color = palette[j];
				int num6 = (num - color.R) * (num - color.R) + (num2 - color.G) * (num2 - color.G) + (num3 - color.B) * (num3 - color.B);
				if (num6 < num4)
				{
					num4 = num6;
					num5 = j;
				}
			}
			colorMap[i] = num5;
		}
	}

	public static Image ConvertImage(Bitmap source, int count, StiMonochromeDitheringType ditheringType)
	{
		long usedMemoryCounter = 0L;
		byte[] pixelsData = null;
		int[] colorMap = CreateColorMap(source, ref pixelsData, ref usedMemoryCounter);
		Color[] paletteV = GetPaletteV2(colorMap, count);
		return ConvertToPalette(source, pixelsData, colorMap, paletteV, ditheringType, ref usedMemoryCounter);
	}

	private static Image ConvertToPalette(Bitmap oldBitmap, byte[] oldPixelsData, int[] colorMap, Color[] palette, StiMonochromeDitheringType ditheringType, ref long usedMemoryCounter)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (StiOptions.Engine.FullTrust)
		{
			return ConvertToPaletteFullTrust(oldBitmap, oldPixelsData, colorMap, palette, ditheringType, ref usedMemoryCounter);
		}
		Bitmap val = new Bitmap(((Image)oldBitmap).Width, ((Image)oldBitmap).Height);
		for (int i = 0; i < ((Image)oldBitmap).Height; i++)
		{
			for (int j = 0; j < ((Image)oldBitmap).Width; j++)
			{
				Color pixel = oldBitmap.GetPixel(j, i);
				int num = int.MaxValue;
				int num2 = 0;
				for (int k = 0; k < palette.Length; k++)
				{
					Color color = palette[k];
					int num3 = (pixel.R - color.R) * (pixel.R - color.R) + (pixel.G - color.G) * (pixel.G - color.G) + (pixel.B - color.B) * (pixel.B - color.B);
					if (num3 < num)
					{
						num = num3;
						num2 = k;
					}
				}
				val.SetPixel(j, i, palette[num2]);
			}
		}
		return (Image)(object)val;
	}

	private static Image ConvertToPaletteFullTrust(Bitmap oldBitmap, byte[] oldPixelsData, int[] colorMap, Color[] palette, StiMonochromeDitheringType ditheringType, ref long usedMemoryCounter)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		PrepareColorMapForFastConversion(colorMap, palette, ditheringType);
		int width = ((Image)oldBitmap).Width;
		int height = ((Image)oldBitmap).Height;
		Bitmap val = new Bitmap(width, height);
		Rectangle rectangle = new Rectangle(0, 0, width, height);
		BitmapData val2 = val.LockBits(rectangle, (ImageLockMode)1, (PixelFormat)2498570);
		int num = Math.Abs(val2.Stride);
		int num2 = num * val2.Height;
		byte[] array = new byte[num2 + num + 4];
		if (ditheringType == StiMonochromeDitheringType.None)
		{
			for (int i = 0; i < height; i++)
			{
				int num3 = i * num;
				for (int num4 = width; num4 > 0; num4--)
				{
					byte b = oldPixelsData[num3 + 2];
					byte b2 = oldPixelsData[num3 + 1];
					byte b3 = oldPixelsData[num3];
					int num5 = colorMap[(b >> 2 << 12) | (b2 >> 2 << 6) | (b3 >> 2)];
					array[num3 + 2] = palette[num5].R;
					array[num3 + 1] = palette[num5].G;
					array[num3] = palette[num5].B;
					array[num3 + 3] = oldPixelsData[num3 + 3];
					num3 += 4;
				}
			}
		}
		if (ditheringType == StiMonochromeDitheringType.FloydSteinberg)
		{
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = 128;
			}
			for (int k = 0; k < height; k++)
			{
				int num6 = k * num;
				for (int num7 = width; num7 > 0; num7--)
				{
					int val3 = oldPixelsData[num6 + 2] + array[num6 + 2] - 128;
					int val4 = oldPixelsData[num6 + 1] + array[num6 + 1] - 128;
					int val5 = oldPixelsData[num6] + array[num6] - 128;
					byte b4 = (byte)Math.Min(255, Math.Max(0, val3));
					byte b5 = (byte)Math.Min(255, Math.Max(0, val4));
					byte b6 = (byte)Math.Min(255, Math.Max(0, val5));
					int num8 = colorMap[(b4 >> 2 << 12) | (b5 >> 2 << 6) | (b6 >> 2)];
					Color color = palette[num8];
					array[num6 + 2] = color.R;
					array[num6 + 1] = color.G;
					array[num6] = color.B;
					array[num6 + 3] = oldPixelsData[num6 + 3];
					int num9 = b6 - color.B;
					array[num6 + 4] += (byte)(0.4375f * (float)num9);
					array[num6 + num - 4] += (byte)(0.1875f * (float)num9);
					array[num6 + num] += (byte)(0.3125f * (float)num9);
					array[num6 + num + 4] += (byte)(0.0625f * (float)num9);
					num6++;
					num9 = b5 - color.G;
					array[num6 + 4] += (byte)(0.4375f * (float)num9);
					array[num6 + num - 4] += (byte)(0.1875f * (float)num9);
					array[num6 + num] += (byte)(0.3125f * (float)num9);
					array[num6 + num + 4] += (byte)(0.0625f * (float)num9);
					num6++;
					num9 = b4 - color.R;
					array[num6 + 4] += (byte)(0.4375f * (float)num9);
					array[num6 + num - 4] += (byte)(0.1875f * (float)num9);
					array[num6 + num] += (byte)(0.3125f * (float)num9);
					array[num6 + num + 4] += (byte)(0.0625f * (float)num9);
					num6 += 2;
				}
			}
		}
		Marshal.Copy(array, 0, val2.Scan0, num2);
		val.UnlockBits(val2);
		usedMemoryCounter += array.Length;
		return (Image)(object)val;
	}

	public static byte[] ConvertToIndexed(byte[] pixelsData, int count, out Color[] palette, StiMonochromeDitheringType ditheringType, ref long usedMemoryCounter)
	{
		int[] array = new int[262144];
		int num = pixelsData.Length / 3;
		int num2 = 0;
		while (num > 0)
		{
			int num3 = (pixelsData[num2] >> 2 << 12) | (pixelsData[num2 + 1] >> 2 << 6) | (pixelsData[num2 + 2] >> 2);
			if (array[num3] < 65535)
			{
				array[num3]++;
			}
			num2 += 3;
			num--;
		}
		usedMemoryCounter += array.Length * 4;
		palette = GetPaletteV2(array, count);
		PrepareColorMapForFastConversion(array, palette, ditheringType);
		byte[] array2 = new byte[pixelsData.Length / 3];
		num = pixelsData.Length / 3;
		int num4 = 0;
		int num5 = 0;
		while (num > 0)
		{
			byte b = pixelsData[num4];
			byte b2 = pixelsData[num4 + 1];
			byte b3 = pixelsData[num4 + 2];
			int num6 = array[(b >> 2 << 12) | (b2 >> 2 << 6) | (b3 >> 2)];
			array2[num5] = (byte)((uint)num6 & 0xFFu);
			num4 += 3;
			num5++;
			num--;
		}
		usedMemoryCounter += array2.Length;
		return array2;
	}
}
