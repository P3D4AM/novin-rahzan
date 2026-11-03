using System.Reflection;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dashboard;

internal static class StiDashboardExportAssembly
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
				Assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", true);
			}
			catch
			{
			}
			isInited = true;
		}
	}
}
