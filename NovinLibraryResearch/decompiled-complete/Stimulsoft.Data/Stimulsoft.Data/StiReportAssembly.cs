using System;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Map;

namespace Stimulsoft.Data;

internal static class StiReportAssembly
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
		private set
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
				Assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Report, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			}
			catch
			{
			}
			isInited = true;
		}
	}

	internal static IStiMapKeyHelper GetMapKeyHelper()
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Report.Helpers.StiMapKeyHelper");
			if (type == null)
			{
				return null;
			}
			return StiActivator.CreateObject(type) as IStiMapKeyHelper;
		}
		catch
		{
		}
		return null;
	}
}
