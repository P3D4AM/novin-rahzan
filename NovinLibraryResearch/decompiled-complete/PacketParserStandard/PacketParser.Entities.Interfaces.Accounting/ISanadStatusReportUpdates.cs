using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ISanadStatusReportUpdates : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid AsGuid { get; set; }

	Document AsType { get; set; }

	int StatusId { get; set; }

	Guid CashierGuid { get; set; }

	DateTime DateM { get; set; }

	string Description { get; set; }

	int Ordinal { get; set; }
}
