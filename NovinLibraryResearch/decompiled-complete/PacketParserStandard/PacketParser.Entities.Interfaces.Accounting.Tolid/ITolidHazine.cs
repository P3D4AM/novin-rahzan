using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid;

public interface ITolidHazine : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, ITolidFormulaRelation
{
	Guid MoeinGuid { get; set; }

	Guid? TafsilGuid { get; set; }

	decimal Price { get; set; }

	string Description { get; set; }

	HazineType Type { get; set; }
}
