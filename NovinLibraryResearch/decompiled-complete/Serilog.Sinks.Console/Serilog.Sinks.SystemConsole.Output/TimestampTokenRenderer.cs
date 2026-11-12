using System;
using System.Globalization;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Rendering;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Output;

internal class TimestampTokenRenderer : OutputTemplateTokenRenderer
{
	private readonly struct DateTimeOffsetValue
	{
		public DateTimeOffset Value { get; }

		public DateTimeOffsetValue(DateTimeOffset value)
		{
			Value = value;
		}

		public void Render(TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
		{
			ICustomFormatter customFormatter = (ICustomFormatter)(formatProvider?.GetFormat(typeof(ICustomFormatter)));
			if (customFormatter != null)
			{
				output.Write(customFormatter.Format(format, Value, formatProvider));
			}
			else
			{
				output.Write(Value.ToString(format, formatProvider ?? CultureInfo.InvariantCulture));
			}
		}
	}

	private readonly ConsoleTheme _theme;

	private readonly PropertyToken _token;

	private readonly IFormatProvider? _formatProvider;

	public TimestampTokenRenderer(ConsoleTheme theme, PropertyToken token, IFormatProvider? formatProvider)
	{
		_theme = theme;
		_token = token;
		_formatProvider = formatProvider;
	}

	public override void Render(LogEvent logEvent, TextWriter output)
	{
		DateTimeOffsetValue dateTimeOffsetValue = new DateTimeOffsetValue(logEvent.Timestamp);
		int invisibleCharacterCount = 0;
		using (_theme.Apply(output, ConsoleThemeStyle.SecondaryText, ref invisibleCharacterCount))
		{
			if (!_token.Alignment.HasValue)
			{
				dateTimeOffsetValue.Render(output, _token.Format, _formatProvider);
				return;
			}
			StringWriter stringWriter = new StringWriter();
			dateTimeOffsetValue.Render(stringWriter, _token.Format, _formatProvider);
			string value = stringWriter.ToString();
			Padding.Apply(output, value, _token.Alignment);
		}
	}
}
