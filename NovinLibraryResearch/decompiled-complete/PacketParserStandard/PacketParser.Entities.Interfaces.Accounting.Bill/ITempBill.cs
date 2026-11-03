using System;

namespace PacketParser.Entities.Interfaces.Accounting.Bill;

public interface ITempBill
{
	Guid PlacementGuid { get; set; }

	Guid OrderGuid { get; set; }
}
