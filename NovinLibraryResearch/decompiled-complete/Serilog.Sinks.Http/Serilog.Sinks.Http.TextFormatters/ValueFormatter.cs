using Serilog.Formatting.Json;

namespace Serilog.Sinks.Http.TextFormatters;

internal static class ValueFormatter
{
	internal static readonly JsonValueFormatter Instance = new JsonValueFormatter();
}
