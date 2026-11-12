using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Formatting;
using Serilog.Sinks.SystemConsole.Rendering;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Output;

internal class PropertiesTokenRenderer : OutputTemplateTokenRenderer
{
	private readonly MessageTemplate _outputTemplate;

	private readonly ConsoleTheme _theme;

	private readonly PropertyToken _token;

	private readonly ThemedValueFormatter _valueFormatter;

	public PropertiesTokenRenderer(ConsoleTheme theme, PropertyToken token, MessageTemplate outputTemplate, IFormatProvider? formatProvider)
	{
		_outputTemplate = outputTemplate;
		_theme = theme ?? throw new ArgumentNullException("theme");
		_token = token ?? throw new ArgumentNullException("token");
		bool flag = false;
		if (token.Format != null)
		{
			for (int i = 0; i < token.Format.Length; i++)
			{
				if (token.Format[i] == 'j')
				{
					flag = true;
				}
			}
		}
		_valueFormatter = (flag ? ((ThemedValueFormatter)new ThemedJsonValueFormatter(theme, formatProvider)) : ((ThemedValueFormatter)new ThemedDisplayValueFormatter(theme, formatProvider)));
	}

	public override void Render(LogEvent logEvent, TextWriter output)
	{
		LogEvent logEvent2 = logEvent;
		StructureValue structureValue = new StructureValue(from p in logEvent2.Properties
			where !TemplateContainsPropertyName(logEvent2.MessageTemplate, p.Key) && !TemplateContainsPropertyName(_outputTemplate, p.Key)
			select new LogEventProperty(p.Key, p.Value));
		if (!_token.Alignment.HasValue || !_theme.CanBuffer)
		{
			_valueFormatter.Format(structureValue, output, null);
			return;
		}
		StringWriter stringWriter = new StringWriter(new StringBuilder(structureValue.Properties.Count * 16));
		int amount = _valueFormatter.Format(structureValue, stringWriter, null);
		string value = stringWriter.ToString();
		Padding.Apply(output, value, _token.Alignment.Value.Widen(amount));
	}

	private static bool TemplateContainsPropertyName(MessageTemplate template, string propertyName)
	{
		foreach (MessageTemplateToken token in template.Tokens)
		{
			if (token is PropertyToken propertyToken && propertyToken.PropertyName == propertyName)
			{
				return true;
			}
		}
		return false;
	}
}
