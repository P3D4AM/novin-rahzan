using System;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Configuration;

public class LoggerAuditSinkConfiguration
{
	private readonly LoggerSinkConfiguration _sinkConfiguration;

	internal LoggerAuditSinkConfiguration(LoggerConfiguration loggerConfiguration, Action<ILogEventSink> addSink)
	{
		_sinkConfiguration = new LoggerSinkConfiguration(loggerConfiguration, addSink);
	}

	public LoggerConfiguration Sink(ILogEventSink logEventSink, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
	{
		return _sinkConfiguration.Sink(logEventSink, restrictedToMinimumLevel, levelSwitch);
	}

	public LoggerConfiguration Sink<TSink>(LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null) where TSink : ILogEventSink, new()
	{
		return _sinkConfiguration.Sink<TSink>(restrictedToMinimumLevel, levelSwitch);
	}

	public LoggerConfiguration Logger(Action<LoggerConfiguration> configureLogger, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
	{
		return _sinkConfiguration.Logger(configureLogger, restrictedToMinimumLevel, levelSwitch);
	}

	public LoggerConfiguration Logger(ILogger logger, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose)
	{
		return _sinkConfiguration.Logger(logger, restrictedToMinimumLevel);
	}
}
