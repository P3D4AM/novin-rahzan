using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging;

internal sealed class Logger : ILogger
{
	private sealed class Scope : IDisposable
	{
		private bool _isDisposed;

		private IDisposable _disposable0;

		private IDisposable _disposable1;

		private readonly IDisposable[] _disposable;

		public Scope(int count)
		{
			if (count > 2)
			{
				_disposable = new IDisposable[count - 2];
			}
		}

		public void SetDisposable(int index, IDisposable disposable)
		{
			switch (index)
			{
			case 0:
				_disposable0 = disposable;
				break;
			case 1:
				_disposable1 = disposable;
				break;
			default:
				_disposable[index - 2] = disposable;
				break;
			}
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}
			_disposable0?.Dispose();
			_disposable1?.Dispose();
			if (_disposable != null)
			{
				int num = _disposable.Length;
				for (int i = 0; i != num; i++)
				{
					_disposable[i]?.Dispose();
				}
			}
			_isDisposed = true;
		}
	}

	public LoggerInformation[] Loggers { get; set; }

	public MessageLogger[] MessageLoggers { get; set; }

	public ScopeLogger[] ScopeLoggers { get; set; }

	public Logger(LoggerInformation[] loggers)
	{
		Loggers = loggers;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		MessageLogger[] messageLoggers = MessageLoggers;
		if (messageLoggers == null)
		{
			return;
		}
		List<Exception> exceptions2 = null;
		for (int i = 0; i < messageLoggers.Length; i++)
		{
			ref MessageLogger reference = ref messageLoggers[i];
			if (reference.IsEnabled(logLevel))
			{
				LoggerLog(logLevel, eventId, reference.Logger, exception, formatter, ref exceptions2, in state);
			}
		}
		if (exceptions2 != null && exceptions2.Count > 0)
		{
			ThrowLoggingError(exceptions2);
		}
		static void LoggerLog(LogLevel logLevel, EventId eventId, ILogger logger, Exception exception, Func<TState, Exception, string> formatter, ref List<Exception> exceptions, in TState state)
		{
			try
			{
				logger.Log(logLevel, eventId, state, exception, formatter);
			}
			catch (Exception item)
			{
				if (exceptions == null)
				{
					exceptions = new List<Exception>();
				}
				exceptions.Add(item);
			}
		}
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		MessageLogger[] messageLoggers = MessageLoggers;
		if (messageLoggers == null)
		{
			return false;
		}
		List<Exception> exceptions2 = null;
		int i;
		for (i = 0; i < messageLoggers.Length; i++)
		{
			ref MessageLogger reference = ref messageLoggers[i];
			if (reference.IsEnabled(logLevel) && LoggerIsEnabled(logLevel, reference.Logger, ref exceptions2))
			{
				break;
			}
		}
		if (exceptions2 != null && exceptions2.Count > 0)
		{
			ThrowLoggingError(exceptions2);
		}
		if (i >= messageLoggers.Length)
		{
			return false;
		}
		return true;
		static bool LoggerIsEnabled(LogLevel logLevel, ILogger logger, ref List<Exception> exceptions)
		{
			try
			{
				if (logger.IsEnabled(logLevel))
				{
					return true;
				}
			}
			catch (Exception item)
			{
				if (exceptions == null)
				{
					exceptions = new List<Exception>();
				}
				exceptions.Add(item);
			}
			return false;
		}
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		ScopeLogger[] scopeLoggers = ScopeLoggers;
		if (scopeLoggers == null)
		{
			return Microsoft.Extensions.Logging.NullScope.Instance;
		}
		if (scopeLoggers.Length == 1)
		{
			return scopeLoggers[0].CreateScope(state);
		}
		Scope scope = new Scope(scopeLoggers.Length);
		List<Exception> list = null;
		for (int i = 0; i < scopeLoggers.Length; i++)
		{
			ref ScopeLogger reference = ref scopeLoggers[i];
			try
			{
				scope.SetDisposable(i, reference.CreateScope(state));
			}
			catch (Exception item)
			{
				if (list == null)
				{
					list = new List<Exception>();
				}
				list.Add(item);
			}
		}
		if (list != null && list.Count > 0)
		{
			ThrowLoggingError(list);
		}
		return scope;
	}

	private static void ThrowLoggingError(List<Exception> exceptions)
	{
		throw new AggregateException("An error occurred while writing to logger(s).", exceptions);
	}
}
