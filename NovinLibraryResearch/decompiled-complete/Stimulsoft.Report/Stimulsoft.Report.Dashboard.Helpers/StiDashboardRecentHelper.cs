using System;
using System.Collections.Generic;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Report.Dashboard.Helpers;

public static class StiDashboardRecentHelper
{
	private class FilesContainer
	{
		public List<string> DbsFiles { get; set; }

		public List<string> ReportFile { get; set; }
	}

	private static List<string> dbsFiles;

	private static List<string> reportFiles;

	private static string GetSettingsPath()
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft\\DashboardRecent.json");
	}

	private static string GetNewSettingsPath()
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft\\DbsRecent.json");
	}

	public static bool Save()
	{
		if (dbsFiles == null)
		{
			return false;
		}
		try
		{
			FilesContainer filesContainer = new FilesContainer
			{
				DbsFiles = dbsFiles,
				ReportFile = reportFiles
			};
			string newSettingsPath = GetNewSettingsPath();
			string contents = JsonConvert.SerializeObject((object)filesContainer, (Formatting)1);
			StiFileUtils.ProcessReadOnly(newSettingsPath);
			File.WriteAllText(newSettingsPath, contents);
		}
		catch
		{
			return true;
		}
		return true;
	}

	private static void Load()
	{
		if (dbsFiles != null)
		{
			return;
		}
		try
		{
			string newSettingsPath = GetNewSettingsPath();
			if (File.Exists(newSettingsPath))
			{
				string text = File.ReadAllText(newSettingsPath);
				FilesContainer filesContainer = JsonConvert.DeserializeObject<FilesContainer>(text);
				dbsFiles = filesContainer.DbsFiles;
				reportFiles = filesContainer.ReportFile;
			}
		}
		catch
		{
		}
		if (dbsFiles == null || reportFiles == null)
		{
			try
			{
				string settingsPath = GetSettingsPath();
				if (File.Exists(settingsPath))
				{
					string text2 = File.ReadAllText(settingsPath);
					dbsFiles = JsonConvert.DeserializeObject<List<string>>(text2);
					reportFiles = new List<string>();
					if (Save())
					{
						File.Delete(settingsPath);
					}
				}
			}
			catch
			{
			}
		}
		if (dbsFiles == null)
		{
			dbsFiles = new List<string>();
		}
		if (reportFiles == null)
		{
			reportFiles = new List<string>();
		}
	}

	public static void Add(StiReport report, string path, bool autoSave = true)
	{
		if (report == null || path == null)
		{
			return;
		}
		Load();
		bool flag = false;
		string item = path.ToLowerInvariant();
		if (report.Pages.ContainsDashboards)
		{
			if (!dbsFiles.Contains(item))
			{
				dbsFiles.Add(item);
				flag = true;
			}
			if (reportFiles.Contains(item))
			{
				reportFiles.Remove(item);
				flag = true;
			}
		}
		else
		{
			if (!reportFiles.Contains(item))
			{
				reportFiles.Add(item);
				flag = true;
			}
			if (dbsFiles.Contains(item))
			{
				dbsFiles.Remove(item);
				flag = true;
			}
		}
		if (flag && autoSave)
		{
			Save();
		}
	}

	public static void Add(bool containsDashboards, string path, bool autoSave = true)
	{
		if (path == null)
		{
			return;
		}
		Load();
		bool flag = false;
		string item = path.ToLowerInvariant();
		if (containsDashboards)
		{
			if (!dbsFiles.Contains(item))
			{
				dbsFiles.Add(item);
				flag = true;
			}
			if (reportFiles.Contains(item))
			{
				reportFiles.Remove(item);
				flag = true;
			}
		}
		else
		{
			if (!reportFiles.Contains(item))
			{
				reportFiles.Add(item);
				flag = true;
			}
			if (dbsFiles.Contains(item))
			{
				dbsFiles.Remove(item);
				flag = true;
			}
		}
		if (flag && autoSave)
		{
			Save();
		}
	}

	public static void Add(string path, bool autoSave = true)
	{
		if (path == null)
		{
			return;
		}
		Load();
		bool flag = false;
		string item = path.ToLowerInvariant();
		if (StiReportContentHelper.ContainsDbs(path))
		{
			if (!dbsFiles.Contains(item))
			{
				dbsFiles.Add(item);
				flag = true;
			}
			if (reportFiles.Contains(item))
			{
				reportFiles.Remove(item);
				flag = true;
			}
		}
		else
		{
			if (!reportFiles.Contains(item))
			{
				reportFiles.Add(item);
				flag = true;
			}
			if (dbsFiles.Contains(item))
			{
				dbsFiles.Remove(item);
				flag = true;
			}
		}
		if (flag && autoSave)
		{
			Save();
		}
	}

	public static void Remove(string path)
	{
		if (path != null)
		{
			Load();
			string item = path.ToLowerInvariant();
			bool flag = false;
			if (dbsFiles.Contains(item))
			{
				dbsFiles.Remove(item);
				flag = true;
			}
			if (reportFiles.Contains(item))
			{
				reportFiles.Remove(item);
				flag = true;
			}
			if (flag)
			{
				Save();
			}
		}
	}

	public static bool ContainsDbs(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return false;
		}
		Load();
		return dbsFiles.Contains(path.ToLowerInvariant());
	}

	public static bool ContainsFile(string path)
	{
		if (path == null)
		{
			return false;
		}
		Load();
		string item = path.ToLowerInvariant();
		if (dbsFiles.Contains(item))
		{
			return true;
		}
		if (reportFiles.Contains(item))
		{
			return true;
		}
		return false;
	}
}
