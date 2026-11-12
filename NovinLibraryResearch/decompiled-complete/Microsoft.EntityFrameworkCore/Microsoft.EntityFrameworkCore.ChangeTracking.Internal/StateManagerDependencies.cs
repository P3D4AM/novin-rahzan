using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public sealed class StateManagerDependencies
{
	public IInternalEntityEntryFactory InternalEntityEntryFactory { get; }

	public IInternalEntityEntrySubscriber InternalEntityEntrySubscriber { get; }

	public IInternalEntityEntryNotifier InternalEntityEntryNotifier { get; }

	public IValueGenerationManager ValueGenerationManager { get; }

	public IModel Model { get; }

	public IDatabase Database { get; }

	public IConcurrencyDetector ConcurrencyDetector { get; }

	public ICurrentDbContext CurrentContext { get; }

	public IDbSetSource SetSource { get; }

	public IEntityFinderSource EntityFinderSource { get; }

	public IEntityMaterializerSource EntityMaterializerSource { get; }

	public IExecutionStrategyFactory ExecutionStrategyFactory { get; }

	public ILoggingOptions LoggingOptions { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Update> UpdateLogger { get; }

	public IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> ChangeTrackingLogger { get; }

	[EntityFrameworkInternal]
	public StateManagerDependencies([JetBrains.Annotations.NotNull] IInternalEntityEntryFactory internalEntityEntryFactory, [JetBrains.Annotations.NotNull] IInternalEntityEntrySubscriber internalEntityEntrySubscriber, [JetBrains.Annotations.NotNull] IInternalEntityEntryNotifier internalEntityEntryNotifier, [JetBrains.Annotations.NotNull] IValueGenerationManager valueGenerationManager, [JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDatabase database, [JetBrains.Annotations.NotNull] IConcurrencyDetector concurrencyDetector, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IEntityFinderSource entityFinderSource, [JetBrains.Annotations.NotNull] IDbSetSource setSource, [JetBrains.Annotations.NotNull] IEntityMaterializerSource entityMaterializerSource, [JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory, [JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Update> updateLogger, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> changeTrackingLogger)
	{
		InternalEntityEntryFactory = internalEntityEntryFactory;
		InternalEntityEntrySubscriber = internalEntityEntrySubscriber;
		InternalEntityEntryNotifier = internalEntityEntryNotifier;
		ValueGenerationManager = valueGenerationManager;
		Model = model;
		Database = database;
		ConcurrencyDetector = concurrencyDetector;
		CurrentContext = currentContext;
		EntityFinderSource = entityFinderSource;
		SetSource = setSource;
		EntityMaterializerSource = entityMaterializerSource;
		ExecutionStrategyFactory = executionStrategyFactory;
		LoggingOptions = loggingOptions;
		UpdateLogger = updateLogger;
		ChangeTrackingLogger = changeTrackingLogger;
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IInternalEntityEntryFactory internalEntityEntryFactory)
	{
		return new StateManagerDependencies(internalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IInternalEntityEntrySubscriber internalEntityEntrySubscriber)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, internalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IInternalEntityEntryNotifier internalEntityEntryNotifier)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, internalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] ValueGenerationManager valueGenerationManager)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, valueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IModel model)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IDatabase database)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IConcurrencyDetector concurrencyDetector)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, concurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, currentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IEntityFinderSource entityFinderSource)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, entityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IDbSetSource setSource)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, setSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IEntityMaterializerSource entityMaterializerSource)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, entityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, executionStrategyFactory, LoggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, loggingOptions, UpdateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Update> updateLogger)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, updateLogger, ChangeTrackingLogger);
	}

	public StateManagerDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> changeTrackingLogger)
	{
		return new StateManagerDependencies(InternalEntityEntryFactory, InternalEntityEntrySubscriber, InternalEntityEntryNotifier, ValueGenerationManager, Model, Database, ConcurrencyDetector, CurrentContext, EntityFinderSource, SetSource, EntityMaterializerSource, ExecutionStrategyFactory, LoggingOptions, UpdateLogger, changeTrackingLogger);
	}
}
