using System;
using Serilog.Events;

namespace Serilog.Core.Sinks;

internal sealed class DisposeDelegatingSink : ILogEventSink, IDisposable
{
	private readonly ILogEventSink _sink;

	private readonly IDisposable? _disposable;

	public DisposeDelegatingSink(ILogEventSink sink, IDisposable? disposable)
	{
		_sink = sink;
		_disposable = disposable;
	}

	public void Dispose()
	{
		_disposable?.Dispose();
	}

	public void Emit(LogEvent logEvent)
	{
		_sink.Emit(logEvent);
	}
}
