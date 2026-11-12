using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Serilog.Sinks.SystemConsole.Themes;

public class SystemConsoleTheme : ConsoleTheme
{
	public static SystemConsoleTheme Grayscale { get; } = SystemConsoleThemes.Grayscale;


	public static SystemConsoleTheme Literate { get; } = SystemConsoleThemes.Literate;


	public static SystemConsoleTheme Colored { get; } = SystemConsoleThemes.Colored;


	public IReadOnlyDictionary<ConsoleThemeStyle, SystemConsoleThemeStyle> Styles { get; }

	public override bool CanBuffer => false;

	protected override int ResetCharCount { get; }

	public SystemConsoleTheme(IReadOnlyDictionary<ConsoleThemeStyle, SystemConsoleThemeStyle> styles)
	{
		if (styles == null)
		{
			throw new ArgumentNullException("styles");
		}
		Styles = styles.ToDictionary((KeyValuePair<ConsoleThemeStyle, SystemConsoleThemeStyle> kv) => kv.Key, (KeyValuePair<ConsoleThemeStyle, SystemConsoleThemeStyle> kv) => kv.Value);
	}

	public override int Set(TextWriter output, ConsoleThemeStyle style)
	{
		if (Styles.TryGetValue(style, out var value))
		{
			if (value.Foreground.HasValue)
			{
				Console.ForegroundColor = value.Foreground.Value;
			}
			if (value.Background.HasValue)
			{
				Console.BackgroundColor = value.Background.Value;
			}
		}
		return 0;
	}

	public override void Reset(TextWriter output)
	{
		Console.ResetColor();
	}
}
