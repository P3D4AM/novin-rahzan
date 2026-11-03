using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Bill;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

[Table("Acc_BillDetail")]
public class BillDetail : IBillDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	[Key]
	public Guid Guid { get; set; }

	public Guid PlacementGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public Guid MasterGuid { get; set; }

	[NotMapped]
	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public BillMaster BillMaster { get; set; }

	public Placement Placement { get; set; }

	public Order Order { get; set; }
}
