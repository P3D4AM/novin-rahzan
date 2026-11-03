using System.IO;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Helpers;

internal static class StiJsPreviewHelper
{
	internal const string CssFileName = "stimulsoft.viewer.office2013.whiteblue.css";

	internal const string ReportsFileName = "stimulsoft.reports.js";

	internal const string ReportsMapsFileName = "stimulsoft.reports.maps.js";

	internal const string ViewerFileName = "stimulsoft.viewer.js";

	internal static bool IsAvailable
	{
		get
		{
			if (GetReportsFile() != null && GetReportsMapsFile() != null && GetViewerFile() != null)
			{
				return GetCssFile() != null;
			}
			return false;
		}
	}

	private static string GetAssemblyFolder()
	{
		Assembly entryAssembly = Assembly.GetEntryAssembly();
		if (!(entryAssembly != null))
		{
			return null;
		}
		return Path.GetDirectoryName(entryAssembly.Location);
	}

	internal static string GetViewerFile()
	{
		return GetFile("stimulsoft.viewer.js");
	}

	internal static string GetReportsFile()
	{
		return GetFile("stimulsoft.reports.js");
	}

	internal static string GetReportsMapsFile()
	{
		return GetFile("stimulsoft.reports.maps.js");
	}

	internal static string GetCssFile()
	{
		return GetFile("stimulsoft.viewer.office2013.whiteblue.css");
	}

	internal static string GetFile(string name)
	{
		string jsFileFromEntryAssembly = GetJsFileFromEntryAssembly(name);
		if (jsFileFromEntryAssembly != null)
		{
			return jsFileFromEntryAssembly;
		}
		return GetJsFileFromLibs(name);
	}

	private static string GetJsFileFromEntryAssembly(string name)
	{
		try
		{
			string assemblyFolder = GetAssemblyFolder();
			if (assemblyFolder == null || !Directory.Exists(assemblyFolder))
			{
				return null;
			}
			string text = Path.Combine(assemblyFolder, name);
			return File.Exists(text) ? text : null;
		}
		catch
		{
		}
		return null;
	}

	private static string GetJsFileFromLibs(string name)
	{
		try
		{
			string jsLibPathIfExists = StiLibStatus.GetJsLibPathIfExists();
			if (jsLibPathIfExists == null || !Directory.Exists(jsLibPathIfExists))
			{
				return null;
			}
			string path = (StringExt.EndsWithInvariantIgnoreCase(name, "css") ? "Css" : "Scripts");
			string text = Path.Combine(jsLibPathIfExists, path, name);
			return File.Exists(text) ? text : null;
		}
		catch
		{
		}
		return null;
	}
}
