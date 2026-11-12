using Serilog.Events;

namespace Serilog.Core.Enrichers;

public class PropertyEnricher : ILogEventEnricher
{
	private readonly string _name;

	private readonly object? _value;

	private readonly bool _destructureObjects;

	public PropertyEnricher(string name, object? value, bool destructureObjects = false)
	{
		LogEventProperty.EnsureValidName(name);
		_name = name;
		_value = value;
		_destructureObjects = destructureObjects;
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		Guard.AgainstNull(logEvent, "logEvent");
		Guard.AgainstNull(propertyFactory, "propertyFactory");
		logEvent.AddPropertyIfAbsent(propertyFactory, _name, _value, _destructureObjects);
	}
}
