using System;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Rendering;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Output;

internal class EventPropertyTokenRenderer : OutputTemplateTokenRenderer
{
	private readonly ConsoleTheme _theme;

	private readonly PropertyToken _token;

	private readonly IFormatProvider? _formatProvider;

	public EventPropertyTokenRenderer(ConsoleTheme theme, PropertyToken token, IFormatProvider? formatProvider)
	{
		_theme = theme;
		_token = token;
		_formatProvider = formatProvider;
	}

	public override void Render(LogEvent logEvent, TextWriter output)
	{
		if (!logEvent.Properties.TryGetValue(_token.PropertyName, out LogEventPropertyValue value))
		{
			Padding.Apply(output, string.Empty, _token.Alignment);
			return;
		}
		int invisibleCharacterCount = 0;
		using (_theme.Apply(output, ConsoleThemeStyle.SecondaryText, ref invisibleCharacterCount))
		{
			TextWriter textWriter = (_token.Alignment.HasValue ? new StringWriter() : output);
			if (value is ScalarValue { Value: string value2 })
			{
				string value3 = Casing.Format(value2, _token.Format);
				textWriter.Write(value3);
			}
			else
			{
				value.Render(textWriter, _token.Format, _formatProvider);
			}
			if (_token.Alignment.HasValue)
			{
				string value4 = textWriter.ToString();
				Padding.Apply(output, value4, _token.Alignment);
			}
		}
	}
}
