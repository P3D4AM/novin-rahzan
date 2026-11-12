using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.Debug;

namespace Serilog;

public static class LoggerSinkConfigurationDebugExtensions
{
	private const string DefaultDebugOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

	public static LoggerConfiguration Debug(this LoggerSinkConfiguration sinkConfiguration, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", IFormatProvider formatProvider = null, LoggingLevelSwitch levelSwitch = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		if (outputTemplate == null)
		{
			throw new ArgumentNullException("outputTemplate");
		}
		MessageTemplateTextFormatter formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
		return sinkConfiguration.Debug(formatter, restrictedToMinimumLevel, levelSwitch);
	}

	public static LoggerConfiguration Debug(this LoggerSinkConfiguration sinkConfiguration, ITextFormatter formatter, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch levelSwitch = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		return sinkConfiguration.Sink(new DebugSink(formatter), restrictedToMinimumLevel, levelSwitch);
	}
}
