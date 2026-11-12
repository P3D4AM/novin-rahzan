using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class FallbackEventDefinition : EventDefinitionBase
{
	public virtual string MessageFormat { get; }

	public FallbackEventDefinition([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, EventId eventId, LogLevel level, [JetBrains.Annotations.NotNull] string eventIdCode, [JetBrains.Annotations.NotNull] string messageFormat)
		: base(loggingOptions, eventId, level, eventIdCode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(messageFormat, "messageFormat");
		MessageFormat = messageFormat;
	}

	public virtual string GenerateMessage([JetBrains.Annotations.NotNull] Action<ILogger> logAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logAction, "logAction");
		MessageExtractingLogger messageExtractingLogger = new MessageExtractingLogger();
		logAction(messageExtractingLogger);
		return messageExtractingLogger.Message;
	}

	public virtual void Log<TLoggerCategory>([JetBrains.Annotations.NotNull] IDiagnosticsLogger<TLoggerCategory> logger, WarningBehavior warningBehavior, [JetBrains.Annotations.NotNull] Action<ILogger> logAction) where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
	{
		switch (warningBehavior)
		{
		case WarningBehavior.Log:
			logAction(logger.Logger);
			break;
		case WarningBehavior.Throw:
			throw WarningAsError(GenerateMessage(logAction));
		}
	}
}
