using System.IO;

namespace Serilog.Sinks.SystemConsole.Themes;

internal class EmptyConsoleTheme : ConsoleTheme
{
	public override bool CanBuffer => true;

	protected override int ResetCharCount { get; }

	public override int Set(TextWriter output, ConsoleThemeStyle style)
	{
		return 0;
	}

	public override void Reset(TextWriter output)
	{
	}
}
