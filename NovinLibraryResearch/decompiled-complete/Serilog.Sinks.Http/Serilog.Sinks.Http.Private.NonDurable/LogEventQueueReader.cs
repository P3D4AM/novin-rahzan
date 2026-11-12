using Serilog.Debugging;

namespace Serilog.Sinks.Http.Private.NonDurable;

public static class LogEventQueueReader
{
	public static Batch Read(LogEventQueue queue, int? logEventsInBatchLimit, long? batchSizeLimitBytes)
	{
		Batch batch = new Batch();
		long? num = batchSizeLimitBytes;
		while (true)
		{
			string logEvent;
			switch (queue.TryDequeue(num, out logEvent))
			{
			default:
				continue;
			case LogEventQueue.DequeueResult.Ok:
				batch.LogEvents.Add(logEvent);
				num -= ByteSize.From(logEvent);
				if (batch.LogEvents.Count != logEventsInBatchLimit)
				{
					continue;
				}
				batch.HasReachedLimit = true;
				break;
			case LogEventQueue.DequeueResult.MaxSizeViolation:
				if (batch.LogEvents.Count == 0)
				{
					queue.TryDequeue(long.MaxValue, out string logEvent2);
					SelfLog.WriteLine("Event exceeds the batch size limit of {0} bytes set for this sink and will be dropped; data: {1}", batchSizeLimitBytes, logEvent2);
					continue;
				}
				batch.HasReachedLimit = true;
				break;
			case LogEventQueue.DequeueResult.QueueEmpty:
				break;
			}
			break;
		}
		return batch;
	}
}
