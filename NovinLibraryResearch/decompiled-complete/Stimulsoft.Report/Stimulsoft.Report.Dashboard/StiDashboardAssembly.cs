using System;
using System.Collections.Generic;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

internal static class StiDashboardAssembly
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
		if (StiOptions.Engine.SkipLoadingDashboardAssembly || isInited)
		{
			return;
		}
		lock (lockObject)
		{
			try
			{
				Assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Dashboard, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", true);
			}
			catch
			{
			}
			isInited = true;
		}
	}

	internal static List<StiComponent> LoadDashboardElements()
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Dashboard.Options.StiDashboardElementsLoader");
			if (type == null)
			{
				return null;
			}
			MethodInfo method = type.GetMethod("FetchAll");
			if (method == null)
			{
				return null;
			}
			return method.Invoke(null, new object[0]) as List<StiComponent>;
		}
		catch
		{
		}
		return null;
	}
}
