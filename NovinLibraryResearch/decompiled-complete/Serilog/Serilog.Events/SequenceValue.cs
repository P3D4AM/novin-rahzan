using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Serilog.Events;

public class SequenceValue : LogEventPropertyValue
{
	private readonly LogEventPropertyValue[] _elements;

	public static SequenceValue Empty { get; } = new SequenceValue(Array.Empty<LogEventPropertyValue>());


	public IReadOnlyList<LogEventPropertyValue> Elements => _elements;

	public SequenceValue(IEnumerable<LogEventPropertyValue> elements)
	{
		Guard.AgainstNull(elements, "elements");
		_elements = elements.ToArray();
	}

	internal SequenceValue(LogEventPropertyValue[] elements)
	{
		Guard.AgainstNull(elements, "elements");
		_elements = elements;
	}

	public override void Render(TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
	{
		Guard.AgainstNull(output, "output");
		output.Write('[');
		int num = _elements.Length - 1;
		for (int i = 0; i < num; i++)
		{
			_elements[i].Render(output, format, formatProvider);
			output.Write(", ");
		}
		if (_elements.Length != 0)
		{
			_elements[_elements.Length - 1].Render(output, format, formatProvider);
		}
		output.Write(']');
	}
}
