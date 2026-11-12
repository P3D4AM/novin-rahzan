using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Serilog.Formatting.Display;

internal static class PropertiesOutputFormat
{
	private static readonly JsonValueFormatter JsonValueFormatter = new JsonValueFormatter("$type");

	public static void Render(MessageTemplate template, IReadOnlyDictionary<string, LogEventPropertyValue> properties, MessageTemplate outputTemplate, TextWriter output, string? format, IFormatProvider? formatProvider = null)
	{
		MessageTemplate template2 = template;
		MessageTemplate outputTemplate2 = outputTemplate;
		if (format != null && format.Contains("j"))
		{
			StructureValue value = new StructureValue(from kvp in properties
				where !TemplateContainsPropertyName(template2, kvp.Key) && !TemplateContainsPropertyName(outputTemplate2, kvp.Key)
				select new LogEventProperty(kvp.Key, kvp.Value));
			JsonValueFormatter.Format(value, output);
			return;
		}
		output.Write("{ ");
		string value2 = "";
		foreach (KeyValuePair<string, LogEventPropertyValue> property in properties)
		{
			if (!TemplateContainsPropertyName(template2, property.Key) && !TemplateContainsPropertyName(outputTemplate2, property.Key))
			{
				output.Write(value2);
				value2 = ", ";
				output.Write(property.Key);
				output.Write(": ");
				property.Value.Render(output, null, formatProvider);
			}
		}
		output.Write(" }");
	}

	private static bool TemplateContainsPropertyName(MessageTemplate template, string propertyName)
	{
		if (template.PositionalProperties != null)
		{
			for (int i = 0; i < template.PositionalProperties.Length; i++)
			{
				if (template.PositionalProperties[i].PropertyName == propertyName)
				{
					return true;
				}
			}
			return false;
		}
		if (template.NamedProperties != null)
		{
			for (int j = 0; j < template.NamedProperties.Length; j++)
			{
				if (template.NamedProperties[j].PropertyName == propertyName)
				{
					return true;
				}
			}
		}
		return false;
	}
}
