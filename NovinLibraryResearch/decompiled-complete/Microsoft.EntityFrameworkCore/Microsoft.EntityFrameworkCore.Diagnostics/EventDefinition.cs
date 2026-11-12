using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class EventDefinition : EventDefinitionBase
{
	private readonly Action<ILogger, Exception> _logAction;

	public EventDefinition([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, EventId eventId, LogLevel level, [JetBrains.Annotations.NotNull] string eventIdCode, [JetBrains.Annotations.NotNull] Func<LogLevel, Action<ILogger, Exception>> logActionFunc)
		: base(loggingOptions, eventId, level, eventIdCode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logActionFunc, "logActionFunc");
		_logAction = logActionFunc(Level);
	}

	public virtual string GenerateMessage()
	{
		MessageExtractingLogger messageExtractingLogger = new MessageExtractingLogger();
		_logAction(messageExtractingLogger, null);
		return messageExtractingLogger.Message;
	}

	public virtual void Log<TLoggerCategory>([JetBrains.Annotations.NotNull] IDiagnosticsLogger<TLoggerCategory> logger, WarningBehavior warningBehavior, [JetBrains.Annotations.CanBeNull] Exception exception = null) where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
	{
		switch (warningBehavior)
		{
		case WarningBehavior.Log:
			_logAction(logger.Logger, null);
			break;
		case WarningBehavior.Throw:
			throw WarningAsError(GenerateMessage());
		}
	}
}
public class EventDefinition<TParam> : EventDefinitionBase
{
	private readonly Action<ILogger, TParam, Exception> _logAction;

	public EventDefinition([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, EventId eventId, LogLevel level, [JetBrains.Annotations.NotNull] string eventIdCode, [JetBrains.Annotations.NotNull] Func<LogLevel, Action<ILogger, TParam, Exception>> logActionFunc)
		: base(loggingOptions, eventId, level, eventIdCode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logActionFunc, "logActionFunc");
		_logAction = logActionFunc(Level);
	}

	public virtual string GenerateMessage([JetBrains.Annotations.CanBeNull] TParam arg)
	{
		MessageExtractingLogger messageExtractingLogger = new MessageExtractingLogger();
		_logAction(messageExtractingLogger, arg, null);
		return messageExtractingLogger.Message;
	}

	public virtual void Log<TLoggerCategory>([JetBrains.Annotations.NotNull] IDiagnosticsLogger<TLoggerCategory> logger, WarningBehavior warningBehavior, [JetBrains.Annotations.CanBeNull] TParam arg) where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
	{
		switch (warningBehavior)
		{
		case WarningBehavior.Log:
			_logAction(logger.Logger, arg, null);
			break;
		case WarningBehavior.Throw:
			throw WarningAsError(GenerateMessage(arg));
		}
	}
}
public class EventDefinition<TParam1, TParam2> : EventDefinitionBase
{
	private readonly Action<ILogger, TParam1, TParam2, Exception> _logAction;

	public EventDefinition([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, EventId eventId, LogLevel level, [JetBrains.Annotations.NotNull] string eventIdCode, [JetBrains.Annotations.NotNull] Func<LogLevel, Action<ILogger, TParam1, TParam2, Exception>> logActionFunc)
		: base(loggingOptions, eventId, level, eventIdCode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logActionFunc, "logActionFunc");
		_logAction = logActionFunc(Level);
	}

	public virtual string GenerateMessage([JetBrains.Annotations.CanBeNull] TParam1 arg1, [JetBrains.Annotations.CanBeNull] TParam2 arg2)
	{
		MessageExtractingLogger messageExtractingLogger = new MessageExtractingLogger();
		_logAction(messageExtractingLogger, arg1, arg2, null);
		return messageExtractingLogger.Message;
	}

	public virtual void Log<TLoggerCategory>([JetBrains.Annotations.NotNull] IDiagnosticsLogger<TLoggerCategory> logger, WarningBehavior warningBehavior, [JetBrains.Annotations.CanBeNull] TParam1 arg1, [JetBrains.Annotations.CanBeNull] TParam2 arg2) where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
	{
		switch (warningBehavior)
		{
		case WarningBehavior.Log:
			_logAction(logger.Logger, arg1, arg2, null);
			break;
		case WarningBehavior.Throw:
			throw WarningAsError(GenerateMessage(arg1, arg2));
		}
	}
}
public class EventDefinition<TParam1, TParam2, TParam3> : EventDefinitionBase
{
	private readonly Action<ILogger, TParam1, TParam2, TParam3, Exception> _logAction;

	public EventDefinition([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, EventId eventId, LogLevel level, [JetBrains.Annotations.NotNull] string eventIdCode, [JetBrains.Annotations.NotNull] Func<LogLevel, Action<ILogger, TParam1, TParam2, TParam3, Exception>> logActionFunc)
		: base(loggingOptions, eventId, level, eventIdCode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logActionFunc, "logActionFunc");
		_logAction = logActionFunc(Level);
	}

	public virtual string GenerateMessage([JetBrains.Annotations.CanBeNull] TParam1 arg1, [JetBrains.Annotations.CanBeNull] TParam2 arg2, [JetBrains.Annotations.CanBeNull] TParam3 arg3, [JetBrains.Annotations.CanBeNull] Exception exception = null)
	{
		MessageExtractingLogger messageExtractingLogger = new MessageExtractingLogger();
		_logAction(messageExtractingLogger, arg1, arg2, arg3, exception);
		return messageExtractingLogger.Message;
	}

	public virtual void Log<TLoggerCategory>([JetBrains.Annotations.NotNull] IDiagnosticsLogger<TLoggerCategory> logger, WarningBehavior warningBehavior, [JetBrains.Annotations.CanBeNull] TParam1 arg1, [JetBrains.Annotations.CanBeNull] TParam2 arg2, [JetBrains.Annotations.CanBeNull] TParam3 arg3, [JetBrains.Annotations.CanBeNull] Exception exception = null) where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
	{
		switch (warningBehavior)
		{
		case WarningBehavior.Log:
			_logAction(logger.Logger, arg1, arg2, arg3, exception);
			break;
		case WarningBehavior.Throw:
			throw WarningAsError(GenerateMessage(arg1, arg2, arg3, exception));
		}
	}
}
public class EventDefinition<TParam1, TParam2, TParam3, TParam4> : EventDefinitionBase
{
	private readonly Action<ILogger, TParam1, TParam2, TParam3, TParam4, Exception> _logAction;

	public EventDefinition([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, EventId eventId, LogLevel level, [JetBrains.Annotations.NotNull] string eventIdCode, [JetBrains.Annotations.NotNull] Func<LogLevel, Action<ILogger, TParam1, TParam2, TParam3, TParam4, Exception>> logActionFunc)
		: base(loggingOptions, eventId, level, eventIdCode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logActionFunc, "logActionFunc");
		_logAction = logActionFunc(Level);
	}

	public virtual string GenerateMessage([JetBrains.Annotations.CanBeNull] TParam1 arg1, [JetBrains.Annotations.CanBeNull] TParam2 arg2, [JetBrains.Annotations.CanBeNull] TParam3 arg3, [JetBrains.Annotations.CanBeNull] TParam4 arg4)
	{
		MessageExtractingLogger messageExtractingLogger = new MessageExtractingLogger();
		_logAction(messageExtractingLogger, arg1, arg2, arg3, arg4, null);
		return messageExtractingLogger.Message;
	}

	public virtual void Log<TLoggerCategory>([JetBrains.Annotations.NotNull] IDiagnosticsLogger<TLoggerCategory> logger, WarningBehavior warningBehavior, [JetBrains.Annotations.CanBeNull] TParam1 arg1, [JetBrains.Annotations.CanBeNull] TParam2 arg2, [JetBrains.Annotations.CanBeNull] TParam3 arg3, [JetBrains.Annotations.CanBeNull] TParam4 arg4) where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
	{
		switch (warningBehavior)
		{
		case WarningBehavior.Log:
			_logAction(logger.Logger, arg1, arg2, arg3, arg4, null);
			break;
		case WarningBehavior.Throw:
			throw WarningAsError(GenerateMessage(arg1, arg2, arg3, arg4));
		}
	}
}
public class EventDefinition<TParam1, TParam2, TParam3, TParam4, TParam5> : EventDefinitionBase
{
	private readonly Action<ILogger, TParam1, TParam2, TParam3, TParam4, TParam5, Exception> _logAction;

	public EventDefinition([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, EventId eventId, LogLevel level, [JetBrains.Annotations.NotNull] string eventIdCode, [JetBrains.Annotations.NotNull] Func<LogLevel, Action<ILogger, TParam1, TParam2, TParam3, TParam4, TParam5, Exception>> logActionFunc)
		: base(loggingOptions, eventId, level, eventIdCode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logActionFunc, "logActionFunc");
		_logAction = logActionFunc(Level);
	}

	public virtual string GenerateMessage([JetBrains.Annotations.CanBeNull] TParam1 arg1, [JetBrains.Annotations.CanBeNull] TParam2 arg2, [JetBrains.Annotations.CanBeNull] TParam3 arg3, [JetBrains.Annotations.CanBeNull] TParam4 arg4, [JetBrains.Annotations.CanBeNull] TParam5 arg5)
	{
		MessageExtractingLogger messageExtractingLogger = new MessageExtractingLogger();
		_logAction(messageExtractingLogger, arg1, arg2, arg3, arg4, arg5, null);
		return messageExtractingLogger.Message;
	}

	public virtual void Log<TLoggerCategory>([JetBrains.Annotations.NotNull] IDiagnosticsLogger<TLoggerCategory> logger, WarningBehavior warningBehavior, [JetBrains.Annotations.CanBeNull] TParam1 arg1, [JetBrains.Annotations.CanBeNull] TParam2 arg2, [JetBrains.Annotations.CanBeNull] TParam3 arg3, [JetBrains.Annotations.CanBeNull] TParam4 arg4, [JetBrains.Annotations.CanBeNull] TParam5 arg5) where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
	{
		switch (warningBehavior)
		{
		case WarningBehavior.Log:
			_logAction(logger.Logger, arg1, arg2, arg3, arg4, arg5, null);
			break;
		case WarningBehavior.Throw:
			throw WarningAsError(GenerateMessage(arg1, arg2, arg3, arg4, arg5));
		}
	}
}
public class EventDefinition<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : EventDefinitionBase
{
	private readonly Action<ILogger, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, Exception> _logAction;

	public EventDefinition([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, EventId eventId, LogLevel level, [JetBrains.Annotations.NotNull] string eventIdCode, [JetBrains.Annotations.NotNull] Func<LogLevel, Action<ILogger, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, Exception>> logActionFunc)
		: base(loggingOptions, eventId, level, eventIdCode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logActionFunc, "logActionFunc");
		_logAction = logActionFunc(Level);
	}

	public virtual string GenerateMessage([JetBrains.Annotations.CanBeNull] TParam1 arg1, [JetBrains.Annotations.CanBeNull] TParam2 arg2, [JetBrains.Annotations.CanBeNull] TParam3 arg3, [JetBrains.Annotations.CanBeNull] TParam4 arg4, [JetBrains.Annotations.CanBeNull] TParam5 arg5, [JetBrains.Annotations.CanBeNull] TParam6 arg6)
	{
		MessageExtractingLogger messageExtractingLogger = new MessageExtractingLogger();
		_logAction(messageExtractingLogger, arg1, arg2, arg3, arg4, arg5, arg6, null);
		return messageExtractingLogger.Message;
	}

	public virtual void Log<TLoggerCategory>([JetBrains.Annotations.NotNull] IDiagnosticsLogger<TLoggerCategory> logger, WarningBehavior warningBehavior, [JetBrains.Annotations.CanBeNull] TParam1 arg1, [JetBrains.Annotations.CanBeNull] TParam2 arg2, [JetBrains.Annotations.CanBeNull] TParam3 arg3, [JetBrains.Annotations.CanBeNull] TParam4 arg4, [JetBrains.Annotations.CanBeNull] TParam5 arg5, [JetBrains.Annotations.CanBeNull] TParam6 arg6) where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
	{
		switch (warningBehavior)
		{
		case WarningBehavior.Log:
			_logAction(logger.Logger, arg1, arg2, arg3, arg4, arg5, arg6, null);
			break;
		case WarningBehavior.Throw:
			throw WarningAsError(GenerateMessage(arg1, arg2, arg3, arg4, arg5, arg6));
		}
	}
}
