using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers;

public abstract class CachedPropertyEnricher : ILogEventEnricher
{
	private LogEventProperty _cachedProperty { get; set; }

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
	}

	private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
	{
		if (_cachedProperty == null)
		{
			_cachedProperty = CreateProperty(propertyFactory);
		}
		return _cachedProperty;
	}

	protected abstract LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory);
}
