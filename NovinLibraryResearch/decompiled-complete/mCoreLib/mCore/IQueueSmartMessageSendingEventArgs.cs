namespace mCore;

public interface IQueueSmartMessageSendingEventArgs
{
	string QueueMessageKey { get; }

	string DestinationNumber { get; }

	QueuePriority Priority { get; }
}
