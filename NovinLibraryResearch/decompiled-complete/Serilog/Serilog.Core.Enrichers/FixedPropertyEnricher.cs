using System;
using Serilog.Events;

namespace Serilog.Core.Enrichers;

internal class FixedPropertyEnricher : ILogEventEnricher
{
	private readonly EventProperty _eventProperty;

	public FixedPropertyEnricher(in EventProperty eventProperty)
	{
		if (eventProperty.Equals(EventProperty.None))
		{
			throw new ArgumentNullException("eventProperty");
		}
		_eventProperty = eventProperty;
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		Guard.AgainstNull(logEvent, "logEvent");
		logEvent.AddPropertyIfAbsent(in _eventProperty);
	}
}
