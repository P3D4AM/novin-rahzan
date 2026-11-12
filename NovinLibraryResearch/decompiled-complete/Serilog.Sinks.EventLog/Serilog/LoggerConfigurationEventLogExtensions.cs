using System;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.EventLog;

namespace Serilog;

public static class LoggerConfigurationEventLogExtensions
{
	private const string DefaultOutputTemplate = "{Message}{NewLine}{Exception}";

	public static LoggerConfiguration EventLog(this LoggerSinkConfiguration loggerConfiguration, string source, string logName = null, string machineName = ".", bool manageEventSource = false, string outputTemplate = "{Message}{NewLine}{Exception}", IFormatProvider formatProvider = null, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, IEventIdProvider eventIdProvider = null)
	{
		if (loggerConfiguration == null)
		{
			throw new ArgumentNullException("loggerConfiguration");
		}
		MessageTemplateTextFormatter textFormatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
		if (eventIdProvider == null)
		{
			return loggerConfiguration.Sink(new EventLogSink(source, logName, textFormatter, machineName, manageEventSource), restrictedToMinimumLevel);
		}
		return loggerConfiguration.Sink(new EventLogSink(source, logName, textFormatter, machineName, manageEventSource, eventIdProvider), restrictedToMinimumLevel);
	}

	public static LoggerConfiguration EventLog(this LoggerSinkConfiguration loggerConfiguration, ITextFormatter formatter, string source, string logName = null, string machineName = ".", bool manageEventSource = false, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, IEventIdProvider eventIdProvider = null)
	{
		if (loggerConfiguration == null)
		{
			throw new ArgumentNullException("loggerConfiguration");
		}
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		if (eventIdProvider == null)
		{
			return loggerConfiguration.Sink(new EventLogSink(source, logName, formatter, machineName, manageEventSource), restrictedToMinimumLevel);
		}
		return loggerConfiguration.Sink(new EventLogSink(source, logName, formatter, machineName, manageEventSource, eventIdProvider), restrictedToMinimumLevel);
	}
}
