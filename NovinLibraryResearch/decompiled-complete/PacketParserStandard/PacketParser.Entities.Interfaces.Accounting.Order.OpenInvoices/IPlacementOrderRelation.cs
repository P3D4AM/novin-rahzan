using System;

namespace PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;

public interface IPlacementOrderRelation
{
	Guid PlacementGuid { get; set; }

	Guid OrderGuid { get; set; }
}
