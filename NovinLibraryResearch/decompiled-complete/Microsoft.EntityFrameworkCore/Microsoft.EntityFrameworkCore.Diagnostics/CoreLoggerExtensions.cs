using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public static class CoreLoggerExtensions
{
	public static void SaveChangesFailed([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] Exception exception)
	{
		EventDefinition<Type, string, Exception> eventDefinition = CoreResources.LogExceptionDuringSaveChanges(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, context.GetType(), Environment.NewLine, exception, exception);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new DbContextErrorEventData(eventDefinition, SaveChangesFailed, context, exception));
		}
	}

	private static string SaveChangesFailed(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<Type, string, Exception> obj = (EventDefinition<Type, string, Exception>)definition;
		DbContextErrorEventData dbContextErrorEventData = (DbContextErrorEventData)payload;
		return obj.GenerateMessage(dbContextErrorEventData.Context.GetType(), Environment.NewLine, dbContextErrorEventData.Exception);
	}

	public static void OptimisticConcurrencyException([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] Exception exception)
	{
		EventDefinition<Exception> eventDefinition = CoreResources.LogOptimisticConcurrencyException(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, exception);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new DbContextErrorEventData(eventDefinition, OptimisticConcurrencyException, context, exception));
		}
	}

	private static string OptimisticConcurrencyException(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<Exception> obj = (EventDefinition<Exception>)definition;
		DbContextErrorEventData dbContextErrorEventData = (DbContextErrorEventData)payload;
		return obj.GenerateMessage(dbContextErrorEventData.Exception);
	}

	public static void DuplicateDependentEntityTypeInstanceWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] IEntityType dependent1, [JetBrains.Annotations.NotNull] IEntityType dependent2)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogDuplicateDependentEntityTypeInstance(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, dependent1.DisplayName(), dependent2.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new SharedDependentEntityEventData(eventDefinition, DuplicateDependentEntityTypeInstanceWarning, dependent1, dependent2));
		}
	}

	private static string DuplicateDependentEntityTypeInstanceWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		SharedDependentEntityEventData sharedDependentEntityEventData = (SharedDependentEntityEventData)payload;
		return obj.GenerateMessage(sharedDependentEntityEventData.FirstEntityType.DisplayName(), sharedDependentEntityEventData.SecondEntityType.DisplayName());
	}

	public static void QueryIterationFailed([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Query> diagnostics, [JetBrains.Annotations.NotNull] Type contextType, [JetBrains.Annotations.NotNull] Exception exception)
	{
		EventDefinition<Type, string, Exception> eventDefinition = CoreResources.LogExceptionDuringQueryIteration(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, contextType, Environment.NewLine, exception, exception);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new DbContextTypeErrorEventData(eventDefinition, QueryIterationFailed, contextType, exception));
		}
	}

	private static string QueryIterationFailed(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<Type, string, Exception> obj = (EventDefinition<Type, string, Exception>)definition;
		DbContextTypeErrorEventData dbContextTypeErrorEventData = (DbContextTypeErrorEventData)payload;
		return obj.GenerateMessage(dbContextTypeErrorEventData.ContextType, Environment.NewLine, dbContextTypeErrorEventData.Exception);
	}

	public static void QueryExecutionPlanned([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Query> diagnostics, [JetBrains.Annotations.NotNull] ExpressionPrinter expressionPrinter, [JetBrains.Annotations.NotNull] Expression queryExecutorExpression)
	{
		EventDefinition<string> eventDefinition = CoreResources.LogQueryExecutionPlanned(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, expressionPrinter.Print(queryExecutorExpression));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new QueryExpressionEventData(eventDefinition, QueryExecutionPlanned, queryExecutorExpression, expressionPrinter));
		}
	}

	private static string QueryExecutionPlanned(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		QueryExpressionEventData queryExpressionEventData = (QueryExpressionEventData)payload;
		return obj.GenerateMessage(queryExpressionEventData.ExpressionPrinter.Print(queryExpressionEventData.Expression));
	}

	public static void SensitiveDataLoggingEnabledWarning<TLoggerCategory>([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<TLoggerCategory> diagnostics) where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
	{
		EventDefinition eventDefinition = CoreResources.LogSensitiveDataLoggingEnabled(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new EventData(eventDefinition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage()));
		}
	}

	public static void PossibleUnintendedCollectionNavigationNullComparisonWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Query> diagnostics, [JetBrains.Annotations.NotNull] INavigation navigation)
	{
		EventDefinition<string> eventDefinition = CoreResources.LogPossibleUnintendedCollectionNavigationNullComparison(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigation.DeclaringEntityType.Name + "." + navigation.GetTargetType().Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new NavigationEventData(eventDefinition, PossibleUnintendedCollectionNavigationNullComparisonWarning, navigation));
		}
	}

	private static string PossibleUnintendedCollectionNavigationNullComparisonWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		NavigationEventData navigationEventData = (NavigationEventData)payload;
		return obj.GenerateMessage(navigationEventData.Navigation.DeclaringEntityType.Name + "." + navigationEventData.Navigation.GetTargetType().Name);
	}

	public static void PossibleUnintendedReferenceComparisonWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Query> diagnostics, [JetBrains.Annotations.NotNull] Expression left, [JetBrains.Annotations.NotNull] Expression right)
	{
		EventDefinition<object, object> eventDefinition = CoreResources.LogPossibleUnintendedReferenceComparison(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, left, right);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new BinaryExpressionEventData(eventDefinition, PossibleUnintendedReferenceComparisonWarning, left, right));
		}
	}

	private static string PossibleUnintendedReferenceComparisonWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<object, object> obj = (EventDefinition<object, object>)definition;
		BinaryExpressionEventData binaryExpressionEventData = (BinaryExpressionEventData)payload;
		return obj.GenerateMessage(binaryExpressionEventData.Left, binaryExpressionEventData.Right);
	}

	public static void ServiceProviderCreated([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics, [JetBrains.Annotations.NotNull] IServiceProvider serviceProvider)
	{
		EventDefinition eventDefinition = CoreResources.LogServiceProviderCreated(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ServiceProviderEventData(eventDefinition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), serviceProvider));
		}
	}

	public static void ManyServiceProvidersCreatedWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics, [JetBrains.Annotations.NotNull] ICollection<IServiceProvider> serviceProviders)
	{
		EventDefinition eventDefinition = CoreResources.LogManyServiceProvidersCreated(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ServiceProvidersEventData(eventDefinition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), serviceProviders));
		}
	}

	public static void ServiceProviderDebugInfo([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics, [JetBrains.Annotations.NotNull] IDictionary<string, string> newDebugInfo, [JetBrains.Annotations.NotNull] IList<IDictionary<string, string>> cachedDebugInfos)
	{
		EventDefinition<string> eventDefinition = CoreResources.LogServiceProviderDebugInfo(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, GenerateDebugInfoString(newDebugInfo, cachedDebugInfos));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ServiceProviderDebugInfoEventData(eventDefinition, (EventDefinitionBase d, EventData p) => ServiceProviderDebugInfo(d, p), newDebugInfo, cachedDebugInfos));
		}
	}

	private static string ServiceProviderDebugInfo(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		ServiceProviderDebugInfoEventData serviceProviderDebugInfoEventData = (ServiceProviderDebugInfoEventData)payload;
		return obj.GenerateMessage(GenerateDebugInfoString(serviceProviderDebugInfoEventData.NewDebugInfo, serviceProviderDebugInfoEventData.CachedDebugInfos));
	}

	private static string GenerateDebugInfoString(IDictionary<string, string> newDebugInfo, IList<IDictionary<string, string>> cachedDebugInfos)
	{
		List<string> list = null;
		foreach (IDictionary<string, string> cachedDebugInfo in cachedDebugInfos)
		{
			HashSet<string> hashSet = new HashSet<string>(newDebugInfo.Keys);
			List<string> list2 = new List<string>();
			foreach (string key in cachedDebugInfo.Keys)
			{
				if (newDebugInfo.TryGetValue(key, out var value))
				{
					if (!value.Equals(cachedDebugInfo[key]))
					{
						list2.Add(CoreStrings.ServiceProviderConfigChanged(key));
					}
				}
				else
				{
					list2.Add(CoreStrings.ServiceProviderConfigRemoved(key));
				}
				hashSet.Remove(key);
			}
			foreach (string item in hashSet)
			{
				list2.Add(CoreStrings.ServiceProviderConfigAdded(item));
			}
			if (list == null || list.Count > list2.Count)
			{
				list = list2;
			}
		}
		return string.Join(", ", list);
	}

	public static void ContextInitialized([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics, [JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] DbContextOptions contextOptions)
	{
		EventDefinition<string, string, string, string> eventDefinition = CoreResources.LogContextInitialized(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, ProductInfo.GetVersion(), context.GetType().ShortDisplayName(), context.Database.ProviderName, contextOptions.BuildOptionsFragment());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ContextInitializedEventData(eventDefinition, ContextInitialized, context, contextOptions));
		}
	}

	private static string ContextInitialized(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string, string> obj = (EventDefinition<string, string, string, string>)definition;
		ContextInitializedEventData contextInitializedEventData = (ContextInitializedEventData)payload;
		return obj.GenerateMessage(ProductInfo.GetVersion(), contextInitializedEventData.Context.GetType().ShortDisplayName(), contextInitializedEventData.Context.Database.ProviderName, contextInitializedEventData.ContextOptions.BuildOptionsFragment());
	}

	public static void ExecutionStrategyRetrying([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics, [JetBrains.Annotations.NotNull] IReadOnlyList<Exception> exceptionsEncountered, TimeSpan delay, bool async)
	{
		EventDefinition<int, string, Exception> eventDefinition = CoreResources.LogExecutionStrategyRetrying(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			Exception ex = exceptionsEncountered[exceptionsEncountered.Count - 1];
			eventDefinition.Log(diagnostics, logBehavior, (int)delay.TotalMilliseconds, Environment.NewLine, ex, ex);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ExecutionStrategyEventData(eventDefinition, ExecutionStrategyRetrying, exceptionsEncountered, delay, async));
		}
	}

	private static string ExecutionStrategyRetrying(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<int, string, Exception> obj = (EventDefinition<int, string, Exception>)definition;
		ExecutionStrategyEventData executionStrategyEventData = (ExecutionStrategyEventData)payload;
		return obj.GenerateMessage((int)executionStrategyEventData.Delay.TotalMilliseconds, Environment.NewLine, executionStrategyEventData.ExceptionsEncountered[executionStrategyEventData.ExceptionsEncountered.Count - 1]);
	}

	public static void LazyLoadOnDisposedContextWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics, [JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] object entityType, [JetBrains.Annotations.NotNull] string navigationName)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogLazyLoadOnDisposedContext(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigationName, entityType.GetType().ShortDisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new LazyLoadingEventData(eventDefinition, LazyLoadOnDisposedContextWarning, context, entityType, navigationName));
		}
	}

	private static string LazyLoadOnDisposedContextWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		LazyLoadingEventData lazyLoadingEventData = (LazyLoadingEventData)payload;
		return obj.GenerateMessage(lazyLoadingEventData.NavigationPropertyName, lazyLoadingEventData.Entity.GetType().ShortDisplayName());
	}

	public static void NavigationLazyLoading([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics, [JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] object entityType, [JetBrains.Annotations.NotNull] string navigationName)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogNavigationLazyLoading(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigationName, entityType.GetType().ShortDisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new LazyLoadingEventData(eventDefinition, NavigationLazyLoading, context, entityType, navigationName));
		}
	}

	private static string NavigationLazyLoading(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		LazyLoadingEventData lazyLoadingEventData = (LazyLoadingEventData)payload;
		return obj.GenerateMessage(lazyLoadingEventData.NavigationPropertyName, lazyLoadingEventData.Entity.GetType().ShortDisplayName());
	}

	public static void DetachedLazyLoadingWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics, [JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] object entityType, [JetBrains.Annotations.NotNull] string navigationName)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogDetachedLazyLoading(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigationName, entityType.GetType().ShortDisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new LazyLoadingEventData(eventDefinition, DetachedLazyLoadingWarning, context, entityType, navigationName));
		}
	}

	private static string DetachedLazyLoadingWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		LazyLoadingEventData lazyLoadingEventData = (LazyLoadingEventData)payload;
		return obj.GenerateMessage(lazyLoadingEventData.NavigationPropertyName, lazyLoadingEventData.Entity.GetType().ShortDisplayName());
	}

	public static void RedundantAddServicesCallWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics, [JetBrains.Annotations.NotNull] IServiceProvider serviceProvider)
	{
		EventDefinition eventDefinition = CoreResources.LogRedundantAddServicesCall(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ServiceProviderEventData(eventDefinition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), serviceProvider));
		}
	}

	public static void ShadowPropertyCreated([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics, [JetBrains.Annotations.NotNull] IProperty property)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogShadowPropertyCreated(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, property.Name, property.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyEventData(eventDefinition, ShadowPropertyCreated, property));
		}
	}

	private static string ShadowPropertyCreated(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		PropertyEventData propertyEventData = (PropertyEventData)payload;
		return obj.GenerateMessage(propertyEventData.Property.Name, propertyEventData.Property.DeclaringEntityType.DisplayName());
	}

	public static void CollectionWithoutComparer([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics, [JetBrains.Annotations.NotNull] IProperty property)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogCollectionWithoutComparer(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, property.Name, property.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyEventData(eventDefinition, CollectionWithoutComparer, property));
		}
	}

	private static string CollectionWithoutComparer(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		PropertyEventData propertyEventData = (PropertyEventData)payload;
		return obj.GenerateMessage(propertyEventData.Property.Name, propertyEventData.Property.DeclaringEntityType.DisplayName());
	}

	public static void RedundantIndexRemoved([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] IReadOnlyList<IPropertyBase> redundantIndex, [JetBrains.Annotations.NotNull] IReadOnlyList<IPropertyBase> otherIndex)
	{
		EventDefinition<string, string, string> eventDefinition = CoreResources.LogRedundantIndexRemoved(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, redundantIndex.Format(), redundantIndex.First().DeclaringType.DisplayName(), otherIndex.Format());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoPropertyBaseCollectionsEventData(eventDefinition, RedundantIndexRemoved, redundantIndex, otherIndex));
		}
	}

	private static string RedundantIndexRemoved(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string> obj = (EventDefinition<string, string, string>)definition;
		TwoPropertyBaseCollectionsEventData twoPropertyBaseCollectionsEventData = (TwoPropertyBaseCollectionsEventData)payload;
		return obj.GenerateMessage(twoPropertyBaseCollectionsEventData.FirstPropertyCollection.Format(), twoPropertyBaseCollectionsEventData.FirstPropertyCollection.First().DeclaringType.DisplayName(), twoPropertyBaseCollectionsEventData.SecondPropertyCollection.Format());
	}

	public static void RedundantForeignKeyWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics, [JetBrains.Annotations.NotNull] IForeignKey redundantForeignKey)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogRedundantForeignKey(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, redundantForeignKey.Properties.Format(), redundantForeignKey.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ForeignKeyEventData(eventDefinition, RedundantForeignKeyWarning, redundantForeignKey));
		}
	}

	private static string RedundantForeignKeyWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		ForeignKeyEventData foreignKeyEventData = (ForeignKeyEventData)payload;
		return obj.GenerateMessage(foreignKeyEventData.ForeignKey.Properties.Format(), foreignKeyEventData.ForeignKey.DeclaringEntityType.DisplayName());
	}

	public static void IncompatibleMatchingForeignKeyProperties([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] IReadOnlyList<IPropertyBase> foreignKeyProperties, [JetBrains.Annotations.NotNull] IReadOnlyList<IPropertyBase> principalKeyProperties)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogIncompatibleMatchingForeignKeyProperties(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, foreignKeyProperties.Format(includeTypes: true), principalKeyProperties.Format(includeTypes: true));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoPropertyBaseCollectionsEventData(eventDefinition, IncompatibleMatchingForeignKeyProperties, foreignKeyProperties, principalKeyProperties));
		}
	}

	private static string IncompatibleMatchingForeignKeyProperties(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		TwoPropertyBaseCollectionsEventData twoPropertyBaseCollectionsEventData = (TwoPropertyBaseCollectionsEventData)payload;
		return obj.GenerateMessage(twoPropertyBaseCollectionsEventData.FirstPropertyCollection.Format(includeTypes: true), twoPropertyBaseCollectionsEventData.SecondPropertyCollection.Format(includeTypes: true));
	}

	public static void RequiredAttributeInverted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] INavigation navigation)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogRequiredAttributeInverted(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigation.Name, navigation.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new NavigationEventData(eventDefinition, RequiredAttributeInverted, navigation));
		}
	}

	private static string RequiredAttributeInverted(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		NavigationEventData navigationEventData = (NavigationEventData)payload;
		return obj.GenerateMessage(navigationEventData.Navigation.Name, navigationEventData.Navigation.DeclaringEntityType.DisplayName());
	}

	public static void NonNullableInverted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] INavigation navigation)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogNonNullableInverted(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigation.Name, navigation.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new NavigationEventData(eventDefinition, NonNullableInverted, navigation));
		}
	}

	private static string NonNullableInverted(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		NavigationEventData navigationEventData = (NavigationEventData)payload;
		return obj.GenerateMessage(navigationEventData.Navigation.Name, navigationEventData.Navigation.DeclaringEntityType.DisplayName());
	}

	public static void RequiredAttributeOnBothNavigations([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] INavigation firstNavigation, [JetBrains.Annotations.NotNull] INavigation secondNavigation)
	{
		EventDefinition<string, string, string, string> eventDefinition = CoreResources.LogRequiredAttributeOnBothNavigations(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, firstNavigation.DeclaringEntityType.DisplayName(), firstNavigation.Name, secondNavigation.DeclaringEntityType.DisplayName(), secondNavigation.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoPropertyBaseCollectionsEventData(eventDefinition, RequiredAttributeOnBothNavigations, new INavigation[1] { firstNavigation }, new INavigation[1] { secondNavigation }));
		}
	}

	private static string RequiredAttributeOnBothNavigations(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string, string> obj = (EventDefinition<string, string, string, string>)definition;
		TwoPropertyBaseCollectionsEventData obj2 = (TwoPropertyBaseCollectionsEventData)payload;
		IPropertyBase propertyBase = obj2.FirstPropertyCollection[0];
		IPropertyBase propertyBase2 = obj2.SecondPropertyCollection[0];
		return obj.GenerateMessage(propertyBase.DeclaringType.DisplayName(), propertyBase.Name, propertyBase2.DeclaringType.DisplayName(), propertyBase2.Name);
	}

	public static void NonNullableReferenceOnBothNavigations([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] INavigation firstNavigation, [JetBrains.Annotations.NotNull] INavigation secondNavigation)
	{
		EventDefinition<string, string, string, string> eventDefinition = CoreResources.LogNonNullableReferenceOnBothNavigations(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, firstNavigation.DeclaringEntityType.DisplayName(), firstNavigation.Name, secondNavigation.DeclaringEntityType.DisplayName(), secondNavigation.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoPropertyBaseCollectionsEventData(eventDefinition, NonNullableReferenceOnBothNavigations, new INavigation[1] { firstNavigation }, new INavigation[1] { secondNavigation }));
		}
	}

	private static string NonNullableReferenceOnBothNavigations(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string, string> obj = (EventDefinition<string, string, string, string>)definition;
		TwoPropertyBaseCollectionsEventData obj2 = (TwoPropertyBaseCollectionsEventData)payload;
		IPropertyBase propertyBase = obj2.FirstPropertyCollection[0];
		IPropertyBase propertyBase2 = obj2.SecondPropertyCollection[0];
		return obj.GenerateMessage(propertyBase.DeclaringType.DisplayName(), propertyBase.Name, propertyBase2.DeclaringType.DisplayName(), propertyBase2.Name);
	}

	public static void RequiredAttributeOnDependent([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] INavigation navigation)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogRequiredAttributeOnDependent(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigation.Name, navigation.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new NavigationEventData(eventDefinition, RequiredAttributeOnDependent, navigation));
		}
	}

	private static string RequiredAttributeOnDependent(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		NavigationEventData navigationEventData = (NavigationEventData)payload;
		return obj.GenerateMessage(navigationEventData.Navigation.Name, navigationEventData.Navigation.DeclaringEntityType.DisplayName());
	}

	public static void NonNullableReferenceOnDependent([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] INavigation navigation)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogNonNullableReferenceOnDependent(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigation.Name, navigation.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new NavigationEventData(eventDefinition, NonNullableReferenceOnDependent, navigation));
		}
	}

	private static string NonNullableReferenceOnDependent(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		NavigationEventData navigationEventData = (NavigationEventData)payload;
		return obj.GenerateMessage(navigationEventData.Navigation.Name, navigationEventData.Navigation.DeclaringEntityType.DisplayName());
	}

	public static void RequiredAttributeOnCollection([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] INavigation navigation)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogRequiredAttributeOnCollection(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigation.Name, navigation.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new NavigationEventData(eventDefinition, RequiredAttributeOnCollection, navigation));
		}
	}

	private static string RequiredAttributeOnCollection(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		NavigationEventData navigationEventData = (NavigationEventData)payload;
		return obj.GenerateMessage(navigationEventData.Navigation.Name, navigationEventData.Navigation.DeclaringEntityType.DisplayName());
	}

	public static void ConflictingShadowForeignKeysWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] IForeignKey foreignKey)
	{
		EventDefinition<string, string, string> eventDefinition = CoreResources.LogConflictingShadowForeignKeys(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			string text = foreignKey.DeclaringEntityType.DisplayName();
			eventDefinition.Log(diagnostics, logBehavior, text, foreignKey.PrincipalEntityType.DisplayName(), text);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ForeignKeyEventData(eventDefinition, ConflictingShadowForeignKeysWarning, foreignKey));
		}
	}

	private static string ConflictingShadowForeignKeysWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string> obj = (EventDefinition<string, string, string>)definition;
		ForeignKeyEventData foreignKeyEventData = (ForeignKeyEventData)payload;
		return obj.GenerateMessage(foreignKeyEventData.ForeignKey.DeclaringEntityType.DisplayName(), foreignKeyEventData.ForeignKey.PrincipalEntityType.DisplayName(), foreignKeyEventData.ForeignKey.DeclaringEntityType.DisplayName());
	}

	public static void MultiplePrimaryKeyCandidates([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] IProperty firstProperty, [JetBrains.Annotations.NotNull] IProperty secondProperty)
	{
		EventDefinition<string, string, string> eventDefinition = CoreResources.LogMultiplePrimaryKeyCandidates(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, firstProperty.Name, secondProperty.Name, firstProperty.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoPropertyBaseCollectionsEventData(eventDefinition, MultiplePrimaryKeyCandidates, new IProperty[1] { firstProperty }, new IProperty[1] { secondProperty }));
		}
	}

	private static string MultiplePrimaryKeyCandidates(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string> obj = (EventDefinition<string, string, string>)definition;
		TwoPropertyBaseCollectionsEventData obj2 = (TwoPropertyBaseCollectionsEventData)payload;
		IPropertyBase propertyBase = obj2.FirstPropertyCollection[0];
		return obj.GenerateMessage(arg2: obj2.SecondPropertyCollection[0].Name, arg1: propertyBase.Name, arg3: propertyBase.DeclaringType.DisplayName());
	}

	public static void MultipleNavigationProperties([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] IEnumerable<Tuple<MemberInfo, Type>> firstPropertyCollection, [JetBrains.Annotations.NotNull] IEnumerable<Tuple<MemberInfo, Type>> secondPropertyCollection)
	{
		EventDefinition<string, string, string, string> eventDefinition = CoreResources.LogMultipleNavigationProperties(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, firstPropertyCollection.First().Item2.ShortDisplayName(), secondPropertyCollection.First().Item2.ShortDisplayName(), Property.Format(firstPropertyCollection.Select((Tuple<MemberInfo, Type> p) => p.Item1?.Name)), Property.Format(secondPropertyCollection.Select((Tuple<MemberInfo, Type> p) => p.Item1?.Name)));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoUnmappedPropertyCollectionsEventData(eventDefinition, MultipleNavigationProperties, firstPropertyCollection, secondPropertyCollection));
		}
	}

	private static string MultipleNavigationProperties(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string, string> obj = (EventDefinition<string, string, string, string>)definition;
		TwoUnmappedPropertyCollectionsEventData twoUnmappedPropertyCollectionsEventData = (TwoUnmappedPropertyCollectionsEventData)payload;
		return obj.GenerateMessage(twoUnmappedPropertyCollectionsEventData.FirstPropertyCollection.First().Item2.ShortDisplayName(), twoUnmappedPropertyCollectionsEventData.SecondPropertyCollection.First().Item2.ShortDisplayName(), Property.Format(twoUnmappedPropertyCollectionsEventData.FirstPropertyCollection.Select((Tuple<MemberInfo, Type> i) => i.Item1?.Name)), Property.Format(twoUnmappedPropertyCollectionsEventData.SecondPropertyCollection.Select((Tuple<MemberInfo, Type> i) => i.Item1?.Name)));
	}

	public static void MultipleInversePropertiesSameTargetWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] IEnumerable<Tuple<MemberInfo, Type>> conflictingNavigations, [JetBrains.Annotations.NotNull] MemberInfo inverseNavigation, [JetBrains.Annotations.NotNull] Type targetType)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogMultipleInversePropertiesSameTarget(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, string.Join(", ", conflictingNavigations.Select((Tuple<MemberInfo, Type> n) => n.Item2.ShortDisplayName() + "." + n.Item1.Name)), inverseNavigation.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoUnmappedPropertyCollectionsEventData(eventDefinition, MultipleInversePropertiesSameTargetWarning, conflictingNavigations, new Tuple<MemberInfo, Type>[1]
			{
				new Tuple<MemberInfo, Type>(inverseNavigation, targetType)
			}));
		}
	}

	private static string MultipleInversePropertiesSameTargetWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		TwoUnmappedPropertyCollectionsEventData twoUnmappedPropertyCollectionsEventData = (TwoUnmappedPropertyCollectionsEventData)payload;
		return obj.GenerateMessage(string.Join(", ", twoUnmappedPropertyCollectionsEventData.FirstPropertyCollection.Select((Tuple<MemberInfo, Type> n) => n.Item2.ShortDisplayName() + "." + n.Item1.Name)), twoUnmappedPropertyCollectionsEventData.SecondPropertyCollection.First().Item1.Name);
	}

	public static void NonDefiningInverseNavigationWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] IEntityType declaringType, [JetBrains.Annotations.NotNull] MemberInfo navigation, [JetBrains.Annotations.NotNull] IEntityType targetType, [JetBrains.Annotations.NotNull] MemberInfo inverseNavigation, [JetBrains.Annotations.NotNull] MemberInfo definingNavigation)
	{
		EventDefinition<string, string, string, string, string> eventDefinition = CoreResources.LogNonDefiningInverseNavigation(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, targetType.DisplayName(), inverseNavigation.Name, declaringType.DisplayName(), navigation.Name, definingNavigation.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoUnmappedPropertyCollectionsEventData(eventDefinition, NonDefiningInverseNavigationWarning, new Tuple<MemberInfo, Type>[1]
			{
				new Tuple<MemberInfo, Type>(navigation, declaringType.ClrType)
			}, new Tuple<MemberInfo, Type>[2]
			{
				new Tuple<MemberInfo, Type>(inverseNavigation, targetType.ClrType),
				new Tuple<MemberInfo, Type>(definingNavigation, targetType.ClrType)
			}));
		}
	}

	private static string NonDefiningInverseNavigationWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string, string, string> obj = (EventDefinition<string, string, string, string, string>)definition;
		TwoUnmappedPropertyCollectionsEventData obj2 = (TwoUnmappedPropertyCollectionsEventData)payload;
		Tuple<MemberInfo, Type> tuple = obj2.FirstPropertyCollection.First();
		Tuple<MemberInfo, Type> tuple2 = obj2.SecondPropertyCollection.First();
		return obj.GenerateMessage(arg5: obj2.SecondPropertyCollection.Last().Item1.Name, arg1: tuple2.Item2.ShortDisplayName(), arg2: tuple2.Item1.Name, arg3: tuple.Item2.ShortDisplayName(), arg4: tuple.Item1.Name);
	}

	public static void NonOwnershipInverseNavigationWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] IEntityType declaringType, [JetBrains.Annotations.NotNull] MemberInfo navigation, [JetBrains.Annotations.NotNull] IEntityType targetType, [JetBrains.Annotations.NotNull] MemberInfo inverseNavigation, [JetBrains.Annotations.NotNull] MemberInfo ownershipNavigation)
	{
		EventDefinition<string, string, string, string, string> eventDefinition = CoreResources.LogNonOwnershipInverseNavigation(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, targetType.DisplayName(), inverseNavigation.Name, declaringType.DisplayName(), navigation.Name, ownershipNavigation.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoUnmappedPropertyCollectionsEventData(eventDefinition, NonOwnershipInverseNavigationWarning, new Tuple<MemberInfo, Type>[1]
			{
				new Tuple<MemberInfo, Type>(navigation, declaringType.ClrType)
			}, new Tuple<MemberInfo, Type>[2]
			{
				new Tuple<MemberInfo, Type>(inverseNavigation, targetType.ClrType),
				new Tuple<MemberInfo, Type>(ownershipNavigation, targetType.ClrType)
			}));
		}
	}

	private static string NonOwnershipInverseNavigationWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string, string, string> obj = (EventDefinition<string, string, string, string, string>)definition;
		TwoUnmappedPropertyCollectionsEventData obj2 = (TwoUnmappedPropertyCollectionsEventData)payload;
		Tuple<MemberInfo, Type> tuple = obj2.FirstPropertyCollection.First();
		Tuple<MemberInfo, Type> tuple2 = obj2.SecondPropertyCollection.First();
		return obj.GenerateMessage(arg5: obj2.SecondPropertyCollection.Last().Item1.Name, arg1: tuple2.Item2.ShortDisplayName(), arg2: tuple2.Item1.Name, arg3: tuple.Item2.ShortDisplayName(), arg4: tuple.Item1.Name);
	}

	public static void ForeignKeyAttributesOnBothPropertiesWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] INavigation firstNavigation, [JetBrains.Annotations.NotNull] INavigation secondNavigation, [JetBrains.Annotations.NotNull] MemberInfo firstProperty, [JetBrains.Annotations.NotNull] MemberInfo secondProperty)
	{
		EventDefinition<string, string, string, string, string, string> eventDefinition = CoreResources.LogForeignKeyAttributesOnBothProperties(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, firstNavigation.DeclaringEntityType.ClrType.ShortDisplayName(), firstNavigation.GetIdentifyingMemberInfo().Name, secondNavigation.DeclaringEntityType.ClrType.ShortDisplayName(), secondNavigation.GetIdentifyingMemberInfo().Name, firstProperty.Name, secondProperty.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoUnmappedPropertyCollectionsEventData(eventDefinition, ForeignKeyAttributesOnBothPropertiesWarning, new Tuple<MemberInfo, Type>[2]
			{
				new Tuple<MemberInfo, Type>(firstNavigation.GetIdentifyingMemberInfo(), firstNavigation.DeclaringEntityType.ClrType),
				new Tuple<MemberInfo, Type>(firstProperty, firstNavigation.DeclaringEntityType.ClrType)
			}, new Tuple<MemberInfo, Type>[2]
			{
				new Tuple<MemberInfo, Type>(secondNavigation.GetIdentifyingMemberInfo(), secondNavigation.DeclaringEntityType.ClrType),
				new Tuple<MemberInfo, Type>(secondProperty, secondNavigation.DeclaringEntityType.ClrType)
			}));
		}
	}

	private static string ForeignKeyAttributesOnBothPropertiesWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string, string, string, string> obj = (EventDefinition<string, string, string, string, string, string>)definition;
		TwoUnmappedPropertyCollectionsEventData obj2 = (TwoUnmappedPropertyCollectionsEventData)payload;
		Tuple<MemberInfo, Type> tuple = obj2.FirstPropertyCollection.First();
		Tuple<MemberInfo, Type> tuple2 = obj2.FirstPropertyCollection.Last();
		Tuple<MemberInfo, Type> tuple3 = obj2.SecondPropertyCollection.First();
		return obj.GenerateMessage(arg6: obj2.SecondPropertyCollection.Last().Item1.Name, arg1: tuple.Item2.ShortDisplayName(), arg2: tuple.Item1.Name, arg3: tuple3.Item2.ShortDisplayName(), arg4: tuple3.Item1.Name, arg5: tuple2.Item1.Name);
	}

	public static void ForeignKeyAttributesOnBothNavigationsWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] INavigation firstNavigation, [JetBrains.Annotations.NotNull] INavigation secondNavigation)
	{
		EventDefinition<string, string, string, string> eventDefinition = CoreResources.LogForeignKeyAttributesOnBothNavigations(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, firstNavigation.DeclaringEntityType.DisplayName(), firstNavigation.Name, secondNavigation.DeclaringEntityType.DisplayName(), secondNavigation.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoPropertyBaseCollectionsEventData(eventDefinition, ForeignKeyAttributesOnBothNavigationsWarning, new INavigation[1] { firstNavigation }, new INavigation[1] { secondNavigation }));
		}
	}

	private static string ForeignKeyAttributesOnBothNavigationsWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string, string> obj = (EventDefinition<string, string, string, string>)definition;
		TwoPropertyBaseCollectionsEventData obj2 = (TwoPropertyBaseCollectionsEventData)payload;
		IPropertyBase propertyBase = obj2.FirstPropertyCollection[0];
		IPropertyBase propertyBase2 = obj2.SecondPropertyCollection[0];
		return obj.GenerateMessage(propertyBase.DeclaringType.DisplayName(), propertyBase.Name, propertyBase2.DeclaringType.DisplayName(), propertyBase2.Name);
	}

	public static void ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model> diagnostics, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] MemberInfo property)
	{
		EventDefinition<string, string, string, string> eventDefinition = CoreResources.LogConflictingForeignKeyAttributesOnNavigationAndProperty(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigation.DeclaringEntityType.ClrType.ShortDisplayName(), navigation.GetIdentifyingMemberInfo()?.Name, property.DeclaringType.ShortDisplayName(), property.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TwoUnmappedPropertyCollectionsEventData(eventDefinition, ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning, new Tuple<MemberInfo, Type>[1]
			{
				new Tuple<MemberInfo, Type>(navigation.GetIdentifyingMemberInfo(), navigation.DeclaringEntityType.ClrType)
			}, new Tuple<MemberInfo, Type>[1]
			{
				new Tuple<MemberInfo, Type>(property, property.DeclaringType)
			}));
		}
	}

	private static string ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string, string> obj = (EventDefinition<string, string, string, string>)definition;
		TwoUnmappedPropertyCollectionsEventData obj2 = (TwoUnmappedPropertyCollectionsEventData)payload;
		Tuple<MemberInfo, Type> tuple = obj2.FirstPropertyCollection.First();
		Tuple<MemberInfo, Type> tuple2 = obj2.SecondPropertyCollection.First();
		return obj.GenerateMessage(tuple.Item2.ShortDisplayName(), tuple.Item1.Name, tuple2.Item2.ShortDisplayName(), tuple2.Item1.Name);
	}

	public static void DetectChangesStarting([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] DbContext context)
	{
		EventDefinition<string> eventDefinition = CoreResources.LogDetectChangesStarting(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, context.GetType().ShortDisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new DbContextEventData(eventDefinition, DetectChangesStarting, context));
		}
	}

	private static string DetectChangesStarting(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		DbContextEventData dbContextEventData = (DbContextEventData)payload;
		return obj.GenerateMessage(dbContextEventData.Context.GetType().ShortDisplayName());
	}

	public static void DetectChangesCompleted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] DbContext context)
	{
		EventDefinition<string> eventDefinition = CoreResources.LogDetectChangesCompleted(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, context.GetType().ShortDisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new DbContextEventData(eventDefinition, DetectChangesCompleted, context));
		}
	}

	private static string DetectChangesCompleted(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		DbContextEventData dbContextEventData = (DbContextEventData)payload;
		return obj.GenerateMessage(dbContextEventData.Context.GetType().ShortDisplayName());
	}

	public static void PropertyChangeDetected([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object oldValue, [JetBrains.Annotations.CanBeNull] object newValue)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogPropertyChangeDetected(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, property.DeclaringEntityType.ShortName(), property.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyChangedEventData(eventDefinition, PropertyChangeDetected, new EntityEntry(internalEntityEntry), property, oldValue, newValue));
		}
	}

	private static string PropertyChangeDetected(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		PropertyChangedEventData propertyChangedEventData = (PropertyChangedEventData)payload;
		return obj.GenerateMessage(propertyChangedEventData.Property.DeclaringEntityType.ShortName(), propertyChangedEventData.Property.Name);
	}

	public static void PropertyChangeDetectedSensitive([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object oldValue, [JetBrains.Annotations.CanBeNull] object newValue)
	{
		EventDefinition<string, string, object, object, string> eventDefinition = CoreResources.LogPropertyChangeDetectedSensitive(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, property.DeclaringEntityType.ShortName(), property.Name, oldValue, newValue, internalEntityEntry.BuildCurrentValuesString(property.DeclaringEntityType.FindPrimaryKey().Properties));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyChangedEventData(eventDefinition, PropertyChangeDetectedSensitive, new EntityEntry(internalEntityEntry), property, oldValue, newValue));
		}
	}

	private static string PropertyChangeDetectedSensitive(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, object, object, string> obj = (EventDefinition<string, string, object, object, string>)definition;
		PropertyChangedEventData propertyChangedEventData = (PropertyChangedEventData)payload;
		return obj.GenerateMessage(propertyChangedEventData.Property.DeclaringEntityType.ShortName(), propertyChangedEventData.Property.Name, propertyChangedEventData.OldValue, propertyChangedEventData.NewValue, propertyChangedEventData.EntityEntry.GetInfrastructure().BuildCurrentValuesString(propertyChangedEventData.Property.DeclaringEntityType.FindPrimaryKey().Properties));
	}

	public static void ForeignKeyChangeDetected([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object oldValue, [JetBrains.Annotations.CanBeNull] object newValue)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogForeignKeyChangeDetected(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, property.DeclaringEntityType.ShortName(), property.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyChangedEventData(eventDefinition, ForeignKeyChangeDetected, new EntityEntry(internalEntityEntry), property, oldValue, newValue));
		}
	}

	private static string ForeignKeyChangeDetected(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		PropertyChangedEventData propertyChangedEventData = (PropertyChangedEventData)payload;
		return obj.GenerateMessage(propertyChangedEventData.Property.DeclaringEntityType.ShortName(), propertyChangedEventData.Property.Name);
	}

	public static void ForeignKeyChangeDetectedSensitive([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object oldValue, [JetBrains.Annotations.CanBeNull] object newValue)
	{
		EventDefinition<string, string, object, object, string> eventDefinition = CoreResources.LogForeignKeyChangeDetectedSensitive(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, property.DeclaringEntityType.ShortName(), property.Name, oldValue, newValue, internalEntityEntry.BuildCurrentValuesString(property.DeclaringEntityType.FindPrimaryKey().Properties));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyChangedEventData(eventDefinition, ForeignKeyChangeDetectedSensitive, new EntityEntry(internalEntityEntry), property, oldValue, newValue));
		}
	}

	private static string ForeignKeyChangeDetectedSensitive(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, object, object, string> obj = (EventDefinition<string, string, object, object, string>)definition;
		PropertyChangedEventData propertyChangedEventData = (PropertyChangedEventData)payload;
		return obj.GenerateMessage(propertyChangedEventData.Property.DeclaringEntityType.ShortName(), propertyChangedEventData.Property.Name, propertyChangedEventData.OldValue, propertyChangedEventData.NewValue, propertyChangedEventData.EntityEntry.GetInfrastructure().BuildCurrentValuesString(propertyChangedEventData.Property.DeclaringEntityType.FindPrimaryKey().Properties));
	}

	public static void CollectionChangeDetected([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] ISet<object> added, [JetBrains.Annotations.NotNull] ISet<object> removed)
	{
		EventDefinition<int, int, string, string> eventDefinition = CoreResources.LogCollectionChangeDetected(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, added.Count, removed.Count, navigation.DeclaringEntityType.ShortName(), navigation.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new CollectionChangedEventData(eventDefinition, CollectionChangeDetected, new EntityEntry(internalEntityEntry), navigation, added, removed));
		}
	}

	private static string CollectionChangeDetected(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<int, int, string, string> obj = (EventDefinition<int, int, string, string>)definition;
		CollectionChangedEventData collectionChangedEventData = (CollectionChangedEventData)payload;
		return obj.GenerateMessage(collectionChangedEventData.Added.Count(), collectionChangedEventData.Removed.Count(), collectionChangedEventData.Navigation.DeclaringEntityType.ShortName(), collectionChangedEventData.Navigation.Name);
	}

	public static void CollectionChangeDetectedSensitive([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] ISet<object> added, [JetBrains.Annotations.NotNull] ISet<object> removed)
	{
		EventDefinition<int, int, string, string, string> eventDefinition = CoreResources.LogCollectionChangeDetectedSensitive(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, added.Count, removed.Count, navigation.DeclaringEntityType.ShortName(), navigation.Name, internalEntityEntry.BuildCurrentValuesString(navigation.DeclaringEntityType.FindPrimaryKey().Properties));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new CollectionChangedEventData(eventDefinition, CollectionChangeDetectedSensitive, new EntityEntry(internalEntityEntry), navigation, added, removed));
		}
	}

	private static string CollectionChangeDetectedSensitive(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<int, int, string, string, string> obj = (EventDefinition<int, int, string, string, string>)definition;
		CollectionChangedEventData collectionChangedEventData = (CollectionChangedEventData)payload;
		return obj.GenerateMessage(collectionChangedEventData.Added.Count(), collectionChangedEventData.Removed.Count(), collectionChangedEventData.Navigation.DeclaringEntityType.ShortName(), collectionChangedEventData.Navigation.Name, collectionChangedEventData.EntityEntry.GetInfrastructure().BuildCurrentValuesString(collectionChangedEventData.Navigation.DeclaringEntityType.FindPrimaryKey().Properties));
	}

	public static void ReferenceChangeDetected([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.CanBeNull] object oldValue, [JetBrains.Annotations.CanBeNull] object newValue)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogReferenceChangeDetected(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigation.DeclaringEntityType.ShortName(), navigation.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ReferenceChangedEventData(eventDefinition, ReferenceChangeDetected, new EntityEntry(internalEntityEntry), navigation, oldValue, newValue));
		}
	}

	private static string ReferenceChangeDetected(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		ReferenceChangedEventData referenceChangedEventData = (ReferenceChangedEventData)payload;
		return obj.GenerateMessage(referenceChangedEventData.Navigation.DeclaringEntityType.ShortName(), referenceChangedEventData.Navigation.Name);
	}

	public static void ReferenceChangeDetectedSensitive([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.CanBeNull] object oldValue, [JetBrains.Annotations.CanBeNull] object newValue)
	{
		EventDefinition<string, string, string> eventDefinition = CoreResources.LogReferenceChangeDetectedSensitive(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, navigation.DeclaringEntityType.ShortName(), navigation.Name, internalEntityEntry.BuildCurrentValuesString(navigation.DeclaringEntityType.FindPrimaryKey().Properties));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ReferenceChangedEventData(eventDefinition, ReferenceChangeDetectedSensitive, new EntityEntry(internalEntityEntry), navigation, oldValue, newValue));
		}
	}

	private static string ReferenceChangeDetectedSensitive(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string> obj = (EventDefinition<string, string, string>)definition;
		ReferenceChangedEventData referenceChangedEventData = (ReferenceChangedEventData)payload;
		return obj.GenerateMessage(referenceChangedEventData.Navigation.DeclaringEntityType.ShortName(), referenceChangedEventData.Navigation.Name, referenceChangedEventData.EntityEntry.GetInfrastructure().BuildCurrentValuesString(referenceChangedEventData.Navigation.DeclaringEntityType.FindPrimaryKey().Properties));
	}

	public static void StartedTracking([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry)
	{
		EventDefinition<string, string> eventDefinition = CoreResources.LogStartedTracking(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, internalEntityEntry.StateManager.Context.GetType().ShortDisplayName(), internalEntityEntry.EntityType.ShortName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new EntityEntryEventData(eventDefinition, StartedTracking, new EntityEntry(internalEntityEntry)));
		}
	}

	private static string StartedTracking(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		EntityEntryEventData entityEntryEventData = (EntityEntryEventData)payload;
		return obj.GenerateMessage(entityEntryEventData.EntityEntry.Context.GetType().ShortDisplayName(), entityEntryEventData.EntityEntry.Metadata.ShortName());
	}

	public static void StartedTrackingSensitive([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry)
	{
		EventDefinition<string, string, string> eventDefinition = CoreResources.LogStartedTrackingSensitive(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, internalEntityEntry.StateManager.Context.GetType().ShortDisplayName(), internalEntityEntry.EntityType.ShortName(), internalEntityEntry.BuildCurrentValuesString(internalEntityEntry.EntityType.FindPrimaryKey().Properties));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new EntityEntryEventData(eventDefinition, StartedTrackingSensitive, new EntityEntry(internalEntityEntry)));
		}
	}

	private static string StartedTrackingSensitive(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string> obj = (EventDefinition<string, string, string>)definition;
		EntityEntryEventData entityEntryEventData = (EntityEntryEventData)payload;
		return obj.GenerateMessage(entityEntryEventData.EntityEntry.Context.GetType().ShortDisplayName(), entityEntryEventData.EntityEntry.Metadata.ShortName(), entityEntryEventData.EntityEntry.GetInfrastructure().BuildCurrentValuesString(entityEntryEventData.EntityEntry.Metadata.FindPrimaryKey().Properties));
	}

	public static void StateChanged([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, EntityState oldState, EntityState newState)
	{
		EventDefinition<string, string, EntityState, EntityState> eventDefinition = CoreResources.LogStateChanged(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, internalEntityEntry.EntityType.ShortName(), internalEntityEntry.StateManager.Context.GetType().ShortDisplayName(), oldState, newState);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new StateChangedEventData(eventDefinition, StateChanged, new EntityEntry(internalEntityEntry), oldState, newState));
		}
	}

	private static string StateChanged(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, EntityState, EntityState> obj = (EventDefinition<string, string, EntityState, EntityState>)definition;
		StateChangedEventData stateChangedEventData = (StateChangedEventData)payload;
		return obj.GenerateMessage(stateChangedEventData.EntityEntry.Metadata.ShortName(), stateChangedEventData.EntityEntry.Context.GetType().ShortDisplayName(), stateChangedEventData.OldState, stateChangedEventData.NewState);
	}

	public static void StateChangedSensitive([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, EntityState oldState, EntityState newState)
	{
		EventDefinition<string, string, string, EntityState, EntityState> eventDefinition = CoreResources.LogStateChangedSensitive(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, internalEntityEntry.EntityType.ShortName(), internalEntityEntry.BuildCurrentValuesString(internalEntityEntry.EntityType.FindPrimaryKey().Properties), internalEntityEntry.StateManager.Context.GetType().ShortDisplayName(), oldState, newState);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new StateChangedEventData(eventDefinition, StateChangedSensitive, new EntityEntry(internalEntityEntry), oldState, newState));
		}
	}

	private static string StateChangedSensitive(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string, EntityState, EntityState> obj = (EventDefinition<string, string, string, EntityState, EntityState>)definition;
		StateChangedEventData stateChangedEventData = (StateChangedEventData)payload;
		return obj.GenerateMessage(stateChangedEventData.EntityEntry.Metadata.ShortName(), stateChangedEventData.EntityEntry.GetInfrastructure().BuildCurrentValuesString(stateChangedEventData.EntityEntry.Metadata.FindPrimaryKey().Properties), stateChangedEventData.EntityEntry.Context.GetType().ShortDisplayName(), stateChangedEventData.OldState, stateChangedEventData.NewState);
	}

	public static void ValueGenerated([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object value, bool temporary)
	{
		EventDefinition<string, string, string> eventDefinition = (temporary ? CoreResources.LogTempValueGenerated(diagnostics) : CoreResources.LogValueGenerated(diagnostics));
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, internalEntityEntry.StateManager.Context.GetType().ShortDisplayName(), property.Name, internalEntityEntry.EntityType.ShortName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyValueEventData(eventDefinition, ValueGenerated, new EntityEntry(internalEntityEntry), property, value));
		}
	}

	private static string ValueGenerated(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, string> obj = (EventDefinition<string, string, string>)definition;
		PropertyValueEventData propertyValueEventData = (PropertyValueEventData)payload;
		return obj.GenerateMessage(propertyValueEventData.EntityEntry.Context.GetType().ShortDisplayName(), propertyValueEventData.Property.Name, propertyValueEventData.EntityEntry.Metadata.ShortName());
	}

	public static void ValueGeneratedSensitive([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object value, bool temporary)
	{
		EventDefinition<string, object, string, string> eventDefinition = (temporary ? CoreResources.LogTempValueGeneratedSensitive(diagnostics) : CoreResources.LogValueGeneratedSensitive(diagnostics));
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, internalEntityEntry.StateManager.Context.GetType().ShortDisplayName(), value, property.Name, internalEntityEntry.EntityType.ShortName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyValueEventData(eventDefinition, ValueGeneratedSensitive, new EntityEntry(internalEntityEntry), property, value));
		}
	}

	private static string ValueGeneratedSensitive(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, object, string, string> obj = (EventDefinition<string, object, string, string>)definition;
		PropertyValueEventData propertyValueEventData = (PropertyValueEventData)payload;
		return obj.GenerateMessage(propertyValueEventData.EntityEntry.Context.GetType().ShortDisplayName(), propertyValueEventData.Value, propertyValueEventData.Property.Name, propertyValueEventData.EntityEntry.Metadata.ShortName());
	}

	public static void CascadeDelete([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalChildEntry, [JetBrains.Annotations.NotNull] InternalEntityEntry internalParentEntry, EntityState state)
	{
		EventDefinition<string, EntityState, string> eventDefinition = CoreResources.LogCascadeDelete(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, internalChildEntry.EntityType.ShortName(), state, internalParentEntry.EntityType.ShortName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new CascadeDeleteEventData(eventDefinition, CascadeDelete, new EntityEntry(internalChildEntry), new EntityEntry(internalParentEntry), state));
		}
	}

	private static string CascadeDelete(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, EntityState, string> obj = (EventDefinition<string, EntityState, string>)definition;
		CascadeDeleteEventData cascadeDeleteEventData = (CascadeDeleteEventData)payload;
		return obj.GenerateMessage(cascadeDeleteEventData.EntityEntry.Metadata.ShortName(), cascadeDeleteEventData.State, cascadeDeleteEventData.ParentEntityEntry.Metadata.ShortName());
	}

	public static void CascadeDeleteSensitive([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalChildEntry, [JetBrains.Annotations.NotNull] InternalEntityEntry internalParentEntry, EntityState state)
	{
		EventDefinition<string, string, EntityState, string, string> eventDefinition = CoreResources.LogCascadeDeleteSensitive(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, internalChildEntry.EntityType.ShortName(), internalChildEntry.BuildCurrentValuesString(internalChildEntry.EntityType.FindPrimaryKey().Properties), state, internalParentEntry.EntityType.ShortName(), internalParentEntry.BuildCurrentValuesString(internalParentEntry.EntityType.FindPrimaryKey().Properties));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new CascadeDeleteEventData(eventDefinition, CascadeDeleteSensitive, new EntityEntry(internalChildEntry), new EntityEntry(internalParentEntry), state));
		}
	}

	private static string CascadeDeleteSensitive(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, EntityState, string, string> obj = (EventDefinition<string, string, EntityState, string, string>)definition;
		CascadeDeleteEventData cascadeDeleteEventData = (CascadeDeleteEventData)payload;
		return obj.GenerateMessage(cascadeDeleteEventData.EntityEntry.Metadata.ShortName(), cascadeDeleteEventData.EntityEntry.GetInfrastructure().BuildCurrentValuesString(cascadeDeleteEventData.EntityEntry.Metadata.FindPrimaryKey().Properties), cascadeDeleteEventData.State, cascadeDeleteEventData.ParentEntityEntry.Metadata.ShortName(), cascadeDeleteEventData.ParentEntityEntry.GetInfrastructure().BuildCurrentValuesString(cascadeDeleteEventData.ParentEntityEntry.Metadata.FindPrimaryKey().Properties));
	}

	public static void CascadeDeleteOrphan([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalChildEntry, [JetBrains.Annotations.NotNull] IEntityType parentEntityType, EntityState state)
	{
		EventDefinition<string, EntityState, string> eventDefinition = CoreResources.LogCascadeDeleteOrphan(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, internalChildEntry.EntityType.ShortName(), state, parentEntityType.ShortName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new CascadeDeleteOrphanEventData(eventDefinition, CascadeDeleteOrphan, new EntityEntry(internalChildEntry), parentEntityType, state));
		}
	}

	private static string CascadeDeleteOrphan(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, EntityState, string> obj = (EventDefinition<string, EntityState, string>)definition;
		CascadeDeleteOrphanEventData cascadeDeleteOrphanEventData = (CascadeDeleteOrphanEventData)payload;
		return obj.GenerateMessage(cascadeDeleteOrphanEventData.EntityEntry.Metadata.ShortName(), cascadeDeleteOrphanEventData.State, cascadeDeleteOrphanEventData.ParentEntityType.ShortName());
	}

	public static void CascadeDeleteOrphanSensitive([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] InternalEntityEntry internalChildEntry, [JetBrains.Annotations.NotNull] IEntityType parentEntityType, EntityState state)
	{
		EventDefinition<string, string, EntityState, string> eventDefinition = CoreResources.LogCascadeDeleteOrphanSensitive(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, internalChildEntry.EntityType.ShortName(), internalChildEntry.BuildCurrentValuesString(internalChildEntry.EntityType.FindPrimaryKey().Properties), state, parentEntityType.ShortName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new CascadeDeleteOrphanEventData(eventDefinition, CascadeDeleteOrphanSensitive, new EntityEntry(internalChildEntry), parentEntityType, state));
		}
	}

	private static string CascadeDeleteOrphanSensitive(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, EntityState, string> obj = (EventDefinition<string, string, EntityState, string>)definition;
		CascadeDeleteOrphanEventData cascadeDeleteOrphanEventData = (CascadeDeleteOrphanEventData)payload;
		return obj.GenerateMessage(cascadeDeleteOrphanEventData.EntityEntry.Metadata.ShortName(), cascadeDeleteOrphanEventData.EntityEntry.GetInfrastructure().BuildCurrentValuesString(cascadeDeleteOrphanEventData.EntityEntry.Metadata.FindPrimaryKey().Properties), cascadeDeleteOrphanEventData.State, cascadeDeleteOrphanEventData.ParentEntityType.ShortName());
	}

	public static void SaveChangesStarting([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] DbContext context)
	{
		EventDefinition<string> eventDefinition = CoreResources.LogSaveChangesStarting(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, context.GetType().ShortDisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new DbContextEventData(eventDefinition, SaveChangesStarting, context));
		}
	}

	private static string SaveChangesStarting(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		DbContextEventData dbContextEventData = (DbContextEventData)payload;
		return obj.GenerateMessage(dbContextEventData.Context.GetType().ShortDisplayName());
	}

	public static void SaveChangesCompleted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] DbContext context, int entitiesSavedCount)
	{
		EventDefinition<string, int> eventDefinition = CoreResources.LogSaveChangesCompleted(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, context.GetType().ShortDisplayName(), entitiesSavedCount);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new SaveChangesCompletedEventData(eventDefinition, SaveChangesCompleted, context, entitiesSavedCount));
		}
	}

	private static string SaveChangesCompleted(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, int> obj = (EventDefinition<string, int>)definition;
		SaveChangesCompletedEventData saveChangesCompletedEventData = (SaveChangesCompletedEventData)payload;
		return obj.GenerateMessage(saveChangesCompletedEventData.Context.GetType().ShortDisplayName(), saveChangesCompletedEventData.EntitiesSavedCount);
	}

	public static void ContextDisposed([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics, [JetBrains.Annotations.NotNull] DbContext context)
	{
		EventDefinition<string> eventDefinition = CoreResources.LogContextDisposed(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, context.GetType().ShortDisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new DbContextEventData(eventDefinition, ContextDisposed, context));
		}
	}

	private static string ContextDisposed(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		DbContextEventData dbContextEventData = (DbContextEventData)payload;
		return obj.GenerateMessage(dbContextEventData.Context.GetType().ShortDisplayName());
	}
}
