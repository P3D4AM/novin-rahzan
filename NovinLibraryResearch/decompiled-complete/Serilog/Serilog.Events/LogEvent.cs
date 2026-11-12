using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Serilog.Core;

namespace Serilog.Events;

public class LogEvent
{
	private readonly Dictionary<string, LogEventPropertyValue> _properties;

	private ActivityTraceId _traceId;

	private ActivitySpanId _spanId;

	public DateTimeOffset Timestamp { get; }

	public LogEventLevel Level { get; }

	[CLSCompliant(false)]
	public ActivityTraceId? TraceId
	{
		get
		{
			if (!(_traceId == default(ActivityTraceId)))
			{
				return _traceId;
			}
			return null;
		}
	}

	[CLSCompliant(false)]
	public ActivitySpanId? SpanId
	{
		get
		{
			if (!(_spanId == default(ActivitySpanId)))
			{
				return _spanId;
			}
			return null;
		}
	}

	public MessageTemplate MessageTemplate { get; }

	public IReadOnlyDictionary<string, LogEventPropertyValue> Properties => _properties;

	public Exception? Exception { get; }

	private LogEvent(DateTimeOffset timestamp, LogEventLevel level, Exception? exception, MessageTemplate messageTemplate, Dictionary<string, LogEventPropertyValue> properties, ActivityTraceId traceId, ActivitySpanId spanId)
	{
		Timestamp = timestamp;
		Level = level;
		Exception = exception;
		_traceId = traceId;
		_spanId = spanId;
		MessageTemplate = Guard.AgainstNull(messageTemplate, "messageTemplate");
		_properties = Guard.AgainstNull(properties, "properties");
	}

	public LogEvent(DateTimeOffset timestamp, LogEventLevel level, Exception? exception, MessageTemplate messageTemplate, IEnumerable<LogEventProperty> properties)
		: this(timestamp, level, exception, messageTemplate, properties, default(ActivityTraceId), default(ActivitySpanId))
	{
	}

	[CLSCompliant(false)]
	public LogEvent(DateTimeOffset timestamp, LogEventLevel level, Exception? exception, MessageTemplate messageTemplate, IEnumerable<LogEventProperty> properties, ActivityTraceId traceId, ActivitySpanId spanId)
		: this(timestamp, level, exception, messageTemplate, new Dictionary<string, LogEventPropertyValue>(), traceId, spanId)
	{
		Guard.AgainstNull(properties, "properties");
		foreach (LogEventProperty property in properties)
		{
			AddOrUpdateProperty(property);
		}
	}

	internal LogEvent(DateTimeOffset timestamp, LogEventLevel level, Exception? exception, MessageTemplate messageTemplate, EventProperty[] properties, ActivityTraceId traceId, ActivitySpanId spanId)
		: this(timestamp, level, exception, messageTemplate, new Dictionary<string, LogEventPropertyValue>(Guard.AgainstNull(properties, "properties").Length), traceId, spanId)
	{
		for (int i = 0; i < properties.Length; i++)
		{
			_properties[properties[i].Name] = properties[i].Value;
		}
	}

	public void RenderMessage(TextWriter output, IFormatProvider? formatProvider = null)
	{
		MessageTemplate.Render(Properties, output, formatProvider);
	}

	public string RenderMessage(IFormatProvider? formatProvider = null)
	{
		return MessageTemplate.Render(Properties, formatProvider);
	}

	public void AddOrUpdateProperty(LogEventProperty property)
	{
		Guard.AgainstNull(property, "property");
		_properties[property.Name] = property.Value;
	}

	internal void AddOrUpdateProperty(in EventProperty property)
	{
		if (property.Equals(EventProperty.None))
		{
			throw new ArgumentNullException("property");
		}
		_properties[property.Name] = property.Value;
	}

	public void AddPropertyIfAbsent(LogEventProperty property)
	{
		Guard.AgainstNull(property, "property");
		if (!_properties.ContainsKey(property.Name))
		{
			_properties.Add(property.Name, property.Value);
		}
	}

	internal void AddPropertyIfAbsent(in EventProperty property)
	{
		if (property.Equals(EventProperty.None))
		{
			throw new ArgumentNullException("property");
		}
		if (!_properties.ContainsKey(property.Name))
		{
			_properties.Add(property.Name, property.Value);
		}
	}

	internal void AddPropertyIfAbsent(ILogEventPropertyFactory factory, string name, object? value, bool destructureObjects = false)
	{
		if (!_properties.ContainsKey(name))
		{
			_properties.Add(name, (factory is ILogEventPropertyValueFactory logEventPropertyValueFactory) ? logEventPropertyValueFactory.CreatePropertyValue(value, destructureObjects) : factory.CreateProperty(name, value, destructureObjects).Value);
		}
	}

	public void RemovePropertyIfPresent(string propertyName)
	{
		_properties.Remove(propertyName);
	}

	internal LogEvent Copy()
	{
		Dictionary<string, LogEventPropertyValue> properties = new Dictionary<string, LogEventPropertyValue>(_properties);
		return new LogEvent(Timestamp, Level, Exception, MessageTemplate, properties, TraceId.GetValueOrDefault(), SpanId.GetValueOrDefault());
	}
}
