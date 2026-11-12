using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Serilog.Core;
using Serilog.Events;

namespace Serilog;

public static class Log
{
	private static ILogger _logger = Serilog.Core.Logger.None;

	public static ILogger Logger
	{
		get
		{
			return _logger;
		}
		set
		{
			_logger = Guard.AgainstNull(value, "value");
		}
	}

	public static void CloseAndFlush()
	{
		(Interlocked.Exchange(ref _logger, Serilog.Core.Logger.None) as IDisposable)?.Dispose();
	}

	public static ILogger ForContext(ILogEventEnricher enricher)
	{
		return Logger.ForContext(enricher);
	}

	public static ILogger ForContext(ILogEventEnricher[] enrichers)
	{
		return Logger.ForContext(enrichers);
	}

	public static ILogger ForContext(string propertyName, object? value, bool destructureObjects = false)
	{
		return Logger.ForContext(propertyName, value, destructureObjects);
	}

	public static ILogger ForContext<TSource>()
	{
		return Logger.ForContext<TSource>();
	}

	public static ILogger ForContext(Type source)
	{
		return Logger.ForContext(source);
	}

	public static void Write(LogEvent logEvent)
	{
		Logger.Write(logEvent);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Write(LogEventLevel level, string messageTemplate)
	{
		Logger.Write(level, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Write<T>(LogEventLevel level, string messageTemplate, T propertyValue)
	{
		Logger.Write(level, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Write<T0, T1>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Logger.Write(level, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Write<T0, T1, T2>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Logger.Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Write(LogEventLevel level, string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Write(level, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Write(LogEventLevel level, Exception? exception, string messageTemplate)
	{
		Logger.Write(level, exception, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Write<T>(LogEventLevel level, Exception? exception, string messageTemplate, T propertyValue)
	{
		Logger.Write(level, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Write<T0, T1>(LogEventLevel level, Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Write<T0, T1, T2>(LogEventLevel level, Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Write(LogEventLevel level, Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Write(level, exception, messageTemplate, propertyValues);
	}

	public static bool IsEnabled(LogEventLevel level)
	{
		return Logger.IsEnabled(level);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Verbose(string messageTemplate)
	{
		Write(LogEventLevel.Verbose, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Verbose<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Verbose, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Verbose, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Verbose, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Verbose(string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Verbose(messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Verbose(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Verbose, exception, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Verbose<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Verbose, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Verbose<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Verbose, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Verbose<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Verbose, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Verbose(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Verbose(exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Debug(string messageTemplate)
	{
		Write(LogEventLevel.Debug, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Debug<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Debug, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Debug, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Debug, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Debug(string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Debug(messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Debug(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Debug, exception, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Debug<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Debug, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Debug<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Debug, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Debug<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Debug, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Debug(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Debug(exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Information(string messageTemplate)
	{
		Write(LogEventLevel.Information, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Information<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Information, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Information, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Information, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Information(string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Information(messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Information(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Information, exception, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Information<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Information, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Information<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Information, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Information<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Information, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Information(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Information(exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Warning(string messageTemplate)
	{
		Write(LogEventLevel.Warning, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Warning<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Warning, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Warning, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Warning, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Warning(string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Warning(messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Warning(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Warning, exception, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Warning<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Warning, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Warning<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Warning, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Warning<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Warning, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Warning(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Warning(exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Error(string messageTemplate)
	{
		Write(LogEventLevel.Error, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Error<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Error, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Error, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Error, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Error(string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Error(messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Error(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Error, exception, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Error<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Error, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Error<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Error, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Error<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Error, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Error(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Error(exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Fatal(string messageTemplate)
	{
		Write(LogEventLevel.Fatal, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Fatal<T>(string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Fatal, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Fatal, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Fatal, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Fatal(string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Fatal(messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Fatal(Exception? exception, string messageTemplate)
	{
		Write(LogEventLevel.Fatal, exception, messageTemplate);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Fatal<T>(Exception? exception, string messageTemplate, T propertyValue)
	{
		Write(LogEventLevel.Fatal, exception, messageTemplate, propertyValue);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Fatal<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
	{
		Write(LogEventLevel.Fatal, exception, messageTemplate, propertyValue0, propertyValue1);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Fatal<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
	{
		Write(LogEventLevel.Fatal, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static void Fatal(Exception? exception, string messageTemplate, params object?[]? propertyValues)
	{
		Logger.Fatal(exception, messageTemplate, propertyValues);
	}

	[MessageTemplateFormatMethod("messageTemplate")]
	public static bool BindMessageTemplate(string messageTemplate, object?[] propertyValues, [NotNullWhen(true)] out MessageTemplate? parsedTemplate, [NotNullWhen(true)] out IEnumerable<LogEventProperty>? boundProperties)
	{
		return Logger.BindMessageTemplate(messageTemplate, propertyValues, out parsedTemplate, out boundProperties);
	}

	public static bool BindProperty(string propertyName, object? value, bool destructureObjects, [NotNullWhen(true)] out LogEventProperty? property)
	{
		return Logger.BindProperty(propertyName, value, destructureObjects, out property);
	}
}
