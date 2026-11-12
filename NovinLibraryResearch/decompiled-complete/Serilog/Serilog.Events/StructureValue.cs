using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Serilog.Events;

public class StructureValue : LogEventPropertyValue
{
	private readonly LogEventProperty[] _properties;

	public string? TypeTag { get; }

	public IReadOnlyList<LogEventProperty> Properties => _properties;

	public StructureValue(IEnumerable<LogEventProperty> properties, string? typeTag = null)
	{
		Guard.AgainstNull(properties, "properties");
		TypeTag = typeTag;
		_properties = properties.ToArray();
	}

	public override void Render(TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
	{
		Guard.AgainstNull(output, "output");
		if (TypeTag != null)
		{
			output.Write(TypeTag);
			output.Write(' ');
		}
		output.Write("{ ");
		int num = _properties.Length - 1;
		for (int i = 0; i < num; i++)
		{
			LogEventProperty property = _properties[i];
			Render(output, property, formatProvider);
			output.Write(", ");
		}
		if (_properties.Length != 0)
		{
			LogEventProperty property2 = _properties[_properties.Length - 1];
			Render(output, property2, formatProvider);
		}
		output.Write(" }");
	}

	private static void Render(TextWriter output, LogEventProperty property, IFormatProvider? formatProvider = null)
	{
		output.Write(property.Name);
		output.Write(": ");
		property.Value.Render(output, null, formatProvider);
	}
}
