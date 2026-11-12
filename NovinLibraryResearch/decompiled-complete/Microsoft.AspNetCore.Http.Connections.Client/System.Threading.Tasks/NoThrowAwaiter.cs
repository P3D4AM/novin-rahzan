using System.Runtime.CompilerServices;

namespace System.Threading.Tasks;

internal readonly struct NoThrowAwaiter : ICriticalNotifyCompletion, INotifyCompletion
{
	private readonly Task _task;

	public bool IsCompleted => _task.IsCompleted;

	public NoThrowAwaiter(Task task)
	{
		_task = task;
	}

	public NoThrowAwaiter GetAwaiter()
	{
		return this;
	}

	public void GetResult()
	{
		_ = _task.Exception;
	}

	public void OnCompleted(Action continuation)
	{
		_task.GetAwaiter().OnCompleted(continuation);
	}

	public void UnsafeOnCompleted(Action continuation)
	{
		OnCompleted(continuation);
	}
}
