using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Bill;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

[Table("Acc_BillMaster")]
public class BillMaster : IBillMaster, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public SanadSt SanadSt { get; set; }

	public int SanadNumber { get; set; }

	public DateTime Date { get; set; }

	public decimal Sum { get; set; }

	[NotMapped]
	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public Cashier Cashier { get; set; }

	public Pardakht Pardakht { get; set; }

	public Reception Reception { get; set; }

	public ICollection<BillDetail> BillDetail { get; set; }
}
