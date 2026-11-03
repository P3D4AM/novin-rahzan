using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid;

public interface ITolidMavad : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, ITolidFormulaRelation
{
	Guid ProductGuid { get; set; }

	Guid DepotGuid { get; set; }

	decimal Quantity { get; set; }

	string Serial { get; set; }

	string Description { get; set; }

	Guid? V2Guid { get; set; }

	decimal Price { get; }

	decimal? V2Qt { get; set; }

	decimal? V2Price { get; set; }

	decimal? RowSum { get; set; }
}
