using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Serilog.Core;
using Serilog.Core.Sinks;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Configuration;

public class LoggerSinkConfiguration
{
	private readonly LoggerConfiguration _loggerConfiguration;

	private readonly Action<ILogEventSink> _addSink;

	internal LoggerSinkConfiguration(LoggerConfiguration loggerConfiguration, Action<ILogEventSink> addSink)
	{
		_loggerConfiguration = Guard.AgainstNull(loggerConfiguration, "loggerConfiguration");
		_addSink = Guard.AgainstNull(addSink, "addSink");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public LoggerConfiguration Sink(ILogEventSink logEventSink, LogEventLevel restrictedToMinimumLevel)
	{
		return Sink(logEventSink, restrictedToMinimumLevel, null);
	}

	public LoggerConfiguration Sink(ILogEventSink logEventSink, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
	{
		ILogEventSink logEventSink2 = logEventSink;
		if (levelSwitch != null)
		{
			if (restrictedToMinimumLevel != 0)
			{
				SelfLog.WriteLine("Sink {0} was configured with both a level switch and minimum level '{1}'; the minimum level will be ignored and the switch level used", logEventSink2, restrictedToMinimumLevel);
			}
			logEventSink2 = new RestrictedSink(logEventSink2, levelSwitch);
		}
		else if (restrictedToMinimumLevel > LogEventLevel.Verbose)
		{
			logEventSink2 = new RestrictedSink(logEventSink2, new LoggingLevelSwitch(restrictedToMinimumLevel));
		}
		_addSink(logEventSink2);
		return _loggerConfiguration;
	}

	public LoggerConfiguration Sink<TSink>(LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null) where TSink : ILogEventSink, new()
	{
		return Sink(new TSink(), restrictedToMinimumLevel, levelSwitch);
	}

	public LoggerConfiguration Logger(Action<LoggerConfiguration> configureLogger, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
	{
		Guard.AgainstNull(configureLogger, "configureLogger");
		LoggerConfiguration loggerConfiguration = new LoggerConfiguration().MinimumLevel.Is(LogEventLevel.Verbose);
		configureLogger(loggerConfiguration);
		Logger logger = loggerConfiguration.CreateLogger();
		if (logger.HasOverrideMap)
		{
			SelfLog.WriteLine("Minimum level overrides are not supported on sub-loggers and may be removed completely in a future version.");
		}
		SecondaryLoggerSink logEventSink = new SecondaryLoggerSink(logger, attemptDispose: true);
		return Sink(logEventSink, restrictedToMinimumLevel, levelSwitch);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public LoggerConfiguration Logger(ILogger logger, LogEventLevel restrictedToMinimumLevel)
	{
		return Logger(logger, attemptDispose: false, restrictedToMinimumLevel);
	}

	public LoggerConfiguration Logger(ILogger logger, bool attemptDispose = false, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
	{
		Guard.AgainstNull(logger, "logger");
		if (logger is Logger { HasOverrideMap: not false })
		{
			SelfLog.WriteLine("Minimum level overrides are not supported on sub-loggers and may be removed completely in a future version.");
		}
		SecondaryLoggerSink logEventSink = new SecondaryLoggerSink(logger, attemptDispose);
		return Sink(logEventSink, restrictedToMinimumLevel, levelSwitch);
	}

	public LoggerConfiguration Conditional(Func<LogEvent, bool> condition, Action<LoggerSinkConfiguration> configureSink)
	{
		Func<LogEvent, bool> condition2 = condition;
		Guard.AgainstNull(condition2, "condition");
		Guard.AgainstNull(configureSink, "configureSink");
		return Wrap(this, (ILogEventSink s) => new ConditionalSink(s, condition2), configureSink);
	}

	public static LoggerConfiguration Wrap(LoggerSinkConfiguration loggerSinkConfiguration, Func<ILogEventSink, ILogEventSink> wrapSink, Action<LoggerSinkConfiguration> configureWrappedSink, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
	{
		Guard.AgainstNull(loggerSinkConfiguration, "loggerSinkConfiguration");
		Guard.AgainstNull(wrapSink, "wrapSink");
		Guard.AgainstNull(configureWrappedSink, "configureWrappedSink");
		List<ILogEventSink> list = new List<ILogEventSink>();
		LoggerConfiguration loggerConfiguration = new LoggerConfiguration();
		LoggerSinkConfiguration obj = (loggerConfiguration.WriteTo = new LoggerSinkConfiguration(loggerConfiguration, list.Add));
		configureWrappedSink(obj);
		if (list.Count == 0)
		{
			return loggerSinkConfiguration._loggerConfiguration;
		}
		ILogEventSink logEventSink2;
		if (list.Count != 1)
		{
			ILogEventSink logEventSink = new DisposingAggregateSink(list);
			logEventSink2 = logEventSink;
		}
		else
		{
			logEventSink2 = list.Single();
		}
		ILogEventSink logEventSink3 = logEventSink2;
		ILogEventSink logEventSink4 = wrapSink(logEventSink3);
		if (!(logEventSink4 is IDisposable) && logEventSink3 is IDisposable)
		{
			logEventSink4 = new DisposeDelegatingSink(logEventSink4, logEventSink3 as IDisposable);
		}
		return loggerSinkConfiguration.Sink(logEventSink4, restrictedToMinimumLevel, levelSwitch);
	}
}
