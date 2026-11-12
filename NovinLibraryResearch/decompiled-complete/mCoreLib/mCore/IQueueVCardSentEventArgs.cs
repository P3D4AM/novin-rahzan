namespace mCore;

public interface IQueueVCardSentEventArgs
{
	string MessageReference { get; }

	string QueueMessageKey { get; }

	string DestinationNumber { get; }

	bool SendResult { get; }

	long ErrorCode { get; }

	string ErrorDescription { get; }
}
