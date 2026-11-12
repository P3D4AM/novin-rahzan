using System;
using System.Collections;
using System.Collections.Generic;
using Serilog.Events;

namespace Serilog.Extensions.Logging;

internal readonly struct SerilogLogValues : IReadOnlyList<KeyValuePair<string, object?>>, IReadOnlyCollection<KeyValuePair<string, object?>>, IEnumerable<KeyValuePair<string, object?>>, IEnumerable
{
	private readonly MessageTemplate _messageTemplate;

	private readonly IReadOnlyDictionary<string, LogEventPropertyValue> _properties;

	private readonly KeyValuePair<string, object?>[] _values;

	public KeyValuePair<string, object?> this[int index] => _values[index];

	public int Count => _properties.Count + 1;

	public SerilogLogValues(MessageTemplate messageTemplate, IReadOnlyDictionary<string, LogEventPropertyValue> properties)
	{
		_messageTemplate = messageTemplate ?? throw new ArgumentNullException("messageTemplate");
		_properties = properties ?? throw new ArgumentNullException("properties");
		_values = new KeyValuePair<string, object>[_properties.Count + 1];
		int num = 0;
		foreach (KeyValuePair<string, LogEventPropertyValue> property in properties)
		{
			_values[num] = new KeyValuePair<string, object>(property.Key, (property.Value is ScalarValue scalarValue) ? scalarValue.Value : property.Value);
			num++;
		}
		_values[num] = new KeyValuePair<string, object>("{OriginalFormat}", _messageTemplate.Text);
	}

	public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
	{
		return ((IEnumerable<KeyValuePair<string, object>>)_values).GetEnumerator();
	}

	public override string ToString()
	{
		return _messageTemplate.Render(_properties);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _values.GetEnumerator();
	}
}
