using System.Collections;
using System.Drawing;
using System.Reflection;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Exceptions;

public static class StiExceptionsImages
{
	private const string RootPath = "Stimulsoft.Base.Exceptions.Images";

	private static Hashtable mainCache = new Hashtable();

	public static Image Copy(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
	{
		return GetImage("Copy", size, allowCache);
	}

	public static Image Save(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
	{
		return GetImage("SaveFile", size, allowCache);
	}

	public static Image GetImage(string path, StiImageSize size = StiImageSize.Normal, bool allowCache = true)
	{
		Bitmap val = (Bitmap)(allowCache ? /*isinst with value type is only supported in some contexts*/: null);
		if (val != null && !IsDisposedImage((Image)(object)val))
		{
			return (Image)(object)val;
		}
		Assembly assembly = typeof(StiExceptionsImages).Assembly;
		Image image = StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Base.Exceptions.Images." + path, size);
		if (allowCache)
		{
			mainCache[path + size] = image;
		}
		return image;
	}

	private static bool IsDisposedImage(Image image)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)image.PixelFormat == 0)
			{
				return true;
			}
			return image.Width == 0;
		}
		catch
		{
			return true;
		}
	}
}
