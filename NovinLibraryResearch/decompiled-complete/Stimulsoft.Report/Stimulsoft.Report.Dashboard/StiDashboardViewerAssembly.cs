using System;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report.Dashboard;

internal static class StiDashboardViewerAssembly
{
	private static object lockObject = new object();

	private static bool isInited;

	private static Assembly assembly;

	internal static bool IsAssemblyLoaded => Assembly != null;

	internal static Assembly Assembly
	{
		get
		{
			LoadAssembly();
			return assembly;
		}
		set
		{
			assembly = value;
		}
	}

	internal static void LoadAssembly()
	{
		if (isInited)
		{
			return;
		}
		lock (lockObject)
		{
			try
			{
				Assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", true);
			}
			catch
			{
			}
			isInited = true;
		}
	}

	internal static void Show(StiReport report)
	{
		LoadAssembly();
		if (!IsAssemblyLoaded)
		{
			throw new Exception("Assembly 'Stimulsoft.Dashboard.Viewer' is not found!");
		}
		Type type = Assembly.GetType("Stimulsoft.Dashboard.Viewer.StiDashboardViewerForm");
		if (type == null)
		{
			throw new Exception("Type 'Stimulsoft.Dashboard.Viewer.StiDashboardViewerForm' is not found!");
		}
		IStiViewerForm stiViewerForm = StiActivator.CreateObject(type) as IStiViewerForm;
		stiViewerForm.Report = report;
		stiViewerForm.ShowViewer();
	}

	internal static UserControl GetViewerControl(StiReport report, int startPage = -1)
	{
		LoadAssembly();
		if (!IsAssemblyLoaded)
		{
			throw new Exception("Assembly 'Stimulsoft.Dashboard.Viewer' is not found!");
		}
		Type type = Assembly.GetType("Stimulsoft.Dashboard.Viewer.StiDashboardViewerControl");
		if (type == null)
		{
			throw new Exception("Type 'Stimulsoft.Dashboard.Viewer.StiDashboardViewerControl' is not found!");
		}
		Type type2 = Assembly.GetType("Stimulsoft.Dashboard.Viewer.StiDashboardViewerHelper");
		if (type2 == null)
		{
			throw new Exception("Type 'Stimulsoft.Dashboard.Viewer.StiDashboardViewerHelper' is not found!");
		}
		object? obj = type2.GetMethod("CreateViewerControl")?.Invoke(null, new object[2] { report, startPage });
		return (UserControl)((obj is UserControl) ? obj : null);
	}
}
