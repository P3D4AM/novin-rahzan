using System;
using System.Drawing;

namespace Stimulsoft.Base.Drawing;

public static class StiImageResizer
{
	public static Image GetImage(Image image, bool allowDispose = true, double? scaleFactor = null)
	{
		if (image == null)
		{
			return null;
		}
		if (scaleFactor.HasValue)
		{
			Bitmap result = StiImageUtils.ResizeImage(image, Scale(image.Width, scaleFactor.Value), Scale(image.Height, scaleFactor.Value));
			if (allowDispose)
			{
				image.Dispose();
			}
			return (Image)(object)result;
		}
		if (StiScale.IsNoScaling)
		{
			return image;
		}
		Bitmap result2 = StiImageUtils.ResizeImage(image, StiScale.XXI(image.Width), StiScale.YYI(image.Height));
		if (allowDispose)
		{
			image.Dispose();
		}
		return (Image)(object)result2;
	}

	public static Image GetImageWithSampling(Image image)
	{
		if (image == null)
		{
			return null;
		}
		if (StiScale.IsNoScaling)
		{
			return image;
		}
		int num = StiScale.XXI(image.Width);
		int num2 = StiScale.YYI(image.Height);
		Bitmap result = StiImageUtils.ResizeImage(image, num, num2, num, num2, allowSampling: true);
		image.Dispose();
		return (Image)(object)result;
	}

	private static int Scale(double value, double scaleFactor)
	{
		return (int)Math.Ceiling(scaleFactor * value);
	}
}
