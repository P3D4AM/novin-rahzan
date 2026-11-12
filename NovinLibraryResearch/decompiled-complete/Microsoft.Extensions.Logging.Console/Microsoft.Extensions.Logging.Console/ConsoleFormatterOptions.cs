namespace Microsoft.Extensions.Logging.Console;

public class ConsoleFormatterOptions
{
	public bool IncludeScopes { get; set; }

	public string TimestampFormat { get; set; }

	public bool UseUtcTimestamp { get; set; }
}
