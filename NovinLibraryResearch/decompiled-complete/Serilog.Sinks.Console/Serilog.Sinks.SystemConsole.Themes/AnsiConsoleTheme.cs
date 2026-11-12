using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Serilog.Sinks.SystemConsole.Themes;

public class AnsiConsoleTheme : ConsoleTheme
{
	private readonly IReadOnlyDictionary<ConsoleThemeStyle, string> _styles;

	private const string AnsiStyleReset = "\u001b[0m";

	public static AnsiConsoleTheme Code { get; } = AnsiConsoleThemes.Code;


	public static AnsiConsoleTheme Grayscale { get; } = AnsiConsoleThemes.Grayscale;


	public static AnsiConsoleTheme Literate { get; } = AnsiConsoleThemes.Literate;


	public static AnsiConsoleTheme Sixteen { get; } = AnsiConsoleThemes.Sixteen;


	public override bool CanBuffer => true;

	protected override int ResetCharCount { get; } = "\u001b[0m".Length;


	public AnsiConsoleTheme(IReadOnlyDictionary<ConsoleThemeStyle, string> styles)
	{
		if (styles == null)
		{
			throw new ArgumentNullException("styles");
		}
		_styles = styles.ToDictionary<KeyValuePair<ConsoleThemeStyle, string>, ConsoleThemeStyle, string>((KeyValuePair<ConsoleThemeStyle, string> kv) => kv.Key, (KeyValuePair<ConsoleThemeStyle, string> kv) => kv.Value);
	}

	public override int Set(TextWriter output, ConsoleThemeStyle style)
	{
		if (_styles.TryGetValue(style, out string value))
		{
			output.Write(value);
			return value.Length;
		}
		return 0;
	}

	public override void Reset(TextWriter output)
	{
		output.Write("\u001b[0m");
	}
}
