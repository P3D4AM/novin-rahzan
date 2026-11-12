using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class ExecutionStrategyExtensions
{
	private class ExecutionState<TState, TResult>
	{
		public Func<TState, TResult> Operation { get; }

		public Func<TState, bool> VerifySucceeded { get; }

		public TState State { get; }

		public TResult Result { get; set; }

		public bool CommitFailed { get; set; }

		public ExecutionState(Func<TState, TResult> operation, Func<TState, bool> verifySucceeded, TState state)
		{
			Operation = operation;
			VerifySucceeded = verifySucceeded;
			State = state;
		}
	}

	private class ExecutionStateAsync<TState, TResult>
	{
		public Func<TState, CancellationToken, Task<TResult>> Operation { get; }

		public Func<TState, CancellationToken, Task<bool>> VerifySucceeded { get; }

		public TState State { get; }

		public TResult Result { get; set; }

		public bool CommitFailed { get; set; }

		public ExecutionStateAsync(Func<TState, CancellationToken, Task<TResult>> operation, Func<TState, CancellationToken, Task<bool>> verifySucceeded, TState state)
		{
			Operation = operation;
			VerifySucceeded = verifySucceeded;
			State = state;
		}
	}

	public static void Execute([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Action operation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		strategy.Execute(operation, delegate(Action operationScoped)
		{
			operationScoped();
			return true;
		});
	}

	public static TResult Execute<TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<TResult> operation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		return strategy.Execute(operation, (Func<TResult> operationScoped) => operationScoped());
	}

	public static void Execute<TState>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Action<TState> operation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		strategy.Execute(new { operation, state }, s =>
		{
			s.operation(s.state);
			return true;
		});
	}

	public static Task ExecuteAsync([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<Task> operation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		return strategy.ExecuteAsync(operation, async delegate(Func<Task> operationScoped, CancellationToken ct)
		{
			await operationScoped();
			return true;
		}, default(CancellationToken));
	}

	public static Task ExecuteAsync([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<CancellationToken, Task> operation, CancellationToken cancellationToken)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		return strategy.ExecuteAsync(operation, async delegate(Func<CancellationToken, Task> operationScoped, CancellationToken ct)
		{
			await operationScoped(ct);
			return true;
		}, cancellationToken);
	}

	public static Task<TResult> ExecuteAsync<TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<Task<TResult>> operation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		return strategy.ExecuteAsync(operation, (Func<Task<TResult>> operationScoped, CancellationToken ct) => operationScoped(), default(CancellationToken));
	}

	public static Task<TResult> ExecuteAsync<TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<CancellationToken, Task<TResult>> operation, CancellationToken cancellationToken)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		return strategy.ExecuteAsync(operation, (Func<CancellationToken, Task<TResult>> operationScoped, CancellationToken ct) => operationScoped(ct), cancellationToken);
	}

	public static Task ExecuteAsync<TState>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, Task> operation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		return strategy.ExecuteAsync(new { operation, state }, async (t, CancellationToken ct) =>
		{
			await t.operation(t.state);
			return true;
		}, default(CancellationToken));
	}

	public static Task ExecuteAsync<TState>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task> operation, CancellationToken cancellationToken)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		return strategy.ExecuteAsync(new { operation, state }, async (t, CancellationToken ct) =>
		{
			await t.operation(t.state, ct);
			return true;
		}, cancellationToken);
	}

	public static Task<TResult> ExecuteAsync<TState, TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, Task<TResult>> operation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		return strategy.ExecuteAsync(new { operation, state }, (t, CancellationToken ct) => t.operation(t.state), default(CancellationToken));
	}

	public static TResult Execute<TState, TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, TResult> operation)
	{
		return strategy.Execute(state, operation, null);
	}

	public static Task<TResult> ExecuteAsync<TState, TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task<TResult>> operation, CancellationToken cancellationToken)
	{
		return strategy.ExecuteAsync(state, operation, null, cancellationToken);
	}

	public static TResult Execute<TState, TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, TResult> operation, [JetBrains.Annotations.CanBeNull] Func<TState, ExecutionResult<TResult>> verifySucceeded)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(strategy, "strategy").Execute(state, (DbContext c, TState s) => operation(s), (verifySucceeded == null) ? null : ((Func<DbContext, TState, ExecutionResult<TResult>>)((DbContext c, TState s) => verifySucceeded(s))));
	}

	[Obsolete("Use overload that takes the state first")]
	public static TResult Execute<TState, TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<TState, TResult> operation, [JetBrains.Annotations.CanBeNull] Func<TState, ExecutionResult<TResult>> verifySucceeded, [JetBrains.Annotations.CanBeNull] TState state)
	{
		return strategy.Execute(state, operation, verifySucceeded);
	}

	public static Task<TResult> ExecuteAsync<TState, TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task<TResult>> operation, [JetBrains.Annotations.CanBeNull] Func<TState, CancellationToken, Task<ExecutionResult<TResult>>> verifySucceeded, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(strategy, "strategy").ExecuteAsync(state, (DbContext c, TState s, CancellationToken ct) => operation(s, ct), (verifySucceeded == null) ? null : ((Func<DbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>>)((DbContext c, TState s, CancellationToken ct) => verifySucceeded(s, ct))), cancellationToken);
	}

	public static void ExecuteInTransaction([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Action operation, [JetBrains.Annotations.NotNull] Func<bool> verifySucceeded)
	{
		strategy.ExecuteInTransaction(null, delegate
		{
			operation();
		}, (object s) => verifySucceeded());
	}

	public static Task ExecuteInTransactionAsync([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<Task> operation, [JetBrains.Annotations.NotNull] Func<Task<bool>> verifySucceeded)
	{
		return strategy.ExecuteInTransactionAsync(null, (object s, CancellationToken ct) => operation(), (object s, CancellationToken ct) => verifySucceeded());
	}

	public static Task ExecuteInTransactionAsync([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<CancellationToken, Task> operation, [JetBrains.Annotations.NotNull] Func<CancellationToken, Task<bool>> verifySucceeded, CancellationToken cancellationToken = default(CancellationToken))
	{
		return strategy.ExecuteInTransactionAsync(null, (object s, CancellationToken ct) => operation(ct), (object s, CancellationToken ct) => verifySucceeded(ct), cancellationToken);
	}

	public static TResult ExecuteInTransaction<TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<TResult> operation, [JetBrains.Annotations.NotNull] Func<bool> verifySucceeded)
	{
		return strategy.ExecuteInTransaction(null, (object s) => operation(), (object s) => verifySucceeded());
	}

	public static Task<TResult> ExecuteInTransactionAsync<TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.NotNull] Func<CancellationToken, Task<TResult>> operation, [JetBrains.Annotations.NotNull] Func<CancellationToken, Task<bool>> verifySucceeded, CancellationToken cancellationToken = default(CancellationToken))
	{
		return strategy.ExecuteInTransactionAsync(null, (object s, CancellationToken ct) => operation(ct), (object s, CancellationToken ct) => verifySucceeded(ct), cancellationToken);
	}

	public static void ExecuteInTransaction<TState>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Action<TState> operation, [JetBrains.Annotations.NotNull] Func<TState, bool> verifySucceeded)
	{
		strategy.ExecuteInTransaction(state, delegate(TState s)
		{
			operation(s);
			return true;
		}, verifySucceeded);
	}

	public static Task ExecuteInTransactionAsync<TState>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task> operation, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task<bool>> verifySucceeded, CancellationToken cancellationToken = default(CancellationToken))
	{
		return strategy.ExecuteInTransactionAsync(state, async delegate(TState s, CancellationToken ct)
		{
			await operation(s, ct);
			return true;
		}, verifySucceeded, cancellationToken);
	}

	public static TResult ExecuteInTransaction<TState, TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, TResult> operation, [JetBrains.Annotations.NotNull] Func<TState, bool> verifySucceeded)
	{
		return ExecuteInTransaction(strategy, state, operation, verifySucceeded, (DbContext c) => c.Database.BeginTransaction());
	}

	public static Task<TResult> ExecuteInTransactionAsync<TState, TResult>([JetBrains.Annotations.NotNull] this IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task<TResult>> operation, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task<bool>> verifySucceeded, CancellationToken cancellationToken = default(CancellationToken))
	{
		return ExecuteInTransactionAsync(strategy, state, operation, verifySucceeded, (DbContext c, CancellationToken ct) => c.Database.BeginTransactionAsync(ct), cancellationToken);
	}

	public static TResult ExecuteInTransaction<TState, TResult>([JetBrains.Annotations.NotNull] IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, TResult> operation, [JetBrains.Annotations.NotNull] Func<TState, bool> verifySucceeded, [JetBrains.Annotations.NotNull] Func<DbContext, IDbContextTransaction> beginTransaction)
	{
		return strategy.Execute(new ExecutionState<TState, TResult>(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation"), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(verifySucceeded, "verifySucceeded"), state), delegate(DbContext c, ExecutionState<TState, TResult> s)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(beginTransaction, "beginTransaction");
			using (IDbContextTransaction dbContextTransaction = beginTransaction(c))
			{
				s.CommitFailed = false;
				s.Result = s.Operation(s.State);
				s.CommitFailed = true;
				dbContextTransaction.Commit();
			}
			return s.Result;
		}, (DbContext c, ExecutionState<TState, TResult> s) => new ExecutionResult<TResult>(s.CommitFailed && s.VerifySucceeded(s.State), s.Result));
	}

	public static Task<TResult> ExecuteInTransactionAsync<TState, TResult>([JetBrains.Annotations.NotNull] IExecutionStrategy strategy, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task<TResult>> operation, [JetBrains.Annotations.NotNull] Func<TState, CancellationToken, Task<bool>> verifySucceeded, [JetBrains.Annotations.NotNull] Func<DbContext, CancellationToken, Task<IDbContextTransaction>> beginTransaction, CancellationToken cancellationToken = default(CancellationToken))
	{
		return strategy.ExecuteAsync(new ExecutionStateAsync<TState, TResult>(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation"), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(verifySucceeded, "verifySucceeded"), state), async delegate(DbContext c, ExecutionStateAsync<TState, TResult> s, CancellationToken ct)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(beginTransaction, "beginTransaction");
			await using (IDbContextTransaction transaction = await beginTransaction(c, cancellationToken))
			{
				s.CommitFailed = false;
				s.Result = await s.Operation(s.State, ct);
				s.CommitFailed = true;
				transaction.Commit();
			}
			return s.Result;
		}, async delegate(DbContext c, ExecutionStateAsync<TState, TResult> s, CancellationToken ct)
		{
			bool flag = s.CommitFailed;
			if (flag)
			{
				flag = await s.VerifySucceeded(s.State, ct);
			}
			return new ExecutionResult<TResult>(flag, s.Result);
		});
	}
}
