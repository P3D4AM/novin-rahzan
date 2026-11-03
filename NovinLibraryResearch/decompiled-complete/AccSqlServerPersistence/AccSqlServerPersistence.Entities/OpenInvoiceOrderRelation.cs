using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_OpenInvoiceOrderRelation")]
public class OpenInvoiceOrderRelation : IOpenInvoiceOrderRelation
{
	public Guid InvoiceGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public short Ordinal { get; set; }

	public OpenInvoice MasterItem { get; set; }

	public Order OrderItem { get; set; }
}
