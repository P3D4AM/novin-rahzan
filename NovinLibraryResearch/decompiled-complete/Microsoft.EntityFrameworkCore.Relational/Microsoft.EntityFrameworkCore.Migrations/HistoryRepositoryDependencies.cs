using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public sealed class HistoryRepositoryDependencies
{
	public IRelationalDatabaseCreator DatabaseCreator { get; }

	public IRawSqlCommandBuilder RawSqlCommandBuilder { get; }

	public IRelationalConnection Connection { get; }

	public IDbContextOptions Options { get; }

	public IMigrationsModelDiffer ModelDiffer { get; }

	public IMigrationsSqlGenerator MigrationsSqlGenerator { get; }

	public ISqlGenerationHelper SqlGenerationHelper { get; }

	public IConventionSetBuilder ConventionSetBuilder { get; }

	public IRelationalTypeMappingSource TypeMappingSource { get; }

	public ICurrentDbContext CurrentContext { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Model> ModelLogger { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Database.Command> CommandLogger { get; }

	[EntityFrameworkInternal]
	public HistoryRepositoryDependencies([JetBrains.Annotations.NotNull] IRelationalDatabaseCreator databaseCreator, [JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] IDbContextOptions options, [JetBrains.Annotations.NotNull] IMigrationsModelDiffer modelDiffer, [JetBrains.Annotations.NotNull] IMigrationsSqlGenerator migrationsSqlGenerator, [JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper, [JetBrains.Annotations.NotNull] IConventionSetBuilder conventionSetBuilder, [JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model> modelLogger, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseCreator, "databaseCreator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(rawSqlCommandBuilder, "rawSqlCommandBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelDiffer, "modelDiffer");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationsSqlGenerator, "migrationsSqlGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlGenerationHelper, "sqlGenerationHelper");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(conventionSetBuilder, "conventionSetBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelLogger, "modelLogger");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandLogger, "commandLogger");
		DatabaseCreator = databaseCreator;
		RawSqlCommandBuilder = rawSqlCommandBuilder;
		Connection = connection;
		Options = options;
		ModelDiffer = modelDiffer;
		MigrationsSqlGenerator = migrationsSqlGenerator;
		SqlGenerationHelper = sqlGenerationHelper;
		ConventionSetBuilder = conventionSetBuilder;
		TypeMappingSource = typeMappingSource;
		CurrentContext = currentContext;
		ModelLogger = modelLogger;
		CommandLogger = commandLogger;
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] IRelationalDatabaseCreator databaseCreator)
	{
		return new HistoryRepositoryDependencies(databaseCreator, RawSqlCommandBuilder, Connection, Options, ModelDiffer, MigrationsSqlGenerator, SqlGenerationHelper, ConventionSetBuilder, TypeMappingSource, CurrentContext, ModelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, rawSqlCommandBuilder, Connection, Options, ModelDiffer, MigrationsSqlGenerator, SqlGenerationHelper, ConventionSetBuilder, TypeMappingSource, CurrentContext, ModelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] IRelationalConnection connection)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, RawSqlCommandBuilder, connection, Options, ModelDiffer, MigrationsSqlGenerator, SqlGenerationHelper, ConventionSetBuilder, TypeMappingSource, CurrentContext, ModelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] IDbContextOptions options)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, RawSqlCommandBuilder, Connection, options, ModelDiffer, MigrationsSqlGenerator, SqlGenerationHelper, ConventionSetBuilder, TypeMappingSource, CurrentContext, ModelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] IMigrationsModelDiffer modelDiffer)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, RawSqlCommandBuilder, Connection, Options, modelDiffer, MigrationsSqlGenerator, SqlGenerationHelper, ConventionSetBuilder, TypeMappingSource, CurrentContext, ModelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] IMigrationsSqlGenerator migrationsSqlGenerator)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, RawSqlCommandBuilder, Connection, Options, ModelDiffer, migrationsSqlGenerator, SqlGenerationHelper, ConventionSetBuilder, TypeMappingSource, CurrentContext, ModelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, RawSqlCommandBuilder, Connection, Options, ModelDiffer, MigrationsSqlGenerator, sqlGenerationHelper, ConventionSetBuilder, TypeMappingSource, CurrentContext, ModelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] IConventionSetBuilder conventionSetBuilder)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, RawSqlCommandBuilder, Connection, Options, ModelDiffer, MigrationsSqlGenerator, SqlGenerationHelper, conventionSetBuilder, TypeMappingSource, CurrentContext, ModelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, RawSqlCommandBuilder, Connection, Options, ModelDiffer, MigrationsSqlGenerator, SqlGenerationHelper, ConventionSetBuilder, typeMappingSource, CurrentContext, ModelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, RawSqlCommandBuilder, Connection, Options, ModelDiffer, MigrationsSqlGenerator, SqlGenerationHelper, ConventionSetBuilder, TypeMappingSource, currentContext, ModelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model> modelLogger)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, RawSqlCommandBuilder, Connection, Options, ModelDiffer, MigrationsSqlGenerator, SqlGenerationHelper, ConventionSetBuilder, TypeMappingSource, CurrentContext, modelLogger, CommandLogger);
	}

	public HistoryRepositoryDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
	{
		return new HistoryRepositoryDependencies(DatabaseCreator, RawSqlCommandBuilder, Connection, Options, ModelDiffer, MigrationsSqlGenerator, SqlGenerationHelper, ConventionSetBuilder, TypeMappingSource, CurrentContext, ModelLogger, commandLogger);
	}
}
