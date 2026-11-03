using System;

namespace PacketParser.Entities;

public interface IBazaryabCust
{
	Guid BazaryabGuid { get; set; }

	Guid CustGuid { get; set; }

	string Description { get; set; }

	Guid Guid { get; set; }

	Guid MantagheGuid { get; set; }
}
