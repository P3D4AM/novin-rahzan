using System;

namespace mCore;

public interface IWapPush
{
	string Destination { get; set; }

	string Text { get; set; }

	string URL { get; set; }

	WapPushAction Action { get; set; }

	DateTime Created { get; set; }

	string Expiry { get; set; }

	bool IsLengthOK { get; }

	string Send();

	string SendToQueue();

	string SendToQueue(QueuePriority Priority);
}
