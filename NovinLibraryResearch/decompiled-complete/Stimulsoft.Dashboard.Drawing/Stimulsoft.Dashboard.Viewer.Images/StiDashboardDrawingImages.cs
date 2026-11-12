using System.Drawing;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Viewer.Images;

public static class StiDashboardDrawingImages
{
	public static class Actions
	{
		public static Image Sort(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.Sort");
			}
			return GetImage("Actions.Dark.Sort");
		}
	}

	private const string RootPath = "Stimulsoft.Dashboard.Drawing.Images";

	internal static Image GetImage(string imagePath)
	{
		Assembly assembly = typeof(StiDashboardDrawingImages).Assembly;
		return StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Dashboard.Drawing.Images." + imagePath);
	}

	internal static Image GetDesignImage(string imagePath)
	{
		Assembly assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Report.Design");
		return StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Report.Design.Images." + imagePath);
	}
}
