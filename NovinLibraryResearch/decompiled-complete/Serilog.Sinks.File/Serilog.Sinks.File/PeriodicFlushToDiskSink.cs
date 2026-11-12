using System;
using System.Threading;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Sinks.File;

[Obsolete("This type will be removed from the public API in a future version; use `WriteTo.File(flushToDiskInterval:)` instead.")]
public class PeriodicFlushToDiskSink : ILogEventSink, IDisposable
{
	private readonly ILogEventSink _sink;

	private readonly Timer _timer;

	private int _flushRequired;

	public PeriodicFlushToDiskSink(ILogEventSink sink, TimeSpan flushInterval)
	{
		PeriodicFlushToDiskSink periodicFlushToDiskSink = this;
		_sink = sink ?? throw new ArgumentNullException("sink");
		IFlushableFileSink flushable = sink as IFlushableFileSink;
		if (flushable != null)
		{
			_timer = new Timer(delegate
			{
				periodicFlushToDiskSink.FlushToDisk(flushable);
			}, null, flushInterval, flushInterval);
		}
		else
		{
			_timer = new Timer(delegate
			{
			}, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
			SelfLog.WriteLine("{0} configured to flush {1}, but {2} not implemented", typeof(PeriodicFlushToDiskSink), sink, "IFlushableFileSink");
		}
	}

	public void Emit(LogEvent logEvent)
	{
		_sink.Emit(logEvent);
		Interlocked.Exchange(ref _flushRequired, 1);
	}

	public void Dispose()
	{
		_timer.Dispose();
		(_sink as IDisposable)?.Dispose();
	}

	private void FlushToDisk(IFlushableFileSink flushable)
	{
		try
		{
			if (Interlocked.CompareExchange(ref _flushRequired, 0, 1) == 1)
			{
				flushable.FlushToDisk();
			}
		}
		catch (Exception arg)
		{
			SelfLog.WriteLine("{0} could not flush the underlying sink to disk: {1}", typeof(PeriodicFlushToDiskSink), arg);
		}
	}
}
