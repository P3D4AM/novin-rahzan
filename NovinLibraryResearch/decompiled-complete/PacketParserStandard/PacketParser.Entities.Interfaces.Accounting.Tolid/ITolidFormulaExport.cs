using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid;

public interface ITolidFormulaExport : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	Guid ProductGuid { get; set; }

	Guid DepotGuid { get; set; }

	decimal Quantity { get; set; }

	Guid? V2Guid { get; set; }

	string Description { get; set; }
}
