using System;
using System.Collections.Generic;
using System.Reflection;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.App;

internal static class StiAppAssembly
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
	}

	internal static List<StiComponent> LoadAppComponents()
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.App.Options.StiAppComponentsLoader");
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
