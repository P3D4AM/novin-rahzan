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

public class CompactTextFormatter : ITextFormatter
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
		output.Write("{\"@t\":\"");
		output.Write(logEvent.Timestamp.UtcDateTime.ToString("o"));
		output.Write("\",\"@mt\":");
		JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Text, output);
		if (IsRenderingMessage)
		{
			output.Write(",\"@m\":");
			JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Render(logEvent.Properties), output);
		}
		IEnumerable<PropertyToken> enumerable = from pt in logEvent.MessageTemplate.Tokens.OfType<PropertyToken>()
			where pt.Format != null
			select pt;
		if (enumerable.Any())
		{
			output.Write(",\"@r\":[");
			string value = string.Empty;
			foreach (PropertyToken item in enumerable)
			{
				output.Write(value);
				value = ",";
				StringWriter stringWriter = new StringWriter();
				item.Render(logEvent.Properties, stringWriter);
				JsonValueFormatter.WriteQuotedJsonString(stringWriter.ToString(), output);
			}
			output.Write(']');
		}
		if (logEvent.Level != LogEventLevel.Information)
		{
			output.Write(",\"@l\":\"");
			output.Write(logEvent.Level);
			output.Write('"');
		}
		if (logEvent.Exception != null)
		{
			output.Write(",\"@x\":");
			JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
		}
		foreach (KeyValuePair<string, LogEventPropertyValue> property in logEvent.Properties)
		{
			string text = property.Key;
			if (text.Length > 0 && text[0] == '@')
			{
				text = "@" + text;
			}
			output.Write(',');
			JsonValueFormatter.WriteQuotedJsonString(text, output);
			output.Write(':');
			ValueFormatter.Instance.Format(property.Value, output);
		}
		output.Write('}');
	}

	private static void LogNonFormattableEvent(LogEvent logEvent, Exception e)
	{
		SelfLog.WriteLine("Event at {0} with message template {1} could not be formatted into JSON and will be dropped: {2}", logEvent.Timestamp.ToString("o"), logEvent.MessageTemplate.Text, e);
	}
}
