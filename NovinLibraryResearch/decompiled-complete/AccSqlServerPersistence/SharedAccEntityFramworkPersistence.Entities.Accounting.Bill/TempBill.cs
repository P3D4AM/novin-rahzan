using System;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities.Interfaces.Accounting.Bill;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

[Table("Acc_TempBill")]
public class TempBill : ITempBill
{
	public Guid PlacementGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public Order Order { get; set; }

	public Placement Placement { get; set; }
}
