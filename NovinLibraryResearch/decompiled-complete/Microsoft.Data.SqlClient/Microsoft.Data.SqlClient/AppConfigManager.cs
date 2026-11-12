using System;
using System.Configuration;
using System.Reflection;

namespace Microsoft.Data.SqlClient;

internal sealed class AppConfigManager
{
	private const string TypeName = "AppConfigManager";

	public static T FetchConfigurationSection<T>(string name)
	{
		string name2 = MethodBase.GetCurrentMethod().Name;
		object obj = null;
		try
		{
			obj = ConfigurationManager.GetSection(name);
		}
		catch (Exception args)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO>: Unable to load section `{2}`. ConfigurationManager failed to load due to configuration errors: {3}", "AppConfigManager", name2, name, args);
		}
		if (obj != null)
		{
			Type typeFromHandle = typeof(T);
			ConfigurationSection val = (ConfigurationSection)((obj is ConfigurationSection) ? obj : null);
			if (val != null && ((object)val).GetType() == typeFromHandle)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Successfully loaded the configurable retry logic settings from the configuration file's section '{2}'.", "AppConfigManager", name2, name);
				return (T)obj;
			}
			SqlClientEventSource.Log.TraceEvent("<sc.{0}.{1}|INFO>: Found a custom {2} configuration but it is not of type {3}.", "AppConfigManager", name2, name, typeFromHandle.FullName);
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO>: Unable to load custom `{2}`. Default value of `{3}` type returns.", "AppConfigManager", name2, name, "T");
		return default(T);
	}
}
