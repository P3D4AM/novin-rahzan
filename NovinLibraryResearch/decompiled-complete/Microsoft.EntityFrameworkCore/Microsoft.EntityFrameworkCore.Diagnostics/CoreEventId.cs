using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public static class CoreEventId
{
	private enum Id
	{
		SaveChangesFailed = 10000,
		DuplicateDependentEntityTypeInstanceWarning = 10001,
		CascadeDelete = 10002,
		CascadeDeleteOrphan = 10003,
		SaveChangesStarting = 10004,
		SaveChangesCompleted = 10005,
		OptimisticConcurrencyException = 10006,
		QueryIterationFailed = 10100,
		QueryModelCompiling = 10101,
		RowLimitingOperationWithoutOrderByWarning = 10102,
		FirstWithoutOrderByAndFilterWarning = 10103,
		QueryModelOptimized = 10104,
		NavigationIncluded = 10105,
		IncludeIgnoredWarning = 10106,
		QueryExecutionPlanned = 10107,
		PossibleUnintendedCollectionNavigationNullComparisonWarning = 10108,
		PossibleUnintendedReferenceComparisonWarning = 10109,
		SensitiveDataLoggingEnabledWarning = 10400,
		ServiceProviderCreated = 10401,
		ManyServiceProvidersCreatedWarning = 10402,
		ContextInitialized = 10403,
		ExecutionStrategyRetrying = 10404,
		LazyLoadOnDisposedContextWarning = 10405,
		NavigationLazyLoading = 10406,
		ContextDisposed = 10407,
		DetachedLazyLoadingWarning = 10408,
		ServiceProviderDebugInfo = 10409,
		RedundantAddServicesCallWarning = 10410,
		ShadowPropertyCreated = 10600,
		RedundantIndexRemoved = 10601,
		IncompatibleMatchingForeignKeyProperties = 10602,
		RequiredAttributeOnDependent = 10603,
		RequiredAttributeOnBothNavigations = 10604,
		ConflictingShadowForeignKeysWarning = 10605,
		MultiplePrimaryKeyCandidates = 10606,
		MultipleNavigationProperties = 10607,
		MultipleInversePropertiesSameTargetWarning = 10608,
		NonDefiningInverseNavigationWarning = 10609,
		NonOwnershipInverseNavigationWarning = 10610,
		ForeignKeyAttributesOnBothPropertiesWarning = 10611,
		ForeignKeyAttributesOnBothNavigationsWarning = 10612,
		ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning = 10613,
		RedundantForeignKeyWarning = 10614,
		NonNullableInverted = 10615,
		NonNullableReferenceOnBothNavigations = 10616,
		NonNullableReferenceOnDependent = 10617,
		RequiredAttributeInverted = 10618,
		RequiredAttributeOnCollection = 10619,
		CollectionWithoutComparer = 10620,
		DetectChangesStarting = 10800,
		DetectChangesCompleted = 10801,
		PropertyChangeDetected = 10802,
		ForeignKeyChangeDetected = 10803,
		CollectionChangeDetected = 10804,
		ReferenceChangeDetected = 10805,
		StartedTracking = 10806,
		StateChanged = 10807,
		ValueGenerated = 10808
	}

	public const int CoreBaseId = 10000;

	public const int RelationalBaseId = 20000;

	public const int ProviderBaseId = 30000;

	public const int ProviderDesignBaseId = 35000;

	private static readonly string _updatePrefix = LoggerCategory<DbLoggerCategory.Update>.Name + ".";

	public static readonly EventId SaveChangesFailed = MakeUpdateId(Id.SaveChangesFailed);

	public static readonly EventId DuplicateDependentEntityTypeInstanceWarning = MakeUpdateId(Id.DuplicateDependentEntityTypeInstanceWarning);

	private static readonly string _queryPrefix = LoggerCategory<DbLoggerCategory.Query>.Name + ".";

	public static readonly EventId QueryIterationFailed = MakeQueryId(Id.QueryIterationFailed);

	[Obsolete]
	public static readonly EventId QueryModelCompiling = MakeQueryId(Id.QueryModelCompiling);

	[Obsolete]
	public static readonly EventId RowLimitingOperationWithoutOrderByWarning = MakeQueryId(Id.RowLimitingOperationWithoutOrderByWarning);

	[Obsolete]
	public static readonly EventId FirstWithoutOrderByAndFilterWarning = MakeQueryId(Id.FirstWithoutOrderByAndFilterWarning);

	[Obsolete]
	public static readonly EventId QueryModelOptimized = MakeQueryId(Id.QueryModelOptimized);

	[Obsolete]
	public static readonly EventId NavigationIncluded = MakeQueryId(Id.NavigationIncluded);

	[Obsolete]
	public static readonly EventId IncludeIgnoredWarning = MakeQueryId(Id.IncludeIgnoredWarning);

	public static readonly EventId QueryExecutionPlanned = MakeQueryId(Id.QueryExecutionPlanned);

	public static readonly EventId PossibleUnintendedCollectionNavigationNullComparisonWarning = MakeQueryId(Id.PossibleUnintendedCollectionNavigationNullComparisonWarning);

	public static readonly EventId PossibleUnintendedReferenceComparisonWarning = MakeQueryId(Id.PossibleUnintendedReferenceComparisonWarning);

	private static readonly string _infraPrefix = LoggerCategory<DbLoggerCategory.Infrastructure>.Name + ".";

	public static readonly EventId SensitiveDataLoggingEnabledWarning = MakeInfraId(Id.SensitiveDataLoggingEnabledWarning);

	public static readonly EventId ServiceProviderCreated = MakeInfraId(Id.ServiceProviderCreated);

	public static readonly EventId ManyServiceProvidersCreatedWarning = MakeInfraId(Id.ManyServiceProvidersCreatedWarning);

	public static readonly EventId ContextInitialized = MakeInfraId(Id.ContextInitialized);

	public static readonly EventId ServiceProviderDebugInfo = MakeInfraId(Id.ServiceProviderDebugInfo);

	public static readonly EventId ExecutionStrategyRetrying = MakeInfraId(Id.ExecutionStrategyRetrying);

	public static readonly EventId NavigationLazyLoading = MakeInfraId(Id.NavigationLazyLoading);

	public static readonly EventId LazyLoadOnDisposedContextWarning = MakeInfraId(Id.LazyLoadOnDisposedContextWarning);

	public static readonly EventId DetachedLazyLoadingWarning = MakeInfraId(Id.DetachedLazyLoadingWarning);

	public static readonly EventId RedundantAddServicesCallWarning = MakeInfraId(Id.RedundantAddServicesCallWarning);

	private static readonly string _modelPrefix = LoggerCategory<DbLoggerCategory.Model>.Name + ".";

	private static readonly string _modelValidationPrefix = LoggerCategory<DbLoggerCategory.Model.Validation>.Name + ".";

	public static readonly EventId ShadowPropertyCreated = MakeModelValidationId(Id.ShadowPropertyCreated);

	public static readonly EventId RedundantIndexRemoved = MakeModelId(Id.RedundantIndexRemoved);

	public static readonly EventId IncompatibleMatchingForeignKeyProperties = MakeModelId(Id.IncompatibleMatchingForeignKeyProperties);

	public static readonly EventId RequiredAttributeInverted = MakeModelId(Id.RequiredAttributeInverted);

	public static readonly EventId NonNullableInverted = MakeModelId(Id.NonNullableInverted);

	public static readonly EventId RequiredAttributeOnBothNavigations = MakeModelId(Id.RequiredAttributeOnBothNavigations);

	public static readonly EventId NonNullableReferenceOnBothNavigations = MakeModelId(Id.NonNullableReferenceOnBothNavigations);

	public static readonly EventId RequiredAttributeOnDependent = MakeModelId(Id.RequiredAttributeOnDependent);

	public static readonly EventId NonNullableReferenceOnDependent = MakeModelId(Id.NonNullableReferenceOnDependent);

	public static readonly EventId RequiredAttributeOnCollection = MakeModelId(Id.RequiredAttributeOnCollection);

	public static readonly EventId ConflictingShadowForeignKeysWarning = MakeModelId(Id.ConflictingShadowForeignKeysWarning);

	public static readonly EventId MultiplePrimaryKeyCandidates = MakeModelId(Id.MultiplePrimaryKeyCandidates);

	public static readonly EventId MultipleNavigationProperties = MakeModelId(Id.MultipleNavigationProperties);

	public static readonly EventId MultipleInversePropertiesSameTargetWarning = MakeModelId(Id.MultipleInversePropertiesSameTargetWarning);

	public static readonly EventId NonDefiningInverseNavigationWarning = MakeModelId(Id.NonDefiningInverseNavigationWarning);

	public static readonly EventId NonOwnershipInverseNavigationWarning = MakeModelId(Id.NonOwnershipInverseNavigationWarning);

	public static readonly EventId ForeignKeyAttributesOnBothPropertiesWarning = MakeModelId(Id.ForeignKeyAttributesOnBothPropertiesWarning);

	public static readonly EventId ForeignKeyAttributesOnBothNavigationsWarning = MakeModelId(Id.ForeignKeyAttributesOnBothNavigationsWarning);

	public static readonly EventId ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning = MakeModelId(Id.ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning);

	public static readonly EventId RedundantForeignKeyWarning = MakeModelValidationId(Id.RedundantForeignKeyWarning);

	private static readonly string _changeTrackingPrefix = LoggerCategory<DbLoggerCategory.ChangeTracking>.Name + ".";

	public static readonly EventId DetectChangesStarting = MakeChangeTrackingId(Id.DetectChangesStarting);

	public static readonly EventId DetectChangesCompleted = MakeChangeTrackingId(Id.DetectChangesCompleted);

	public static readonly EventId PropertyChangeDetected = MakeChangeTrackingId(Id.PropertyChangeDetected);

	public static readonly EventId ForeignKeyChangeDetected = MakeChangeTrackingId(Id.ForeignKeyChangeDetected);

	public static readonly EventId CollectionChangeDetected = MakeChangeTrackingId(Id.CollectionChangeDetected);

	public static readonly EventId ReferenceChangeDetected = MakeChangeTrackingId(Id.ReferenceChangeDetected);

	public static readonly EventId StartedTracking = MakeChangeTrackingId(Id.StartedTracking);

	public static readonly EventId StateChanged = MakeChangeTrackingId(Id.StateChanged);

	public static readonly EventId ValueGenerated = MakeChangeTrackingId(Id.ValueGenerated);

	public static readonly EventId CascadeDelete = MakeUpdateId(Id.CascadeDelete);

	public static readonly EventId CascadeDeleteOrphan = MakeUpdateId(Id.CascadeDeleteOrphan);

	public static readonly EventId SaveChangesStarting = MakeUpdateId(Id.SaveChangesStarting);

	public static readonly EventId SaveChangesCompleted = MakeUpdateId(Id.SaveChangesCompleted);

	public static readonly EventId OptimisticConcurrencyException = MakeUpdateId(Id.OptimisticConcurrencyException);

	public static readonly EventId ContextDisposed = MakeInfraId(Id.ContextDisposed);

	public static readonly EventId CollectionWithoutComparer = MakeModelValidationId(Id.CollectionWithoutComparer);

	private static EventId MakeUpdateId(Id id)
	{
		return new EventId((int)id, _updatePrefix + id);
	}

	private static EventId MakeQueryId(Id id)
	{
		return new EventId((int)id, _queryPrefix + id);
	}

	private static EventId MakeInfraId(Id id)
	{
		return new EventId((int)id, _infraPrefix + id);
	}

	private static EventId MakeModelId(Id id)
	{
		return new EventId((int)id, _modelPrefix + id);
	}

	private static EventId MakeModelValidationId(Id id)
	{
		return new EventId((int)id, _modelValidationPrefix + id);
	}

	private static EventId MakeChangeTrackingId(Id id)
	{
		return new EventId((int)id, _changeTrackingPrefix + id);
	}
}
