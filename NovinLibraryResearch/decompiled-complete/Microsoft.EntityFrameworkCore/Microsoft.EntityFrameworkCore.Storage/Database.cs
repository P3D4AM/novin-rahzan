using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class Database : IDatabase
{
	protected virtual DatabaseDependencies Dependencies { get; }

	protected Database([JetBrains.Annotations.NotNull] DatabaseDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public abstract int SaveChanges(IList<IUpdateEntry> entries);

	public abstract Task<int> SaveChangesAsync(IList<IUpdateEntry> entries, CancellationToken cancellationToken = default(CancellationToken));

	public virtual Func<QueryContext, TResult> CompileQuery<TResult>(Expression query, bool async)
	{
		return Dependencies.QueryCompilationContextFactory.Create(async).CreateQueryExecutor<TResult>(query);
	}
}
