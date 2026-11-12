using System;
using System.Collections.Generic;

namespace Serilog.Sinks.Http.Private.NonDurable;

public class LogEventQueue
{
	public enum EnqueueResult
	{
		Ok,
		QueueFull
	}

	public enum DequeueResult
	{
		Ok,
		QueueEmpty,
		MaxSizeViolation
	}

	private readonly Queue<string> queue;

	private readonly long? queueLimitBytes;

	private readonly object syncRoot = new object();

	private long queueBytes;

	public LogEventQueue(long? queueLimitBytes = null)
	{
		if (queueLimitBytes < 1)
		{
			throw new ArgumentException("queueLimitBytes must be either null or greater than 0", "queueLimitBytes");
		}
		queue = new Queue<string>();
		this.queueLimitBytes = queueLimitBytes;
		queueBytes = 0L;
	}

	public void Enqueue(string logEvent)
	{
		EnqueueResult enqueueResult = TryEnqueue(logEvent);
		if (enqueueResult != 0)
		{
			throw new Exception($"Enqueue log event failed: {enqueueResult}");
		}
	}

	public EnqueueResult TryEnqueue(string logEvent)
	{
		lock (syncRoot)
		{
			long num = ByteSize.From(logEvent);
			if (queueBytes + num > queueLimitBytes)
			{
				return EnqueueResult.QueueFull;
			}
			queueBytes += num;
			queue.Enqueue(logEvent);
			return EnqueueResult.Ok;
		}
	}

	public DequeueResult TryDequeue(long? logEventMaxSize, out string logEvent)
	{
		lock (syncRoot)
		{
			if (queue.Count == 0)
			{
				logEvent = string.Empty;
				return DequeueResult.QueueEmpty;
			}
			logEvent = queue.Peek();
			long num = ByteSize.From(logEvent);
			if (num > logEventMaxSize)
			{
				logEvent = string.Empty;
				return DequeueResult.MaxSizeViolation;
			}
			queueBytes -= num;
			queue.Dequeue();
			return DequeueResult.Ok;
		}
	}
}
