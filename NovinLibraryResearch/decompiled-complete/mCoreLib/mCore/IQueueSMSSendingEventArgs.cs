namespace mCore;

public interface IQueueSMSSendingEventArgs
{
	string QueueMessageKey { get; }

	string DestinationNumber { get; }

	QueuePriority Priority { get; }
}
