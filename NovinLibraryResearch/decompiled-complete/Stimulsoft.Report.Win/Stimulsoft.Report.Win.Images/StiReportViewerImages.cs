using System.Collections;
using System.Drawing;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Win.Images;

public static class StiReportViewerImages
{
	public static class Actions
	{
		public static Image ReportSave(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("ReportSave", size, allowCache);
		}

		public static Image ReportOpen(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("ReportOpen", size, allowCache);
		}

		public static Image ReportPrint(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("ReportPrint", size, allowCache);
		}

		public static Image Close(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("Close", size, allowCache);
		}
	}

	private const string RootPath = "Stimulsoft.Report.Win.Resources";

	private static Hashtable mainCache = new Hashtable();

	public static Image GetImage(string path, StiImageSize size = 0, bool allowCache = true)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		Bitmap val = (Bitmap)(allowCache ? /*isinst with value type is only supported in some contexts*/: null);
		if (val != null && !IsDisposedImage((Image)(object)val))
		{
			return (Image)(object)val;
		}
		Assembly assembly = typeof(StiReportViewerImages).Assembly;
		Image image = StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Report.Win.Resources." + path, size);
		if (allowCache)
		{
			mainCache[path + ((object)(StiImageSize)(ref size)).ToString()] = image;
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
