using System;
using Serilog.Events;

namespace Serilog.Core.Sinks;

internal sealed class RestrictedSink : ILogEventSink, IDisposable
{
	private readonly ILogEventSink _sink;

	private readonly LoggingLevelSwitch _levelSwitch;

	public RestrictedSink(ILogEventSink sink, LoggingLevelSwitch levelSwitch)
	{
		_sink = Guard.AgainstNull(sink, "sink");
		_levelSwitch = Guard.AgainstNull(levelSwitch, "levelSwitch");
	}

	public void Emit(LogEvent logEvent)
	{
		Guard.AgainstNull(logEvent, "logEvent");
		if (logEvent.Level >= _levelSwitch.MinimumLevel)
		{
			_sink.Emit(logEvent);
		}
	}

	public void Dispose()
	{
		(_sink as IDisposable)?.Dispose();
	}
}
