using System.IO;
using Serilog.Events;

namespace Serilog.Sinks.SystemConsole.Output;

internal abstract class OutputTemplateTokenRenderer
{
	public abstract void Render(LogEvent logEvent, TextWriter output);
}
