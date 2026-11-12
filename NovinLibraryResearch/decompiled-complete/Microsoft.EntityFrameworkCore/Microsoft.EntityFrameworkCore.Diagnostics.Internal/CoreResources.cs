using System;
using System.Reflection;
using System.Resources;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics.Internal;

public static class CoreResources
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.Properties.CoreStrings", typeof(CoreResources).GetTypeInfo().Assembly);

	public static EventDefinition LogServiceProviderCreated([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogServiceProviderCreated;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogServiceProviderCreated, () => new EventDefinition(logger.Options, CoreEventId.ServiceProviderCreated, LogLevel.Debug, "CoreEventId.ServiceProviderCreated", (LogLevel level) => LoggerMessage.Define(level, CoreEventId.ServiceProviderCreated, _resourceManager.GetString("LogServiceProviderCreated"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition LogManyServiceProvidersCreated([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogManyServiceProvidersCreated;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogManyServiceProvidersCreated, () => new EventDefinition(logger.Options, CoreEventId.ManyServiceProvidersCreatedWarning, LogLevel.Warning, "CoreEventId.ManyServiceProvidersCreatedWarning", (LogLevel level) => LoggerMessage.Define(level, CoreEventId.ManyServiceProvidersCreatedWarning, _resourceManager.GetString("LogManyServiceProvidersCreated"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	public static EventDefinition<string> LogServiceProviderDebugInfo([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogServiceProviderDebugInfo;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogServiceProviderDebugInfo, () => new EventDefinition<string>(logger.Options, CoreEventId.ServiceProviderDebugInfo, LogLevel.Debug, "CoreEventId.ServiceProviderDebugInfo", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.ServiceProviderDebugInfo, _resourceManager.GetString("LogServiceProviderDebugInfo"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string> LogContextInitialized([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogContextInitialized;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogContextInitialized, () => new EventDefinition<string, string, string, string>(logger.Options, CoreEventId.ContextInitialized, LogLevel.Information, "CoreEventId.ContextInitialized", (LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, CoreEventId.ContextInitialized, _resourceManager.GetString("LogContextInitialized"))));
		}
		return (EventDefinition<string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<Type, string, Exception> LogExceptionDuringQueryIteration([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogExceptionDuringQueryIteration;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogExceptionDuringQueryIteration, () => new EventDefinition<Type, string, Exception>(logger.Options, CoreEventId.QueryIterationFailed, LogLevel.Error, "CoreEventId.QueryIterationFailed", (LogLevel level) => LoggerMessage.Define<Type, string, Exception>(level, CoreEventId.QueryIterationFailed, _resourceManager.GetString("LogExceptionDuringQueryIteration"))));
		}
		return (EventDefinition<Type, string, Exception>)eventDefinitionBase;
	}

	public static EventDefinition<Type, string, Exception> LogExceptionDuringSaveChanges([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogExceptionDuringSaveChanges;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogExceptionDuringSaveChanges, () => new EventDefinition<Type, string, Exception>(logger.Options, CoreEventId.SaveChangesFailed, LogLevel.Error, "CoreEventId.SaveChangesFailed", (LogLevel level) => LoggerMessage.Define<Type, string, Exception>(level, CoreEventId.SaveChangesFailed, _resourceManager.GetString("LogExceptionDuringSaveChanges"))));
		}
		return (EventDefinition<Type, string, Exception>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogDetectChangesStarting([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogDetectChangesStarting;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogDetectChangesStarting, () => new EventDefinition<string>(logger.Options, CoreEventId.DetectChangesStarting, LogLevel.Debug, "CoreEventId.DetectChangesStarting", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.DetectChangesStarting, _resourceManager.GetString("LogDetectChangesStarting"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogDetectChangesCompleted([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogDetectChangesCompleted;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogDetectChangesCompleted, () => new EventDefinition<string>(logger.Options, CoreEventId.DetectChangesCompleted, LogLevel.Debug, "CoreEventId.DetectChangesCompleted", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.DetectChangesCompleted, _resourceManager.GetString("LogDetectChangesCompleted"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogPropertyChangeDetected([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogPropertyChangeDetected;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogPropertyChangeDetected, () => new EventDefinition<string, string>(logger.Options, CoreEventId.PropertyChangeDetected, LogLevel.Debug, "CoreEventId.PropertyChangeDetected", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.PropertyChangeDetected, _resourceManager.GetString("LogPropertyChangeDetected"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, object, object, string> LogPropertyChangeDetectedSensitive([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogPropertyChangeDetectedSensitive;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogPropertyChangeDetectedSensitive, () => new EventDefinition<string, string, object, object, string>(logger.Options, CoreEventId.PropertyChangeDetected, LogLevel.Debug, "CoreEventId.PropertyChangeDetected", (LogLevel level) => LoggerMessage.Define<string, string, object, object, string>(level, CoreEventId.PropertyChangeDetected, _resourceManager.GetString("LogPropertyChangeDetectedSensitive"))));
		}
		return (EventDefinition<string, string, object, object, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogForeignKeyChangeDetected([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogForeignKeyChangeDetected;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogForeignKeyChangeDetected, () => new EventDefinition<string, string>(logger.Options, CoreEventId.ForeignKeyChangeDetected, LogLevel.Debug, "CoreEventId.ForeignKeyChangeDetected", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.ForeignKeyChangeDetected, _resourceManager.GetString("LogForeignKeyChangeDetected"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, object, object, string> LogForeignKeyChangeDetectedSensitive([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogForeignKeyChangeDetectedSensitive;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogForeignKeyChangeDetectedSensitive, () => new EventDefinition<string, string, object, object, string>(logger.Options, CoreEventId.ForeignKeyChangeDetected, LogLevel.Debug, "CoreEventId.ForeignKeyChangeDetected", (LogLevel level) => LoggerMessage.Define<string, string, object, object, string>(level, CoreEventId.ForeignKeyChangeDetected, _resourceManager.GetString("LogForeignKeyChangeDetectedSensitive"))));
		}
		return (EventDefinition<string, string, object, object, string>)eventDefinitionBase;
	}

	public static EventDefinition<int, int, string, string> LogCollectionChangeDetected([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogCollectionChangeDetected;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogCollectionChangeDetected, () => new EventDefinition<int, int, string, string>(logger.Options, CoreEventId.CollectionChangeDetected, LogLevel.Debug, "CoreEventId.CollectionChangeDetected", (LogLevel level) => LoggerMessage.Define<int, int, string, string>(level, CoreEventId.CollectionChangeDetected, _resourceManager.GetString("LogCollectionChangeDetected"))));
		}
		return (EventDefinition<int, int, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<int, int, string, string, string> LogCollectionChangeDetectedSensitive([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogCollectionChangeDetectedSensitive;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogCollectionChangeDetectedSensitive, () => new EventDefinition<int, int, string, string, string>(logger.Options, CoreEventId.CollectionChangeDetected, LogLevel.Debug, "CoreEventId.CollectionChangeDetected", (LogLevel level) => LoggerMessage.Define<int, int, string, string, string>(level, CoreEventId.CollectionChangeDetected, _resourceManager.GetString("LogCollectionChangeDetectedSensitive"))));
		}
		return (EventDefinition<int, int, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogReferenceChangeDetected([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogReferenceChangeDetected;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogReferenceChangeDetected, () => new EventDefinition<string, string>(logger.Options, CoreEventId.ReferenceChangeDetected, LogLevel.Debug, "CoreEventId.ReferenceChangeDetected", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.ReferenceChangeDetected, _resourceManager.GetString("LogReferenceChangeDetected"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string> LogReferenceChangeDetectedSensitive([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogReferenceChangeDetectedSensitive;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogReferenceChangeDetectedSensitive, () => new EventDefinition<string, string, string>(logger.Options, CoreEventId.ReferenceChangeDetected, LogLevel.Debug, "CoreEventId.ReferenceChangeDetected", (LogLevel level) => LoggerMessage.Define<string, string, string>(level, CoreEventId.ReferenceChangeDetected, _resourceManager.GetString("LogReferenceChangeDetectedSensitive"))));
		}
		return (EventDefinition<string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, EntityState, string> LogCascadeDelete([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogCascadeDelete;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogCascadeDelete, () => new EventDefinition<string, EntityState, string>(logger.Options, CoreEventId.CascadeDelete, LogLevel.Debug, "CoreEventId.CascadeDelete", (LogLevel level) => LoggerMessage.Define<string, EntityState, string>(level, CoreEventId.CascadeDelete, _resourceManager.GetString("LogCascadeDelete"))));
		}
		return (EventDefinition<string, EntityState, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, EntityState, string, string> LogCascadeDeleteSensitive([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogCascadeDeleteSensitive;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogCascadeDeleteSensitive, () => new EventDefinition<string, string, EntityState, string, string>(logger.Options, CoreEventId.CascadeDelete, LogLevel.Debug, "CoreEventId.CascadeDelete", (LogLevel level) => LoggerMessage.Define<string, string, EntityState, string, string>(level, CoreEventId.CascadeDelete, _resourceManager.GetString("LogCascadeDeleteSensitive"))));
		}
		return (EventDefinition<string, string, EntityState, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, EntityState, string> LogCascadeDeleteOrphan([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogCascadeDeleteOrphan;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogCascadeDeleteOrphan, () => new EventDefinition<string, EntityState, string>(logger.Options, CoreEventId.CascadeDeleteOrphan, LogLevel.Debug, "CoreEventId.CascadeDeleteOrphan", (LogLevel level) => LoggerMessage.Define<string, EntityState, string>(level, CoreEventId.CascadeDeleteOrphan, _resourceManager.GetString("LogCascadeDeleteOrphan"))));
		}
		return (EventDefinition<string, EntityState, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, EntityState, string> LogCascadeDeleteOrphanSensitive([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogCascadeDeleteOrphanSensitive;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogCascadeDeleteOrphanSensitive, () => new EventDefinition<string, string, EntityState, string>(logger.Options, CoreEventId.CascadeDeleteOrphan, LogLevel.Debug, "CoreEventId.CascadeDeleteOrphan", (LogLevel level) => LoggerMessage.Define<string, string, EntityState, string>(level, CoreEventId.CascadeDeleteOrphan, _resourceManager.GetString("LogCascadeDeleteOrphanSensitive"))));
		}
		return (EventDefinition<string, string, EntityState, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogStartedTracking([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogStartedTracking;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogStartedTracking, () => new EventDefinition<string, string>(logger.Options, CoreEventId.StartedTracking, LogLevel.Debug, "CoreEventId.StartedTracking", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.StartedTracking, _resourceManager.GetString("LogStartedTracking"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string> LogStartedTrackingSensitive([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogStartedTrackingSensitive;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogStartedTrackingSensitive, () => new EventDefinition<string, string, string>(logger.Options, CoreEventId.StartedTracking, LogLevel.Debug, "CoreEventId.StartedTracking", (LogLevel level) => LoggerMessage.Define<string, string, string>(level, CoreEventId.StartedTracking, _resourceManager.GetString("LogStartedTrackingSensitive"))));
		}
		return (EventDefinition<string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, EntityState, EntityState> LogStateChanged([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogStateChanged;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogStateChanged, () => new EventDefinition<string, string, EntityState, EntityState>(logger.Options, CoreEventId.StateChanged, LogLevel.Debug, "CoreEventId.StateChanged", (LogLevel level) => LoggerMessage.Define<string, string, EntityState, EntityState>(level, CoreEventId.StateChanged, _resourceManager.GetString("LogStateChanged"))));
		}
		return (EventDefinition<string, string, EntityState, EntityState>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, EntityState, EntityState> LogStateChangedSensitive([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogStateChangedSensitive;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogStateChangedSensitive, () => new EventDefinition<string, string, string, EntityState, EntityState>(logger.Options, CoreEventId.StateChanged, LogLevel.Debug, "CoreEventId.StateChanged", (LogLevel level) => LoggerMessage.Define<string, string, string, EntityState, EntityState>(level, CoreEventId.StateChanged, _resourceManager.GetString("LogStateChangedSensitive"))));
		}
		return (EventDefinition<string, string, string, EntityState, EntityState>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string> LogValueGenerated([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogValueGenerated;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogValueGenerated, () => new EventDefinition<string, string, string>(logger.Options, CoreEventId.ValueGenerated, LogLevel.Debug, "CoreEventId.ValueGenerated", (LogLevel level) => LoggerMessage.Define<string, string, string>(level, CoreEventId.ValueGenerated, _resourceManager.GetString("LogValueGenerated"))));
		}
		return (EventDefinition<string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, object, string, string> LogValueGeneratedSensitive([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogValueGeneratedSensitive;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogValueGeneratedSensitive, () => new EventDefinition<string, object, string, string>(logger.Options, CoreEventId.ValueGenerated, LogLevel.Debug, "CoreEventId.ValueGenerated", (LogLevel level) => LoggerMessage.Define<string, object, string, string>(level, CoreEventId.ValueGenerated, _resourceManager.GetString("LogValueGeneratedSensitive"))));
		}
		return (EventDefinition<string, object, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string> LogTempValueGenerated([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogTempValueGenerated;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogTempValueGenerated, () => new EventDefinition<string, string, string>(logger.Options, CoreEventId.ValueGenerated, LogLevel.Debug, "CoreEventId.ValueGenerated", (LogLevel level) => LoggerMessage.Define<string, string, string>(level, CoreEventId.ValueGenerated, _resourceManager.GetString("LogTempValueGenerated"))));
		}
		return (EventDefinition<string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, object, string, string> LogTempValueGeneratedSensitive([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogTempValueGeneratedSensitive;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogTempValueGeneratedSensitive, () => new EventDefinition<string, object, string, string>(logger.Options, CoreEventId.ValueGenerated, LogLevel.Debug, "CoreEventId.ValueGenerated", (LogLevel level) => LoggerMessage.Define<string, object, string, string>(level, CoreEventId.ValueGenerated, _resourceManager.GetString("LogTempValueGeneratedSensitive"))));
		}
		return (EventDefinition<string, object, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogSaveChangesStarting([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogSaveChangesStarting;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogSaveChangesStarting, () => new EventDefinition<string>(logger.Options, CoreEventId.SaveChangesStarting, LogLevel.Debug, "CoreEventId.SaveChangesStarting", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.SaveChangesStarting, _resourceManager.GetString("LogSaveChangesStarting"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string, int> LogSaveChangesCompleted([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogSaveChangesCompleted;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogSaveChangesCompleted, () => new EventDefinition<string, int>(logger.Options, CoreEventId.SaveChangesCompleted, LogLevel.Debug, "CoreEventId.SaveChangesCompleted", (LogLevel level) => LoggerMessage.Define<string, int>(level, CoreEventId.SaveChangesCompleted, _resourceManager.GetString("LogSaveChangesCompleted"))));
		}
		return (EventDefinition<string, int>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogContextDisposed([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogContextDisposed;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogContextDisposed, () => new EventDefinition<string>(logger.Options, CoreEventId.ContextDisposed, LogLevel.Debug, "CoreEventId.ContextDisposed", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.ContextDisposed, _resourceManager.GetString("LogContextDisposed"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	[Obsolete]
	public static EventDefinition<string, string> LogCompilingQueryModel([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogCompilingQueryModel;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogCompilingQueryModel, () => new EventDefinition<string, string>(logger.Options, CoreEventId.QueryModelCompiling, LogLevel.Debug, "CoreEventId.QueryModelCompiling", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.QueryModelCompiling, _resourceManager.GetString("LogCompilingQueryModel"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	[Obsolete]
	public static EventDefinition<string, string> LogOptimizedQueryModel([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogOptimizedQueryModel;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogOptimizedQueryModel, () => new EventDefinition<string, string>(logger.Options, CoreEventId.QueryModelOptimized, LogLevel.Debug, "CoreEventId.QueryModelOptimized", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.QueryModelOptimized, _resourceManager.GetString("LogOptimizedQueryModel"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	[Obsolete]
	public static EventDefinition<string> LogIncludingNavigation([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogIncludingNavigation;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogIncludingNavigation, () => new EventDefinition<string>(logger.Options, CoreEventId.NavigationIncluded, LogLevel.Debug, "CoreEventId.NavigationIncluded", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.NavigationIncluded, _resourceManager.GetString("LogIncludingNavigation"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogQueryExecutionPlanned([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogQueryExecutionPlanned;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogQueryExecutionPlanned, () => new EventDefinition<string>(logger.Options, CoreEventId.QueryExecutionPlanned, LogLevel.Debug, "CoreEventId.QueryExecutionPlanned", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.QueryExecutionPlanned, _resourceManager.GetString("LogQueryExecutionPlanned"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition LogSensitiveDataLoggingEnabled([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogSensitiveDataLoggingEnabled;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogSensitiveDataLoggingEnabled, () => new EventDefinition(logger.Options, CoreEventId.SensitiveDataLoggingEnabledWarning, LogLevel.Warning, "CoreEventId.SensitiveDataLoggingEnabledWarning", (LogLevel level) => LoggerMessage.Define(level, CoreEventId.SensitiveDataLoggingEnabledWarning, _resourceManager.GetString("LogSensitiveDataLoggingEnabled"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}

	[Obsolete]
	public static EventDefinition<string> LogIgnoredInclude([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogIgnoredInclude;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogIgnoredInclude, () => new EventDefinition<string>(logger.Options, CoreEventId.IncludeIgnoredWarning, LogLevel.Warning, "CoreEventId.IncludeIgnoredWarning", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.IncludeIgnoredWarning, _resourceManager.GetString("LogIgnoredInclude"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	[Obsolete]
	public static EventDefinition<string> LogRowLimitingOperationWithoutOrderBy([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogRowLimitingOperationWithoutOrderBy;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogRowLimitingOperationWithoutOrderBy, () => new EventDefinition<string>(logger.Options, CoreEventId.RowLimitingOperationWithoutOrderByWarning, LogLevel.Warning, "CoreEventId.RowLimitingOperationWithoutOrderByWarning", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.RowLimitingOperationWithoutOrderByWarning, _resourceManager.GetString("LogRowLimitingOperationWithoutOrderBy"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	[Obsolete]
	public static EventDefinition<string> LogFirstWithoutOrderByAndFilter([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogFirstWithoutOrderByAndFilter;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogFirstWithoutOrderByAndFilter, () => new EventDefinition<string>(logger.Options, CoreEventId.FirstWithoutOrderByAndFilterWarning, LogLevel.Warning, "CoreEventId.FirstWithoutOrderByAndFilterWarning", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.FirstWithoutOrderByAndFilterWarning, _resourceManager.GetString("LogFirstWithoutOrderByAndFilter"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogPossibleUnintendedCollectionNavigationNullComparison([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogPossibleUnintendedCollectionNavigationNullComparison;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogPossibleUnintendedCollectionNavigationNullComparison, () => new EventDefinition<string>(logger.Options, CoreEventId.PossibleUnintendedCollectionNavigationNullComparisonWarning, LogLevel.Warning, "CoreEventId.PossibleUnintendedCollectionNavigationNullComparisonWarning", (LogLevel level) => LoggerMessage.Define<string>(level, CoreEventId.PossibleUnintendedCollectionNavigationNullComparisonWarning, _resourceManager.GetString("LogPossibleUnintendedCollectionNavigationNullComparison"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<object, object> LogPossibleUnintendedReferenceComparison([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogPossibleUnintendedReferenceComparison;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogPossibleUnintendedReferenceComparison, () => new EventDefinition<object, object>(logger.Options, CoreEventId.PossibleUnintendedReferenceComparisonWarning, LogLevel.Warning, "CoreEventId.PossibleUnintendedReferenceComparisonWarning", (LogLevel level) => LoggerMessage.Define<object, object>(level, CoreEventId.PossibleUnintendedReferenceComparisonWarning, _resourceManager.GetString("LogPossibleUnintendedReferenceComparison"))));
		}
		return (EventDefinition<object, object>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogDuplicateDependentEntityTypeInstance([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogDuplicateDependentEntityTypeInstance;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogDuplicateDependentEntityTypeInstance, () => new EventDefinition<string, string>(logger.Options, CoreEventId.DuplicateDependentEntityTypeInstanceWarning, LogLevel.Warning, "CoreEventId.DuplicateDependentEntityTypeInstanceWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.DuplicateDependentEntityTypeInstanceWarning, _resourceManager.GetString("LogDuplicateDependentEntityTypeInstance"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogShadowPropertyCreated([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogShadowPropertyCreated;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogShadowPropertyCreated, () => new EventDefinition<string, string>(logger.Options, CoreEventId.ShadowPropertyCreated, LogLevel.Debug, "CoreEventId.ShadowPropertyCreated", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.ShadowPropertyCreated, _resourceManager.GetString("LogShadowPropertyCreated"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogCollectionWithoutComparer([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogCollectionWithoutComparer;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogCollectionWithoutComparer, () => new EventDefinition<string, string>(logger.Options, CoreEventId.CollectionWithoutComparer, LogLevel.Warning, "CoreEventId.CollectionWithoutComparer", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.CollectionWithoutComparer, _resourceManager.GetString("LogCollectionWithoutComparer"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<int, string, Exception> LogExecutionStrategyRetrying([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogExecutionStrategyRetrying;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogExecutionStrategyRetrying, () => new EventDefinition<int, string, Exception>(logger.Options, CoreEventId.ExecutionStrategyRetrying, LogLevel.Information, "CoreEventId.ExecutionStrategyRetrying", (LogLevel level) => LoggerMessage.Define<int, string, Exception>(level, CoreEventId.ExecutionStrategyRetrying, _resourceManager.GetString("LogExecutionStrategyRetrying"))));
		}
		return (EventDefinition<int, string, Exception>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogNavigationLazyLoading([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogNavigationLazyLoading;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogNavigationLazyLoading, () => new EventDefinition<string, string>(logger.Options, CoreEventId.NavigationLazyLoading, LogLevel.Debug, "CoreEventId.NavigationLazyLoading", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.NavigationLazyLoading, _resourceManager.GetString("LogNavigationLazyLoading"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogLazyLoadOnDisposedContext([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogLazyLoadOnDisposedContext;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogLazyLoadOnDisposedContext, () => new EventDefinition<string, string>(logger.Options, CoreEventId.LazyLoadOnDisposedContextWarning, LogLevel.Warning, "CoreEventId.LazyLoadOnDisposedContextWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.LazyLoadOnDisposedContextWarning, _resourceManager.GetString("LogLazyLoadOnDisposedContext"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogDetachedLazyLoading([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogDetachedLazyLoading;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogDetachedLazyLoading, () => new EventDefinition<string, string>(logger.Options, CoreEventId.DetachedLazyLoadingWarning, LogLevel.Warning, "CoreEventId.DetachedLazyLoadingWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.DetachedLazyLoadingWarning, _resourceManager.GetString("LogDetachedLazyLoading"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string> LogRedundantIndexRemoved([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogRedundantIndexRemoved;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogRedundantIndexRemoved, () => new EventDefinition<string, string, string>(logger.Options, CoreEventId.RedundantIndexRemoved, LogLevel.Debug, "CoreEventId.RedundantIndexRemoved", (LogLevel level) => LoggerMessage.Define<string, string, string>(level, CoreEventId.RedundantIndexRemoved, _resourceManager.GetString("LogRedundantIndexRemoved"))));
		}
		return (EventDefinition<string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogIncompatibleMatchingForeignKeyProperties([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogIncompatibleMatchingForeignKeyProperties;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogIncompatibleMatchingForeignKeyProperties, () => new EventDefinition<string, string>(logger.Options, CoreEventId.IncompatibleMatchingForeignKeyProperties, LogLevel.Debug, "CoreEventId.IncompatibleMatchingForeignKeyProperties", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.IncompatibleMatchingForeignKeyProperties, _resourceManager.GetString("LogIncompatibleMatchingForeignKeyProperties"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogRequiredAttributeInverted([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogRequiredAttributeInverted;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogRequiredAttributeInverted, () => new EventDefinition<string, string>(logger.Options, CoreEventId.RequiredAttributeInverted, LogLevel.Debug, "CoreEventId.RequiredAttributeInverted", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.RequiredAttributeInverted, _resourceManager.GetString("LogRequiredAttributeInverted"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogNonNullableInverted([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogNonNullableInverted;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogNonNullableInverted, () => new EventDefinition<string, string>(logger.Options, CoreEventId.NonNullableInverted, LogLevel.Debug, "CoreEventId.NonNullableInverted", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.NonNullableInverted, _resourceManager.GetString("LogNonNullableInverted"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string> LogRequiredAttributeOnBothNavigations([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogRequiredAttributeOnBothNavigations;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogRequiredAttributeOnBothNavigations, () => new EventDefinition<string, string, string, string>(logger.Options, CoreEventId.RequiredAttributeOnBothNavigations, LogLevel.Debug, "CoreEventId.RequiredAttributeOnBothNavigations", (LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, CoreEventId.RequiredAttributeOnBothNavigations, _resourceManager.GetString("LogRequiredAttributeOnBothNavigations"))));
		}
		return (EventDefinition<string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string> LogNonNullableReferenceOnBothNavigations([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogNonNullableReferenceOnBothNavigations;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogNonNullableReferenceOnBothNavigations, () => new EventDefinition<string, string, string, string>(logger.Options, CoreEventId.NonNullableReferenceOnBothNavigations, LogLevel.Debug, "CoreEventId.NonNullableReferenceOnBothNavigations", (LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, CoreEventId.NonNullableReferenceOnBothNavigations, _resourceManager.GetString("LogNonNullableReferenceOnBothNavigations"))));
		}
		return (EventDefinition<string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string> LogForeignKeyAttributesOnBothNavigations([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogForeignKeyAttributesOnBothNavigations;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogForeignKeyAttributesOnBothNavigations, () => new EventDefinition<string, string, string, string>(logger.Options, CoreEventId.ForeignKeyAttributesOnBothNavigationsWarning, LogLevel.Warning, "CoreEventId.ForeignKeyAttributesOnBothNavigationsWarning", (LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, CoreEventId.ForeignKeyAttributesOnBothNavigationsWarning, _resourceManager.GetString("LogForeignKeyAttributesOnBothNavigations"))));
		}
		return (EventDefinition<string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string, string, string> LogForeignKeyAttributesOnBothProperties([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogForeignKeyAttributesOnBothProperties;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogForeignKeyAttributesOnBothProperties, () => new EventDefinition<string, string, string, string, string, string>(logger.Options, CoreEventId.ForeignKeyAttributesOnBothPropertiesWarning, LogLevel.Warning, "CoreEventId.ForeignKeyAttributesOnBothPropertiesWarning", (LogLevel level) => LoggerMessage.Define<string, string, string, string, string, string>(level, CoreEventId.ForeignKeyAttributesOnBothPropertiesWarning, _resourceManager.GetString("LogForeignKeyAttributesOnBothProperties"))));
		}
		return (EventDefinition<string, string, string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string> LogConflictingForeignKeyAttributesOnNavigationAndProperty([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogConflictingForeignKeyAttributesOnNavigationAndProperty;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogConflictingForeignKeyAttributesOnNavigationAndProperty, () => new EventDefinition<string, string, string, string>(logger.Options, CoreEventId.ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning, LogLevel.Warning, "CoreEventId.ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning", (LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, CoreEventId.ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning, _resourceManager.GetString("LogConflictingForeignKeyAttributesOnNavigationAndProperty"))));
		}
		return (EventDefinition<string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogMultipleInversePropertiesSameTarget([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogMultipleInversePropertiesSameTarget;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogMultipleInversePropertiesSameTarget, () => new EventDefinition<string, string>(logger.Options, CoreEventId.MultipleInversePropertiesSameTargetWarning, LogLevel.Warning, "CoreEventId.MultipleInversePropertiesSameTargetWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.MultipleInversePropertiesSameTargetWarning, _resourceManager.GetString("LogMultipleInversePropertiesSameTarget"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string> LogConflictingShadowForeignKeys([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogConflictingShadowForeignKeys;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogConflictingShadowForeignKeys, () => new EventDefinition<string, string, string>(logger.Options, CoreEventId.ConflictingShadowForeignKeysWarning, LogLevel.Warning, "CoreEventId.ConflictingShadowForeignKeysWarning", (LogLevel level) => LoggerMessage.Define<string, string, string>(level, CoreEventId.ConflictingShadowForeignKeysWarning, _resourceManager.GetString("LogConflictingShadowForeignKeys"))));
		}
		return (EventDefinition<string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string> LogMultipleNavigationProperties([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogMultipleNavigationProperties;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogMultipleNavigationProperties, () => new EventDefinition<string, string, string, string>(logger.Options, CoreEventId.MultipleNavigationProperties, LogLevel.Debug, "CoreEventId.MultipleNavigationProperties", (LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, CoreEventId.MultipleNavigationProperties, _resourceManager.GetString("LogMultipleNavigationProperties"))));
		}
		return (EventDefinition<string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string> LogMultiplePrimaryKeyCandidates([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogMultiplePrimaryKeyCandidates;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogMultiplePrimaryKeyCandidates, () => new EventDefinition<string, string, string>(logger.Options, CoreEventId.MultiplePrimaryKeyCandidates, LogLevel.Debug, "CoreEventId.MultiplePrimaryKeyCandidates", (LogLevel level) => LoggerMessage.Define<string, string, string>(level, CoreEventId.MultiplePrimaryKeyCandidates, _resourceManager.GetString("LogMultiplePrimaryKeyCandidates"))));
		}
		return (EventDefinition<string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string, string> LogNonDefiningInverseNavigation([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogNonDefiningInverseNavigation;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogNonDefiningInverseNavigation, () => new EventDefinition<string, string, string, string, string>(logger.Options, CoreEventId.NonDefiningInverseNavigationWarning, LogLevel.Warning, "CoreEventId.NonDefiningInverseNavigationWarning", (LogLevel level) => LoggerMessage.Define<string, string, string, string, string>(level, CoreEventId.NonDefiningInverseNavigationWarning, _resourceManager.GetString("LogNonDefiningInverseNavigation"))));
		}
		return (EventDefinition<string, string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string, string> LogNonOwnershipInverseNavigation([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogNonOwnershipInverseNavigation;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogNonOwnershipInverseNavigation, () => new EventDefinition<string, string, string, string, string>(logger.Options, CoreEventId.NonOwnershipInverseNavigationWarning, LogLevel.Warning, "CoreEventId.NonOwnershipInverseNavigationWarning", (LogLevel level) => LoggerMessage.Define<string, string, string, string, string>(level, CoreEventId.NonOwnershipInverseNavigationWarning, _resourceManager.GetString("LogNonOwnershipInverseNavigation"))));
		}
		return (EventDefinition<string, string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<Exception> LogOptimisticConcurrencyException([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogOptimisticConcurrencyException;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogOptimisticConcurrencyException, () => new EventDefinition<Exception>(logger.Options, CoreEventId.OptimisticConcurrencyException, LogLevel.Debug, "CoreEventId.OptimisticConcurrencyException", (LogLevel level) => LoggerMessage.Define<Exception>(level, CoreEventId.OptimisticConcurrencyException, _resourceManager.GetString("LogOptimisticConcurrencyException"))));
		}
		return (EventDefinition<Exception>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogRedundantForeignKey([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogRedundantForeignKey;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogRedundantForeignKey, () => new EventDefinition<string, string>(logger.Options, CoreEventId.RedundantForeignKeyWarning, LogLevel.Warning, "CoreEventId.RedundantForeignKeyWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.RedundantForeignKeyWarning, _resourceManager.GetString("LogRedundantForeignKey"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogRequiredAttributeOnCollection([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogRequiredAttributeOnCollection;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogRequiredAttributeOnCollection, () => new EventDefinition<string, string>(logger.Options, CoreEventId.RequiredAttributeOnCollection, LogLevel.Debug, "CoreEventId.RequiredAttributeOnCollection", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.RequiredAttributeOnCollection, _resourceManager.GetString("LogRequiredAttributeOnCollection"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogRequiredAttributeOnDependent([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogRequiredAttributeOnDependent;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogRequiredAttributeOnDependent, () => new EventDefinition<string, string>(logger.Options, CoreEventId.RequiredAttributeOnDependent, LogLevel.Debug, "CoreEventId.RequiredAttributeOnDependent", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.RequiredAttributeOnDependent, _resourceManager.GetString("LogRequiredAttributeOnDependent"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogNonNullableReferenceOnDependent([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogNonNullableReferenceOnDependent;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogNonNullableReferenceOnDependent, () => new EventDefinition<string, string>(logger.Options, CoreEventId.NonNullableReferenceOnDependent, LogLevel.Debug, "CoreEventId.NonNullableReferenceOnDependent", (LogLevel level) => LoggerMessage.Define<string, string>(level, CoreEventId.NonNullableReferenceOnDependent, _resourceManager.GetString("LogNonNullableReferenceOnDependent"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition LogRedundantAddServicesCall([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = logger.Definitions.LogRedundantAddServicesCall;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref logger.Definitions.LogRedundantAddServicesCall, () => new EventDefinition(logger.Options, CoreEventId.RedundantAddServicesCallWarning, LogLevel.Warning, "CoreEventId.RedundantAddServicesCallWarning", (LogLevel level) => LoggerMessage.Define(level, CoreEventId.RedundantAddServicesCallWarning, _resourceManager.GetString("LogRedundantAddServicesCall"))));
		}
		return (EventDefinition)eventDefinitionBase;
	}
}
