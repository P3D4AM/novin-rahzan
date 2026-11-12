using System;

namespace mCore;

public interface IMessage
{
	int Index { get; }

	string Phone { get; }

	string Text { get; }

	DateTime TimeStamp { get; }

	string TimeStampRFC { get; }

	int TimeZone { get; }

	string SMSC { get; }

	bool Delete();
}
