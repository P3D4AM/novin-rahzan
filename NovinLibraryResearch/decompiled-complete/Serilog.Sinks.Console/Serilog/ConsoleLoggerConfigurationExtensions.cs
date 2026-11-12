using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.SystemConsole;
using Serilog.Sinks.SystemConsole.Output;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog;

public static class ConsoleLoggerConfigurationExtensions
{
	internal static readonly object DefaultSyncRoot = new object();

	internal const string DefaultConsoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

	public static LoggerConfiguration Console(this LoggerSinkConfiguration sinkConfiguration, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", IFormatProvider? formatProvider = null, LoggingLevelSwitch? levelSwitch = null, LogEventLevel? standardErrorFromLevel = null, ConsoleTheme? theme = null, bool applyThemeToRedirectedOutput = false, object? syncRoot = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		if (outputTemplate == null)
		{
			throw new ArgumentNullException("outputTemplate");
		}
		ConsoleTheme theme2 = ((!applyThemeToRedirectedOutput && (System.Console.IsOutputRedirected || System.Console.IsErrorRedirected)) ? ConsoleTheme.None : (theme ?? SystemConsoleThemes.Literate));
		if (syncRoot == null)
		{
			syncRoot = DefaultSyncRoot;
		}
		OutputTemplateRenderer formatter = new OutputTemplateRenderer(theme2, outputTemplate, formatProvider);
		return sinkConfiguration.Sink(new ConsoleSink(theme2, formatter, standardErrorFromLevel, syncRoot), restrictedToMinimumLevel, levelSwitch);
	}

	public static LoggerConfiguration Console(this LoggerSinkConfiguration sinkConfiguration, ITextFormatter formatter, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null, LogEventLevel? standardErrorFromLevel = null, object? syncRoot = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		if (syncRoot == null)
		{
			syncRoot = DefaultSyncRoot;
		}
		return sinkConfiguration.Sink(new ConsoleSink(ConsoleTheme.None, formatter, standardErrorFromLevel, syncRoot), restrictedToMinimumLevel, levelSwitch);
	}
}
