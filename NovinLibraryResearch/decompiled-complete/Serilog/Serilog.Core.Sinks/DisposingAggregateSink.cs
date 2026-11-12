using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Core.Sinks;

internal sealed class DisposingAggregateSink : ILogEventSink, IDisposable
{
	private readonly ILogEventSink[] _sinks;

	public DisposingAggregateSink(IEnumerable<ILogEventSink> sinks)
	{
		Guard.AgainstNull(sinks, "sinks");
		_sinks = sinks.ToArray();
	}

	public void Emit(LogEvent logEvent)
	{
		List<Exception> list = null;
		ILogEventSink[] sinks = _sinks;
		foreach (ILogEventSink logEventSink in sinks)
		{
			try
			{
				logEventSink.Emit(logEvent);
			}
			catch (Exception ex)
			{
				SelfLog.WriteLine("Caught exception while emitting to sink {0}: {1}", logEventSink, ex);
				if (list == null)
				{
					list = new List<Exception>();
				}
				list.Add(ex);
			}
		}
		if (list != null)
		{
			throw new AggregateException("Failed to emit a log event.", list);
		}
	}

	public void Dispose()
	{
		ILogEventSink[] sinks = _sinks;
		foreach (ILogEventSink logEventSink in sinks)
		{
			if (logEventSink is IDisposable disposable)
			{
				try
				{
					disposable.Dispose();
				}
				catch (Exception ex)
				{
					ReportDisposingException(logEventSink, ex);
				}
			}
		}
	}

	private static void ReportDisposingException(ILogEventSink sink, Exception ex)
	{
		SelfLog.WriteLine("Caught exception while disposing sink {0}: {1}", sink, ex);
	}
}
