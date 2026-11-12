using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class ExecutionStrategy : IExecutionStrategy
{
	protected static readonly int DefaultMaxRetryCount = 6;

	protected static readonly TimeSpan DefaultMaxDelay = TimeSpan.FromSeconds(30.0);

	private const double DefaultRandomFactor = 1.1;

	private const double DefaultExponentialBase = 2.0;

	private static readonly TimeSpan _defaultCoefficient = TimeSpan.FromSeconds(1.0);

	private static readonly AsyncLocal<bool?> _suspended = new AsyncLocal<bool?>();

	protected virtual List<Exception> ExceptionsEncountered { get; } = new List<Exception>();


	protected virtual Random Random { get; } = new Random();


	protected virtual int MaxRetryCount { get; }

	protected virtual TimeSpan MaxRetryDelay { get; }

	protected virtual ExecutionStrategyDependencies Dependencies { get; }

	protected static bool Suspended
	{
		get
		{
			return _suspended.Value.GetValueOrDefault();
		}
		set
		{
			_suspended.Value = value;
		}
	}

	public virtual bool RetriesOnFailure => !Suspended;

	protected ExecutionStrategy([JetBrains.Annotations.NotNull] DbContext context, int maxRetryCount, TimeSpan maxRetryDelay)
		: this(context.GetService<ExecutionStrategyDependencies>(), maxRetryCount, maxRetryDelay)
	{
	}

	protected ExecutionStrategy([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies, int maxRetryCount, TimeSpan maxRetryDelay)
	{
		if (maxRetryCount < 0)
		{
			throw new ArgumentOutOfRangeException("maxRetryCount");
		}
		if (maxRetryDelay.TotalMilliseconds < 0.0)
		{
			throw new ArgumentOutOfRangeException("maxRetryDelay");
		}
		Dependencies = dependencies;
		MaxRetryCount = maxRetryCount;
		MaxRetryDelay = maxRetryDelay;
	}

	public virtual TResult Execute<TState, TResult>(TState state, Func<DbContext, TState, TResult> operation, Func<DbContext, TState, ExecutionResult<TResult>> verifySucceeded)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		if (Suspended)
		{
			return operation(Dependencies.CurrentContext.Context, state);
		}
		OnFirstExecution();
		return ExecuteImplementation(operation, verifySucceeded, state);
	}

	private TResult ExecuteImplementation<TState, TResult>(Func<DbContext, TState, TResult> operation, Func<DbContext, TState, ExecutionResult<TResult>> verifySucceeded, TState state)
	{
		while (true)
		{
			TimeSpan? nextDelay;
			try
			{
				Suspended = true;
				TResult result = operation(Dependencies.CurrentContext.Context, state);
				Suspended = false;
				return result;
			}
			catch (Exception ex)
			{
				Suspended = false;
				if (verifySucceeded != null && CallOnWrappedException(ex, ShouldVerifySuccessOn))
				{
					ExecutionResult<TResult> executionResult = ExecuteImplementation(verifySucceeded, null, state);
					if (executionResult.IsSuccessful)
					{
						return executionResult.Result;
					}
				}
				if (!CallOnWrappedException(ex, ShouldRetryOn))
				{
					throw;
				}
				ExceptionsEncountered.Add(ex);
				nextDelay = GetNextDelay(ex);
				if (!nextDelay.HasValue)
				{
					throw new RetryLimitExceededException(CoreStrings.RetryLimitExceeded(MaxRetryCount, GetType().Name), ex);
				}
				Dependencies.Logger.ExecutionStrategyRetrying(ExceptionsEncountered, nextDelay.Value, async: true);
				OnRetry();
			}
			using ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(initialState: false);
			manualResetEventSlim.WaitHandle.WaitOne(nextDelay.Value);
		}
	}

	public virtual Task<TResult> ExecuteAsync<TState, TResult>(TState state, Func<DbContext, TState, CancellationToken, Task<TResult>> operation, Func<DbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>> verifySucceeded, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		if (Suspended)
		{
			return operation(Dependencies.CurrentContext.Context, state, cancellationToken);
		}
		OnFirstExecution();
		return ExecuteImplementationAsync(operation, verifySucceeded, state, cancellationToken);
	}

	private async Task<TResult> ExecuteImplementationAsync<TState, TResult>(Func<DbContext, TState, CancellationToken, Task<TResult>> operation, Func<DbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>> verifySucceeded, TState state, CancellationToken cancellationToken)
	{
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();
			TimeSpan? delay;
			try
			{
				Suspended = true;
				TResult result = await operation(Dependencies.CurrentContext.Context, state, cancellationToken);
				Suspended = false;
				return result;
			}
			catch (Exception ex)
			{
				Suspended = false;
				if (verifySucceeded != null && CallOnWrappedException(ex, ShouldVerifySuccessOn))
				{
					ExecutionResult<TResult> executionResult = await ExecuteImplementationAsync(verifySucceeded, null, state, cancellationToken);
					if (executionResult.IsSuccessful)
					{
						return executionResult.Result;
					}
				}
				if (!CallOnWrappedException(ex, ShouldRetryOn))
				{
					throw;
				}
				ExceptionsEncountered.Add(ex);
				delay = GetNextDelay(ex);
				if (!delay.HasValue)
				{
					throw new RetryLimitExceededException(CoreStrings.RetryLimitExceeded(MaxRetryCount, GetType().Name), ex);
				}
				Dependencies.Logger.ExecutionStrategyRetrying(ExceptionsEncountered, delay.Value, async: true);
				OnRetry();
			}
			await Task.Delay(delay.Value, cancellationToken);
		}
	}

	protected virtual void OnFirstExecution()
	{
		if (Dependencies.CurrentContext.Context.Database.CurrentTransaction != null || Dependencies.CurrentContext.Context.Database.GetEnlistedTransaction() != null || Transaction.Current != null)
		{
			throw new InvalidOperationException(CoreStrings.ExecutionStrategyExistingTransaction(GetType().Name, "DbContext.Database.CreateExecutionStrategy()"));
		}
		ExceptionsEncountered.Clear();
	}

	protected virtual void OnRetry()
	{
	}

	protected virtual TimeSpan? GetNextDelay([JetBrains.Annotations.NotNull] Exception lastException)
	{
		int num = ExceptionsEncountered.Count - 1;
		if (num < MaxRetryCount)
		{
			double num2 = (Math.Pow(2.0, num) - 1.0) * (1.0 + Random.NextDouble() * 0.10000000000000009);
			TimeSpan defaultCoefficient = _defaultCoefficient;
			return TimeSpan.FromMilliseconds(Math.Min(defaultCoefficient.TotalMilliseconds * num2, MaxRetryDelay.TotalMilliseconds));
		}
		return null;
	}

	protected internal virtual bool ShouldVerifySuccessOn([JetBrains.Annotations.NotNull] Exception exception)
	{
		return ShouldRetryOn(exception);
	}

	protected internal abstract bool ShouldRetryOn([JetBrains.Annotations.NotNull] Exception exception);

	public static TResult CallOnWrappedException<TResult>([JetBrains.Annotations.NotNull] Exception exception, [JetBrains.Annotations.NotNull] Func<Exception, TResult> exceptionHandler)
	{
		if (!(exception is DbUpdateException ex))
		{
			return exceptionHandler(exception);
		}
		return CallOnWrappedException(ex.InnerException, exceptionHandler);
	}
}
