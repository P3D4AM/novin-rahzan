using System;
using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public abstract class RelationalDbContextOptionsBuilder<TBuilder, TExtension> : IRelationalDbContextOptionsBuilderInfrastructure where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TExtension> where TExtension : RelationalOptionsExtension, new()
{
	protected virtual DbContextOptionsBuilder OptionsBuilder { get; }

	DbContextOptionsBuilder IRelationalDbContextOptionsBuilderInfrastructure.OptionsBuilder => OptionsBuilder;

	protected RelationalDbContextOptionsBuilder([JetBrains.Annotations.NotNull] DbContextOptionsBuilder optionsBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(optionsBuilder, "optionsBuilder");
		OptionsBuilder = optionsBuilder;
	}

	public virtual TBuilder MaxBatchSize(int maxBatchSize)
	{
		return WithOption((TExtension e) => (TExtension)e.WithMaxBatchSize(maxBatchSize));
	}

	public virtual TBuilder MinBatchSize(int minBatchSize)
	{
		return WithOption((TExtension e) => (TExtension)e.WithMinBatchSize(minBatchSize));
	}

	public virtual TBuilder CommandTimeout(int? commandTimeout)
	{
		return WithOption((TExtension e) => (TExtension)e.WithCommandTimeout(commandTimeout));
	}

	public virtual TBuilder MigrationsAssembly([JetBrains.Annotations.CanBeNull] string assemblyName)
	{
		return WithOption((TExtension e) => (TExtension)e.WithMigrationsAssembly(Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(assemblyName, "assemblyName")));
	}

	public virtual TBuilder MigrationsHistoryTable([JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(tableName, "tableName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		return WithOption((TExtension e) => (TExtension)e.WithMigrationsHistoryTableName(tableName).WithMigrationsHistoryTableSchema(schema));
	}

	public virtual TBuilder UseRelationalNulls(bool useRelationalNulls = true)
	{
		return WithOption((TExtension e) => (TExtension)e.WithUseRelationalNulls(useRelationalNulls));
	}

	public virtual TBuilder ExecutionStrategy([JetBrains.Annotations.NotNull] Func<ExecutionStrategyDependencies, IExecutionStrategy> getExecutionStrategy)
	{
		return WithOption((TExtension e) => (TExtension)e.WithExecutionStrategyFactory(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(getExecutionStrategy, "getExecutionStrategy")));
	}

	protected virtual TBuilder WithOption([JetBrains.Annotations.NotNull] Func<TExtension, TExtension> setAction)
	{
		((IDbContextOptionsBuilderInfrastructure)OptionsBuilder).AddOrUpdateExtension(setAction(OptionsBuilder.Options.FindExtension<TExtension>() ?? new TExtension()));
		return (TBuilder)this;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
