using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Core.Sinks;

internal class SafeAggregateSink : ILogEventSink
{
	private readonly ILogEventSink[] _sinks;

	public SafeAggregateSink(IEnumerable<ILogEventSink> sinks)
	{
		Guard.AgainstNull(sinks, "sinks");
		_sinks = sinks.ToArray();
	}

	public void Emit(LogEvent logEvent)
	{
		ILogEventSink[] sinks = _sinks;
		foreach (ILogEventSink logEventSink in sinks)
		{
			try
			{
				logEventSink.Emit(logEvent);
			}
			catch (Exception arg)
			{
				SelfLog.WriteLine("Caught exception while emitting to sink {0}: {1}", logEventSink, arg);
			}
		}
	}
}
