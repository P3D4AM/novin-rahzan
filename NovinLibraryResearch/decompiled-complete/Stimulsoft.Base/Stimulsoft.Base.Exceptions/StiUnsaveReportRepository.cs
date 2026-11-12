using System;
using System.IO;

namespace Stimulsoft.Base.Exceptions;

public sealed class StiUnsaveReportRepository
{
	private static string GetRootUnsavedReportFolder()
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft", "UnsavedReports");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	public static string GetReportPath(string reportName)
	{
		string rootUnsavedReportFolder = GetRootUnsavedReportFolder();
		if (string.IsNullOrEmpty(reportName))
		{
			reportName = "Report";
		}
		string text = DateTime.Now.ToString("yyyyMMddThhmmss");
		return rootUnsavedReportFolder + "\\" + reportName + "-" + text + ".mrt";
	}

	public static string[] GetReportPaths()
	{
		string rootUnsavedReportFolder = GetRootUnsavedReportFolder();
		return Directory.GetFiles(rootUnsavedReportFolder, "*.mrt");
	}

	public static bool IsAvailableReports()
	{
		string[] reportPaths = GetReportPaths();
		if (reportPaths != null)
		{
			return reportPaths.Length != 0;
		}
		return false;
	}

	public static void DeleteRootUnsavedReportFolder()
	{
		try
		{
			string rootUnsavedReportFolder = GetRootUnsavedReportFolder();
			Directory.Delete(rootUnsavedReportFolder, recursive: true);
		}
		catch
		{
		}
	}

	public static void DeleteUnsavedReportFile(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch
		{
		}
	}
}
