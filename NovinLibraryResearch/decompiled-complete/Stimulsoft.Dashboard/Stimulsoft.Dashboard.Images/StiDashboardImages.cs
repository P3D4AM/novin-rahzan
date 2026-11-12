using System.Drawing;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Images;

public static class StiDashboardImages
{
	public static class Actions
	{
		public static Image DataFilter()
		{
			return GetImage("Actions.DataFilter");
		}

		public static Image DropDownArrowMouseOver()
		{
			return GetImage("Actions.DropDownArrowMouseOver");
		}

		public static Image DropDownArrowMouseOverWhite()
		{
			return GetImage("Actions.DropDownArrowMouseOver-white");
		}

		public static Image DropDownFilter()
		{
			return GetImage("Actions.DropDownFilter");
		}

		public static Image DropDownFilterWhite()
		{
			return GetImage("Actions.DropDownFilter-white");
		}

		public static Image DropDownFilterMouseOver()
		{
			return GetImage("Actions.DropDownFilterMouseOver");
		}

		public static Image DropDownFilterMouseOverWhite()
		{
			return GetImage("Actions.DropDownFilterMouseOver-white");
		}

		public static Image Edit()
		{
			return GetImage("Actions.Edit");
		}

		public static Image SortArrowAsc()
		{
			return GetImage("Actions.SortArrowAsc");
		}

		public static Image SortArrowAscWhite()
		{
			return GetImage("Actions.SortArrowAsc-white");
		}

		public static Image SortArrowDesc()
		{
			return GetImage("Actions.SortArrowDesc");
		}

		public static Image SortArrowDescWhite()
		{
			return GetImage("Actions.SortArrowDesc-white");
		}
	}

	private const string RootPath = "Stimulsoft.Dashboard.Images";

	public static Image GetImage(string path, StiImageSize size = StiImageSize.Normal)
	{
		Assembly assembly = typeof(StiDashboardImages).Assembly;
		return StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Dashboard.Images." + path, size);
	}
}
