using System;
using System.IO;

namespace Serilog.Sinks.SystemConsole.Themes;

internal struct StyleReset : IDisposable
{
	private readonly ConsoleTheme _theme;

	private readonly TextWriter _output;

	public StyleReset(ConsoleTheme theme, TextWriter output)
	{
		_theme = theme;
		_output = output;
	}

	public void Dispose()
	{
		_theme.Reset(_output);
	}
}
