using System;
using Serilog.Events;

namespace Serilog.Core.Sinks;

internal sealed class ConditionalSink : ILogEventSink, IDisposable
{
	private readonly ILogEventSink _wrapped;

	private readonly Func<LogEvent, bool> _condition;

	public ConditionalSink(ILogEventSink wrapped, Func<LogEvent, bool> condition)
	{
		_wrapped = Guard.AgainstNull(wrapped, "wrapped");
		_condition = Guard.AgainstNull(condition, "condition");
	}

	public void Emit(LogEvent logEvent)
	{
		if (_condition(logEvent))
		{
			_wrapped.Emit(logEvent);
		}
	}

	public void Dispose()
	{
		(_wrapped as IDisposable)?.Dispose();
	}
}
