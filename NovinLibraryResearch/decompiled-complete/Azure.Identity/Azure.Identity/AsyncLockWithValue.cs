using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Pipeline;

namespace Azure.Identity;

internal sealed class AsyncLockWithValue<T>
{
	public readonly struct Lock : IDisposable
	{
		private readonly AsyncLockWithValue<T> _owner;

		public bool HasValue => _owner == null;

		public T Value { get; }

		public Lock(T value)
		{
			_owner = null;
			Value = value;
		}

		public Lock(AsyncLockWithValue<T> owner)
		{
			_owner = owner;
			Value = default(T);
		}

		public void SetValue(T value)
		{
			_owner.SetValue(value);
		}

		public void Dispose()
		{
			_owner?.Reset();
		}
	}

	private readonly object _syncObj = new object();

	private Queue<TaskCompletionSource<Lock>> _waiters;

	private bool _isLocked;

	private bool _hasValue;

	private T _value;

	public async ValueTask<Lock> GetLockOrValueAsync(bool async, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskCompletionSource<Lock> valueTcs;
		lock (_syncObj)
		{
			if (_hasValue)
			{
				return new Lock(_value);
			}
			if (!_isLocked)
			{
				_isLocked = true;
				return new Lock(this);
			}
			cancellationToken.ThrowIfCancellationRequested();
			if (_waiters == null)
			{
				_waiters = new Queue<TaskCompletionSource<Lock>>();
			}
			valueTcs = new TaskCompletionSource<Lock>(async ? TaskCreationOptions.RunContinuationsAsynchronously : TaskCreationOptions.None);
			_waiters.Enqueue(valueTcs);
		}
		try
		{
			if (async)
			{
				return await valueTcs.Task.AwaitWithCancellation(cancellationToken);
			}
			valueTcs.Task.Wait(cancellationToken);
			return valueTcs.Task.EnsureCompleted();
		}
		catch (OperationCanceledException)
		{
			if (valueTcs.TrySetCanceled())
			{
				throw;
			}
			return valueTcs.Task.Result;
		}
	}

	private void SetValue(T value)
	{
		Queue<TaskCompletionSource<Lock>> waiters;
		lock (_syncObj)
		{
			_value = value;
			_hasValue = true;
			_isLocked = false;
			if (_waiters == null)
			{
				return;
			}
			waiters = _waiters;
			_waiters = null;
		}
		while (waiters.Count > 0)
		{
			waiters.Dequeue().TrySetResult(new Lock(value));
		}
	}

	private void Reset()
	{
		TaskCompletionSource<Lock> taskCompletionSource = UnlockOrGetNextWaiter();
		while (taskCompletionSource != null && !taskCompletionSource.TrySetResult(new Lock(this)))
		{
			taskCompletionSource = UnlockOrGetNextWaiter();
		}
	}

	private TaskCompletionSource<Lock> UnlockOrGetNextWaiter()
	{
		lock (_syncObj)
		{
			if (!_isLocked)
			{
				return null;
			}
			if (_waiters == null)
			{
				_isLocked = false;
				return null;
			}
			while (_waiters.Count > 0)
			{
				TaskCompletionSource<Lock> taskCompletionSource = _waiters.Dequeue();
				if (!taskCompletionSource.Task.IsCompleted)
				{
					return taskCompletionSource;
				}
			}
			_isLocked = false;
			return null;
		}
	}
}
