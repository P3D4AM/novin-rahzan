using System.Data;
using System.Reflection;
using System.Resources;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics.Internal;

public static class RelationalResources
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.Properties.RelationalStrings", typeof(RelationalResources).GetTypeInfo().Assembly);

	public static EventDefinition<string, string> LogBoolWithDefaultWarning([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogBoolWithDefaultWarning;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogBoolWithDefaultWarning, () => new EventDefinition<string, string>(logger.Options, RelationalEventId.BoolWithDefaultWarning, LogLevel.Warning, "RelationalEventId.BoolWithDefaultWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, RelationalEventId.BoolWithDefaultWarning, _resourceManager.GetString("LogBoolWithDefaultWarning"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogOpeningConnection([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogOpeningConnection;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogOpeningConnection, () => new EventDefinition<string, string>(logger.Options, RelationalEventId.ConnectionOpening, LogLevel.Debug, "RelationalEventId.ConnectionOpening", (LogLevel level) => LoggerMessage.Define<string, string>(level, RelationalEventId.ConnectionOpening, _resourceManager.GetString("LogOpeningConnection"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogOpenedConnection([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogOpenedConnection;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogOpenedConnection, () => new EventDefinition<string, string>(logger.Options, RelationalEventId.ConnectionOpened, LogLevel.Debug, "RelationalEventId.ConnectionOpened", (LogLevel level) => LoggerMessage.Define<string, string>(level, RelationalEventId.ConnectionOpened, _resourceManager.GetString("LogOpenedConnection"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogClosingConnection([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogClosingConnection;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogClosingConnection, () => new EventDefinition<string, string>(logger.Options, RelationalEventId.ConnectionClosing, LogLevel.Debug, "RelationalEventId.ConnectionClosing", (LogLevel level) => LoggerMessage.Define<string, string>(level, RelationalEventId.ConnectionClosing, _resourceManager.GetString("LogClosingConnection"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogClosedConnection([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogClosedConnection;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogClosedConnection, () => new EventDefinition<string, string>(logger.Options, RelationalEventId.ConnectionClosed, LogLevel.Debug, "RelationalEventId.ConnectionClosed", (LogLevel level) => LoggerMessage.Define<string, string>(level, RelationalEventId.ConnectionClosed, _resourceManager.GetString("LogClosedConnection"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogConnectionError([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogConnectionError;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogConnectionError, () => new EventDefinition<string, string>(logger.Options, RelationalEventId.ConnectionError, LogLevel.Error, "RelationalEventId.ConnectionError", (LogLevel level) => LoggerMessage.Define<string, string>(level, RelationalEventId.ConnectionError, _resourceManager.GetString("LogConnectionError"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogBeganTransaction([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogBeganTransaction;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogBeganTransaction, () => new EventDefinition<string>(logger.Options, RelationalEventId.TransactionStarted, LogLevel.Debug, "RelationalEventId.TransactionStarted", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.TransactionStarted, _resourceManager.GetString("LogBeganTransaction"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogBeginningTransaction([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogBeginningTransaction;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogBeginningTransaction, () => new EventDefinition<string>(logger.Options, RelationalEventId.TransactionStarting, LogLevel.Debug, "RelationalEventId.TransactionStarting", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.TransactionStarting, _resourceManager.GetString("LogBeginningTransaction"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogUsingTransaction([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogUsingTransaction;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogUsingTransaction, () => new EventDefinition<string>(logger.Options, RelationalEventId.TransactionUsed, LogLevel.Debug, "RelationalEventId.TransactionUsed", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.TransactionUsed, _resourceManager.GetString("LogUsingTransaction"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition LogCommittingTransaction([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogCommittingTransaction;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogCommittingTransaction, () => new EventDefinition(logger.Options, RelationalEventId.TransactionCommitting, LogLevel.Debug, "RelationalEventId.TransactionCommitting", (LogLevel level) => LoggerMessage.Define(level, RelationalEventId.TransactionCommitting, _resourceManager.GetString("LogCommittingTransaction"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition LogCommittedTransaction([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogCommittedTransaction;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogCommittedTransaction, () => new EventDefinition(logger.Options, RelationalEventId.TransactionCommitted, LogLevel.Debug, "RelationalEventId.TransactionCommitted", (LogLevel level) => LoggerMessage.Define(level, RelationalEventId.TransactionCommitted, _resourceManager.GetString("LogCommittedTransaction"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition LogRollingBackTransaction([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogRollingBackTransaction;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogRollingBackTransaction, () => new EventDefinition(logger.Options, RelationalEventId.TransactionRollingBack, LogLevel.Debug, "RelationalEventId.TransactionRollingBack", (LogLevel level) => LoggerMessage.Define(level, RelationalEventId.TransactionRollingBack, _resourceManager.GetString("LogRollingBackTransaction"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition LogRolledBackTransaction([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogRolledBackTransaction;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogRolledBackTransaction, () => new EventDefinition(logger.Options, RelationalEventId.TransactionRolledBack, LogLevel.Debug, "RelationalEventId.TransactionRolledBack", (LogLevel level) => LoggerMessage.Define(level, RelationalEventId.TransactionRolledBack, _resourceManager.GetString("LogRolledBackTransaction"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition LogDisposingTransaction([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogDisposingTransaction;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogDisposingTransaction, () => new EventDefinition(logger.Options, RelationalEventId.TransactionDisposed, LogLevel.Debug, "RelationalEventId.TransactionDisposed", (LogLevel level) => LoggerMessage.Define(level, RelationalEventId.TransactionDisposed, _resourceManager.GetString("LogDisposingTransaction"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition LogTransactionError([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogTransactionError;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogTransactionError, () => new EventDefinition(logger.Options, RelationalEventId.TransactionError, LogLevel.Error, "RelationalEventId.TransactionError", (LogLevel level) => LoggerMessage.Define(level, RelationalEventId.TransactionError, _resourceManager.GetString("LogTransactionError"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition LogDisposingDataReader([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogDisposingDataReader;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogDisposingDataReader, () => new EventDefinition(logger.Options, RelationalEventId.DataReaderDisposing, LogLevel.Debug, "RelationalEventId.DataReaderDisposing", (LogLevel level) => LoggerMessage.Define(level, RelationalEventId.DataReaderDisposing, _resourceManager.GetString("LogDisposingDataReader"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition LogAmbientTransaction([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogAmbientTransaction;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogAmbientTransaction, () => new EventDefinition(logger.Options, RelationalEventId.AmbientTransactionWarning, LogLevel.Warning, "RelationalEventId.AmbientTransactionWarning", (LogLevel level) => LoggerMessage.Define(level, RelationalEventId.AmbientTransactionWarning, _resourceManager.GetString("LogAmbientTransaction"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition<object> LogPossibleUnintendedUseOfEquals([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogPossibleUnintendedUseOfEquals;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogPossibleUnintendedUseOfEquals, () => new EventDefinition<object>(logger.Options, RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning, LogLevel.Warning, "RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning", (LogLevel level) => LoggerMessage.Define<object>(level, RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning, _resourceManager.GetString("LogPossibleUnintendedUseOfEquals"))));
		}
		return (EventDefinition<object>)eventDefinitionBase;
	}

	public static EventDefinition LogQueryPossibleExceptionWithAggregateOperatorWarning([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogQueryPossibleExceptionWithAggregateOperatorWarning;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogQueryPossibleExceptionWithAggregateOperatorWarning, () => new EventDefinition(logger.Options, RelationalEventId.QueryPossibleExceptionWithAggregateOperatorWarning, LogLevel.Warning, "RelationalEventId.QueryPossibleExceptionWithAggregateOperatorWarning", (LogLevel level) => LoggerMessage.Define(level, RelationalEventId.QueryPossibleExceptionWithAggregateOperatorWarning, _resourceManager.GetString("LogQueryPossibleExceptionWithAggregateOperatorWarning"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition<string> LogGeneratingDown([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogGeneratingDown;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogGeneratingDown, () => new EventDefinition<string>(logger.Options, RelationalEventId.MigrationGeneratingDownScript, LogLevel.Debug, "RelationalEventId.MigrationGeneratingDownScript", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.MigrationGeneratingDownScript, _resourceManager.GetString("LogGeneratingDown"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogGeneratingUp([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogGeneratingUp;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogGeneratingUp, () => new EventDefinition<string>(logger.Options, RelationalEventId.MigrationGeneratingUpScript, LogLevel.Debug, "RelationalEventId.MigrationGeneratingUpScript", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.MigrationGeneratingUpScript, _resourceManager.GetString("LogGeneratingUp"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogApplyingMigration([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogApplyingMigration;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogApplyingMigration, () => new EventDefinition<string>(logger.Options, RelationalEventId.MigrationApplying, LogLevel.Information, "RelationalEventId.MigrationApplying", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.MigrationApplying, _resourceManager.GetString("LogApplyingMigration"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogRevertingMigration([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogRevertingMigration;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogRevertingMigration, () => new EventDefinition<string>(logger.Options, RelationalEventId.MigrationReverting, LogLevel.Information, "RelationalEventId.MigrationReverting", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.MigrationReverting, _resourceManager.GetString("LogRevertingMigration"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogMigrating([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogMigrating;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogMigrating, () => new EventDefinition<string, string>(logger.Options, RelationalEventId.MigrateUsingConnection, LogLevel.Debug, "RelationalEventId.MigrateUsingConnection", (LogLevel level) => LoggerMessage.Define<string, string>(level, RelationalEventId.MigrateUsingConnection, _resourceManager.GetString("LogMigrating"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition LogNoMigrationsApplied([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogNoMigrationsApplied;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogNoMigrationsApplied, () => new EventDefinition(logger.Options, RelationalEventId.MigrationsNotApplied, LogLevel.Information, "RelationalEventId.MigrationsNotApplied", (LogLevel level) => LoggerMessage.Define(level, RelationalEventId.MigrationsNotApplied, _resourceManager.GetString("LogNoMigrationsApplied"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition<string> LogNoMigrationsFound([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogNoMigrationsFound;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogNoMigrationsFound, () => new EventDefinition<string>(logger.Options, RelationalEventId.MigrationsNotFound, LogLevel.Debug, "RelationalEventId.MigrationsNotFound", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.MigrationsNotFound, _resourceManager.GetString("LogNoMigrationsFound"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogKeyHasDefaultValue([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogKeyHasDefaultValue;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogKeyHasDefaultValue, () => new EventDefinition<string, string>(logger.Options, RelationalEventId.ModelValidationKeyDefaultValueWarning, LogLevel.Warning, "RelationalEventId.ModelValidationKeyDefaultValueWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, RelationalEventId.ModelValidationKeyDefaultValueWarning, _resourceManager.GetString("LogKeyHasDefaultValue"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogCommandCreating([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogCommandCreating;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogCommandCreating, () => new EventDefinition<string>(logger.Options, RelationalEventId.CommandCreating, LogLevel.Debug, "RelationalEventId.CommandCreating", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.CommandCreating, _resourceManager.GetString("LogCommandCreating"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string, int> LogCommandCreated([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogCommandCreated;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogCommandCreated, () => new EventDefinition<string, int>(logger.Options, RelationalEventId.CommandCreated, LogLevel.Debug, "RelationalEventId.CommandCreated", (LogLevel level) => LoggerMessage.Define<string, int>(level, RelationalEventId.CommandCreated, _resourceManager.GetString("LogCommandCreated"))));
		}
		return (EventDefinition<string, int>)eventDefinitionBase;
	}

	public static EventDefinition<string, CommandType, int, string, string> LogExecutingCommand([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogExecutingCommand;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogExecutingCommand, () => new EventDefinition<string, CommandType, int, string, string>(logger.Options, RelationalEventId.CommandExecuting, LogLevel.Debug, "RelationalEventId.CommandExecuting", (LogLevel level) => LoggerMessage.Define<string, CommandType, int, string, string>(level, RelationalEventId.CommandExecuting, _resourceManager.GetString("LogExecutingCommand"))));
		}
		return (EventDefinition<string, CommandType, int, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, CommandType, int, string, string> LogExecutedCommand([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogExecutedCommand;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogExecutedCommand, () => new EventDefinition<string, string, CommandType, int, string, string>(logger.Options, RelationalEventId.CommandExecuted, LogLevel.Information, "RelationalEventId.CommandExecuted", (LogLevel level) => LoggerMessage.Define<string, string, CommandType, int, string, string>(level, RelationalEventId.CommandExecuted, _resourceManager.GetString("LogExecutedCommand"))));
		}
		return (EventDefinition<string, string, CommandType, int, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, CommandType, int, string, string> LogCommandFailed([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogCommandFailed;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogCommandFailed, () => new EventDefinition<string, string, CommandType, int, string, string>(logger.Options, RelationalEventId.CommandError, LogLevel.Error, "RelationalEventId.CommandError", (LogLevel level) => LoggerMessage.Define<string, string, CommandType, int, string, string>(level, RelationalEventId.CommandError, _resourceManager.GetString("LogCommandFailed"))));
		}
		return (EventDefinition<string, string, CommandType, int, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogConnectionErrorAsDebug([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogConnectionErrorAsDebug;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogConnectionErrorAsDebug, () => new EventDefinition<string, string>(logger.Options, RelationalEventId.ConnectionError, LogLevel.Debug, "RelationalEventId.ConnectionError", (LogLevel level) => LoggerMessage.Define<string, string>(level, RelationalEventId.ConnectionError, _resourceManager.GetString("LogConnectionErrorAsDebug"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogAmbientTransactionEnlisted([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogAmbientTransactionEnlisted;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogAmbientTransactionEnlisted, () => new EventDefinition<string>(logger.Options, RelationalEventId.AmbientTransactionEnlisted, LogLevel.Debug, "RelationalEventId.AmbientTransactionEnlisted", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.AmbientTransactionEnlisted, _resourceManager.GetString("LogAmbientTransactionEnlisted"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogExplicitTransactionEnlisted([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogExplicitTransactionEnlisted;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogExplicitTransactionEnlisted, () => new EventDefinition<string>(logger.Options, RelationalEventId.ExplicitTransactionEnlisted, LogLevel.Debug, "RelationalEventId.ExplicitTransactionEnlisted", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.ExplicitTransactionEnlisted, _resourceManager.GetString("LogExplicitTransactionEnlisted"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<int, int> LogBatchSmallerThanMinBatchSize([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogBatchSmallerThanMinBatchSize;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogBatchSmallerThanMinBatchSize, () => new EventDefinition<int, int>(logger.Options, RelationalEventId.BatchSmallerThanMinBatchSize, LogLevel.Debug, "RelationalEventId.BatchSmallerThanMinBatchSize", (LogLevel level) => LoggerMessage.Define<int, int>(level, RelationalEventId.BatchSmallerThanMinBatchSize, _resourceManager.GetString("LogBatchSmallerThanMinBatchSize"))));
		}
		return (EventDefinition<int, int>)eventDefinitionBase;
	}

	public static EventDefinition<int> LogBatchReadyForExecution([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogBatchReadyForExecution;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogBatchReadyForExecution, () => new EventDefinition<int>(logger.Options, RelationalEventId.BatchReadyForExecution, LogLevel.Debug, "RelationalEventId.BatchReadyForExecution", (LogLevel level) => LoggerMessage.Define<int>(level, RelationalEventId.BatchReadyForExecution, _resourceManager.GetString("LogBatchReadyForExecution"))));
		}
		return (EventDefinition<int>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogMigrationAttributeMissingWarning([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((RelationalLoggingDefinitions)logger.Definitions).LogMigrationAttributeMissingWarning;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((RelationalLoggingDefinitions)logger.Definitions).LogMigrationAttributeMissingWarning, () => new EventDefinition<string>(logger.Options, RelationalEventId.MigrationAttributeMissingWarning, LogLevel.Warning, "RelationalEventId.MigrationAttributeMissingWarning", (LogLevel level) => LoggerMessage.Define<string>(level, RelationalEventId.MigrationAttributeMissingWarning, _resourceManager.GetString("LogMigrationAttributeMissingWarning"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}
}
