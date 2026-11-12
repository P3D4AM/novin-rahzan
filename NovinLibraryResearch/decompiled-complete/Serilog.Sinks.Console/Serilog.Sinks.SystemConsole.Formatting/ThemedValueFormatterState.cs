using System.IO;

namespace Serilog.Sinks.SystemConsole.Formatting;

internal struct ThemedValueFormatterState
{
	public TextWriter Output;

	public string? Format;

	public bool IsTopLevel;

	public ThemedValueFormatterState Nest()
	{
		ThemedValueFormatterState result = default(ThemedValueFormatterState);
		result.Output = Output;
		return result;
	}
}
