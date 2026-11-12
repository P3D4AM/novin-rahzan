using Serilog.Events;

namespace Serilog;

public static class LoggerExtensions
{
	public static ILogger ForContext<TValue>(this ILogger logger, LogEventLevel level, string propertyName, TValue value, bool destructureObjects = false)
	{
		Guard.AgainstNull(logger, "logger");
		if (!logger.IsEnabled(level))
		{
			return logger;
		}
		return logger.ForContext(propertyName, value, destructureObjects);
	}
}
