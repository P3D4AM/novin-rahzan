using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace Serilog.Extensions.Logging;

public static class LevelConvert
{
	public static LogEventLevel ToSerilogLevel(LogLevel logLevel)
	{
		switch (logLevel)
		{
		case LogLevel.Critical:
		case LogLevel.None:
			return LogEventLevel.Fatal;
		case LogLevel.Error:
			return LogEventLevel.Error;
		case LogLevel.Warning:
			return LogEventLevel.Warning;
		case LogLevel.Information:
			return LogEventLevel.Information;
		case LogLevel.Debug:
			return LogEventLevel.Debug;
		default:
			return LogEventLevel.Verbose;
		}
	}

	public static LogLevel ToExtensionsLevel(LogEventLevel logEventLevel)
	{
		return logEventLevel switch
		{
			LogEventLevel.Fatal => LogLevel.Critical, 
			LogEventLevel.Error => LogLevel.Error, 
			LogEventLevel.Warning => LogLevel.Warning, 
			LogEventLevel.Information => LogLevel.Information, 
			LogEventLevel.Debug => LogLevel.Debug, 
			_ => LogLevel.Trace, 
		};
	}
}
