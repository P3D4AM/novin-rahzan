using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Formatting;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Rendering;

internal class ThemedMessageTemplateRenderer
{
	private readonly ConsoleTheme _theme;

	private readonly ThemedValueFormatter _valueFormatter;

	private readonly bool _isLiteral;

	private static readonly ConsoleTheme NoTheme = new EmptyConsoleTheme();

	private readonly ThemedValueFormatter _unthemedValueFormatter;

	public ThemedMessageTemplateRenderer(ConsoleTheme theme, ThemedValueFormatter valueFormatter, bool isLiteral)
	{
		_theme = theme ?? throw new ArgumentNullException("theme");
		_valueFormatter = valueFormatter;
		_isLiteral = isLiteral;
		_unthemedValueFormatter = valueFormatter.SwitchTheme(NoTheme);
	}

	public int Render(MessageTemplate template, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
	{
		int num = 0;
		foreach (MessageTemplateToken token in template.Tokens)
		{
			if (token is TextToken tt)
			{
				num += RenderTextToken(tt, output);
				continue;
			}
			PropertyToken pt = (PropertyToken)token;
			num += RenderPropertyToken(pt, properties, output);
		}
		return num;
	}

	private int RenderTextToken(TextToken tt, TextWriter output)
	{
		int invisibleCharacterCount = 0;
		using (_theme.Apply(output, ConsoleThemeStyle.Text, ref invisibleCharacterCount))
		{
			output.Write(tt.Text);
			return invisibleCharacterCount;
		}
	}

	private int RenderPropertyToken(PropertyToken pt, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
	{
		if (!properties.TryGetValue(pt.PropertyName, out LogEventPropertyValue value))
		{
			int invisibleCharacterCount = 0;
			using (_theme.Apply(output, ConsoleThemeStyle.Invalid, ref invisibleCharacterCount))
			{
				output.Write(pt.ToString());
				return invisibleCharacterCount;
			}
		}
		if (!pt.Alignment.HasValue)
		{
			return RenderValue(_theme, _valueFormatter, value, output, pt.Format);
		}
		StringWriter stringWriter = new StringWriter();
		if (!_theme.CanBuffer)
		{
			return RenderAlignedPropertyTokenUnbuffered(pt, output, value);
		}
		int num = RenderValue(_theme, _valueFormatter, value, stringWriter, pt.Format);
		string text = stringWriter.ToString();
		if (text.Length - num >= pt.Alignment.Value.Width)
		{
			output.Write(text);
		}
		else
		{
			Padding.Apply(output, text, pt.Alignment.Value.Widen(num));
		}
		return num;
	}

	private int RenderAlignedPropertyTokenUnbuffered(PropertyToken pt, TextWriter output, LogEventPropertyValue propertyValue)
	{
		if (!pt.Alignment.HasValue)
		{
			throw new ArgumentException("The PropertyToken should have a non-null Alignment.", "pt");
		}
		StringWriter stringWriter = new StringWriter();
		RenderValue(NoTheme, _unthemedValueFormatter, propertyValue, stringWriter, pt.Format);
		int length = stringWriter.ToString().Length;
		if (length >= pt.Alignment.Value.Width)
		{
			return RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
		}
		if (pt.Alignment.Value.Direction == AlignmentDirection.Left)
		{
			int result = RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
			Padding.Apply(output, string.Empty, pt.Alignment.Value.Widen(-length));
			return result;
		}
		Padding.Apply(output, string.Empty, pt.Alignment.Value.Widen(-length));
		return RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
	}

	private int RenderValue(ConsoleTheme theme, ThemedValueFormatter valueFormatter, LogEventPropertyValue propertyValue, TextWriter output, string? format)
	{
		if (_isLiteral && propertyValue is ScalarValue scalarValue && scalarValue.Value is string)
		{
			int invisibleCharacterCount = 0;
			using (theme.Apply(output, ConsoleThemeStyle.String, ref invisibleCharacterCount))
			{
				output.Write(scalarValue.Value);
				return invisibleCharacterCount;
			}
		}
		return valueFormatter.Format(propertyValue, output, format, _isLiteral);
	}
}
