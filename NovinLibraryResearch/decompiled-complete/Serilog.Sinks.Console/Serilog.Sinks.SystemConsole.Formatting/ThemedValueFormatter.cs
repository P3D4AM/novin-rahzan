using System;
using System.IO;
using Serilog.Data;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Formatting;

internal abstract class ThemedValueFormatter : LogEventPropertyValueVisitor<ThemedValueFormatterState, int>
{
	private readonly ConsoleTheme _theme;

	protected ThemedValueFormatter(ConsoleTheme theme)
	{
		_theme = theme ?? throw new ArgumentNullException("theme");
	}

	protected StyleReset ApplyStyle(TextWriter output, ConsoleThemeStyle style, ref int invisibleCharacterCount)
	{
		return _theme.Apply(output, style, ref invisibleCharacterCount);
	}

	public int Format(LogEventPropertyValue value, TextWriter output, string? format, bool literalTopLevel = false)
	{
		return Visit(new ThemedValueFormatterState
		{
			Output = output,
			Format = format,
			IsTopLevel = literalTopLevel
		}, value);
	}

	public abstract ThemedValueFormatter SwitchTheme(ConsoleTheme theme);
}
