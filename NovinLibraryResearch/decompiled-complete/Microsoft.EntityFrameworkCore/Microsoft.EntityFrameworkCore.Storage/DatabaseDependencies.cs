using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public sealed class DatabaseDependencies
{
	public IQueryCompilationContextFactory QueryCompilationContextFactory { get; }

	public IUpdateAdapterFactory UpdateAdapterFactory { get; }

	[EntityFrameworkInternal]
	public DatabaseDependencies([JetBrains.Annotations.NotNull] IQueryCompilationContextFactory queryCompilationContextFactory, [JetBrains.Annotations.NotNull] IUpdateAdapterFactory updateAdapterFactory)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(queryCompilationContextFactory, "queryCompilationContextFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(updateAdapterFactory, "updateAdapterFactory");
		QueryCompilationContextFactory = queryCompilationContextFactory;
		UpdateAdapterFactory = updateAdapterFactory;
	}

	public DatabaseDependencies With([JetBrains.Annotations.NotNull] IQueryCompilationContextFactory queryCompilationContextFactory)
	{
		return new DatabaseDependencies(queryCompilationContextFactory, UpdateAdapterFactory);
	}

	public DatabaseDependencies With([JetBrains.Annotations.NotNull] IUpdateAdapterFactory updateAdapterFactory)
	{
		return new DatabaseDependencies(QueryCompilationContextFactory, updateAdapterFactory);
	}
}
