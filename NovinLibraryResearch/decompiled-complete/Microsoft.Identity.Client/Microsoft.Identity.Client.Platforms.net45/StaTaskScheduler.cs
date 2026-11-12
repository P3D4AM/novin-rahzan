using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client.Platforms.net45;

internal sealed class StaTaskScheduler : TaskScheduler, IDisposable
{
	private readonly List<Thread> _threads;

	private BlockingCollection<Task> _tasks;

	public override int MaximumConcurrencyLevel => _threads.Count;

	public StaTaskScheduler(int numberOfThreads)
	{
		if (numberOfThreads < 1)
		{
			throw new ArgumentOutOfRangeException("numberOfThreads");
		}
		_tasks = new BlockingCollection<Task>();
		_threads = Enumerable.Range(0, numberOfThreads).Select((Func<int, Thread>)delegate
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				foreach (Task item in _tasks.GetConsumingEnumerable())
				{
					TryExecuteTask(item);
				}
			});
			thread.IsBackground = true;
			thread.SetApartmentState(ApartmentState.STA);
			return thread;
		}).ToList();
		_threads.ForEach(delegate(Thread t)
		{
			t.Start();
		});
	}

	public void Dispose()
	{
		if (_tasks == null)
		{
			return;
		}
		_tasks.CompleteAdding();
		foreach (Thread thread in _threads)
		{
			thread.Join();
		}
		_tasks.Dispose();
		_tasks = null;
	}

	protected override void QueueTask(Task task)
	{
		_tasks.Add(task);
	}

	protected override IEnumerable<Task> GetScheduledTasks()
	{
		return _tasks.ToArray();
	}

	protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
		{
			return TryExecuteTask(task);
		}
		return false;
	}
}
