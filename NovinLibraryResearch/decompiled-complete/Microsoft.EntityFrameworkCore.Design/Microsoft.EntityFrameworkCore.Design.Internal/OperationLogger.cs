using System;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class OperationLogger : ILogger
{
	private readonly string _categoryName;

	private readonly IOperationReporter _reporter;

	public OperationLogger([JetBrains.Annotations.NotNull] string categoryName, [JetBrains.Annotations.NotNull] IOperationReporter reporter)
	{
		_categoryName = categoryName;
		_reporter = reporter;
	}

	public virtual bool IsEnabled(LogLevel logLevel)
	{
		return true;
	}

	public virtual IDisposable BeginScope<TState>(TState state)
	{
		return null;
	}

	public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		if (_categoryName == LoggerCategory<DbLoggerCategory.Database.Command>.Name && eventId.Id == RelationalEventId.CommandExecuted.Id)
		{
			logLevel = LogLevel.Debug;
		}
		string message = GetMessage(state, exception, formatter);
		switch (logLevel)
		{
		case LogLevel.Error:
		case LogLevel.Critical:
			_reporter.WriteError(message);
			break;
		case LogLevel.Warning:
			_reporter.WriteWarning(message);
			break;
		case LogLevel.Information:
			_reporter.WriteInformation(message);
			break;
		default:
			_reporter.WriteVerbose(message);
			break;
		}
	}

	private static string GetMessage<TState>(TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (formatter != null)
		{
			stringBuilder.Append(formatter(state, exception));
		}
		else if (state != null)
		{
			stringBuilder.Append(state);
			if (exception != null)
			{
				stringBuilder.AppendLine().Append(exception);
			}
		}
		return stringBuilder.ToString();
	}
}
