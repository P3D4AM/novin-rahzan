using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ILottery : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid TafsilGuid { get; set; }

	Guid CashierGuid { get; set; }

	Guid? PardakhtGuid { get; set; }

	DateTime DateM { get; set; }

	new LotteryStatus Status { get; set; }

	string Description { get; set; }
}
