using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Update;

public sealed class ModificationCommandBatchFactoryDependencies
{
	public IDiagnosticsLogger<DbLoggerCategory.Database.Command> Logger { get; }

	public IRelationalCommandBuilderFactory CommandBuilderFactory { get; }

	public ISqlGenerationHelper SqlGenerationHelper { get; }

	public IUpdateSqlGenerator UpdateSqlGenerator { get; }

	public IRelationalValueBufferFactoryFactory ValueBufferFactoryFactory { get; }

	public ICurrentDbContext CurrentContext { get; }

	[EntityFrameworkInternal]
	public ModificationCommandBatchFactoryDependencies([JetBrains.Annotations.NotNull] IRelationalCommandBuilderFactory commandBuilderFactory, [JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper, [JetBrains.Annotations.NotNull] IUpdateSqlGenerator updateSqlGenerator, [JetBrains.Annotations.NotNull] IRelationalValueBufferFactoryFactory valueBufferFactoryFactory, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilderFactory, "commandBuilderFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlGenerationHelper, "sqlGenerationHelper");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(updateSqlGenerator, "updateSqlGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(valueBufferFactoryFactory, "valueBufferFactoryFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		CommandBuilderFactory = commandBuilderFactory;
		SqlGenerationHelper = sqlGenerationHelper;
		UpdateSqlGenerator = updateSqlGenerator;
		ValueBufferFactoryFactory = valueBufferFactoryFactory;
		CurrentContext = currentContext;
		Logger = logger;
	}

	public ModificationCommandBatchFactoryDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger)
	{
		return new ModificationCommandBatchFactoryDependencies(CommandBuilderFactory, SqlGenerationHelper, UpdateSqlGenerator, ValueBufferFactoryFactory, CurrentContext, logger);
	}

	public ModificationCommandBatchFactoryDependencies With([JetBrains.Annotations.NotNull] IRelationalValueBufferFactoryFactory valueBufferFactoryFactory)
	{
		return new ModificationCommandBatchFactoryDependencies(CommandBuilderFactory, SqlGenerationHelper, UpdateSqlGenerator, valueBufferFactoryFactory, CurrentContext, Logger);
	}

	public ModificationCommandBatchFactoryDependencies With([JetBrains.Annotations.NotNull] IRelationalCommandBuilderFactory commandBuilderFactory)
	{
		return new ModificationCommandBatchFactoryDependencies(commandBuilderFactory, SqlGenerationHelper, UpdateSqlGenerator, ValueBufferFactoryFactory, CurrentContext, Logger);
	}

	public ModificationCommandBatchFactoryDependencies With([JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper)
	{
		return new ModificationCommandBatchFactoryDependencies(CommandBuilderFactory, sqlGenerationHelper, UpdateSqlGenerator, ValueBufferFactoryFactory, CurrentContext, Logger);
	}

	public ModificationCommandBatchFactoryDependencies With([JetBrains.Annotations.NotNull] IUpdateSqlGenerator updateSqlGenerator)
	{
		return new ModificationCommandBatchFactoryDependencies(CommandBuilderFactory, SqlGenerationHelper, updateSqlGenerator, ValueBufferFactoryFactory, CurrentContext, Logger);
	}

	public ModificationCommandBatchFactoryDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new ModificationCommandBatchFactoryDependencies(CommandBuilderFactory, SqlGenerationHelper, UpdateSqlGenerator, ValueBufferFactoryFactory, currentContext, Logger);
	}
}
