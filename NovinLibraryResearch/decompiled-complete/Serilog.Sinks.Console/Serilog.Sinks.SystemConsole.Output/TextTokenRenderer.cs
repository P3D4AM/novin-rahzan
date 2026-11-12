using System.IO;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Output;

internal class TextTokenRenderer : OutputTemplateTokenRenderer
{
	private readonly ConsoleTheme _theme;

	private readonly string _text;

	public TextTokenRenderer(ConsoleTheme theme, string text)
	{
		_theme = theme;
		_text = text;
	}

	public override void Render(LogEvent logEvent, TextWriter output)
	{
		int invisibleCharacterCount = 0;
		using (_theme.Apply(output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
		{
			output.Write(_text);
		}
	}
}
