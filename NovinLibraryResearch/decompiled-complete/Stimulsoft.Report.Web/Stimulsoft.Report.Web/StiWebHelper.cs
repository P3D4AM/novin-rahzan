using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace Stimulsoft.Report.Web;

internal static class StiWebHelper
{
	public static void InitWeb()
	{
		if (StiOptions.Configuration.IsWeb)
		{
			return;
		}
		StiOptions.Configuration.IsWeb = true;
		try
		{
			if (HttpContext.Current != null && HttpContext.Current.Server != null)
			{
				StiOptions.Configuration.ApplicationDirectory = HttpContext.Current.Server.MapPath(string.Empty);
				return;
			}
		}
		catch
		{
			InitWeb2();
		}
		InitWeb2();
	}

	private static void InitWeb2()
	{
		try
		{
			if (HttpContext.Current != null)
			{
				StiOptions.Configuration.ApplicationDirectory = HttpRuntime.AppDomainAppPath;
				return;
			}
		}
		catch
		{
			InitWeb3();
		}
		InitWeb3();
	}

	private static void InitWeb3()
	{
		Assembly entryAssembly = Assembly.GetEntryAssembly();
		if (entryAssembly != null)
		{
			StiOptions.Configuration.ApplicationDirectory = Path.GetDirectoryName(entryAssembly.Location);
		}
		else
		{
			StiOptions.Configuration.ApplicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
		}
	}
}
