using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Core.Sinks;

internal class FilteringSink : ILogEventSink
{
	private readonly ILogEventSink _sink;

	private readonly bool _propagateExceptions;

	private readonly ILogEventFilter[] _filters;

	public FilteringSink(ILogEventSink sink, IEnumerable<ILogEventFilter> filters, bool propagateExceptions)
	{
		_sink = Guard.AgainstNull(sink, "sink");
		_filters = Guard.AgainstNull(filters, "filters").ToArray();
		_propagateExceptions = propagateExceptions;
	}

	public void Emit(LogEvent logEvent)
	{
		try
		{
			ILogEventFilter[] filters = _filters;
			for (int i = 0; i < filters.Length; i++)
			{
				if (!filters[i].IsEnabled(logEvent))
				{
					return;
				}
			}
			_sink.Emit(logEvent);
		}
		catch (Exception arg)
		{
			SelfLog.WriteLine("Caught exception while applying filters: {0}", arg);
			if (_propagateExceptions)
			{
				throw;
			}
		}
	}
}
