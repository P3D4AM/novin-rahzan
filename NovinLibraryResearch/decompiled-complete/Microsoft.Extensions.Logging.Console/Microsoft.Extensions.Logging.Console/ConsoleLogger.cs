using System;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.Extensions.Logging.Console;

[UnsupportedOSPlatform("browser")]
internal sealed class ConsoleLogger : ILogger
{
	private readonly string _name;

	private readonly ConsoleLoggerProcessor _queueProcessor;

	[ThreadStatic]
	private static StringWriter t_stringWriter;

	internal ConsoleFormatter Formatter { get; set; }

	internal IExternalScopeProvider ScopeProvider { get; set; }

	internal ConsoleLoggerOptions Options { get; set; }

	internal ConsoleLogger(string name, ConsoleLoggerProcessor loggerProcessor)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		_name = name;
		_queueProcessor = loggerProcessor;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		if (!IsEnabled(logLevel))
		{
			return;
		}
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		if (t_stringWriter == null)
		{
			t_stringWriter = new StringWriter();
		}
		LogEntry<TState> logEntry = new LogEntry<TState>(logLevel, _name, eventId, state, exception, formatter);
		Formatter.Write(in logEntry, ScopeProvider, t_stringWriter);
		StringBuilder stringBuilder = t_stringWriter.GetStringBuilder();
		if (stringBuilder.Length != 0)
		{
			string message = stringBuilder.ToString();
			stringBuilder.Clear();
			if (stringBuilder.Capacity > 1024)
			{
				stringBuilder.Capacity = 1024;
			}
			_queueProcessor.EnqueueMessage(new LogMessageEntry(message, logLevel >= Options.LogToStandardErrorThreshold));
		}
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return logLevel != LogLevel.None;
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return ScopeProvider?.Push(state) ?? Microsoft.Extensions.Logging.NullScope.Instance;
	}
}
