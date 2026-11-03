using System;

namespace PacketParser.Entities.Interfaces;

public interface IOrderTemp
{
	Guid DepotGuid { get; set; }

	Guid CustomerGuid { get; set; }

	WebOrder TempOrder { get; set; }
}
