using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class SqlServerDbContextOptionsBuilder : RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>
{
	public SqlServerDbContextOptionsBuilder([JetBrains.Annotations.NotNull] DbContextOptionsBuilder optionsBuilder)
		: base(optionsBuilder)
	{
	}

	[Obsolete("Row-number paging is no longer supported. See https://aka.ms/AA6h122 for more information.")]
	public virtual SqlServerDbContextOptionsBuilder UseRowNumberForPaging(bool useRowNumberForPaging = true)
	{
		return WithOption((SqlServerOptionsExtension e) => e.WithRowNumberPaging(useRowNumberForPaging));
	}

	public virtual SqlServerDbContextOptionsBuilder EnableRetryOnFailure()
	{
		return ExecutionStrategy((ExecutionStrategyDependencies c) => new SqlServerRetryingExecutionStrategy(c));
	}

	public virtual SqlServerDbContextOptionsBuilder EnableRetryOnFailure(int maxRetryCount)
	{
		return ExecutionStrategy((ExecutionStrategyDependencies c) => new SqlServerRetryingExecutionStrategy(c, maxRetryCount));
	}

	public virtual SqlServerDbContextOptionsBuilder EnableRetryOnFailure(int maxRetryCount, TimeSpan maxRetryDelay, [JetBrains.Annotations.CanBeNull] ICollection<int> errorNumbersToAdd)
	{
		return ExecutionStrategy((ExecutionStrategyDependencies c) => new SqlServerRetryingExecutionStrategy(c, maxRetryCount, maxRetryDelay, errorNumbersToAdd));
	}
}
