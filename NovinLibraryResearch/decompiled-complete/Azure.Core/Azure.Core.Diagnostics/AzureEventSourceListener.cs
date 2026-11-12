#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using Azure.Core.Shared;

namespace Azure.Core.Diagnostics;

public class AzureEventSourceListener : EventListener
{
	public const string TraitName = "AzureEventSource";

	public const string TraitValue = "true";

	private readonly List<EventSource> _eventSources = new List<EventSource>();

	private readonly Action<EventWrittenEventArgs, string> _log;

	private readonly EventLevel _level;

	public AzureEventSourceListener(Action<EventWrittenEventArgs, string> log, EventLevel level)
	{
		_log = log ?? throw new ArgumentNullException("log");
		_level = level;
		foreach (EventSource eventSource in _eventSources)
		{
			OnEventSourceCreated(eventSource);
		}
		_eventSources.Clear();
	}

	protected sealed override void OnEventSourceCreated(EventSource eventSource)
	{
		base.OnEventSourceCreated(eventSource);
		if (_log == null)
		{
			_eventSources.Add(eventSource);
		}
		if (eventSource.GetTrait("AzureEventSource") == "true")
		{
			EnableEvents(eventSource, _level);
		}
	}

	protected sealed override void OnEventWritten(EventWrittenEventArgs eventData)
	{
		if (eventData.EventId != -1)
		{
			_log?.Invoke(eventData, EventSourceEventFormatting.Format(eventData));
		}
	}

	public static AzureEventSourceListener CreateConsoleLogger(EventLevel level = EventLevel.Informational)
	{
		return new AzureEventSourceListener(delegate(EventWrittenEventArgs eventData, string text)
		{
			Console.WriteLine("[{1}] {0}: {2}", eventData.EventSource.Name, eventData.Level, text);
		}, level);
	}

	public static AzureEventSourceListener CreateTraceLogger(EventLevel level = EventLevel.Informational)
	{
		return new AzureEventSourceListener(delegate(EventWrittenEventArgs eventData, string text)
		{
			Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "[{0}] {1}", eventData.Level, text), eventData.EventSource.Name);
		}, level);
	}
}
