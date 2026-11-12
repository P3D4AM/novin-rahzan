namespace mCore;

public interface IQueueVCardSendingEventArgs
{
	string QueueMessageKey { get; }

	string DestinationNumber { get; }

	QueuePriority Priority { get; }
}
