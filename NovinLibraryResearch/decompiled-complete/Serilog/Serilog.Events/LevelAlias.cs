namespace Serilog.Events;

public static class LevelAlias
{
	public const LogEventLevel Minimum = LogEventLevel.Verbose;

	public const LogEventLevel Maximum = LogEventLevel.Fatal;

	public const LogEventLevel Off = (LogEventLevel)6;
}
