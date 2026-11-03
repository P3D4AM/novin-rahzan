using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dialogs;

namespace Stimulsoft.Report.Toolbox;

public static class StiToolboxSetup
{
	public sealed class StiReportsInfo
	{
		public bool ShowBandsCategory { get; set; }

		public bool ShowCrossBandsCategory { get; set; }

		public bool ShowComponentsCategory { get; set; }

		public bool ShowBarCodesCategory { get; set; }

		public bool ShowShapesCategory { get; set; }

		public bool ShowChartCategory { get; set; }

		public bool ShowGaugeCategory { get; set; }

		public bool ShowMapCategory { get; set; }

		public List<string> VisibleComponents { get; set; } = new List<string>();


		internal StiReportsInfo()
		{
		}

		public bool IsComponentVisible(StiComponent comp)
		{
			string fullName = ((object)comp).GetType().FullName;
			return VisibleComponents.Contains(fullName);
		}
	}

	public sealed class StiDashbordsInfo
	{
		public List<string> VisibleComponents { get; set; } = new List<string>();


		internal StiDashbordsInfo()
		{
		}

		public bool IsComponentVisible(StiComponent comp)
		{
			string fullName = ((object)comp).GetType().FullName;
			return VisibleComponents.Contains(fullName);
		}
	}

	public sealed class StiComponentUIsInfo
	{
		public List<string> VisibleComponents { get; set; } = new List<string>();


		internal StiComponentUIsInfo()
		{
		}

		public bool IsComponentVisible(StiComponent comp)
		{
			string fullName = ((object)comp).GetType().FullName;
			return VisibleComponents.Contains(fullName);
		}
	}

	public sealed class StiDialogsInfo
	{
		public List<string> VisibleComponents { get; set; } = new List<string>();


		internal StiDialogsInfo()
		{
		}

		public bool IsComponentVisible(StiComponent comp)
		{
			string fullName = ((object)comp).GetType().FullName;
			return VisibleComponents.Contains(fullName);
		}
	}

	private class StiContainerInfo
	{
		public StiReportsInfo Reports = new StiReportsInfo();

		public StiDashbordsInfo Dashboards = new StiDashbordsInfo();

		public StiComponentUIsInfo ComponentUIs = new StiComponentUIsInfo();

		public StiDialogsInfo Dialogs = new StiDialogsInfo();
	}

	public static StiReportsInfo Reports { get; private set; }

	public static StiDashbordsInfo Dashboards { get; private set; }

	public static StiDialogsInfo Dialogs { get; private set; }

	public static StiComponentUIsInfo ComponentUIs { get; private set; }

	static StiToolboxSetup()
	{
		Reports = new StiReportsInfo();
		Dashboards = new StiDashbordsInfo();
		Dialogs = new StiDialogsInfo();
		ComponentUIs = new StiComponentUIsInfo();
		Load();
	}

	private static string GetSettingsPath()
	{
		char directorySeparatorChar = Path.DirectorySeparatorChar;
		string text = "Stimulsoft" + directorySeparatorChar + "ToolboxSetup.json";
		try
		{
			string folderPath;
			if (StiOptions.Engine.FullTrust)
			{
				folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				if (folderPath.Length != 0)
				{
					text = Path.Combine(folderPath, text);
				}
			}
			folderPath = Path.GetDirectoryName(text);
			if (StiOptions.Engine.FullTrust && !string.IsNullOrWhiteSpace(folderPath) && !Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}
		}
		catch
		{
			try
			{
				directorySeparatorChar = Path.DirectorySeparatorChar;
				text = "Stimulsoft" + directorySeparatorChar + "ToolboxSetup.json";
				string folderPath = "Stimulsoft";
				if (StiOptions.Engine.FullTrust && !Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}
			}
			catch
			{
				text = "ToolboxSetup.json";
			}
		}
		return text;
	}

	public static void Load()
	{
		bool flag = false;
		try
		{
			string settingsPath = GetSettingsPath();
			if (File.Exists(settingsPath))
			{
				string text = File.ReadAllText(settingsPath);
				StiContainerInfo stiContainerInfo = JsonConvert.DeserializeObject<StiContainerInfo>(text);
				Reports = stiContainerInfo.Reports;
				Dashboards = stiContainerInfo.Dashboards;
				Dialogs = stiContainerInfo.Dialogs;
				ComponentUIs = stiContainerInfo.ComponentUIs;
				flag = true;
			}
		}
		catch
		{
		}
		if (!flag)
		{
			Reports = new StiReportsInfo();
			Dashboards = new StiDashbordsInfo();
			Dialogs = new StiDialogsInfo();
			ComponentUIs = new StiComponentUIsInfo();
			InitDefaultSettings();
		}
	}

	public static void Save()
	{
		try
		{
			StiContainerInfo stiContainerInfo = new StiContainerInfo
			{
				Reports = Reports,
				Dashboards = Dashboards,
				ComponentUIs = ComponentUIs,
				Dialogs = Dialogs
			};
			string settingsPath = GetSettingsPath();
			string directoryName = Path.GetDirectoryName(settingsPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string contents = JsonConvert.SerializeObject((object)stiContainerInfo, (Formatting)1);
			StiFileUtils.ProcessReadOnly(settingsPath);
			File.WriteAllText(settingsPath, contents);
		}
		catch
		{
		}
	}

	public static void InitDefaultSettings()
	{
		Reports.ShowBandsCategory = true;
		Reports.ShowCrossBandsCategory = true;
		Reports.ShowComponentsCategory = true;
		Reports.ShowBarCodesCategory = true;
		Reports.ShowShapesCategory = true;
		Reports.ShowChartCategory = true;
		Reports.ShowGaugeCategory = true;
		Reports.ShowMapCategory = true;
		Reports.VisibleComponents.Clear();
		foreach (StiComponent item in StiOptions.Services.Components.Where((StiComponent s) => !(s is IStiElement) && !(s is IStiReportControl)))
		{
			if (item.PlaceOnToolbox)
			{
				Reports.VisibleComponents.Add(((object)item).GetType().FullName);
			}
		}
		Dashboards.VisibleComponents.Clear();
		foreach (StiComponent item2 in StiOptions.Services.Components.Where((StiComponent s) => s is IStiElement))
		{
			if (item2.PlaceOnToolbox)
			{
				Dashboards.VisibleComponents.Add(((object)item2).GetType().FullName);
			}
		}
		Dialogs.VisibleComponents.Clear();
		foreach (StiComponent item3 in StiOptions.Services.Components.Where((StiComponent s) => s is IStiReportControl))
		{
			if (item3.PlaceOnToolbox)
			{
				Dialogs.VisibleComponents.Add(((object)item3).GetType().FullName);
			}
		}
		ComponentUIs.VisibleComponents.Clear();
		foreach (StiComponent item4 in StiOptions.Services.Components.Where((StiComponent s) => s is IStiComponentUI))
		{
			if (item4.PlaceOnToolbox)
			{
				ComponentUIs.VisibleComponents.Add(((object)item4).GetType().FullName);
			}
		}
	}
}
