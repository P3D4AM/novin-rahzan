using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using Serilog.Parsing;

namespace Serilog.Sinks.Http.TextFormatters;

public class NormalTextFormatter : ITextFormatter
{
	protected bool IsRenderingMessage { get; set; }

	public void Format(LogEvent logEvent, TextWriter output)
	{
		try
		{
			StringWriter stringWriter = new StringWriter();
			FormatContent(logEvent, stringWriter);
			output.WriteLine(stringWriter.ToString());
		}
		catch (Exception e)
		{
			LogNonFormattableEvent(logEvent, e);
		}
	}

	private void FormatContent(LogEvent logEvent, TextWriter output)
	{
		if (logEvent == null)
		{
			throw new ArgumentNullException("logEvent");
		}
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		output.Write("{\"Timestamp\":\"");
		output.Write(logEvent.Timestamp.UtcDateTime.ToString("o"));
		output.Write("\",\"Level\":\"");
		output.Write(logEvent.Level);
		output.Write("\",\"MessageTemplate\":");
		JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Text, output);
		if (IsRenderingMessage)
		{
			output.Write(",\"RenderedMessage\":");
			JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Render(logEvent.Properties), output);
		}
		if (logEvent.Exception != null)
		{
			output.Write(",\"Exception\":");
			JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
		}
		if (logEvent.Properties.Count != 0)
		{
			WriteProperties(logEvent.Properties, output);
		}
		IEnumerable<PropertyToken> source = from pt in logEvent.MessageTemplate.Tokens.OfType<PropertyToken>()
			where pt.Format != null
			select pt;
		if (source.Any())
		{
			WriteRenderings(from pt in source
				group pt by pt.PropertyName, logEvent.Properties, output);
		}
		output.Write('}');
	}

	private static void WriteProperties(IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
	{
		output.Write(",\"Properties\":{");
		string value = string.Empty;
		foreach (KeyValuePair<string, LogEventPropertyValue> property in properties)
		{
			output.Write(value);
			value = ",";
			JsonValueFormatter.WriteQuotedJsonString(property.Key, output);
			output.Write(':');
			ValueFormatter.Instance.Format(property.Value, output);
		}
		output.Write('}');
	}

	private static void WriteRenderings(IEnumerable<IGrouping<string, PropertyToken>> tokensWithFormat, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
	{
		output.Write(",\"Renderings\":{");
		string value = string.Empty;
		foreach (IGrouping<string, PropertyToken> item in tokensWithFormat)
		{
			output.Write(value);
			value = ",";
			JsonValueFormatter.WriteQuotedJsonString(item.Key, output);
			output.Write(":[");
			string value2 = string.Empty;
			foreach (PropertyToken item2 in item)
			{
				output.Write(value2);
				value2 = ",";
				output.Write("{\"Format\":");
				JsonValueFormatter.WriteQuotedJsonString(item2.Format, output);
				output.Write(",\"Rendering\":");
				StringWriter stringWriter = new StringWriter();
				item2.Render(properties, stringWriter);
				JsonValueFormatter.WriteQuotedJsonString(stringWriter.ToString(), output);
				output.Write('}');
			}
			output.Write(']');
		}
		output.Write('}');
	}

	private static void LogNonFormattableEvent(LogEvent logEvent, Exception e)
	{
		SelfLog.WriteLine("Event at {0} with message template {1} could not be formatted into JSON and will be dropped: {2}", logEvent.Timestamp.ToString("o"), logEvent.MessageTemplate.Text, e);
	}
}
