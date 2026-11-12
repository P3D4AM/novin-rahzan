using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Data.SqlClient;

internal static class AsyncHelper
{
	internal static Task CreateContinuationTask(Task task, Action onSuccess, SqlInternalConnectionTds connectionToDoom = null, Action<Exception> onFailure = null)
	{
		if (task == null)
		{
			onSuccess();
			return null;
		}
		TaskCompletionSource<object> completion = new TaskCompletionSource<object>();
		ContinueTask(task, completion, delegate
		{
			onSuccess();
			completion.SetResult(null);
		}, connectionToDoom, onFailure);
		return completion.Task;
	}

	internal static Task CreateContinuationTask<T1, T2>(Task task, Action<T1, T2> onSuccess, T1 arg1, T2 arg2, SqlInternalConnectionTds connectionToDoom = null, Action<Exception> onFailure = null)
	{
		return CreateContinuationTask(task, (Action)delegate
		{
			onSuccess(arg1, arg2);
		}, connectionToDoom, onFailure);
	}

	internal static void ContinueTask(Task task, TaskCompletionSource<object> completion, Action onSuccess, SqlInternalConnectionTds connectionToDoom = null, Action<Exception> onFailure = null, Action onCancellation = null, Func<Exception, Exception> exceptionConverter = null, SqlConnection connectionToAbort = null)
	{
		task.ContinueWith(delegate(Task tsk)
		{
			if (tsk.Exception != null)
			{
				Exception ex = tsk.Exception.InnerException;
				if (exceptionConverter != null)
				{
					ex = exceptionConverter(ex);
				}
				try
				{
					if (onFailure != null)
					{
						onFailure(ex);
					}
					return;
				}
				finally
				{
					completion.TrySetException(ex);
				}
			}
			if (tsk.IsCanceled)
			{
				try
				{
					if (onCancellation != null)
					{
						onCancellation();
					}
					return;
				}
				finally
				{
					completion.TrySetCanceled();
				}
			}
			if (connectionToDoom != null || connectionToAbort != null)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					onSuccess();
					return;
				}
				catch (OutOfMemoryException ex2)
				{
					if (connectionToDoom != null)
					{
						connectionToDoom.DoomThisConnection();
					}
					else
					{
						connectionToAbort.Abort(ex2);
					}
					completion.SetException(ex2);
					throw;
				}
				catch (StackOverflowException ex3)
				{
					if (connectionToDoom != null)
					{
						connectionToDoom.DoomThisConnection();
					}
					else
					{
						connectionToAbort.Abort(ex3);
					}
					completion.SetException(ex3);
					throw;
				}
				catch (ThreadAbortException ex4)
				{
					if (connectionToDoom != null)
					{
						connectionToDoom.DoomThisConnection();
					}
					else
					{
						connectionToAbort.Abort(ex4);
					}
					completion.SetException(ex4);
					throw;
				}
				catch (Exception exception)
				{
					completion.SetException(exception);
					return;
				}
			}
			try
			{
				onSuccess();
			}
			catch (Exception exception2)
			{
				completion.SetException(exception2);
			}
		}, TaskScheduler.Default);
	}

	internal static void WaitForCompletion(Task task, int timeout, Action onTimeout = null, bool rethrowExceptions = true)
	{
		try
		{
			task.Wait((timeout > 0) ? (1000 * timeout) : (-1));
		}
		catch (AggregateException ex)
		{
			if (rethrowExceptions)
			{
				ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
			}
		}
		if (!task.IsCompleted)
		{
			task.ContinueWith(delegate(Task t)
			{
				AggregateException exception = t.Exception;
			});
			onTimeout?.Invoke();
		}
	}

	internal static void SetTimeoutException(TaskCompletionSource<object> completion, int timeout, Func<Exception> exc, CancellationToken ctoken)
	{
		if (timeout <= 0)
		{
			return;
		}
		Task.Delay(timeout * 1000, ctoken).ContinueWith(delegate(Task tsk)
		{
			if (!tsk.IsCanceled && !completion.Task.IsCompleted)
			{
				completion.TrySetException(exc());
			}
		});
	}
}
