using System;
using System.ComponentModel;
using System.Text;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.File;

namespace Serilog;

public static class FileLoggerConfigurationExtensions
{
	private const int DefaultRetainedFileCountLimit = 31;

	private const long DefaultFileSizeLimitBytes = 1073741824L;

	private const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

	[Obsolete("New code should not be compiled against this obsolete overload")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static LoggerConfiguration File(this LoggerSinkConfiguration sinkConfiguration, string path, LogEventLevel restrictedToMinimumLevel, string outputTemplate, IFormatProvider formatProvider, long? fileSizeLimitBytes, LoggingLevelSwitch levelSwitch, bool buffered, bool shared, TimeSpan? flushToDiskInterval)
	{
		return sinkConfiguration.File(path, restrictedToMinimumLevel, outputTemplate, formatProvider, fileSizeLimitBytes, levelSwitch, buffered, shared, flushToDiskInterval, RollingInterval.Infinite, rollOnFileSizeLimit: false, null);
	}

	[Obsolete("New code should not be compiled against this obsolete overload")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static LoggerConfiguration File(this LoggerSinkConfiguration sinkConfiguration, ITextFormatter formatter, string path, LogEventLevel restrictedToMinimumLevel, long? fileSizeLimitBytes, LoggingLevelSwitch levelSwitch, bool buffered, bool shared, TimeSpan? flushToDiskInterval)
	{
		return sinkConfiguration.File(formatter, path, restrictedToMinimumLevel, fileSizeLimitBytes, levelSwitch, buffered, shared, flushToDiskInterval, RollingInterval.Infinite, rollOnFileSizeLimit: false, null);
	}

	[Obsolete("New code should not be compiled against this obsolete overload")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static LoggerConfiguration File(this LoggerSinkConfiguration sinkConfiguration, string path, LogEventLevel restrictedToMinimumLevel, string outputTemplate, IFormatProvider formatProvider, long? fileSizeLimitBytes, LoggingLevelSwitch levelSwitch, bool buffered, bool shared, TimeSpan? flushToDiskInterval, RollingInterval rollingInterval, bool rollOnFileSizeLimit, int? retainedFileCountLimit, Encoding encoding)
	{
		return sinkConfiguration.File(path, restrictedToMinimumLevel, outputTemplate, formatProvider, fileSizeLimitBytes, levelSwitch, buffered, shared, flushToDiskInterval, rollingInterval, rollOnFileSizeLimit, retainedFileCountLimit, encoding, null, null);
	}

	[Obsolete("New code should not be compiled against this obsolete overload")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static LoggerConfiguration File(this LoggerSinkConfiguration sinkConfiguration, ITextFormatter formatter, string path, LogEventLevel restrictedToMinimumLevel, long? fileSizeLimitBytes, LoggingLevelSwitch levelSwitch, bool buffered, bool shared, TimeSpan? flushToDiskInterval, RollingInterval rollingInterval, bool rollOnFileSizeLimit, int? retainedFileCountLimit, Encoding encoding)
	{
		return sinkConfiguration.File(formatter, path, restrictedToMinimumLevel, fileSizeLimitBytes, levelSwitch, buffered, shared, flushToDiskInterval, rollingInterval, rollOnFileSizeLimit, retainedFileCountLimit, encoding, null, null);
	}

	public static LoggerConfiguration File(this LoggerSinkConfiguration sinkConfiguration, string path, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", IFormatProvider? formatProvider = null, long? fileSizeLimitBytes = 1073741824L, LoggingLevelSwitch? levelSwitch = null, bool buffered = false, bool shared = false, TimeSpan? flushToDiskInterval = null, RollingInterval rollingInterval = RollingInterval.Infinite, bool rollOnFileSizeLimit = false, int? retainedFileCountLimit = 31, Encoding? encoding = null, FileLifecycleHooks? hooks = null, TimeSpan? retainedFileTimeLimit = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (outputTemplate == null)
		{
			throw new ArgumentNullException("outputTemplate");
		}
		MessageTemplateTextFormatter formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
		return sinkConfiguration.File(formatter, path, restrictedToMinimumLevel, fileSizeLimitBytes, levelSwitch, buffered, shared, flushToDiskInterval, rollingInterval, rollOnFileSizeLimit, retainedFileCountLimit, encoding, hooks, retainedFileTimeLimit);
	}

	public static LoggerConfiguration File(this LoggerSinkConfiguration sinkConfiguration, ITextFormatter formatter, string path, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, long? fileSizeLimitBytes = 1073741824L, LoggingLevelSwitch? levelSwitch = null, bool buffered = false, bool shared = false, TimeSpan? flushToDiskInterval = null, RollingInterval rollingInterval = RollingInterval.Infinite, bool rollOnFileSizeLimit = false, int? retainedFileCountLimit = 31, Encoding? encoding = null, FileLifecycleHooks? hooks = null, TimeSpan? retainedFileTimeLimit = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		return ConfigureFile(sinkConfiguration.Sink, formatter, path, restrictedToMinimumLevel, fileSizeLimitBytes, levelSwitch, buffered, propagateExceptions: false, shared, flushToDiskInterval, encoding, rollingInterval, rollOnFileSizeLimit, retainedFileCountLimit, hooks, retainedFileTimeLimit);
	}

	[Obsolete("New code should not be compiled against this obsolete overload")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static LoggerConfiguration File(this LoggerAuditSinkConfiguration sinkConfiguration, string path, LogEventLevel restrictedToMinimumLevel, string outputTemplate, IFormatProvider formatProvider, LoggingLevelSwitch levelSwitch)
	{
		return sinkConfiguration.File(path, restrictedToMinimumLevel, outputTemplate, formatProvider, levelSwitch, null, null);
	}

	[Obsolete("New code should not be compiled against this obsolete overload")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static LoggerConfiguration File(this LoggerAuditSinkConfiguration sinkConfiguration, ITextFormatter formatter, string path, LogEventLevel restrictedToMinimumLevel, LoggingLevelSwitch levelSwitch)
	{
		return sinkConfiguration.File(formatter, path, restrictedToMinimumLevel, levelSwitch, null, null);
	}

	public static LoggerConfiguration File(this LoggerAuditSinkConfiguration sinkConfiguration, string path, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", IFormatProvider? formatProvider = null, LoggingLevelSwitch? levelSwitch = null, Encoding? encoding = null, FileLifecycleHooks? hooks = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (outputTemplate == null)
		{
			throw new ArgumentNullException("outputTemplate");
		}
		MessageTemplateTextFormatter formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
		return sinkConfiguration.File(formatter, path, restrictedToMinimumLevel, levelSwitch, encoding, hooks);
	}

	public static LoggerConfiguration File(this LoggerAuditSinkConfiguration sinkConfiguration, ITextFormatter formatter, string path, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null, Encoding? encoding = null, FileLifecycleHooks? hooks = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		return ConfigureFile(sinkConfiguration.Sink, formatter, path, restrictedToMinimumLevel, null, levelSwitch, buffered: false, propagateExceptions: true, shared: false, null, encoding, RollingInterval.Infinite, rollOnFileSizeLimit: false, null, hooks, null);
	}

	private static LoggerConfiguration ConfigureFile(this Func<ILogEventSink, LogEventLevel, LoggingLevelSwitch?, LoggerConfiguration> addSink, ITextFormatter formatter, string path, LogEventLevel restrictedToMinimumLevel, long? fileSizeLimitBytes, LoggingLevelSwitch? levelSwitch, bool buffered, bool propagateExceptions, bool shared, TimeSpan? flushToDiskInterval, Encoding? encoding, RollingInterval rollingInterval, bool rollOnFileSizeLimit, int? retainedFileCountLimit, FileLifecycleHooks? hooks, TimeSpan? retainedFileTimeLimit)
	{
		if (addSink == null)
		{
			throw new ArgumentNullException("addSink");
		}
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (fileSizeLimitBytes.HasValue && fileSizeLimitBytes < 1)
		{
			throw new ArgumentException("Invalid value provided; file size limit must be at least 1 byte, or null.", "fileSizeLimitBytes");
		}
		if (retainedFileCountLimit.HasValue && retainedFileCountLimit < 1)
		{
			throw new ArgumentException("At least one file must be retained.", "retainedFileCountLimit");
		}
		if (retainedFileTimeLimit.HasValue && retainedFileTimeLimit < TimeSpan.Zero)
		{
			throw new ArgumentException("Negative value provided; retained file time limit must be non-negative.", "retainedFileTimeLimit");
		}
		if (shared && buffered)
		{
			throw new ArgumentException("Buffered writes are not available when file sharing is enabled.", "buffered");
		}
		if (shared && hooks != null)
		{
			throw new ArgumentException("File lifecycle hooks are not currently supported for shared log files.", "hooks");
		}
		ILogEventSink logEventSink;
		try
		{
			logEventSink = ((!rollOnFileSizeLimit && rollingInterval == RollingInterval.Infinite) ? ((!shared) ? ((ILogEventSink)new FileSink(path, formatter, fileSizeLimitBytes, encoding, buffered, hooks)) : ((ILogEventSink)new SharedFileSink(path, formatter, fileSizeLimitBytes, encoding))) : new RollingFileSink(path, formatter, fileSizeLimitBytes, retainedFileCountLimit, encoding, buffered, shared, rollingInterval, rollOnFileSizeLimit, hooks, retainedFileTimeLimit));
		}
		catch (Exception arg)
		{
			SelfLog.WriteLine("Unable to open file sink for {0}: {1}", path, arg);
			if (propagateExceptions)
			{
				throw;
			}
			return addSink(new NullSink(), LogEventLevel.Fatal, null);
		}
		if (flushToDiskInterval.HasValue)
		{
			logEventSink = new PeriodicFlushToDiskSink(logEventSink, flushToDiskInterval.Value);
		}
		return addSink(logEventSink, restrictedToMinimumLevel, levelSwitch);
	}
}
