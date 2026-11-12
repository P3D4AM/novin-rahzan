using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public abstract class LoggingDefinitions
{
	[EntityFrameworkInternal]
	public EventDefinitionBase LogServiceProviderCreated;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogManyServiceProvidersCreated;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogServiceProviderDebugInfo;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogContextInitialized;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogExceptionDuringQueryIteration;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogExceptionDuringSaveChanges;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogDetectChangesStarting;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogDetectChangesCompleted;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogPropertyChangeDetected;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogPropertyChangeDetectedSensitive;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogForeignKeyChangeDetected;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogForeignKeyChangeDetectedSensitive;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCollectionChangeDetected;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCollectionChangeDetectedSensitive;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogReferenceChangeDetected;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogReferenceChangeDetectedSensitive;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCascadeDelete;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCascadeDeleteSensitive;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCascadeDeleteOrphan;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCascadeDeleteOrphanSensitive;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogStartedTracking;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogStartedTrackingSensitive;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogStateChanged;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogStateChangedSensitive;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogValueGenerated;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogValueGeneratedSensitive;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogTempValueGenerated;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogTempValueGeneratedSensitive;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogSaveChangesStarting;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogSaveChangesCompleted;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogContextDisposed;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCompilingQueryModel;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogOptimizedQueryModel;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogIncludingNavigation;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogQueryExecutionPlanned;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogSensitiveDataLoggingEnabled;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogIgnoredInclude;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRowLimitingOperationWithoutOrderBy;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogPossibleUnintendedCollectionNavigationNullComparison;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogPossibleUnintendedReferenceComparison;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogDuplicateDependentEntityTypeInstance;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogShadowPropertyCreated;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogCollectionWithoutComparer;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogExecutionStrategyRetrying;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogNavigationLazyLoading;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogLazyLoadOnDisposedContext;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogDetachedLazyLoading;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRedundantAddServicesCall;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRedundantIndexRemoved;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogIncompatibleMatchingForeignKeyProperties;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRequiredAttributeInverted;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogNonNullableInverted;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRequiredAttributeOnDependent;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogNonNullableReferenceOnDependent;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRequiredAttributeOnBothNavigations;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogNonNullableReferenceOnBothNavigations;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogForeignKeyAttributesOnBothNavigations;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRequiredAttributeOnCollection;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogForeignKeyAttributesOnBothProperties;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogConflictingForeignKeyAttributesOnNavigationAndProperty;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogMultipleInversePropertiesSameTarget;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogConflictingShadowForeignKeys;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogMultipleNavigationProperties;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogMultiplePrimaryKeyCandidates;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogNonOwnershipInverseNavigation;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogFirstWithoutOrderByAndFilter;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogNonDefiningInverseNavigation;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogOptimisticConcurrencyException;

	[EntityFrameworkInternal]
	public EventDefinitionBase LogRedundantForeignKey;
}
