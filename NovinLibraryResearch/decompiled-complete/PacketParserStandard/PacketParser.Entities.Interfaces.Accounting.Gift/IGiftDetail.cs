using System;

namespace PacketParser.Entities.Interfaces.Accounting.Gift;

public interface IGiftDetail
{
	Guid ProductGuid { get; set; }

	decimal Count { get; set; }
}
