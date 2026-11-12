using System;
using System.ComponentModel;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Sinks.Async;

namespace Serilog;

public static class LoggerConfigurationAsyncExtensions
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static LoggerConfiguration Async(this LoggerSinkConfiguration loggerSinkConfiguration, Action<LoggerSinkConfiguration> configure, int bufferSize)
	{
		return loggerSinkConfiguration.Async(configure, bufferSize, blockWhenFull: false);
	}

	public static LoggerConfiguration Async(this LoggerSinkConfiguration loggerSinkConfiguration, Action<LoggerSinkConfiguration> configure, int bufferSize = 10000, bool blockWhenFull = false)
	{
		return loggerSinkConfiguration.Async(configure, null, bufferSize, blockWhenFull);
	}

	public static LoggerConfiguration Async(this LoggerSinkConfiguration loggerSinkConfiguration, Action<LoggerSinkConfiguration> configure, IAsyncLogEventSinkMonitor monitor, int bufferSize = 10000, bool blockWhenFull = false)
	{
		return LoggerSinkConfiguration.Wrap(loggerSinkConfiguration, (ILogEventSink wrappedSink) => new BackgroundWorkerSink(wrappedSink, bufferSize, blockWhenFull, monitor), configure);
	}
}
