namespace mCore;

public interface IQueueWapPushSendingEventArgs
{
	string QueueMessageKey { get; }

	string DestinationNumber { get; }

	QueuePriority Priority { get; }
}
