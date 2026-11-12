#define DEBUG
using System;
using System.Diagnostics;

namespace Microsoft.Extensions.Logging.Debug;

internal sealed class DebugLogger : ILogger
{
	private readonly string _name;

	public DebugLogger(string name)
	{
		_name = name;
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return Microsoft.Extensions.Logging.NullScope.Instance;
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		if (Debugger.IsAttached)
		{
			return logLevel != LogLevel.None;
		}
		return false;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		if (!IsEnabled(logLevel))
		{
			return;
		}
		System.ThrowHelper.ThrowIfNull(formatter, "formatter");
		string text = formatter(state, exception);
		if (!string.IsNullOrEmpty(text))
		{
			text = $"{logLevel}: {text}";
			if (exception != null)
			{
				text = text + Environment.NewLine + Environment.NewLine + exception;
			}
			DebugWriteLine(text, _name);
		}
	}

	private static void DebugWriteLine(string message, string name)
	{
		System.Diagnostics.Debug.WriteLine(message, name);
	}
}
