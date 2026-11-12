namespace mCore;

public interface IQueueSmartMessageSentEventArgs
{
	string QueueMessageKey { get; }

	string DestinationNumber { get; }

	bool SendResult { get; }

	string MessageReference { get; }

	long ErrorCode { get; }

	string ErrorDescription { get; }
}
