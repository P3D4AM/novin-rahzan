using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Serilog.Capturing;
using Serilog.Core.Enrichers;
using Serilog.Core.Pipeline;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Core;

public sealed class Logger : ILogger, ILogEventSink, IDisposable
{
	private static readonly object[] NoPropertyValues = Array.Empty<object>();

	private static readonly LogEventProperty[] NoProperties = Array.Empty<LogEventProperty>();

	private readonly MessageTemplateProcessor _messageTemplateProcessor;

	private readonly ILogEventSink _sink;

	private readonly Action? _dispose;

	private readonly ILogEventEnricher _enricher;

	private readonly LogEventLevel _minimumLevel;

	private readonly LoggingLevelSwitch? _levelSwitch;

	private readonly LevelOverrideMap? _overrideMap;

	internal bool HasOverrideMap => _overrideMap != null;

	public static ILogger None { get; } = new SilentLogger();


	internal Logger(MessageTemplateProcessor messageTemplateProcessor, LogEventLevel minimumLevel, LoggingLevelSwitch? levelSwitch, ILogEventSink sink, ILogEventEnricher enricher, Action? dispose, LevelOverrideMap? overrideMap)
	{
		_messageTemplateProcessor = messageTemplateProcessor;
		_minimumLevel = minimumLevel;
		_sink = sink;
		_dispose = dispose;
		_levelSwitch = levelSwitch;
		_overrideMap = overrideMap;
		_enricher = enricher;
	}

	public ILogger ForContext(ILogEventEnricher enricher)
	{
		if (enricher == null)
		{
			return this;
		}
		return new Logger(_messageTemplateProcessor, _minimumLevel, _levelSwitch, this, enricher, null, _overrideMap);
	}

	public ILogger ForContext(IEnumerable<ILogEventEnricher> enrichers)
	{
		if (enrichers == null)
		{
			return this;
		}
		return ForContext(new SafeAggregateEnricher(enrichers));
	}

	public ILogger ForContext(string propertyName, object? value, bool destructureObjects = false)
	{
		if (!LogEventProperty.IsValidName(propertyName))
		{
			SelfLog.WriteLine("Attempt to call ForContext() with invalid property name `{0}` (value: `{1}`)", propertyName, value);
			return this;
		}
		LogEventPropertyValue value2 = _messageTemplateProcessor.CreatePropertyValue(value, destructureObjects);
		EventProperty eventProperty = new EventProperty(propertyName, value2);
		FixedPropertyEnricher enricher = new FixedPropertyEnricher(in eventProperty);
		LogEventLevel minimumLevel;
		LoggingLevelSwitch levelSwitch;
		if (_overrideMap != null && propertyName == "SourceContext" && value is string context)
		{
			_overrideMap.GetEffectiveLevel(context, out minimumLevel, out levelSwitch);
		}
		else
		{
			minimumLevel = _minimumLevel;
			levelSwitch = _levelSwitch;
		}
		return new Logger(_messageTemplateProcessor, minimumLevel, levelSwitch, this, enricher, null, _overrideMap);
	}

	public ILogger ForContext(Type source)
	{
		if (source == null)
		{
			return this;
		}
		return ForContext("SourceContext", source.FullName);
	}

	public ILogger ForContext<TSource>()
	{
		return ForContext(typeof(TSource));
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Write(LogEventLevel level, string messageTemplate)
	{
		if (IsEnabled(level))
		{
			Write(level, messageTemplate, NoPropertyValues);
		}
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Write<T>(LogEventLevel level, string messageTemplate, T propertyValue)
	{
		if (IsEnabled(level))
		{
			Write(level, messageTemplate, new object[1] { propertyValue });
		}
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Write<T0, T1>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		if (IsEnabled(level))
		{
			Write(level, messageTemplate, new object[2] { propertyValue0, propertyValue1 });
		}
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Write<T0, T1, T2>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		if (IsEnabled(level))
		{
			Write(level, messageTemplate, new object[3] { propertyValue0, propertyValue1, propertyValue2 });
		}
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Write(LogEventLevel level, string messageTemplate, params object?[]? propertyValues)
	{
		Write(level, (Exception?)null, messageTemplate, propertyValues);
	}

	public bool IsEnabled(LogEventLevel level)
	{
		if (level < _minimumLevel)
		{
			return false;
		}
		if (_levelSwitch != null)
		{
			return level >= _levelSwitch.MinimumLevel;
		}
		return true;
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Write(LogEventLevel level, Exception? exception, string messageTemplate)
	{
		if (IsEnabled(level))
		{
			Write(level, exception, messageTemplate, NoPropertyValues);
		}
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Write<T>(LogEventLevel level, Exception? exception, string messageTemplate, T propertyValue)
	{
		if (IsEnabled(level))
		{
			Write(level, exception, messageTemplate, new object[1] { propertyValue });
		}
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Write<T0, T1>(LogEventLevel level, Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		if (IsEnabled(level))
		{
			Write(level, exception, messageTemplate, new object[2] { propertyValue0, propertyValue1 });
		}
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Write<T0, T1, T2>(LogEventLevel level, Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		if (IsEnabled(level))
		{
			Write(level, exception, messageTemplate, new object[3] { propertyValue0, propertyValue1, propertyValue2 });
		}
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Write(LogEventLevel level, Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		if (IsEnabled(level) && messageTemplate != null)
		{
			if (propertyValues != null && propertyValues.GetType() != typeof(object[]))
			{
				propertyValues = new object[1] { propertyValues };
			}
			DateTimeOffset now = DateTimeOffset.Now;
			_messageTemplateProcessor.Process(messageTemplate, propertyValues ?? NoPropertyValues, out MessageTemplate parsedTemplate, out EventProperty[] properties);
			Activity current = Activity.Current;
			LogEvent logEvent = new LogEvent(now, level, exception, parsedTemplate, properties, current?.TraceId ?? default(ActivityTraceId), current?.SpanId ?? default(ActivitySpanId));
			Dispatch(logEvent);
		}
	}

	public void Write(LogEvent logEvent)
	{
		if (logEvent != null && IsEnabled(logEvent.Level))
		{
			Dispatch(logEvent);
		}
	}

	void ILogEventSink.Emit(LogEvent logEvent)
	{
		Guard.AgainstNull(logEvent, "logEvent");
		Dispatch(logEvent);
	}

	private void Dispatch(LogEvent logEvent)
	{
		try
		{
			_enricher.Enrich(logEvent, _messageTemplateProcessor);
		}
		catch (Exception arg)
		{
			SelfLog.WriteLine("Exception {0} caught while enriching {1} with {2}.", arg, logEvent, _enricher);
		}
		_sink.Emit(logEvent);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Verbose(string messageTemplate)
	{
		Write(LogEventLevel.Verbose, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Verbose<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Verbose, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Verbose, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Verbose, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Verbose(string messageTemplate, params object?[]? propertyValues)
	{
		Verbose((Exception?)null, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Verbose(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Verbose, exception, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Verbose<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Verbose, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Verbose<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Verbose, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Verbose<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Verbose, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Verbose(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Write(LogEventLevel.Verbose, exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Debug(string messageTemplate)
	{
		Write(LogEventLevel.Debug, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Debug<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Debug, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Debug, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Debug, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Debug(string messageTemplate, params object?[]? propertyValues)
	{
		Debug((Exception?)null, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Debug(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Debug, exception, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Debug<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Debug, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Debug<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Debug, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Debug<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Debug, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Debug(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Write(LogEventLevel.Debug, exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Information(string messageTemplate)
	{
		Write(LogEventLevel.Information, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Information<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Information, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Information, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Information, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Information(string messageTemplate, params object?[]? propertyValues)
	{
		Information((Exception?)null, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Information(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Information, exception, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Information<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Information, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Information<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Information, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Information<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Information, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Information(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Write(LogEventLevel.Information, exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Warning(string messageTemplate)
	{
		Write(LogEventLevel.Warning, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Warning<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Warning, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Warning, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Warning, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Warning(string messageTemplate, params object?[]? propertyValues)
	{
		Warning((Exception?)null, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Warning(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Warning, exception, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Warning<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Warning, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Warning<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Warning, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Warning<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Warning, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Warning(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Write(LogEventLevel.Warning, exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Error(string messageTemplate)
	{
		Write(LogEventLevel.Error, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Error<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Error, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Error, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Error, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Error(string messageTemplate, params object?[]? propertyValues)
	{
		Error((Exception?)null, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Error(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Error, exception, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Error<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Error, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Error<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Error, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Error<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Error, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Error(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Write(LogEventLevel.Error, exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Fatal(string messageTemplate)
	{
		Write(LogEventLevel.Fatal, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Fatal<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Fatal, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Fatal, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Fatal, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Fatal(string messageTemplate, params object?[]? propertyValues)
	{
		Fatal((Exception?)null, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Fatal(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Fatal, exception, messageTemplate, NoPropertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Fatal<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Fatal, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Fatal<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Fatal, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Fatal<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Fatal, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public void Fatal(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Write(LogEventLevel.Fatal, exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public bool BindMessageTemplate(string messageTemplate, object?[]? propertyValues, [NotNullWhen(true)] out MessageTemplate? parsedTemplate, [NotNullWhen(true)] out IEnumerable<LogEventProperty>? boundProperties)
	{
		if (messageTemplate == null)
		{
			parsedTemplate = null;
			boundProperties = null;
			return false;
		}
		_messageTemplateProcessor.Process(messageTemplate, propertyValues ?? NoPropertyValues, out parsedTemplate, out EventProperty[] properties);
		IEnumerable<LogEventProperty> enumerable;
		if (properties.Length != 0)
		{
			enumerable = properties.Select((EventProperty p) => new LogEventProperty(p));
		}
		else
		{
			IEnumerable<LogEventProperty> noProperties = NoProperties;
			enumerable = noProperties;
		}
		boundProperties = enumerable;
		return true;
	}

	public bool BindProperty(string? propertyName, object? value, bool destructureObjects, [NotNullWhen(true)] out LogEventProperty? property)
	{
		if (!LogEventProperty.IsValidName(propertyName))
		{
			property = null;
			return false;
		}
		property = _messageTemplateProcessor.CreateProperty(propertyName, value, destructureObjects);
		return true;
	}

	public void Dispose()
	{
		_dispose?.Invoke();
	}
}
