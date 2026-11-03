using System;

namespace PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;

public interface IOpenInvoiceOrderRelation
{
	Guid InvoiceGuid { get; set; }

	Guid OrderGuid { get; set; }

	short Ordinal { get; set; }
}
