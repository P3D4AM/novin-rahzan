using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_Lottery")]
public class Lottery : ILottery, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	bool IHasStatus.Status { get; }

	public Guid TafsilGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public DateTime DateM { get; set; }

	public LotteryStatus Status { get; set; }

	[Required]
	public string Description { get; set; }

	public Cashier Cashier { get; set; }

	public Pardakht Pardakht { get; set; }

	public Tafsil Tafsil { get; set; }
}
