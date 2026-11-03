using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebFareOrderCredit : IFareOrderCredit, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public bool Status { get; set; }

	public int ViewOrder { get; set; }

	public string TafsilName { get; set; }

	public long TafsilCode { get; set; }

	public string MoeinName { get; set; }

	public DateTime Modified { get; set; }
}
