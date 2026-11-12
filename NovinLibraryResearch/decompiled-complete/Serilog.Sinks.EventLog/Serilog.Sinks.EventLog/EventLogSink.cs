using System;
using System.Diagnostics;
using System.IO;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.EventLog;

public class EventLogSink : ILogEventSink
{
	private const string ApplicationLogName = "Application";

	private const int MaximumPayloadLengthChars = 31839;

	private const int MaximumSourceNameLengthChars = 212;

	private const int SourceMovedEventId = 3;

	private readonly IEventIdProvider _eventIdProvider;

	private readonly ITextFormatter _textFormatter;

	private readonly EventLog _log;

	public EventLogSink(string source, string logName, ITextFormatter textFormatter, string machineName, bool manageEventSource)
		: this(source, logName, textFormatter, machineName, manageEventSource, new EventIdHashProvider())
	{
	}

	public EventLogSink(string source, string logName, ITextFormatter textFormatter, string machineName, bool manageEventSource, IEventIdProvider eventIdProvider)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (textFormatter == null)
		{
			throw new ArgumentNullException("textFormatter");
		}
		if (eventIdProvider == null)
		{
			throw new ArgumentNullException("eventIdProvider");
		}
		if (source.Length > 212)
		{
			SelfLog.WriteLine("Trimming long event log source name to {0} characters", 212);
			source = source.Substring(0, 212);
		}
		source = source.Replace("<", "_");
		source = source.Replace(">", "_");
		_eventIdProvider = eventIdProvider;
		_textFormatter = textFormatter;
		_log = new EventLog(string.IsNullOrWhiteSpace(logName) ? "Application" : logName, machineName);
		if (manageEventSource)
		{
			ConfigureSource(_log, source);
		}
		else
		{
			_log.Source = source;
		}
	}

	private static void ConfigureSource(EventLog log, string source)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		EventSourceCreationData val = new EventSourceCreationData(source, log.Log)
		{
			MachineName = log.MachineName
		};
		string text = null;
		if (EventLog.SourceExists(source, log.MachineName))
		{
			string text2 = EventLog.LogNameFromSourceName(source, log.MachineName);
			if (!string.IsNullOrWhiteSpace(text2) && !log.Log.Equals(text2, StringComparison.OrdinalIgnoreCase))
			{
				EventLog.DeleteEventSource(source, log.MachineName);
				text = text2;
			}
		}
		else
		{
			EventLog.CreateEventSource(val);
		}
		if (text != null)
		{
			string text3 = $"serilog-{log.Log}";
			if (!EventLog.SourceExists(text3, log.MachineName))
			{
				EventLog.CreateEventSource(new EventSourceCreationData(text3, log.Log)
				{
					MachineName = log.MachineName
				});
			}
			log.Source = text3;
			log.WriteEntry($"Event source {source} was previously registered in log {text}. " + $"The source has been registered with this log, {log.Log}, however a computer restart may be required " + $"before event logs will appear in {log.Log} with source {source}. Until then, messages may be logged to {text}.", (EventLogEntryType)2, 3);
		}
		log.Source = source;
	}

	public void Emit(LogEvent logEvent)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (logEvent == null)
		{
			throw new ArgumentNullException("logEvent");
		}
		EventLogEntryType val = LevelToEventLogEntryType(logEvent.Level);
		StringWriter stringWriter = new StringWriter();
		_textFormatter.Format(logEvent, stringWriter);
		string text = stringWriter.ToString();
		if (text.Length > 31839)
		{
			SelfLog.WriteLine("Trimming long event log entry payload to {0} characters", 31839);
			text = text.Substring(0, 31839);
		}
		_log.WriteEntry(text, val, (int)_eventIdProvider.ComputeEventId(logEvent));
	}

	private static EventLogEntryType LevelToEventLogEntryType(LogEventLevel logEventLevel)
	{
		switch (logEventLevel)
		{
		case LogEventLevel.Verbose:
		case LogEventLevel.Debug:
		case LogEventLevel.Information:
			return (EventLogEntryType)4;
		case LogEventLevel.Error:
		case LogEventLevel.Fatal:
			return (EventLogEntryType)1;
		case LogEventLevel.Warning:
			return (EventLogEntryType)2;
		default:
			SelfLog.WriteLine("Unexpected logging level {0}, writing to the event log as `Information`", logEventLevel);
			return (EventLogEntryType)4;
		}
	}
}
