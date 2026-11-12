using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Parsing;

namespace Serilog.Rendering;

internal static class MessageTemplateRenderer
{
	private static readonly JsonValueFormatter JsonValueFormatter = new JsonValueFormatter("$type");

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Render(MessageTemplate messageTemplate, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
	{
		bool isLiteral = false;
		bool isJson = false;
		if (format != null)
		{
			for (int i = 0; i < format.Length; i++)
			{
				if (format[i] == 'l')
				{
					isLiteral = true;
				}
				else if (format[i] == 'j')
				{
					isJson = true;
				}
			}
		}
		for (int j = 0; j < messageTemplate.TokenArray.Length; j++)
		{
			MessageTemplateToken messageTemplateToken = messageTemplate.TokenArray[j];
			if (messageTemplateToken is TextToken tt)
			{
				RenderTextToken(tt, output);
			}
			else
			{
				RenderPropertyToken((PropertyToken)messageTemplateToken, properties, output, formatProvider, isLiteral, isJson);
			}
		}
	}

	public static void RenderTextToken(TextToken tt, TextWriter output)
	{
		output.Write(tt.Text);
	}

	public static void RenderPropertyToken(PropertyToken pt, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output, IFormatProvider? formatProvider, bool isLiteral, bool isJson)
	{
		if (!properties.TryGetValue(pt.PropertyName, out LogEventPropertyValue value))
		{
			output.Write(pt.RawText);
			return;
		}
		if (!pt.Alignment.HasValue)
		{
			RenderValue(value, isLiteral, isJson, output, pt.Format, formatProvider);
			return;
		}
		using StringWriter stringWriter = ReusableStringWriter.GetOrCreate();
		RenderValue(value, isLiteral, isJson, stringWriter, pt.Format, formatProvider);
		StringBuilder stringBuilder = stringWriter.GetStringBuilder();
		if (stringBuilder.Length >= pt.Alignment.Value.Width)
		{
			output.Write(stringBuilder.ToString());
			return;
		}
		string value2 = stringBuilder.ToString();
		Alignment? alignment = pt.Alignment.Value;
		Padding.Apply(output, value2, in alignment);
	}

	private static void RenderValue(LogEventPropertyValue propertyValue, bool literal, bool json, TextWriter output, string? format, IFormatProvider? formatProvider)
	{
		if (literal && propertyValue is ScalarValue { Value: string value })
		{
			output.Write(value);
		}
		else if (json && format == null)
		{
			JsonValueFormatter.Format(propertyValue, output);
		}
		else
		{
			propertyValue.Render(output, format, formatProvider);
		}
	}
}
