using System;
using Serilog.Events;

namespace Serilog.Core.Enrichers;

internal class ConditionalEnricher : ILogEventEnricher, IDisposable
{
	private readonly ILogEventEnricher _wrapped;

	private readonly Func<LogEvent, bool> _condition;

	public ConditionalEnricher(ILogEventEnricher wrapped, Func<LogEvent, bool> condition)
	{
		_wrapped = Guard.AgainstNull(wrapped, "wrapped");
		_condition = Guard.AgainstNull(condition, "condition");
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		if (_condition(logEvent))
		{
			_wrapped.Enrich(logEvent, propertyFactory);
		}
	}

	public void Dispose()
	{
		(_wrapped as IDisposable)?.Dispose();
	}
}
