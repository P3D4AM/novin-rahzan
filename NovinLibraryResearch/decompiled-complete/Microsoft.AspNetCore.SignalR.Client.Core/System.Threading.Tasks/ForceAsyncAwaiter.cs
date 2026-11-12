using System.Runtime.CompilerServices;

namespace System.Threading.Tasks;

internal readonly struct ForceAsyncAwaiter : ICriticalNotifyCompletion, INotifyCompletion
{
	private readonly Task _task;

	public bool IsCompleted => false;

	internal ForceAsyncAwaiter(Task task)
	{
		_task = task;
	}

	public ForceAsyncAwaiter GetAwaiter()
	{
		return this;
	}

	public void GetResult()
	{
		_task.GetAwaiter().GetResult();
	}

	public void OnCompleted(Action action)
	{
		_task.ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().OnCompleted(action);
	}

	public void UnsafeOnCompleted(Action action)
	{
		_task.ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().UnsafeOnCompleted(action);
	}
}
internal readonly struct ForceAsyncAwaiter<T> : ICriticalNotifyCompletion, INotifyCompletion
{
	private readonly Task<T> _task;

	public bool IsCompleted => false;

	internal ForceAsyncAwaiter(Task<T> task)
	{
		_task = task;
	}

	public ForceAsyncAwaiter<T> GetAwaiter()
	{
		return this;
	}

	public T GetResult()
	{
		return _task.GetAwaiter().GetResult();
	}

	public void OnCompleted(Action action)
	{
		_task.ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().OnCompleted(action);
	}

	public void UnsafeOnCompleted(Action action)
	{
		_task.ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().UnsafeOnCompleted(action);
	}
}
