using System;
using System.Diagnostics;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.Logging;

[DebuggerDisplay("{DebuggerToString(),nq}")]
public class Logger<T> : ILogger<T>, ILogger
{
	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	private readonly ILogger _logger;

	public Logger(ILoggerFactory factory)
	{
		System.ThrowHelper.ThrowIfNull(factory, "factory");
		_logger = factory.CreateLogger(GetCategoryName());
	}

	IDisposable ILogger.BeginScope<TState>(TState state)
	{
		return _logger.BeginScope(state);
	}

	bool ILogger.IsEnabled(LogLevel logLevel)
	{
		return _logger.IsEnabled(logLevel);
	}

	void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		_logger.Log(logLevel, eventId, state, exception, formatter);
	}

	private static string GetCategoryName()
	{
		return TypeNameHelper.GetTypeDisplayName(typeof(T), fullName: true, includeGenericParameterNames: false, includeGenericParameters: false, '.');
	}

	internal string DebuggerToString()
	{
		return DebuggerDisplayFormatting.DebuggerToString(GetCategoryName(), this);
	}
}
