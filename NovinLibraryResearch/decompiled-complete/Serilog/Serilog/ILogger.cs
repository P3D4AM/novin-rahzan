using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Serilog.Core;
using Serilog.Events;

namespace Serilog;

public interface ILogger
{
	ILogger ForContext(ILogEventEnricher enricher);

	ILogger ForContext(IEnumerable<ILogEventEnricher> enrichers);

	ILogger ForContext(string propertyName, object? value, bool destructureObjects = false);

	ILogger ForContext<TSource>();

	ILogger ForContext(Type source);

	void Write(LogEvent logEvent);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Write(LogEventLevel level, string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Write<T>(LogEventLevel level, string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Write<T0, T1>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Write<T0, T1, T2>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Write(LogEventLevel level, string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Write(LogEventLevel level, Exception? exception, string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Write<T>(LogEventLevel level, Exception? exception, string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Write<T0, T1>(LogEventLevel level, Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Write<T0, T1, T2>(LogEventLevel level, Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Write(LogEventLevel level, Exception? exception, string messageTemplate, params object?[]? propertyValues);

	bool IsEnabled(LogEventLevel level);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Verbose(string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Verbose<T>(string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Verbose(string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Verbose(Exception? exception, string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Verbose<T>(Exception? exception, string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Verbose<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Verbose<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Verbose(Exception? exception, string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Debug(string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Debug<T>(string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Debug(string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Debug(Exception? exception, string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Debug<T>(Exception? exception, string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Debug<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Debug<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Debug(Exception? exception, string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Information(string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Information<T>(string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Information(string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Information(Exception? exception, string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Information<T>(Exception? exception, string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Information<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Information<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Information(Exception? exception, string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Warning(string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Warning<T>(string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Warning(string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Warning(Exception? exception, string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Warning<T>(Exception? exception, string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Warning<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Warning<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Warning(Exception? exception, string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Error(string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Error<T>(string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Error(string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Error(Exception? exception, string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Error<T>(Exception? exception, string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Error<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Error<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Error(Exception? exception, string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Fatal(string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Fatal<T>(string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Fatal(string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Fatal(Exception? exception, string messageTemplate);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Fatal<T>(Exception? exception, string messageTemplate, T propertyValue);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Fatal<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Fatal<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

	[MessageTemplateFormatMethod("messageTemplate")]
	void Fatal(Exception? exception, string messageTemplate, params object?[]? propertyValues);

	[MessageTemplateFormatMethod("messageTemplate")]
	bool BindMessageTemplate(string messageTemplate, object?[]? propertyValues, [NotNullWhen(true)] out MessageTemplate? parsedTemplate, [NotNullWhen(true)] out IEnumerable<LogEventProperty>? boundProperties);

	bool BindProperty(string? propertyName, object? value, bool destructureObjects, [NotNullWhen(true)] out LogEventProperty? property);
}
