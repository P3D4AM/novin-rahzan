using System.Drawing;
using System.Reflection;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Viewer.Images;

public static class StiDashboardViewerImages
{
	public static class Actions
	{
		public static Image CloseFullScreen(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.CloseFullScreen");
			}
			return GetImage("Actions.Dark.CloseFullScreen");
		}

		public static Image ColumnSelection(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.ColumnSelection");
			}
			return GetImage("Actions.Dark.ColumnSelection");
		}

		public static Image DrillUp(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.DrillUp");
			}
			return GetImage("Actions.Dark.DrillUp");
		}

		public static Image DrillDown(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.DrillDown");
			}
			return GetImage("Actions.Dark.DrillDown");
		}

		public static Image Edit(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.Edit");
			}
			return GetImage("Actions.Dark.Edit");
		}

		public static Image FullScreen(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.FullScreen");
			}
			return GetImage("Actions.Dark.FullScreen");
		}

		public static Image Filter(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.Filter");
			}
			return GetImage("Actions.Dark.Filter");
		}

		public static Image FilterMultiOptionOff(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.FilterMultiOptionOff");
			}
			return GetImage("Actions.Dark.FilterMultiOptionOff");
		}

		public static Image FilterMultiOptionOn(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.FilterMultiOptionOn");
			}
			return GetImage("Actions.Dark.FilterMultiOptionOn");
		}

		public static Image Open(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.Open");
			}
			return GetImage("Actions.Dark.Open");
		}

		public static Image Refresh(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.Refresh");
			}
			return GetImage("Actions.Dark.Refresh");
		}

		public static Image RemoveFilter(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.RemoveFilter");
			}
			return GetImage("Actions.Dark.RemoveFilter");
		}

		public static Image Save(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.Save");
			}
			return GetImage("Actions.Dark.Save");
		}

		public static Image Sort(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.Sort");
			}
			return GetImage("Actions.Dark.Sort");
		}

		public static Image ViewData(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.ViewData");
			}
			return GetImage("Actions.Dark.ViewData");
		}

		public static Image ZoomPlus(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.ZoomPlus");
			}
			return GetImage("Actions.Dark.ZoomPlus");
		}

		public static Image ZoomMinus(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.ZoomMinus");
			}
			return GetImage("Actions.Dark.ZoomMinus");
		}

		public static Image ZoomReset(bool isDark)
		{
			if (!isDark)
			{
				return GetImage("Actions.Light.ResetZoom");
			}
			return GetImage("Actions.Dark.ResetZoom");
		}
	}

	public static class Editors
	{
		public static Image EditSmall()
		{
			return GetImage("Editors.EditSmall");
		}

		public static Image MoveDown()
		{
			return GetImage("Editors.MoveDown");
		}

		public static Image MoveUp()
		{
			return GetImage("Editors.MoveUp");
		}

		public static Image NewFilter()
		{
			return GetImage("Editors.NewFilter");
		}

		public static Image Remove()
		{
			return GetImage("Editors.Remove");
		}
	}

	public static class NoResult
	{
		public static Image Dashboard()
		{
			return GetImage("NoResult.Dashboard");
		}
	}

	public static class Controls
	{
		public static Image Remove()
		{
			return GetImage("Controls.Remove");
		}
	}

	private const string RootPath = "Stimulsoft.Dashboard.Viewer.Images";

	internal static Image GetImage(string imagePath)
	{
		Assembly assembly = typeof(StiDashboardViewerImages).Assembly;
		return StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Dashboard.Viewer.Images." + imagePath);
	}
}
