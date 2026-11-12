using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Rendering;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Output;

internal class LevelTokenRenderer : OutputTemplateTokenRenderer
{
	private readonly ConsoleTheme _theme;

	private readonly PropertyToken _levelToken;

	private static readonly Dictionary<LogEventLevel, ConsoleThemeStyle> Levels = new Dictionary<LogEventLevel, ConsoleThemeStyle>
	{
		{
			LogEventLevel.Verbose,
			ConsoleThemeStyle.LevelVerbose
		},
		{
			LogEventLevel.Debug,
			ConsoleThemeStyle.LevelDebug
		},
		{
			LogEventLevel.Information,
			ConsoleThemeStyle.LevelInformation
		},
		{
			LogEventLevel.Warning,
			ConsoleThemeStyle.LevelWarning
		},
		{
			LogEventLevel.Error,
			ConsoleThemeStyle.LevelError
		},
		{
			LogEventLevel.Fatal,
			ConsoleThemeStyle.LevelFatal
		}
	};

	public LevelTokenRenderer(ConsoleTheme theme, PropertyToken levelToken)
	{
		_theme = theme;
		_levelToken = levelToken;
	}

	public override void Render(LogEvent logEvent, TextWriter output)
	{
		string levelMoniker = LevelOutputFormat.GetLevelMoniker(logEvent.Level, _levelToken.Format);
		if (!Levels.TryGetValue(logEvent.Level, out var value))
		{
			value = ConsoleThemeStyle.Invalid;
		}
		int invisibleCharacterCount = 0;
		using (_theme.Apply(output, value, ref invisibleCharacterCount))
		{
			Padding.Apply(output, levelMoniker, _levelToken.Alignment);
		}
	}
}
