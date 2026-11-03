using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog.Core;
using Serilog.Events;

namespace PacketParser.Performance;

public class VersionEnricher : ILogEventEnricher
{
	private LogEventProperty _cachedVersionProperty;

	public const string VersionPropertyName = "Version";

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
	}

	private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
	{
		if (_cachedVersionProperty == null)
		{
			_cachedVersionProperty = CreateProperty(propertyFactory);
		}
		return _cachedVersionProperty;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
	{
		return propertyFactory.CreateProperty("Version", (object)Assembly.GetExecutingAssembly().GetName().Version.ToString(), false);
	}
}
