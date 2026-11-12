using System.Collections.Generic;

namespace Serilog.Sinks.SystemConsole.Themes;

internal static class AnsiConsoleThemes
{
	public static AnsiConsoleTheme Literate { get; } = new AnsiConsoleTheme(new Dictionary<ConsoleThemeStyle, string>
	{
		[ConsoleThemeStyle.Text] = "\u001b[38;5;0015m",
		[ConsoleThemeStyle.SecondaryText] = "\u001b[38;5;0007m",
		[ConsoleThemeStyle.TertiaryText] = "\u001b[38;5;0008m",
		[ConsoleThemeStyle.Invalid] = "\u001b[38;5;0011m",
		[ConsoleThemeStyle.Null] = "\u001b[38;5;0027m",
		[ConsoleThemeStyle.Name] = "\u001b[38;5;0007m",
		[ConsoleThemeStyle.String] = "\u001b[38;5;0045m",
		[ConsoleThemeStyle.Number] = "\u001b[38;5;0200m",
		[ConsoleThemeStyle.Boolean] = "\u001b[38;5;0027m",
		[ConsoleThemeStyle.Scalar] = "\u001b[38;5;0085m",
		[ConsoleThemeStyle.LevelVerbose] = "\u001b[38;5;0007m",
		[ConsoleThemeStyle.LevelDebug] = "\u001b[38;5;0007m",
		[ConsoleThemeStyle.LevelInformation] = "\u001b[38;5;0015m",
		[ConsoleThemeStyle.LevelWarning] = "\u001b[38;5;0011m",
		[ConsoleThemeStyle.LevelError] = "\u001b[38;5;0015m\u001b[48;5;0196m",
		[ConsoleThemeStyle.LevelFatal] = "\u001b[38;5;0015m\u001b[48;5;0196m"
	});


	public static AnsiConsoleTheme Grayscale { get; } = new AnsiConsoleTheme(new Dictionary<ConsoleThemeStyle, string>
	{
		[ConsoleThemeStyle.Text] = "\u001b[37;1m",
		[ConsoleThemeStyle.SecondaryText] = "\u001b[37m",
		[ConsoleThemeStyle.TertiaryText] = "\u001b[30;1m",
		[ConsoleThemeStyle.Invalid] = "\u001b[37;1m\u001b[47m",
		[ConsoleThemeStyle.Null] = "\u001b[1m\u001b[37;1m",
		[ConsoleThemeStyle.Name] = "\u001b[37m",
		[ConsoleThemeStyle.String] = "\u001b[1m\u001b[37;1m",
		[ConsoleThemeStyle.Number] = "\u001b[1m\u001b[37;1m",
		[ConsoleThemeStyle.Boolean] = "\u001b[1m\u001b[37;1m",
		[ConsoleThemeStyle.Scalar] = "\u001b[1m\u001b[37;1m",
		[ConsoleThemeStyle.LevelVerbose] = "\u001b[30;1m",
		[ConsoleThemeStyle.LevelDebug] = "\u001b[30;1m",
		[ConsoleThemeStyle.LevelInformation] = "\u001b[37;1m",
		[ConsoleThemeStyle.LevelWarning] = "\u001b[37;1m\u001b[47m",
		[ConsoleThemeStyle.LevelError] = "\u001b[30m\u001b[47m",
		[ConsoleThemeStyle.LevelFatal] = "\u001b[30m\u001b[47m"
	});


	public static AnsiConsoleTheme Code { get; } = new AnsiConsoleTheme(new Dictionary<ConsoleThemeStyle, string>
	{
		[ConsoleThemeStyle.Text] = "\u001b[38;5;0253m",
		[ConsoleThemeStyle.SecondaryText] = "\u001b[38;5;0246m",
		[ConsoleThemeStyle.TertiaryText] = "\u001b[38;5;0242m",
		[ConsoleThemeStyle.Invalid] = "\u001b[33;1m",
		[ConsoleThemeStyle.Null] = "\u001b[38;5;0038m",
		[ConsoleThemeStyle.Name] = "\u001b[38;5;0081m",
		[ConsoleThemeStyle.String] = "\u001b[38;5;0216m",
		[ConsoleThemeStyle.Number] = "\u001b[38;5;151m",
		[ConsoleThemeStyle.Boolean] = "\u001b[38;5;0038m",
		[ConsoleThemeStyle.Scalar] = "\u001b[38;5;0079m",
		[ConsoleThemeStyle.LevelVerbose] = "\u001b[37m",
		[ConsoleThemeStyle.LevelDebug] = "\u001b[37m",
		[ConsoleThemeStyle.LevelInformation] = "\u001b[37;1m",
		[ConsoleThemeStyle.LevelWarning] = "\u001b[38;5;0229m",
		[ConsoleThemeStyle.LevelError] = "\u001b[38;5;0197m\u001b[48;5;0238m",
		[ConsoleThemeStyle.LevelFatal] = "\u001b[38;5;0197m\u001b[48;5;0238m"
	});


	public static AnsiConsoleTheme Sixteen { get; } = new AnsiConsoleTheme(new Dictionary<ConsoleThemeStyle, string>
	{
		[ConsoleThemeStyle.Text] = "",
		[ConsoleThemeStyle.SecondaryText] = "",
		[ConsoleThemeStyle.TertiaryText] = "",
		[ConsoleThemeStyle.Invalid] = "\u001b[33m",
		[ConsoleThemeStyle.Null] = "\u001b[34m",
		[ConsoleThemeStyle.Name] = "",
		[ConsoleThemeStyle.String] = "\u001b[36m",
		[ConsoleThemeStyle.Number] = "\u001b[35m",
		[ConsoleThemeStyle.Boolean] = "\u001b[34m",
		[ConsoleThemeStyle.Scalar] = "\u001b[32m",
		[ConsoleThemeStyle.LevelVerbose] = "",
		[ConsoleThemeStyle.LevelDebug] = "\u001b[1m",
		[ConsoleThemeStyle.LevelInformation] = "\u001b[36;1m",
		[ConsoleThemeStyle.LevelWarning] = "\u001b[33;1m",
		[ConsoleThemeStyle.LevelError] = "\u001b[31;1m",
		[ConsoleThemeStyle.LevelFatal] = "\u001b[31;1m"
	});

}
