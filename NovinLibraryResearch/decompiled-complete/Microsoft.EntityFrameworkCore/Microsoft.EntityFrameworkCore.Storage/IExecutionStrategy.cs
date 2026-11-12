using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IExecutionStrategy
{
	bool RetriesOnFailure { get; }

	TResult Execute<TState, TResult>([JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<DbContext, TState, TResult> operation, [JetBrains.Annotations.CanBeNull] Func<DbContext, TState, ExecutionResult<TResult>> verifySucceeded);

	Task<TResult> ExecuteAsync<TState, TResult>([JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<DbContext, TState, CancellationToken, Task<TResult>> operation, [JetBrains.Annotations.CanBeNull] Func<DbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>> verifySucceeded, CancellationToken cancellationToken = default(CancellationToken));
}
