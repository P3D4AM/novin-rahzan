using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public abstract class RelationalLoggingDefinitions : LoggingDefinitions
{
	[EntityFrameworkInternal]
	public EventDefinitionBase LogTransactionError;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogBoolWithDefaultWarning;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogOpeningConnection;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogOpenedConnection;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogClosingConnection;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogClosedConnection;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogConnectionError;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogBeginningTransaction;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogBeganTransaction;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogUsingTransaction;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCommittingTransaction;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRollingBackTransaction;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCommittedTransaction;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRolledBackTransaction;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogDisposingTransaction;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogDisposingDataReader;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogAmbientTransaction;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogPossibleUnintendedUseOfEquals;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogQueryPossibleExceptionWithAggregateOperatorWarning;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogGeneratingDown;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogGeneratingUp;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogApplyingMigration;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRevertingMigration;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogMigrating;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogNoMigrationsApplied;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogNoMigrationsFound;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogKeyHasDefaultValue;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCommandCreating;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCommandCreated;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogExecutingCommand;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogExecutedCommand;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCommandFailed;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogConnectionErrorAsDebug;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogAmbientTransactionEnlisted;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogExplicitTransactionEnlisted;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogBatchSmallerThanMinBatchSize;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogBatchReadyForExecution;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogMigrationAttributeMissingWarning;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogValueConversionSqlLiteralWarning;
}
