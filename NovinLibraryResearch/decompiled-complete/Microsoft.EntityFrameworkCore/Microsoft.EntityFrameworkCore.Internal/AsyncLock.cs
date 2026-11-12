using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public sealed class AsyncLock
{
	public readonly struct Releaser : IDisposable
	{
		private readonly AsyncLock _toRelease;

		internal Releaser([JetBrains.Annotations.NotNull] AsyncLock toRelease)
		{
			_toRelease = toRelease;
		}

		public void Dispose()
		{
			_toRelease._semaphore.Release();
		}
	}

	private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

	private readonly Releaser _releaser;

	private readonly Task<Releaser> _releaserTask;

	public AsyncLock()
	{
		_releaser = new Releaser(this);
		_releaserTask = Task.FromResult(_releaser);
	}

	public Task<Releaser> LockAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Task task = _semaphore.WaitAsync(cancellationToken);
		if (!task.IsCompleted)
		{
			return task.ContinueWith((Task _, object state) => ((AsyncLock)state)._releaser, this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
		}
		return _releaserTask;
	}

	public Releaser Lock()
	{
		_semaphore.Wait();
		return _releaser;
	}
}
