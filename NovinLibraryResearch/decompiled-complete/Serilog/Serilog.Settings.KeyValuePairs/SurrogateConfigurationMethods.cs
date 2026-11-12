using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Settings.KeyValuePairs;

internal static class SurrogateConfigurationMethods
{
	private static readonly Dictionary<Type, MethodInfo[]> SurrogateMethodCandidates = (from m in typeof(SurrogateConfigurationMethods).GetTypeInfo().DeclaredMethods
		group m by m.GetParameters().First().ParameterType).ToDictionary((IGrouping<Type, MethodInfo> g) => g.Key, (IGrouping<Type, MethodInfo> g) => g.ToArray());

	internal static readonly MethodInfo[] WriteTo = SurrogateMethodCandidates[typeof(LoggerSinkConfiguration)];

	internal static readonly MethodInfo[] AuditTo = SurrogateMethodCandidates[typeof(LoggerAuditSinkConfiguration)];

	internal static readonly MethodInfo[] Enrich = SurrogateMethodCandidates[typeof(LoggerEnrichmentConfiguration)];

	internal static readonly MethodInfo[] Destructure = SurrogateMethodCandidates[typeof(LoggerDestructuringConfiguration)];

	internal static readonly MethodInfo[] Filter = SurrogateMethodCandidates[typeof(LoggerFilterConfiguration)];

	internal static LoggerConfiguration Sink(LoggerSinkConfiguration loggerSinkConfiguration, ILogEventSink sink, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
	{
		return loggerSinkConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
	}

	internal static LoggerConfiguration Sink(LoggerAuditSinkConfiguration auditSinkConfiguration, ILogEventSink sink, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
	{
		return auditSinkConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
	}

	internal static LoggerConfiguration With(LoggerEnrichmentConfiguration loggerEnrichmentConfiguration, ILogEventEnricher enricher)
	{
		return loggerEnrichmentConfiguration.With(enricher);
	}

	internal static LoggerConfiguration FromLogContext(LoggerEnrichmentConfiguration loggerEnrichmentConfiguration)
	{
		return loggerEnrichmentConfiguration.FromLogContext();
	}

	internal static LoggerConfiguration With(LoggerDestructuringConfiguration loggerDestructuringConfiguration, IDestructuringPolicy policy)
	{
		return loggerDestructuringConfiguration.With(policy);
	}

	internal static LoggerConfiguration AsScalar(LoggerDestructuringConfiguration loggerDestructuringConfiguration, Type scalarType)
	{
		return loggerDestructuringConfiguration.AsScalar(scalarType);
	}

	internal static LoggerConfiguration ToMaximumCollectionCount(LoggerDestructuringConfiguration loggerDestructuringConfiguration, int maximumCollectionCount)
	{
		return loggerDestructuringConfiguration.ToMaximumCollectionCount(maximumCollectionCount);
	}

	internal static LoggerConfiguration ToMaximumDepth(LoggerDestructuringConfiguration loggerDestructuringConfiguration, int maximumDestructuringDepth)
	{
		return loggerDestructuringConfiguration.ToMaximumDepth(maximumDestructuringDepth);
	}

	internal static LoggerConfiguration ToMaximumStringLength(LoggerDestructuringConfiguration loggerDestructuringConfiguration, int maximumStringLength)
	{
		return loggerDestructuringConfiguration.ToMaximumStringLength(maximumStringLength);
	}

	internal static LoggerConfiguration With(LoggerFilterConfiguration loggerFilterConfiguration, ILogEventFilter filter)
	{
		return loggerFilterConfiguration.With(filter);
	}
}
