using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid;

public interface ITolidExport : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, ITolidFormulaRelation
{
	Guid ProductGuid { get; set; }

	Guid DepotGuid { get; set; }

	decimal Quantity { get; set; }

	Guid? V2Guid { get; set; }

	string Description { get; set; }

	decimal Price { get; set; }

	string Serial { get; set; }

	decimal? V2Qt { get; set; }

	decimal? V2Price { get; set; }
}
