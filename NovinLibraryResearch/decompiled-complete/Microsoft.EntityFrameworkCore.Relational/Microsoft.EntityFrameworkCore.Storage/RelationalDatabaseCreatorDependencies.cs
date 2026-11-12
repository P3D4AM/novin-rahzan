using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public sealed class RelationalDatabaseCreatorDependencies
{
	public IMigrationsModelDiffer ModelDiffer { get; }

	public IMigrationsSqlGenerator MigrationsSqlGenerator { get; }

	public IModel Model { get; }

	public IRelationalConnection Connection { get; }

	public IMigrationCommandExecutor MigrationCommandExecutor { get; }

	public ISqlGenerationHelper SqlGenerationHelper { get; }

	public IExecutionStrategyFactory ExecutionStrategyFactory { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Database.Command> CommandLogger { get; }

	public ICurrentDbContext CurrentContext { get; }

	[EntityFrameworkInternal]
	public RelationalDatabaseCreatorDependencies([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] IMigrationsModelDiffer modelDiffer, [JetBrains.Annotations.NotNull] IMigrationsSqlGenerator migrationsSqlGenerator, [JetBrains.Annotations.NotNull] IMigrationCommandExecutor migrationCommandExecutor, [JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper, [JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelDiffer, "modelDiffer");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationsSqlGenerator, "migrationsSqlGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationCommandExecutor, "migrationCommandExecutor");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlGenerationHelper, "sqlGenerationHelper");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executionStrategyFactory, "executionStrategyFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandLogger, "commandLogger");
		Model = model;
		Connection = connection;
		ModelDiffer = modelDiffer;
		MigrationsSqlGenerator = migrationsSqlGenerator;
		MigrationCommandExecutor = migrationCommandExecutor;
		SqlGenerationHelper = sqlGenerationHelper;
		ExecutionStrategyFactory = executionStrategyFactory;
		CurrentContext = currentContext;
		CommandLogger = commandLogger;
	}

	public RelationalDatabaseCreatorDependencies With([JetBrains.Annotations.NotNull] IModel model)
	{
		return new RelationalDatabaseCreatorDependencies(model, Connection, ModelDiffer, MigrationsSqlGenerator, MigrationCommandExecutor, SqlGenerationHelper, ExecutionStrategyFactory, CurrentContext, CommandLogger);
	}

	public RelationalDatabaseCreatorDependencies With([JetBrains.Annotations.NotNull] IRelationalConnection connection)
	{
		return new RelationalDatabaseCreatorDependencies(Model, connection, ModelDiffer, MigrationsSqlGenerator, MigrationCommandExecutor, SqlGenerationHelper, ExecutionStrategyFactory, CurrentContext, CommandLogger);
	}

	public RelationalDatabaseCreatorDependencies With([JetBrains.Annotations.NotNull] IMigrationsModelDiffer modelDiffer)
	{
		return new RelationalDatabaseCreatorDependencies(Model, Connection, modelDiffer, MigrationsSqlGenerator, MigrationCommandExecutor, SqlGenerationHelper, ExecutionStrategyFactory, CurrentContext, CommandLogger);
	}

	public RelationalDatabaseCreatorDependencies With([JetBrains.Annotations.NotNull] IMigrationsSqlGenerator migrationsSqlGenerator)
	{
		return new RelationalDatabaseCreatorDependencies(Model, Connection, ModelDiffer, migrationsSqlGenerator, MigrationCommandExecutor, SqlGenerationHelper, ExecutionStrategyFactory, CurrentContext, CommandLogger);
	}

	public RelationalDatabaseCreatorDependencies With([JetBrains.Annotations.NotNull] IMigrationCommandExecutor migrationCommandExecutor)
	{
		return new RelationalDatabaseCreatorDependencies(Model, Connection, ModelDiffer, MigrationsSqlGenerator, migrationCommandExecutor, SqlGenerationHelper, ExecutionStrategyFactory, CurrentContext, CommandLogger);
	}

	public RelationalDatabaseCreatorDependencies With([JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper)
	{
		return new RelationalDatabaseCreatorDependencies(Model, Connection, ModelDiffer, MigrationsSqlGenerator, MigrationCommandExecutor, sqlGenerationHelper, ExecutionStrategyFactory, CurrentContext, CommandLogger);
	}

	public RelationalDatabaseCreatorDependencies With([JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory)
	{
		return new RelationalDatabaseCreatorDependencies(Model, Connection, ModelDiffer, MigrationsSqlGenerator, MigrationCommandExecutor, SqlGenerationHelper, executionStrategyFactory, CurrentContext, CommandLogger);
	}

	public RelationalDatabaseCreatorDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new RelationalDatabaseCreatorDependencies(Model, Connection, ModelDiffer, MigrationsSqlGenerator, MigrationCommandExecutor, SqlGenerationHelper, ExecutionStrategyFactory, currentContext, CommandLogger);
	}

	public RelationalDatabaseCreatorDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
	{
		return new RelationalDatabaseCreatorDependencies(Model, Connection, ModelDiffer, MigrationsSqlGenerator, MigrationCommandExecutor, SqlGenerationHelper, ExecutionStrategyFactory, CurrentContext, commandLogger);
	}
}
