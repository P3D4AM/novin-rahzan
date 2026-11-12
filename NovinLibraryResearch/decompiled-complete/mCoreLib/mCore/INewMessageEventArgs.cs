using System;

namespace mCore;

public interface INewMessageEventArgs
{
	string Phone { get; }

	string TextMessage { get; }

	string SMSC { get; }

	DateTime TimeStamp { get; }

	string TimeStampRFC { get; }

	int TimeZone { get; }

	int TotalParts { get; }

	int ReferenceNumber { get; }

	int SequenceNumber { get; }
}
