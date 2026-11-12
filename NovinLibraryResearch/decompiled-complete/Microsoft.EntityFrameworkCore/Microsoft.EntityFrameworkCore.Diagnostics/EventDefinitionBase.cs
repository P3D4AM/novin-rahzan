using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public abstract class EventDefinitionBase
{
	internal sealed class MessageExtractingLogger : ILogger
	{
		private string _message;

		public string Message
		{
			get
			{
				return _message ?? throw new InvalidOperationException();
			}
			private set
			{
				_message = value;
			}
		}

		void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.CanBeNull] Exception exception, [JetBrains.Annotations.NotNull] Func<TState, Exception, string> formatter)
		{
			Message = formatter(state, exception);
		}

		bool ILogger.IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		IDisposable ILogger.BeginScope<TState>([JetBrains.Annotations.CanBeNull] TState state)
		{
			throw new NotImplementedException();
		}
	}

	private readonly WarningBehavior _warningBehavior;

	public virtual EventId EventId
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual LogLevel Level
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual string EventIdCode { get; }

	protected EventDefinitionBase([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, EventId eventId, LogLevel level, [JetBrains.Annotations.NotNull] string eventIdCode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(loggingOptions, "loggingOptions");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(eventIdCode, "eventIdCode");
		EventId = eventId;
		EventIdCode = eventIdCode;
		WarningsConfiguration warningsConfiguration = loggingOptions.WarningsConfiguration;
		if (warningsConfiguration != null)
		{
			LogLevel? level2 = warningsConfiguration.GetLevel(eventId);
			if (level2.HasValue)
			{
				level = level2.Value;
			}
			_warningBehavior = (WarningBehavior)(((int?)warningsConfiguration.GetBehavior(eventId)) ?? ((level == LogLevel.Warning && warningsConfiguration.DefaultBehavior == WarningBehavior.Throw) ? 2 : 0));
		}
		else
		{
			_warningBehavior = WarningBehavior.Log;
		}
		Level = level;
	}

	protected virtual Exception WarningAsError([JetBrains.Annotations.NotNull] string message)
	{
		return new InvalidOperationException(CoreStrings.WarningAsErrorTemplate(EventId.ToString(), message, EventIdCode));
	}

	public virtual WarningBehavior GetLogBehavior<TLoggerCategory>([JetBrains.Annotations.NotNull] IDiagnosticsLogger<TLoggerCategory> logger) where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
	{
		if (_warningBehavior != 0)
		{
			return _warningBehavior;
		}
		if (!logger.Logger.IsEnabled(Level))
		{
			return WarningBehavior.Ignore;
		}
		return WarningBehavior.Log;
	}
}
