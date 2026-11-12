using System;

namespace mCore;

public class QueueWapPushSendingEventArgs : EventArgs, IQueueWapPushSendingEventArgs
{
	private string a;

	private QueuePriority b;

	private string c;

	public string DestinationNumber => a;

	public QueuePriority Priority => b;

	public string QueueMessageKey => c;

	public QueueWapPushSendingEventArgs(string _DestinationNumber, QueuePriority _Priority, string _QueueMessageKey)
	{
		a = "";
		c = "";
		a = _DestinationNumber;
		b = _Priority;
		c = _QueueMessageKey;
	}
}
