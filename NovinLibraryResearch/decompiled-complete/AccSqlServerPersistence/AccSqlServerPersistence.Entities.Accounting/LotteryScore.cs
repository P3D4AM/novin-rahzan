using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_LotteryScores")]
public class LotteryScore : ILotteryScore, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	bool IHasStatus.Status { get; }

	public DateTime ExpirationDate { get; set; }

	public Guid OrderGuid { get; set; }

	public DateTime DateM { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public decimal Score { get; set; }

	[Required]
	public string Description { get; set; }

	public LotteryStatus Status { get; set; }

	public Tafsil Tafsil { get; set; }

	public Cashier Cashier { get; set; }

	public Order Order { get; set; }
}
