using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Output;

internal class ExceptionTokenRenderer : OutputTemplateTokenRenderer
{
	private const string StackFrameLinePrefix = "   ";

	private readonly ConsoleTheme _theme;

	public ExceptionTokenRenderer(ConsoleTheme theme, PropertyToken pt)
	{
		_theme = theme;
	}

	public override void Render(LogEvent logEvent, TextWriter output)
	{
		if (logEvent.Exception == null)
		{
			return;
		}
		StringReader stringReader = new StringReader(logEvent.Exception.ToString());
		string text;
		while ((text = stringReader.ReadLine()) != null)
		{
			ConsoleThemeStyle style = (text.StartsWith("   ") ? ConsoleThemeStyle.SecondaryText : ConsoleThemeStyle.Text);
			int invisibleCharacterCount = 0;
			using (_theme.Apply(output, style, ref invisibleCharacterCount))
			{
				output.Write(text);
			}
			output.WriteLine();
		}
	}
}
