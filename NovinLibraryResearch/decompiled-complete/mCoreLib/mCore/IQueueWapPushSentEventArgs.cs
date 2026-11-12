namespace mCore;

public interface IQueueWapPushSentEventArgs
{
	string MessageReference { get; }

	string QueueMessageKey { get; }

	string DestinationNumber { get; }

	bool SendResult { get; }

	long ErrorCode { get; }

	string ErrorDescription { get; }
}
