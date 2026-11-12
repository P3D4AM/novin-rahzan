using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Stimulsoft.Base.Drawing;

public class StiGaussianBlur
{
	private readonly int[] alpha;

	private readonly int[] red;

	private readonly int[] green;

	private readonly int[] blue;

	private readonly int width;

	private readonly int height;

	private readonly ParallelOptions options = new ParallelOptions
	{
		MaxDegreeOfParallelism = 16
	};

	public Bitmap Process(int radial)
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		int[] newAlpha = new int[width * height];
		int[] newRed = new int[width * height];
		int[] newGreen = new int[width * height];
		int[] newBlue = new int[width * height];
		int[] dest = new int[width * height];
		Parallel.Invoke(delegate
		{
			GaussBlur_4(alpha, newAlpha, radial);
		}, delegate
		{
			GaussBlur_4(red, newRed, radial);
		}, delegate
		{
			GaussBlur_4(green, newGreen, radial);
		}, delegate
		{
			GaussBlur_4(blue, newBlue, radial);
		});
		Parallel.For(0, dest.Length, options, delegate(int i)
		{
			if (newAlpha[i] > 255)
			{
				newAlpha[i] = 255;
			}
			if (newRed[i] > 255)
			{
				newRed[i] = 255;
			}
			if (newGreen[i] > 255)
			{
				newGreen[i] = 255;
			}
			if (newBlue[i] > 255)
			{
				newBlue[i] = 255;
			}
			if (newAlpha[i] < 0)
			{
				newAlpha[i] = 0;
			}
			if (newRed[i] < 0)
			{
				newRed[i] = 0;
			}
			if (newGreen[i] < 0)
			{
				newGreen[i] = 0;
			}
			if (newBlue[i] < 0)
			{
				newBlue[i] = 0;
			}
			dest[i] = (newAlpha[i] << 24) | (newRed[i] << 16) | (newGreen[i] << 8) | newBlue[i];
		});
		Bitmap val = new Bitmap(width, height);
		Rectangle rectangle = new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height);
		BitmapData val2 = val.LockBits(rectangle, (ImageLockMode)3, (PixelFormat)2498570);
		Marshal.Copy(dest, 0, val2.Scan0, dest.Length);
		val.UnlockBits(val2);
		return val;
	}

	private void GaussBlur_4(int[] source, int[] dest, int r)
	{
		int[] array = BoxesForGauss(r, 3);
		BoxBlur_4(source, dest, width, height, (array[0] - 1) / 2);
		BoxBlur_4(dest, source, width, height, (array[1] - 1) / 2);
		BoxBlur_4(source, dest, width, height, (array[2] - 1) / 2);
	}

	private int[] BoxesForGauss(int sigma, int n)
	{
		double d = Math.Sqrt(12 * sigma * sigma / n + 1);
		int num = (int)Math.Floor(d);
		if (num % 2 == 0)
		{
			num--;
		}
		int num2 = num + 2;
		double a = (double)(12 * sigma * sigma - n * num * num - 4 * n * num - 3 * n) / (double)(-4 * num - 4);
		double num3 = Math.Round(a);
		List<int> list = new List<int>();
		for (int i = 0; i < n; i++)
		{
			list.Add(((double)i < num3) ? num : num2);
		}
		return list.ToArray();
	}

	private void BoxBlur_4(int[] source, int[] dest, int w, int h, int r)
	{
		for (int i = 0; i < source.Length; i++)
		{
			dest[i] = source[i];
		}
		BoxBlurH_4(dest, source, w, h, r);
		BoxBlurT_4(source, dest, w, h, r);
	}

	private void BoxBlurH_4(int[] source, int[] dest, int w, int h, int r)
	{
		double iar = 1.0 / (double)(r + r + 1);
		Parallel.For(0, h, options, delegate(int i)
		{
			int num = i * w;
			int num2 = num;
			int num3 = num + r;
			int num4 = source[num];
			int num5 = source[num + w - 1];
			int num6 = (r + 1) * num4;
			for (int j = 0; j < r; j++)
			{
				num6 += source[num + j];
			}
			for (int k = 0; k <= r; k++)
			{
				num6 += source[num3++] - num4;
				dest[num++] = (int)Math.Round((double)num6 * iar);
			}
			for (int l = r + 1; l < w - r; l++)
			{
				num6 += source[num3++] - dest[num2++];
				dest[num++] = (int)Math.Round((double)num6 * iar);
			}
			for (int m = w - r; m < w; m++)
			{
				num6 += num5 - source[num2++];
				dest[num++] = (int)Math.Round((double)num6 * iar);
			}
		});
	}

	private void BoxBlurT_4(int[] source, int[] dest, int w, int h, int r)
	{
		double iar = 1.0 / (double)(r + r + 1);
		Parallel.For(0, w, options, delegate(int i)
		{
			int num = i;
			int num2 = num;
			int num3 = num + r * w;
			int num4 = source[num];
			int num5 = source[num + w * (h - 1)];
			int num6 = (r + 1) * num4;
			for (int j = 0; j < r; j++)
			{
				num6 += source[num + j * w];
			}
			for (int k = 0; k <= r; k++)
			{
				num6 += source[num3] - num4;
				dest[num] = (int)Math.Round((double)num6 * iar);
				num3 += w;
				num += w;
			}
			for (int l = r + 1; l < h - r; l++)
			{
				num6 += source[num3] - source[num2];
				dest[num] = (int)Math.Round((double)num6 * iar);
				num2 += w;
				num3 += w;
				num += w;
			}
			for (int m = h - r; m < h; m++)
			{
				num6 += num5 - source[num2];
				dest[num] = (int)Math.Round((double)num6 * iar);
				num2 += w;
				num += w;
			}
		});
	}

	public StiGaussianBlur(Bitmap image)
	{
		StiGaussianBlur stiGaussianBlur = this;
		Rectangle rectangle = new Rectangle(0, 0, ((Image)image).Width, ((Image)image).Height);
		int[] source = new int[rectangle.Width * rectangle.Height];
		BitmapData val = image.LockBits(rectangle, (ImageLockMode)3, (PixelFormat)2498570);
		Marshal.Copy(val.Scan0, source, 0, source.Length);
		image.UnlockBits(val);
		width = ((Image)image).Width;
		height = ((Image)image).Height;
		alpha = new int[width * height];
		red = new int[width * height];
		green = new int[width * height];
		blue = new int[width * height];
		Parallel.For(0, source.Length, options, delegate(int i)
		{
			stiGaussianBlur.alpha[i] = (int)((source[i] & 0xFF000000u) >> 24);
			stiGaussianBlur.red[i] = (source[i] & 0xFF0000) >> 16;
			stiGaussianBlur.green[i] = (source[i] & 0xFF00) >> 8;
			stiGaussianBlur.blue[i] = source[i] & 0xFF;
		});
	}
}
