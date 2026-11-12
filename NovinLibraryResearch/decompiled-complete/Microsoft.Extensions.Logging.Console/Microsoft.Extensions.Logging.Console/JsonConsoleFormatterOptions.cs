using System.Text.Json;

namespace Microsoft.Extensions.Logging.Console;

public class JsonConsoleFormatterOptions : ConsoleFormatterOptions
{
	public JsonWriterOptions JsonWriterOptions { get; set; }
}
