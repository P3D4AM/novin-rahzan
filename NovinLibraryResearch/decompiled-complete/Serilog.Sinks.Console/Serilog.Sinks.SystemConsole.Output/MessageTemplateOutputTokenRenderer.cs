using System;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Formatting;
using Serilog.Sinks.SystemConsole.Rendering;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Output;

internal class MessageTemplateOutputTokenRenderer : OutputTemplateTokenRenderer
{
	private readonly ConsoleTheme _theme;

	private readonly PropertyToken _token;

	private readonly ThemedMessageTemplateRenderer _renderer;

	public MessageTemplateOutputTokenRenderer(ConsoleTheme theme, PropertyToken token, IFormatProvider? formatProvider)
	{
		_theme = theme ?? throw new ArgumentNullException("theme");
		_token = token ?? throw new ArgumentNullException("token");
		bool isLiteral = false;
		bool flag = false;
		if (token.Format != null)
		{
			for (int i = 0; i < token.Format.Length; i++)
			{
				if (token.Format[i] == 'l')
				{
					isLiteral = true;
				}
				else if (token.Format[i] == 'j')
				{
					flag = true;
				}
			}
		}
		_renderer = new ThemedMessageTemplateRenderer(theme, flag ? ((ThemedValueFormatter)new ThemedJsonValueFormatter(theme, formatProvider)) : ((ThemedValueFormatter)new ThemedDisplayValueFormatter(theme, formatProvider)), isLiteral);
	}

	public override void Render(LogEvent logEvent, TextWriter output)
	{
		if (!_token.Alignment.HasValue || !_theme.CanBuffer)
		{
			_renderer.Render(logEvent.MessageTemplate, logEvent.Properties, output);
			return;
		}
		StringWriter stringWriter = new StringWriter();
		int amount = _renderer.Render(logEvent.MessageTemplate, logEvent.Properties, stringWriter);
		string value = stringWriter.ToString();
		Padding.Apply(output, value, _token.Alignment.Value.Widen(amount));
	}
}
