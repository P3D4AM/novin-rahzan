using System;
using System.Reflection;

namespace Stimulsoft.Base.Exceptions;

public class StiNotFoundProviderCreater
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
				Assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			}
			catch
			{
			}
			isInited = true;
		}
	}

	internal static IStiNotFoundProvider GetNotFoundProvider()
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Report.Design.Dictionary.StiNotFoundPackageProvider");
			if (type != null)
			{
				return StiActivator.CreateObject(type, new object[0]) as IStiNotFoundProvider;
			}
		}
		catch
		{
			return null;
		}
		return null;
	}
}
