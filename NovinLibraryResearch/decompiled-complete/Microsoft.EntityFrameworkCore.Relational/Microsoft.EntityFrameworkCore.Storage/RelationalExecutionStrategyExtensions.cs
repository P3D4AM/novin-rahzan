using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public static class RelationalExecutionStrategyExtensions
{
	public static void ExecuteInTransaction([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Action operation, [JetBrains.Annotations.NotNull] Func<bool> verifySucceeded, IsolationLevel isolationLevel)
	{
		strategy.ExecuteInTransaction(null, delegate
		{
			operation();
		}, (object s) => verifySucceeded(), isolationLevel);
	}

	public static Task ExecuteInTransactionAsync([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<Task> operation, [JetBrains.Annotations.NotNull] Func<Task<bool>> verifySucceeded, IsolationLevel isolationLevel)
	{
		return strategy.ExecuteInTransactionAsync(null, (object s, CancellationToken ct) => operation(), (object s, CancellationToken ct) => verifySucceeded(), isolationLevel);
	}

	public static Task ExecuteInTransactionAsync([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<CancellationToken, Task> operation, [JetBrains.Annotations.NotNull] Func<CancellationToken, Task<bool>> verifySucceeded, IsolationLevel isolationLevel, CancellationToken cancellationToken = default(CancellationToken))
	{
		return strategy.ExecuteInTransactionAsync(null, (object s, CancellationToken ct) => operation(ct), (object s, CancellationToken ct) => verifySucceeded(ct), isolationLevel, cancellationToken);
	}

	public static TResult ExecuteInTransaction<TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<TResult> operation, [JetBrains.Annotations.NotNull] Func<bool> verifySucceeded, IsolationLevel isolationLevel)
	{
		return strategy.ExecuteInTransaction(null, (object s) => operation(), (object s) => verifySucceeded(), isolationLevel);
	}

	public static Task<TResult> ExecuteInTransactionAsync<TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<CancellationToken, Task<TResult>> operation, [JetBrains.Annotations.NotNull] Func<CancellationToken, Task<bool>> verifySucceeded, IsolationLevel isolationLevel, CancellationToken cancellationToken = default(CancellationToken))
	{
		return strategy.ExecuteInTransactionAsync(null, (object s, CancellationToken ct) => operation(ct), (object s, CancellationToken ct) => verifySucceeded(ct), isolationLevel, cancellationToken);
	}

	public static void ExecuteInTransaction<TState>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Action<TState> operation, [JetBrains.Annotations.NotNull] Func<TState, bool> verifySucceeded, IsolationLevel isolationLevel)
	{
		strategy.ExecuteInTransaction(state, delegate(TState s)
		{
			operation(s);
			return true;
		}, verifySucceeded, isolationLevel);
	}

	public static Task ExecuteInTransactionAsync<TState>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task> operation, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task<bool>> verifySucceeded, IsolationLevel isolationLevel, CancellationToken cancellationToken = default(CancellationToken))
	{
		return strategy.ExecuteInTransactionAsync(state, async delegate(TState s, CancellationToken ct)
		{
			await operation(s, ct);
			return true;
		}, verifySucceeded, isolationLevel, cancellationToken);
	}

	public static TResult ExecuteInTransaction<TState, TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, TResult> operation, [JetBrains.Annotations.NotNull] Func<TState, bool> verifySucceeded, IsolationLevel isolationLevel)
	{
		return ExecutionStrategyExtensions.ExecuteInTransaction(strategy, state, operation, verifySucceeded, (DbContext c) => c.Database.BeginTransaction(isolationLevel));
	}

	public static Task<TResult> ExecuteInTransactionAsync<TState, TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task<TResult>> operation, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task<bool>> verifySucceeded, IsolationLevel isolationLevel, CancellationToken cancellationToken = default(CancellationToken))
	{
		return ExecutionStrategyExtensions.ExecuteInTransactionAsync(strategy, state, operation, verifySucceeded, (DbContext c, CancellationToken ct) => c.Database.BeginTransactionAsync(isolationLevel, ct), cancellationToken);
	}
}
