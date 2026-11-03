using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ILotteryScore : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid OrderGuid { get; set; }

	DateTime DateM { get; set; }

	Guid TafsilGuid { get; set; }

	Guid CashierGuid { get; set; }

	decimal Score { get; set; }

	string Description { get; set; }

	new LotteryStatus Status { get; set; }

	DateTime ExpirationDate { get; set; }
}
