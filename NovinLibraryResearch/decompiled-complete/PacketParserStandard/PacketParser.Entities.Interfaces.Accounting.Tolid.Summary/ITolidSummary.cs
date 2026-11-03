using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid.Summary;

public interface ITolidSummary : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int Id2 { get; set; }

	string Description { get; set; }

	Guid CashierGuid { get; set; }

	DateTime DateM { get; set; }
}
