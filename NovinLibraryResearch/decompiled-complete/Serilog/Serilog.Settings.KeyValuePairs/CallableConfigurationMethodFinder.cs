using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog.Configuration;

namespace Serilog.Settings.KeyValuePairs;

internal static class CallableConfigurationMethodFinder
{
	internal static IList<MethodInfo> FindConfigurationMethods(IEnumerable<Assembly> configurationAssemblies, Type configType)
	{
		Type configType2 = configType;
		List<MethodInfo> list = (from m in configurationAssemblies.SelectMany((Assembly a) => a.ExportedTypes.Where((Type t) => t.IsSealed && t.IsAbstract && !t.IsNested)).SelectMany((Type t) => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
			where m.IsDefined(typeof(ExtensionAttribute), inherit: false) && m.GetParameters()[0].ParameterType == configType2
			select m).ToList();
		if (configType2 == typeof(LoggerSinkConfiguration))
		{
			list.AddRange(SurrogateConfigurationMethods.WriteTo);
		}
		if (configType2 == typeof(LoggerAuditSinkConfiguration))
		{
			list.AddRange(SurrogateConfigurationMethods.AuditTo);
		}
		if (configType2 == typeof(LoggerEnrichmentConfiguration))
		{
			list.AddRange(SurrogateConfigurationMethods.Enrich);
		}
		if (configType2 == typeof(LoggerDestructuringConfiguration))
		{
			list.AddRange(SurrogateConfigurationMethods.Destructure);
		}
		if (configType2 == typeof(LoggerFilterConfiguration))
		{
			list.AddRange(SurrogateConfigurationMethods.Filter);
		}
		return list;
	}
}
