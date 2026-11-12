using System;
using System.Collections.Generic;

namespace Serilog.Sinks.SystemConsole.Themes;

internal static class SystemConsoleThemes
{
	public static SystemConsoleTheme Literate { get; } = new SystemConsoleTheme(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>
	{
		[ConsoleThemeStyle.Text] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.SecondaryText] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Gray
		},
		[ConsoleThemeStyle.TertiaryText] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.DarkGray
		},
		[ConsoleThemeStyle.Invalid] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Yellow
		},
		[ConsoleThemeStyle.Null] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Blue
		},
		[ConsoleThemeStyle.Name] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Gray
		},
		[ConsoleThemeStyle.String] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Cyan
		},
		[ConsoleThemeStyle.Number] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Magenta
		},
		[ConsoleThemeStyle.Boolean] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Blue
		},
		[ConsoleThemeStyle.Scalar] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Green
		},
		[ConsoleThemeStyle.LevelVerbose] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Gray
		},
		[ConsoleThemeStyle.LevelDebug] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Gray
		},
		[ConsoleThemeStyle.LevelInformation] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.LevelWarning] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Yellow
		},
		[ConsoleThemeStyle.LevelError] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White,
			Background = ConsoleColor.Red
		},
		[ConsoleThemeStyle.LevelFatal] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White,
			Background = ConsoleColor.Red
		}
	});


	public static SystemConsoleTheme Grayscale { get; } = new SystemConsoleTheme(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>
	{
		[ConsoleThemeStyle.Text] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.SecondaryText] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Gray
		},
		[ConsoleThemeStyle.TertiaryText] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.DarkGray
		},
		[ConsoleThemeStyle.Invalid] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White,
			Background = ConsoleColor.DarkGray
		},
		[ConsoleThemeStyle.Null] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.Name] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Gray
		},
		[ConsoleThemeStyle.String] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.Number] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.Boolean] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.Scalar] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.LevelVerbose] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.DarkGray
		},
		[ConsoleThemeStyle.LevelDebug] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.DarkGray
		},
		[ConsoleThemeStyle.LevelInformation] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.LevelWarning] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White,
			Background = ConsoleColor.DarkGray
		},
		[ConsoleThemeStyle.LevelError] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Black,
			Background = ConsoleColor.White
		},
		[ConsoleThemeStyle.LevelFatal] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Black,
			Background = ConsoleColor.White
		}
	});


	public static SystemConsoleTheme Colored { get; } = new SystemConsoleTheme(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>
	{
		[ConsoleThemeStyle.Text] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Gray
		},
		[ConsoleThemeStyle.SecondaryText] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.DarkGray
		},
		[ConsoleThemeStyle.TertiaryText] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.DarkGray
		},
		[ConsoleThemeStyle.Invalid] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Yellow
		},
		[ConsoleThemeStyle.Null] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.Name] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.String] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.Number] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.Boolean] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.Scalar] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White
		},
		[ConsoleThemeStyle.LevelVerbose] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.Gray,
			Background = ConsoleColor.DarkGray
		},
		[ConsoleThemeStyle.LevelDebug] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White,
			Background = ConsoleColor.DarkGray
		},
		[ConsoleThemeStyle.LevelInformation] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White,
			Background = ConsoleColor.Blue
		},
		[ConsoleThemeStyle.LevelWarning] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.DarkGray,
			Background = ConsoleColor.Yellow
		},
		[ConsoleThemeStyle.LevelError] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White,
			Background = ConsoleColor.Red
		},
		[ConsoleThemeStyle.LevelFatal] = new SystemConsoleThemeStyle
		{
			Foreground = ConsoleColor.White,
			Background = ConsoleColor.Red
		}
	});

}
