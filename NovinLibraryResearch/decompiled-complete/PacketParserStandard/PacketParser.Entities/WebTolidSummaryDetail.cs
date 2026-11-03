using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Summary;

namespace PacketParser.Entities;

public class WebTolidSummaryDetail : ITolidSummaryDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public Guid ProductGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public Guid? TolidGuid { get; set; }

	public decimal RealAmount { get; set; }

	public decimal AvailableAmount { get; set; }

	public decimal ProducedAmount { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }
}
