using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_OpenInvoicePlacementRelation")]
public class OpenInvoicePlacementRelation : IOpenInvoicePlacementRelation
{
	public Guid InvoiceGuid { get; set; }

	public Guid PlacementGuid { get; set; }

	public short Ordinal { get; set; }

	public OpenInvoice MasterItem { get; set; }

	public Placement PlacementItem { get; set; }
}
