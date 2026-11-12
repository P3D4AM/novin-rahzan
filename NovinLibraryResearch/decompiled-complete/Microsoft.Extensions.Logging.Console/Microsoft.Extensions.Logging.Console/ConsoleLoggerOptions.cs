using System;

namespace Microsoft.Extensions.Logging.Console;

public class ConsoleLoggerOptions
{
	private ConsoleLoggerFormat _format;

	[Obsolete("ConsoleLoggerOptions.DisableColors has been deprecated. Use SimpleConsoleFormatterOptions.ColorBehavior instead.")]
	public bool DisableColors { get; set; }

	[Obsolete("ConsoleLoggerOptions.Format has been deprecated. Use ConsoleLoggerOptions.FormatterName instead.")]
	public ConsoleLoggerFormat Format
	{
		get
		{
			return _format;
		}
		set
		{
			if (value < ConsoleLoggerFormat.Default || value > ConsoleLoggerFormat.Systemd)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			_format = value;
		}
	}

	public string FormatterName { get; set; }

	[Obsolete("ConsoleLoggerOptions.IncludeScopes has been deprecated. Use ConsoleFormatterOptions.IncludeScopes instead.")]
	public bool IncludeScopes { get; set; }

	public LogLevel LogToStandardErrorThreshold { get; set; } = LogLevel.None;


	[Obsolete("ConsoleLoggerOptions.TimestampFormat has been deprecated. Use ConsoleFormatterOptions.TimestampFormat instead.")]
	public string TimestampFormat { get; set; }

	[Obsolete("ConsoleLoggerOptions.UseUtcTimestamp has been deprecated. Use ConsoleFormatterOptions.UseUtcTimestamp instead.")]
	public bool UseUtcTimestamp { get; set; }
}
