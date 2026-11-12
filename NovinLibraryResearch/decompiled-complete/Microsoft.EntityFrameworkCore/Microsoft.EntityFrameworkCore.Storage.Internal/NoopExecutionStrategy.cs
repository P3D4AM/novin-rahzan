using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public class NoopExecutionStrategy : IExecutionStrategy
{
	private ExecutionStrategyDependencies Dependencies { get; }

	public virtual bool RetriesOnFailure => false;

	public NoopExecutionStrategy([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual TResult Execute<TState, TResult>(TState state, Func<DbContext, TState, TResult> operation, Func<DbContext, TState, ExecutionResult<TResult>> verifySucceeded)
	{
		return operation(Dependencies.CurrentContext.Context, state);
	}

	public virtual Task<TResult> ExecuteAsync<TState, TResult>(TState state, Func<DbContext, TState, CancellationToken, Task<TResult>> operation, Func<DbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>> verifySucceeded, CancellationToken cancellationToken = default(CancellationToken))
	{
		return operation(Dependencies.CurrentContext.Context, state, cancellationToken);
	}
}
