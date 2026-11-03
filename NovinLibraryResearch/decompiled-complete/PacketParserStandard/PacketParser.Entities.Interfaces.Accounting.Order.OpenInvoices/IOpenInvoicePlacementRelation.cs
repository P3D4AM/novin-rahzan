using System;

namespace PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;

public interface IOpenInvoicePlacementRelation
{
	Guid InvoiceGuid { get; set; }

	Guid PlacementGuid { get; set; }

	short Ordinal { get; set; }
}
