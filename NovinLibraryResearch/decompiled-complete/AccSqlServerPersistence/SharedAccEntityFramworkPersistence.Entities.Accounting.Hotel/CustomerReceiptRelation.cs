using System;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities.Interfaces.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

[Table("Hotel_CustomerReceiptRelation")]
public class CustomerReceiptRelation : ICustomerReceiptRelation
{
	public Guid CustomerGuid { get; set; }

	public Guid ReceiptGuid { get; set; }

	public AccSqlServerPersistence.Entities.Customer Customer { get; set; }

	public Receipt Receipt { get; set; }
}
