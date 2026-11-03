using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid.Summary;

public interface ITolidSummaryDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	Guid ProductGuid { get; set; }

	Guid DepotGuid { get; set; }

	Guid? TolidGuid { get; set; }

	decimal RealAmount { get; set; }

	decimal AvailableAmount { get; set; }

	decimal ProducedAmount { get; set; }
}
