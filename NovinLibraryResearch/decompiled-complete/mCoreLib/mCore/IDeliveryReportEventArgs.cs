using System;

namespace mCore;

public interface IDeliveryReportEventArgs
{
	int MessageReference { get; }

	string Phone { get; }

	bool Status { get; }

	DateTime DeliveryTimeStamp { get; }

	int DeliveryTimeZone { get; }

	DateTime SentTimeStamp { get; }

	int SentTimeZone { get; }
}
