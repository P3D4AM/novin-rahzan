using System;
using Microsoft.Extensions.Logging;
using PacketParser.Entities;

namespace PacketParser;

public class NpLogger : Microsoft.Extensions.Logging.ILogger
{
	private readonly string _name;

	private readonly LogLevel _logLevel;

	public NpLogger(string name, LogLevel logLevel)
	{
		_name = name;
		_logLevel = logLevel;
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return null;
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return logLevel >= _logLevel;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		if (IsEnabled(logLevel))
		{
			if (exception == null)
			{
				exception = new Exception(formatter(state, exception));
			}
			WebErrorLog.Instance.StartLog(exception, formatter(state, exception), logLevel, "", "TState", 0);
		}
	}
}
