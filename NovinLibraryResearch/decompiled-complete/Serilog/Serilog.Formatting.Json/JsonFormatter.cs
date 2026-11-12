using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Rendering;

namespace Serilog.Formatting.Json;

public sealed class JsonFormatter : ITextFormatter
{
	private readonly string _closingDelimiter;

	private readonly bool _renderMessage;

	private readonly IFormatProvider? _formatProvider;

	private readonly JsonValueFormatter _jsonValueFormatter = new JsonValueFormatter();

	public JsonFormatter(string? closingDelimiter = null, bool renderMessage = false, IFormatProvider? formatProvider = null)
	{
		_closingDelimiter = closingDelimiter ?? Environment.NewLine;
		_renderMessage = renderMessage;
		_formatProvider = formatProvider;
	}

	public void Format(LogEvent logEvent, TextWriter output)
	{
		Guard.AgainstNull(logEvent, "logEvent");
		Guard.AgainstNull(output, "output");
		output.Write("{\"Timestamp\":\"");
		output.Write(logEvent.Timestamp.ToString("O"));
		output.Write("\",\"Level\":\"");
		output.Write(logEvent.Level);
		output.Write("\",\"MessageTemplate\":");
		JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Text, output);
		if (_renderMessage)
		{
			output.Write(",\"RenderedMessage\":");
			JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Render(logEvent.Properties), output);
		}
		if (logEvent.TraceId.HasValue)
		{
			output.Write(",\"TraceId\":");
			JsonValueFormatter.WriteQuotedJsonString(logEvent.TraceId.ToString(), output);
		}
		if (logEvent.SpanId.HasValue)
		{
			output.Write(",\"SpanId\":");
			JsonValueFormatter.WriteQuotedJsonString(logEvent.SpanId.ToString(), output);
		}
		if (logEvent.Exception != null)
		{
			output.Write(",\"Exception\":");
			JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
		}
		if (logEvent.Properties.Count != 0)
		{
			output.Write(",\"Properties\":{");
			char? c = null;
			foreach (KeyValuePair<string, LogEventPropertyValue> property in logEvent.Properties)
			{
				if (c.HasValue)
				{
					output.Write(c.Value);
				}
				else
				{
					c = ',';
				}
				JsonValueFormatter.WriteQuotedJsonString(property.Key, output);
				output.Write(':');
				_jsonValueFormatter.Format(property.Value, output);
			}
			output.Write('}');
		}
		IGrouping<string, PropertyToken>[] array = (from pt in logEvent.MessageTemplate.TokenArray.OfType<PropertyToken>()
			where pt.Format != null
			group pt by pt.PropertyName).ToArray();
		if (array.Length != 0)
		{
			output.Write(",\"Renderings\":{");
			WriteRenderingsValues(array, logEvent.Properties, output);
			output.Write('}');
		}
		output.Write('}');
		output.Write(_closingDelimiter);
	}

	private void WriteRenderingsValues(IEnumerable<IGrouping<string, PropertyToken>> tokensWithFormat, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
	{
		char? c = null;
		foreach (IGrouping<string, PropertyToken> item in tokensWithFormat)
		{
			if (c.HasValue)
			{
				output.Write(c.Value);
			}
			else
			{
				c = ',';
			}
			output.Write('"');
			output.Write(item.Key);
			output.Write("\":[");
			char? c2 = null;
			foreach (PropertyToken item2 in item)
			{
				if (c2.HasValue)
				{
					output.Write(c2.Value);
				}
				c2 = ',';
				output.Write('{');
				char? precedingDelimiter2 = null;
				WriteNameValuePair("Format", item2.Format, ref precedingDelimiter2, output);
				using StringWriter stringWriter = ReusableStringWriter.GetOrCreate();
				MessageTemplateRenderer.RenderPropertyToken(item2, properties, stringWriter, _formatProvider, isLiteral: true, isJson: false);
				WriteNameValuePair("Rendering", stringWriter.ToString(), ref precedingDelimiter2, output);
				output.Write('}');
			}
			output.Write(']');
		}
		static void WriteNameValuePair(string name, string value, ref char? precedingDelimiter, TextWriter output)
		{
			if (precedingDelimiter.HasValue)
			{
				output.Write(precedingDelimiter.Value);
			}
			JsonValueFormatter.WriteQuotedJsonString(name, output);
			output.Write(':');
			JsonValueFormatter.WriteQuotedJsonString(value, output);
			precedingDelimiter = ',';
		}
	}
}
