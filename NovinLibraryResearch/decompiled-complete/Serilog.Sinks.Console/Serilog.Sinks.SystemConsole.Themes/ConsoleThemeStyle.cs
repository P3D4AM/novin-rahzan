using System;
using System.ComponentModel;

namespace Serilog.Sinks.SystemConsole.Themes;

public enum ConsoleThemeStyle
{
	Text = 0,
	SecondaryText = 1,
	TertiaryText = 2,
	Invalid = 3,
	Null = 4,
	Name = 5,
	String = 6,
	Number = 7,
	Boolean = 8,
	Scalar = 9,
	[Obsolete("Use ConsoleThemeStyle.Scalar instead")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Object = 9,
	LevelVerbose = 10,
	LevelDebug = 11,
	LevelInformation = 12,
	LevelWarning = 13,
	LevelError = 14,
	LevelFatal = 15
}
