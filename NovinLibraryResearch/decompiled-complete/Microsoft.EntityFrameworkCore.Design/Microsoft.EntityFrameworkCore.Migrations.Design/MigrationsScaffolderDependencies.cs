using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public sealed class MigrationsScaffolderDependencies
{
	public ICurrentDbContext CurrentContext { get; }

	public IModel Model { get; }

	public IMigrationsAssembly MigrationsAssembly { get; }

	public IMigrationsModelDiffer MigrationsModelDiffer { get; }

	public IMigrationsIdGenerator MigrationsIdGenerator { get; }

	public IMigrationsCodeGeneratorSelector MigrationsCodeGeneratorSelector { get; }

	public IHistoryRepository HistoryRepository { get; }

	public IOperationReporter OperationReporter { get; }

	public IDatabaseProvider DatabaseProvider { get; }

	public ISnapshotModelProcessor SnapshotModelProcessor { get; }

	public IMigrator Migrator { get; }

	[EntityFrameworkInternal]
	public MigrationsScaffolderDependencies([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IMigrationsAssembly migrationsAssembly, [JetBrains.Annotations.NotNull] IMigrationsModelDiffer migrationsModelDiffer, [JetBrains.Annotations.NotNull] IMigrationsIdGenerator migrationsIdGenerator, [JetBrains.Annotations.NotNull] IMigrationsCodeGeneratorSelector migrationsCodeGeneratorSelector, [JetBrains.Annotations.NotNull] IHistoryRepository historyRepository, [JetBrains.Annotations.NotNull] IOperationReporter operationReporter, [JetBrains.Annotations.NotNull] IDatabaseProvider databaseProvider, [JetBrains.Annotations.NotNull] ISnapshotModelProcessor snapshotModelProcessor, [JetBrains.Annotations.NotNull] IMigrator migrator)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationsAssembly, "migrationsAssembly");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationsModelDiffer, "migrationsModelDiffer");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationsIdGenerator, "migrationsIdGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationsCodeGeneratorSelector, "migrationsCodeGeneratorSelector");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(historyRepository, "historyRepository");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operationReporter, "operationReporter");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseProvider, "databaseProvider");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(snapshotModelProcessor, "snapshotModelProcessor");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrator, "migrator");
		CurrentContext = currentContext;
		Model = model;
		MigrationsAssembly = migrationsAssembly;
		MigrationsModelDiffer = migrationsModelDiffer;
		MigrationsIdGenerator = migrationsIdGenerator;
		MigrationsCodeGeneratorSelector = migrationsCodeGeneratorSelector;
		HistoryRepository = historyRepository;
		OperationReporter = operationReporter;
		DatabaseProvider = databaseProvider;
		SnapshotModelProcessor = snapshotModelProcessor;
		Migrator = migrator;
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new MigrationsScaffolderDependencies(currentContext, Model, MigrationsAssembly, MigrationsModelDiffer, MigrationsIdGenerator, MigrationsCodeGeneratorSelector, HistoryRepository, OperationReporter, DatabaseProvider, SnapshotModelProcessor, Migrator);
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] IModel model)
	{
		return new MigrationsScaffolderDependencies(CurrentContext, model, MigrationsAssembly, MigrationsModelDiffer, MigrationsIdGenerator, MigrationsCodeGeneratorSelector, HistoryRepository, OperationReporter, DatabaseProvider, SnapshotModelProcessor, Migrator);
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] IMigrationsAssembly migrationsAssembly)
	{
		return new MigrationsScaffolderDependencies(CurrentContext, Model, migrationsAssembly, MigrationsModelDiffer, MigrationsIdGenerator, MigrationsCodeGeneratorSelector, HistoryRepository, OperationReporter, DatabaseProvider, SnapshotModelProcessor, Migrator);
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] IMigrationsModelDiffer migrationsModelDiffer)
	{
		return new MigrationsScaffolderDependencies(CurrentContext, Model, MigrationsAssembly, migrationsModelDiffer, MigrationsIdGenerator, MigrationsCodeGeneratorSelector, HistoryRepository, OperationReporter, DatabaseProvider, SnapshotModelProcessor, Migrator);
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] IMigrationsIdGenerator migrationsIdGenerator)
	{
		return new MigrationsScaffolderDependencies(CurrentContext, Model, MigrationsAssembly, MigrationsModelDiffer, migrationsIdGenerator, MigrationsCodeGeneratorSelector, HistoryRepository, OperationReporter, DatabaseProvider, SnapshotModelProcessor, Migrator);
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] IMigrationsCodeGeneratorSelector migrationsCodeGeneratorSelector)
	{
		return new MigrationsScaffolderDependencies(CurrentContext, Model, MigrationsAssembly, MigrationsModelDiffer, MigrationsIdGenerator, migrationsCodeGeneratorSelector, HistoryRepository, OperationReporter, DatabaseProvider, SnapshotModelProcessor, Migrator);
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] IHistoryRepository historyRepository)
	{
		return new MigrationsScaffolderDependencies(CurrentContext, Model, MigrationsAssembly, MigrationsModelDiffer, MigrationsIdGenerator, MigrationsCodeGeneratorSelector, historyRepository, OperationReporter, DatabaseProvider, SnapshotModelProcessor, Migrator);
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] IOperationReporter operationReporter)
	{
		return new MigrationsScaffolderDependencies(CurrentContext, Model, MigrationsAssembly, MigrationsModelDiffer, MigrationsIdGenerator, MigrationsCodeGeneratorSelector, HistoryRepository, operationReporter, DatabaseProvider, SnapshotModelProcessor, Migrator);
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] IDatabaseProvider databaseProvider)
	{
		return new MigrationsScaffolderDependencies(CurrentContext, Model, MigrationsAssembly, MigrationsModelDiffer, MigrationsIdGenerator, MigrationsCodeGeneratorSelector, HistoryRepository, OperationReporter, databaseProvider, SnapshotModelProcessor, Migrator);
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] ISnapshotModelProcessor snapshotModelProcessor)
	{
		return new MigrationsScaffolderDependencies(CurrentContext, Model, MigrationsAssembly, MigrationsModelDiffer, MigrationsIdGenerator, MigrationsCodeGeneratorSelector, HistoryRepository, OperationReporter, DatabaseProvider, snapshotModelProcessor, Migrator);
	}

	public MigrationsScaffolderDependencies With([JetBrains.Annotations.NotNull] IMigrator migrator)
	{
		return new MigrationsScaffolderDependencies(CurrentContext, Model, MigrationsAssembly, MigrationsModelDiffer, MigrationsIdGenerator, MigrationsCodeGeneratorSelector, HistoryRepository, OperationReporter, DatabaseProvider, SnapshotModelProcessor, migrator);
	}
}
