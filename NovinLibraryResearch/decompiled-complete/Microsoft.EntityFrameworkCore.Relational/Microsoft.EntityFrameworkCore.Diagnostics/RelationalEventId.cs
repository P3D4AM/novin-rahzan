using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public static class RelationalEventId
{
	private enum Id
	{
		ConnectionOpening = 20000,
		ConnectionOpened = 20001,
		ConnectionClosing = 20002,
		ConnectionClosed = 20003,
		ConnectionError = 20004,
		CommandExecuting = 20100,
		CommandExecuted = 20101,
		CommandError = 20102,
		CommandCreating = 20103,
		CommandCreated = 20104,
		TransactionStarted = 20200,
		TransactionUsed = 20201,
		TransactionCommitted = 20202,
		TransactionRolledBack = 20203,
		TransactionDisposed = 20204,
		TransactionError = 20205,
		AmbientTransactionWarning = 20206,
		AmbientTransactionEnlisted = 20207,
		ExplicitTransactionEnlisted = 20208,
		TransactionStarting = 20209,
		TransactionCommitting = 20210,
		TransactionRollingBack = 20211,
		DataReaderDisposing = 20300,
		MigrateUsingConnection = 20400,
		MigrationReverting = 20401,
		MigrationApplying = 20402,
		MigrationGeneratingDownScript = 20403,
		MigrationGeneratingUpScript = 20404,
		MigrationsNotApplied = 20405,
		MigrationsNotFound = 20406,
		MigrationAttributeMissingWarning = 20407,
		QueryClientEvaluationWarning = 20500,
		QueryPossibleUnintendedUseOfEqualsWarning = 20501,
		QueryPossibleExceptionWithAggregateOperatorWarning = 20502,
		ValueConversionSqlLiteralWarning = 20503,
		ModelValidationKeyDefaultValueWarning = 20600,
		BoolWithDefaultWarning = 20601,
		BatchReadyForExecution = 20700,
		BatchSmallerThanMinBatchSize = 20701
	}

	private static readonly string _connectionPrefix = LoggerCategory<DbLoggerCategory.Database.Connection>.Name + ".";

	public static readonly EventId ConnectionOpening = MakeConnectionId(Id.ConnectionOpening);

	public static readonly EventId ConnectionOpened = MakeConnectionId(Id.ConnectionOpened);

	public static readonly EventId ConnectionClosing = MakeConnectionId(Id.ConnectionClosing);

	public static readonly EventId ConnectionClosed = MakeConnectionId(Id.ConnectionClosed);

	public static readonly EventId ConnectionError = MakeConnectionId(Id.ConnectionError);

	private static readonly string _sqlPrefix = LoggerCategory<DbLoggerCategory.Database.Command>.Name + ".";

	public static readonly EventId CommandCreating = MakeCommandId(Id.CommandCreating);

	public static readonly EventId CommandCreated = MakeCommandId(Id.CommandCreated);

	public static readonly EventId CommandExecuting = MakeCommandId(Id.CommandExecuting);

	public static readonly EventId CommandExecuted = MakeCommandId(Id.CommandExecuted);

	public static readonly EventId CommandError = MakeCommandId(Id.CommandError);

	private static readonly string _transactionPrefix = LoggerCategory<DbLoggerCategory.Database.Transaction>.Name + ".";

	public static readonly EventId TransactionStarted = MakeTransactionId(Id.TransactionStarted);

	public static readonly EventId TransactionStarting = MakeTransactionId(Id.TransactionStarting);

	public static readonly EventId TransactionUsed = MakeTransactionId(Id.TransactionUsed);

	public static readonly EventId TransactionCommitting = MakeTransactionId(Id.TransactionCommitting);

	public static readonly EventId TransactionCommitted = MakeTransactionId(Id.TransactionCommitted);

	public static readonly EventId TransactionRollingBack = MakeTransactionId(Id.TransactionRollingBack);

	public static readonly EventId TransactionRolledBack = MakeTransactionId(Id.TransactionRolledBack);

	public static readonly EventId TransactionDisposed = MakeTransactionId(Id.TransactionDisposed);

	public static readonly EventId TransactionError = MakeTransactionId(Id.TransactionError);

	public static readonly EventId AmbientTransactionWarning = MakeTransactionId(Id.AmbientTransactionWarning);

	public static readonly EventId AmbientTransactionEnlisted = MakeTransactionId(Id.AmbientTransactionEnlisted);

	public static readonly EventId ExplicitTransactionEnlisted = MakeTransactionId(Id.ExplicitTransactionEnlisted);

	public static readonly EventId DataReaderDisposing = MakeCommandId(Id.DataReaderDisposing);

	private static readonly string _migrationsPrefix = LoggerCategory<DbLoggerCategory.Migrations>.Name + ".";

	public static readonly EventId MigrateUsingConnection = MakeMigrationsId(Id.MigrateUsingConnection);

	public static readonly EventId MigrationReverting = MakeMigrationsId(Id.MigrationReverting);

	public static readonly EventId MigrationApplying = MakeMigrationsId(Id.MigrationApplying);

	public static readonly EventId MigrationGeneratingDownScript = MakeMigrationsId(Id.MigrationGeneratingDownScript);

	public static readonly EventId MigrationGeneratingUpScript = MakeMigrationsId(Id.MigrationGeneratingUpScript);

	public static readonly EventId MigrationsNotApplied = MakeMigrationsId(Id.MigrationsNotApplied);

	public static readonly EventId MigrationsNotFound = MakeMigrationsId(Id.MigrationsNotFound);

	public static readonly EventId MigrationAttributeMissingWarning = MakeMigrationsId(Id.MigrationAttributeMissingWarning);

	private static readonly string _queryPrefix = LoggerCategory<DbLoggerCategory.Query>.Name + ".";

	[Obsolete("Automatic client evaluation is no longer supported. This event is no longer generated.")]
	public static readonly EventId QueryClientEvaluationWarning = MakeQueryId(Id.QueryClientEvaluationWarning);

	public static readonly EventId QueryPossibleUnintendedUseOfEqualsWarning = MakeQueryId(Id.QueryPossibleUnintendedUseOfEqualsWarning);

	public static readonly EventId QueryPossibleExceptionWithAggregateOperatorWarning = MakeQueryId(Id.QueryPossibleExceptionWithAggregateOperatorWarning);

	private static readonly string _validationPrefix = LoggerCategory<DbLoggerCategory.Model.Validation>.Name + ".";

	public static readonly EventId ModelValidationKeyDefaultValueWarning = MakeValidationId(Id.ModelValidationKeyDefaultValueWarning);

	public static readonly EventId BoolWithDefaultWarning = MakeValidationId(Id.BoolWithDefaultWarning);

	private static readonly string _updatePrefix = LoggerCategory<DbLoggerCategory.Update>.Name + ".";

	public static readonly EventId BatchReadyForExecution = MakeUpdateId(Id.BatchReadyForExecution);

	public static readonly EventId BatchSmallerThanMinBatchSize = MakeUpdateId(Id.BatchSmallerThanMinBatchSize);

	private static EventId MakeConnectionId(Id id)
	{
		return new EventId((int)id, _connectionPrefix + id);
	}

	private static EventId MakeCommandId(Id id)
	{
		return new EventId((int)id, _sqlPrefix + id);
	}

	private static EventId MakeTransactionId(Id id)
	{
		return new EventId((int)id, _transactionPrefix + id);
	}

	private static EventId MakeMigrationsId(Id id)
	{
		return new EventId((int)id, _migrationsPrefix + id);
	}

	private static EventId MakeQueryId(Id id)
	{
		return new EventId((int)id, _queryPrefix + id);
	}

	private static EventId MakeValidationId(Id id)
	{
		return new EventId((int)id, _validationPrefix + id);
	}

	private static EventId MakeUpdateId(Id id)
	{
		return new EventId((int)id, _updatePrefix + id);
	}
}
