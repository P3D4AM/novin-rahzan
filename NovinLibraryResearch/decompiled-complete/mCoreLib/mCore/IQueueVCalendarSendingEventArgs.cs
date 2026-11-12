namespace mCore;

public interface IQueueVCalendarSendingEventArgs
{
	string QueueMessageKey { get; }

	string DestinationNumber { get; }

	QueuePriority Priority { get; }
}
