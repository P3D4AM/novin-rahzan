using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Sinks.Async;

internal sealed class BackgroundWorkerSink : ILogEventSink, IAsyncLogEventSinkInspector, IDisposable
{
	private readonly ILogEventSink _wrappedSink;

	private readonly bool _blockWhenFull;

	private readonly BlockingCollection<LogEvent> _queue;

	private readonly Task _worker;

	private readonly IAsyncLogEventSinkMonitor _monitor;

	private long _droppedMessages;

	int IAsyncLogEventSinkInspector.BufferSize => _queue.BoundedCapacity;

	int IAsyncLogEventSinkInspector.Count => _queue.Count;

	long IAsyncLogEventSinkInspector.DroppedMessagesCount => _droppedMessages;

	public BackgroundWorkerSink(ILogEventSink wrappedSink, int bufferCapacity, bool blockWhenFull, IAsyncLogEventSinkMonitor monitor = null)
	{
		if (bufferCapacity <= 0)
		{
			throw new ArgumentOutOfRangeException("bufferCapacity");
		}
		_wrappedSink = wrappedSink ?? throw new ArgumentNullException("wrappedSink");
		_blockWhenFull = blockWhenFull;
		_queue = new BlockingCollection<LogEvent>(bufferCapacity);
		_worker = Task.Factory.StartNew(Pump, CancellationToken.None, TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		_monitor = monitor;
		monitor?.StartMonitoring(this);
	}

	public void Emit(LogEvent logEvent)
	{
		if (_queue.IsAddingCompleted)
		{
			return;
		}
		try
		{
			if (_blockWhenFull)
			{
				_queue.Add(logEvent);
			}
			else if (!_queue.TryAdd(logEvent))
			{
				Interlocked.Increment(ref _droppedMessages);
				SelfLog.WriteLine("{0} unable to enqueue, capacity {1}", typeof(BackgroundWorkerSink), _queue.BoundedCapacity);
			}
		}
		catch (InvalidOperationException)
		{
		}
	}

	public void Dispose()
	{
		_queue.CompleteAdding();
		_worker.Wait();
		(_wrappedSink as IDisposable)?.Dispose();
		_monitor?.StopMonitoring(this);
	}

	private void Pump()
	{
		try
		{
			foreach (LogEvent item in _queue.GetConsumingEnumerable())
			{
				try
				{
					_wrappedSink.Emit(item);
				}
				catch (Exception arg)
				{
					SelfLog.WriteLine("{0} failed to emit event to wrapped sink: {1}", typeof(BackgroundWorkerSink), arg);
				}
			}
		}
		catch (Exception arg2)
		{
			SelfLog.WriteLine("{0} fatal error in worker thread: {1}", typeof(BackgroundWorkerSink), arg2);
		}
	}
}
