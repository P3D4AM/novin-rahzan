using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid;

public interface ITolidFormulaHazine : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	Guid MoeinGuid { get; set; }

	Guid? TafsilGuid { get; set; }

	decimal Price { get; set; }

	string Description { get; set; }

	HazineType Type { get; set; }
}
