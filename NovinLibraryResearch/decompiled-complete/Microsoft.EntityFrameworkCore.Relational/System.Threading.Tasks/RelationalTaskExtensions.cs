namespace System.Threading.Tasks;

internal static class RelationalTaskExtensions
{
	public static Task<TDerived> Cast<T, TDerived>(this Task<T> task) where TDerived : T
	{
		TaskCompletionSource<TDerived> taskCompletionSource = new TaskCompletionSource<TDerived>();
		task.ContinueWith(delegate(Task<T> t)
		{
			if (t.IsFaulted)
			{
				taskCompletionSource.TrySetException(t.Exception.InnerExceptions);
			}
			else if (t.IsCanceled)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				taskCompletionSource.TrySetResult((TDerived)(object)t.Result);
			}
		}, TaskContinuationOptions.ExecuteSynchronously);
		return taskCompletionSource.Task;
	}

	public static Task<T?> CastToNullable<T>(this Task<T> task) where T : struct
	{
		TaskCompletionSource<T?> taskCompletionSource = new TaskCompletionSource<T?>();
		task.ContinueWith(delegate(Task<T> t)
		{
			if (t.IsFaulted)
			{
				taskCompletionSource.TrySetException(t.Exception.InnerExceptions);
			}
			else if (t.IsCanceled)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				taskCompletionSource.TrySetResult(t.Result);
			}
		}, TaskContinuationOptions.ExecuteSynchronously);
		return taskCompletionSource.Task;
	}
}
