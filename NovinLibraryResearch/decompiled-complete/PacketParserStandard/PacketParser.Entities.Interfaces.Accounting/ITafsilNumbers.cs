using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ITafsilNumbers
{
	Guid TafsilGuid { get; set; }

	long OriginalNumber { get; set; }

	long CalcedNumber { get; }

	short Prefix { get; }

	string Title { get; set; }

	short Ordinal { get; set; }
}
