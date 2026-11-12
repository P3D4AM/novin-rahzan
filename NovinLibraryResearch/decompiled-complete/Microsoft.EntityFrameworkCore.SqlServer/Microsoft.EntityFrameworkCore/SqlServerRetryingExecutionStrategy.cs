using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore;

public class SqlServerRetryingExecutionStrategy : ExecutionStrategy
{
	private readonly ICollection<int> _additionalErrorNumbers;

	public SqlServerRetryingExecutionStrategy([JetBrains.Annotations.NotNull] DbContext context)
		: this(context, ExecutionStrategy.DefaultMaxRetryCount)
	{
	}

	public SqlServerRetryingExecutionStrategy([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies)
		: this(dependencies, ExecutionStrategy.DefaultMaxRetryCount)
	{
	}

	public SqlServerRetryingExecutionStrategy([JetBrains.Annotations.NotNull] DbContext context, int maxRetryCount)
		: this(context, maxRetryCount, ExecutionStrategy.DefaultMaxDelay, null)
	{
	}

	public SqlServerRetryingExecutionStrategy([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies, int maxRetryCount)
		: this(dependencies, maxRetryCount, ExecutionStrategy.DefaultMaxDelay, null)
	{
	}

	public SqlServerRetryingExecutionStrategy([JetBrains.Annotations.NotNull] DbContext context, int maxRetryCount, TimeSpan maxRetryDelay, [JetBrains.Annotations.CanBeNull] ICollection<int> errorNumbersToAdd)
		: base(context, maxRetryCount, maxRetryDelay)
	{
		_additionalErrorNumbers = errorNumbersToAdd;
	}

	public SqlServerRetryingExecutionStrategy([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies, int maxRetryCount, TimeSpan maxRetryDelay, [JetBrains.Annotations.CanBeNull] ICollection<int> errorNumbersToAdd)
		: base(dependencies, maxRetryCount, maxRetryDelay)
	{
		_additionalErrorNumbers = errorNumbersToAdd;
	}

	protected override bool ShouldRetryOn(Exception exception)
	{
		if (_additionalErrorNumbers != null && exception is SqlException ex)
		{
			foreach (SqlError error in ex.Errors)
			{
				if (_additionalErrorNumbers.Contains(error.Number))
				{
					return true;
				}
			}
		}
		return SqlServerTransientExceptionDetector.ShouldRetryOn(exception);
	}

	protected override TimeSpan? GetNextDelay(Exception lastException)
	{
		TimeSpan? nextDelay = base.GetNextDelay(lastException);
		if (!nextDelay.HasValue)
		{
			return null;
		}
		if (!ExecutionStrategy.CallOnWrappedException(lastException, IsMemoryOptimizedError))
		{
			return nextDelay;
		}
		return TimeSpan.FromMilliseconds(nextDelay.Value.TotalSeconds);
	}

	private static bool IsMemoryOptimizedError(Exception exception)
	{
		if (exception is SqlException ex)
		{
			{
				IEnumerator enumerator = ex.Errors.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						switch (((SqlError)enumerator.Current).Number)
						{
						case 41301:
						case 41302:
						case 41305:
						case 41325:
						case 41839:
							return true;
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
		return false;
	}
}
