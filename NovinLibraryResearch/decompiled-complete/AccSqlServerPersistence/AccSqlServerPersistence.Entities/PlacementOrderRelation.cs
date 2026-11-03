using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_PlacementOrderRelation")]
public class PlacementOrderRelation : IPlacementOrderRelation
{
	public Guid PlacementGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public Placement PlacementItem { get; set; }

	public Order OrderItem { get; set; }
}
