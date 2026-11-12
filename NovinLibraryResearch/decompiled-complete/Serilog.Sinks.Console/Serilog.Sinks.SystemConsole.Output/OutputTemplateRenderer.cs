using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Output;

internal class OutputTemplateRenderer : ITextFormatter
{
	private readonly OutputTemplateTokenRenderer[] _renderers;

	public OutputTemplateRenderer(ConsoleTheme theme, string outputTemplate, IFormatProvider? formatProvider)
	{
		if (outputTemplate == null)
		{
			throw new ArgumentNullException("outputTemplate");
		}
		MessageTemplate messageTemplate = new MessageTemplateParser().Parse(outputTemplate);
		List<OutputTemplateTokenRenderer> list = new List<OutputTemplateTokenRenderer>();
		foreach (MessageTemplateToken token in messageTemplate.Tokens)
		{
			if (token is TextToken textToken)
			{
				list.Add(new TextTokenRenderer(theme, textToken.Text));
				continue;
			}
			PropertyToken propertyToken = (PropertyToken)token;
			if (propertyToken.PropertyName == "Level")
			{
				list.Add(new LevelTokenRenderer(theme, propertyToken));
			}
			else if (propertyToken.PropertyName == "NewLine")
			{
				list.Add(new NewLineTokenRenderer(propertyToken.Alignment));
			}
			else if (propertyToken.PropertyName == "TraceId")
			{
				list.Add(new TraceIdTokenRenderer(theme, propertyToken));
			}
			else if (propertyToken.PropertyName == "SpanId")
			{
				list.Add(new SpanIdTokenRenderer(theme, propertyToken));
			}
			else if (propertyToken.PropertyName == "Exception")
			{
				list.Add(new ExceptionTokenRenderer(theme, propertyToken));
			}
			else if (propertyToken.PropertyName == "Message")
			{
				list.Add(new MessageTemplateOutputTokenRenderer(theme, propertyToken, formatProvider));
			}
			else if (propertyToken.PropertyName == "Timestamp")
			{
				list.Add(new TimestampTokenRenderer(theme, propertyToken, formatProvider));
			}
			else if (propertyToken.PropertyName == "Properties")
			{
				list.Add(new PropertiesTokenRenderer(theme, propertyToken, messageTemplate, formatProvider));
			}
			else
			{
				list.Add(new EventPropertyTokenRenderer(theme, propertyToken, formatProvider));
			}
		}
		_renderers = list.ToArray();
	}

	public void Format(LogEvent logEvent, TextWriter output)
	{
		if (logEvent == null)
		{
			throw new ArgumentNullException("logEvent");
		}
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		OutputTemplateTokenRenderer[] renderers = _renderers;
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].Render(logEvent, output);
		}
	}
}
