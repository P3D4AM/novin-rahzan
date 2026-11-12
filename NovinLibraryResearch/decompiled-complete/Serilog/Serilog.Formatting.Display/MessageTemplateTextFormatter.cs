using System;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Rendering;

namespace Serilog.Formatting.Display;

public class MessageTemplateTextFormatter : ITextFormatter
{
	private readonly IFormatProvider? _formatProvider;

	private readonly MessageTemplate _outputTemplate;

	public MessageTemplateTextFormatter(string outputTemplate, IFormatProvider? formatProvider = null)
	{
		Guard.AgainstNull(outputTemplate, "outputTemplate");
		_outputTemplate = new MessageTemplateParser().Parse(outputTemplate);
		_formatProvider = formatProvider;
	}

	public void Format(LogEvent logEvent, TextWriter output)
	{
		Guard.AgainstNull(logEvent, "logEvent");
		Guard.AgainstNull(output, "output");
		MessageTemplateToken[] tokenArray = _outputTemplate.TokenArray;
		foreach (MessageTemplateToken messageTemplateToken in tokenArray)
		{
			if (messageTemplateToken is TextToken tt)
			{
				MessageTemplateRenderer.RenderTextToken(tt, output);
				continue;
			}
			PropertyToken propertyToken = (PropertyToken)messageTemplateToken;
			if (propertyToken.PropertyName == "Level")
			{
				string levelMoniker = LevelOutputFormat.GetLevelMoniker(logEvent.Level, propertyToken.Format);
				Alignment? alignment = propertyToken.Alignment;
				Padding.Apply(output, levelMoniker, in alignment);
				continue;
			}
			if (propertyToken.PropertyName == "TraceId")
			{
				string value = logEvent.TraceId?.ToString() ?? "";
				Alignment? alignment = propertyToken.Alignment;
				Padding.Apply(output, value, in alignment);
				continue;
			}
			if (propertyToken.PropertyName == "SpanId")
			{
				string value2 = logEvent.SpanId?.ToString() ?? "";
				Alignment? alignment = propertyToken.Alignment;
				Padding.Apply(output, value2, in alignment);
				continue;
			}
			if (propertyToken.PropertyName == "NewLine")
			{
				string newLine = Environment.NewLine;
				Alignment? alignment = propertyToken.Alignment;
				Padding.Apply(output, newLine, in alignment);
				continue;
			}
			if (propertyToken.PropertyName == "Exception")
			{
				string value3 = ((logEvent.Exception == null) ? "" : (logEvent.Exception?.ToString() + Environment.NewLine));
				Alignment? alignment = propertyToken.Alignment;
				Padding.Apply(output, value3, in alignment);
				continue;
			}
			TextWriter textWriter = (propertyToken.Alignment.HasValue ? ReusableStringWriter.GetOrCreate() : output);
			if (propertyToken.PropertyName == "Message")
			{
				MessageTemplateRenderer.Render(logEvent.MessageTemplate, logEvent.Properties, textWriter, propertyToken.Format, _formatProvider);
			}
			else if (propertyToken.PropertyName == "Timestamp")
			{
				ScalarValue.Render(logEvent.Timestamp, textWriter, propertyToken.Format, _formatProvider);
			}
			else if (propertyToken.PropertyName == "Properties")
			{
				PropertiesOutputFormat.Render(logEvent.MessageTemplate, logEvent.Properties, _outputTemplate, textWriter, propertyToken.Format, _formatProvider);
			}
			else
			{
				if (!logEvent.Properties.TryGetValue(propertyToken.PropertyName, out LogEventPropertyValue value4))
				{
					continue;
				}
				if ((value4 as ScalarValue)?.Value is string value5)
				{
					string value6 = Casing.Format(value5, propertyToken.Format);
					textWriter.Write(value6);
				}
				else
				{
					value4.Render(textWriter, propertyToken.Format, _formatProvider);
				}
			}
			if (propertyToken.Alignment.HasValue)
			{
				string value7 = ((StringWriter)textWriter).ToString();
				Alignment? alignment = propertyToken.Alignment;
				Padding.Apply(output, value7, in alignment);
				textWriter.Dispose();
			}
		}
	}
}
