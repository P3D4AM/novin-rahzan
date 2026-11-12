namespace Microsoft.Extensions.Logging.Console;

public class SimpleConsoleFormatterOptions : ConsoleFormatterOptions
{
	public LoggerColorBehavior ColorBehavior { get; set; }

	public bool SingleLine { get; set; }
}
