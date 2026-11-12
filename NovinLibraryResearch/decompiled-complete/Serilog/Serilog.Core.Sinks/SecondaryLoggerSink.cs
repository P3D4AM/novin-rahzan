using System;
using Serilog.Events;

namespace Serilog.Core.Sinks;

internal sealed class SecondaryLoggerSink : ILogEventSink, IDisposable
{
	private readonly ILogger _logger;

	private readonly bool _attemptDispose;

	public SecondaryLoggerSink(ILogger logger, bool attemptDispose = false)
	{
		_logger = Guard.AgainstNull(logger, "logger");
		_attemptDispose = attemptDispose;
	}

	public void Emit(LogEvent logEvent)
	{
		Guard.AgainstNull(logEvent, "logEvent");
		LogEvent logEvent2 = logEvent.Copy();
		_logger.Write(logEvent2);
	}

	public void Dispose()
	{
		if (_attemptDispose)
		{
			(_logger as IDisposable)?.Dispose();
		}
	}
}
