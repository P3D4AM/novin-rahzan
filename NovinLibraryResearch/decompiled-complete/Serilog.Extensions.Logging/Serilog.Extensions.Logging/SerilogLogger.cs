using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Extensions.Logging;

internal class SerilogLogger : Microsoft.Extensions.Logging.ILogger
{
	internal static readonly ConcurrentDictionary<string, string> DestructureDictionary = new ConcurrentDictionary<string, string>();

	internal static readonly ConcurrentDictionary<string, string> StringifyDictionary = new ConcurrentDictionary<string, string>();

	private readonly SerilogLoggerProvider _provider;

	private readonly ILogger _logger;

	private static readonly CachingMessageTemplateParser MessageTemplateParser = new CachingMessageTemplateParser();

	private static readonly LogEventProperty[] LowEventIdValues = (from n in Enumerable.Range(0, 48)
		select new LogEventProperty("Id", new ScalarValue(n))).ToArray();

	internal static string GetKeyWithoutFirstSymbol(ConcurrentDictionary<string, string> source, string key)
	{
		if (source.TryGetValue(key, out string value))
		{
			return value;
		}
		if (source.Count < 1000)
		{
			return source.GetOrAdd(key, (string k) => k.Substring(1));
		}
		return key.Substring(1);
	}

	public SerilogLogger(SerilogLoggerProvider provider, ILogger? logger = null, string? name = null)
	{
		_provider = provider ?? throw new ArgumentNullException("provider");
		_logger = logger ?? Serilog.Log.Logger.ForContext(new SerilogLoggerProvider[1] { provider });
		if (name != null)
		{
			_logger = _logger.ForContext("SourceContext", name);
		}
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		if (logLevel != LogLevel.None)
		{
			return _logger.IsEnabled(LevelConvert.ToSerilogLevel(logLevel));
		}
		return false;
	}

	public IDisposable BeginScope<TState>(TState state) where TState : notnull
	{
		return _provider.BeginScope(state);
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
	{
		if (logLevel == LogLevel.None)
		{
			return;
		}
		LogEventLevel level = LevelConvert.ToSerilogLevel(logLevel);
		if (_logger.IsEnabled(level))
		{
			LogEvent logEvent = null;
			try
			{
				logEvent = PrepareWrite(level, eventId, state, exception, formatter);
			}
			catch (Exception arg)
			{
				SelfLog.WriteLine(string.Format("Failed to write event through {0}: {1}", "SerilogLogger", arg));
			}
			if (logEvent != null)
			{
				_logger.Write(logEvent);
			}
		}
	}

	private LogEvent PrepareWrite<TState>(LogEventLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
	{
		string text = null;
		List<LogEventProperty> list = new List<LogEventProperty>();
		if (state is IEnumerable<KeyValuePair<string, object>> enumerable)
		{
			foreach (KeyValuePair<string, object> item in enumerable)
			{
				LogEventProperty property3;
				if (item.Key == "{OriginalFormat}" && item.Value is string text2)
				{
					text = text2;
				}
				else if (item.Key.StartsWith("@"))
				{
					if (_logger.BindProperty(GetKeyWithoutFirstSymbol(DestructureDictionary, item.Key), item.Value, destructureObjects: true, out LogEventProperty property))
					{
						list.Add(property);
					}
				}
				else if (item.Key.StartsWith("$"))
				{
					if (_logger.BindProperty(GetKeyWithoutFirstSymbol(StringifyDictionary, item.Key), item.Value?.ToString(), destructureObjects: true, out LogEventProperty property2))
					{
						list.Add(property2);
					}
				}
				else if (_logger.BindProperty(item.Key, item.Value, destructureObjects: false, out property3))
				{
					list.Add(property3);
				}
			}
			Type type = state.GetType();
			TypeInfo typeInfo = type.GetTypeInfo();
			if (text == null && !typeInfo.IsGenericType)
			{
				text = "{" + type.Name + ":l}";
				if (_logger.BindProperty(type.Name, AsLoggableValue(state, formatter), destructureObjects: false, out LogEventProperty property4))
				{
					list.Add(property4);
				}
			}
		}
		if (text == null)
		{
			string text3 = null;
			if (state != null)
			{
				text3 = "State";
				text = "{State:l}";
			}
			else if (formatter != null)
			{
				text3 = "Message";
				text = "{Message:l}";
			}
			if (text3 != null && _logger.BindProperty(text3, AsLoggableValue(state, formatter), destructureObjects: false, out LogEventProperty property5))
			{
				list.Add(property5);
			}
		}
		if (eventId.Id != 0 || eventId.Name != null)
		{
			list.Add(CreateEventIdProperty(eventId));
		}
		MessageTemplate messageTemplate = MessageTemplateParser.Parse(text ?? "");
		return new LogEvent(DateTimeOffset.Now, level, exception, messageTemplate, list);
	}

	private static object? AsLoggableValue<TState>(TState state, Func<TState, Exception?, string>? formatter)
	{
		object result = state;
		if (formatter != null)
		{
			result = formatter(state, null);
		}
		return result;
	}

	internal static LogEventProperty CreateEventIdProperty(EventId eventId)
	{
		List<LogEventProperty> list = new List<LogEventProperty>(2);
		if (eventId.Id != 0)
		{
			if (eventId.Id >= 0 && eventId.Id < LowEventIdValues.Length)
			{
				list.Add(LowEventIdValues[eventId.Id]);
			}
			else
			{
				list.Add(new LogEventProperty("Id", new ScalarValue(eventId.Id)));
			}
		}
		if (eventId.Name != null)
		{
			list.Add(new LogEventProperty("Name", new ScalarValue(eventId.Name)));
		}
		return new LogEventProperty("EventId", new StructureValue(list));
	}
}
