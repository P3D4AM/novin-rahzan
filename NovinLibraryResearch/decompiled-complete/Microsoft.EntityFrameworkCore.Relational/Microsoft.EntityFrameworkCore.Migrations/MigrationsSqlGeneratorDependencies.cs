using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public sealed class MigrationsSqlGeneratorDependencies
{
	public IRelationalCommandBuilderFactory CommandBuilderFactory { get; }

	public IUpdateSqlGenerator UpdateSqlGenerator { get; }

	public ISqlGenerationHelper SqlGenerationHelper { get; }

	public IRelationalTypeMappingSource TypeMappingSource { get; }

	public ICurrentDbContext CurrentContext { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Database.Command> Logger { get; }

	[EntityFrameworkInternal]
	public MigrationsSqlGeneratorDependencies([JetBrains.Annotations.NotNull] IRelationalCommandBuilderFactory commandBuilderFactory, [JetBrains.Annotations.NotNull] IUpdateSqlGenerator updateSqlGenerator, [JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper, [JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilderFactory, "commandBuilderFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(updateSqlGenerator, "updateSqlGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlGenerationHelper, "sqlGenerationHelper");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		CommandBuilderFactory = commandBuilderFactory;
		SqlGenerationHelper = sqlGenerationHelper;
		UpdateSqlGenerator = updateSqlGenerator;
		TypeMappingSource = typeMappingSource;
		CurrentContext = currentContext;
		Logger = logger;
	}

	public MigrationsSqlGeneratorDependencies With([JetBrains.Annotations.NotNull] IRelationalCommandBuilderFactory commandBuilderFactory)
	{
		return new MigrationsSqlGeneratorDependencies(commandBuilderFactory, UpdateSqlGenerator, SqlGenerationHelper, TypeMappingSource, CurrentContext, Logger);
	}

	public MigrationsSqlGeneratorDependencies With([JetBrains.Annotations.NotNull] IUpdateSqlGenerator updateSqlGenerator)
	{
		return new MigrationsSqlGeneratorDependencies(CommandBuilderFactory, updateSqlGenerator, SqlGenerationHelper, TypeMappingSource, CurrentContext, Logger);
	}

	public MigrationsSqlGeneratorDependencies With([JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper)
	{
		return new MigrationsSqlGeneratorDependencies(CommandBuilderFactory, UpdateSqlGenerator, sqlGenerationHelper, TypeMappingSource, CurrentContext, Logger);
	}

	public MigrationsSqlGeneratorDependencies With([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		return new MigrationsSqlGeneratorDependencies(CommandBuilderFactory, UpdateSqlGenerator, SqlGenerationHelper, typeMappingSource, CurrentContext, Logger);
	}

	public MigrationsSqlGeneratorDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new MigrationsSqlGeneratorDependencies(CommandBuilderFactory, UpdateSqlGenerator, SqlGenerationHelper, TypeMappingSource, currentContext, Logger);
	}

	public MigrationsSqlGeneratorDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger)
	{
		return new MigrationsSqlGeneratorDependencies(CommandBuilderFactory, UpdateSqlGenerator, SqlGenerationHelper, TypeMappingSource, CurrentContext, logger);
	}
}
