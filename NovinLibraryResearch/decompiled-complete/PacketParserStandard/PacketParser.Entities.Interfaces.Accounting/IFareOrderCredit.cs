using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IFareOrderCredit : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid TafsilGuid { get; set; }

	Guid MoeinGuid { get; set; }

	new bool Status { get; set; }

	int ViewOrder { get; set; }

	string TafsilName { get; set; }

	long TafsilCode { get; set; }

	string MoeinName { get; set; }
}
