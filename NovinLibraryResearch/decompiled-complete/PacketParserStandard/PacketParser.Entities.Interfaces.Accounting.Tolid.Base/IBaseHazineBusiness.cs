using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid.Base;

public interface IBaseHazineBusiness : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	Guid MoeinGuid { get; set; }

	Guid? TafsilGuid { get; set; }

	decimal Price { get; set; }

	string Description { get; set; }

	HazineType Type { get; set; }

	long MoeinCode { get; }

	string MoeinName { get; }

	long TafsilCode { get; }

	string TafsilName { get; }

	string TypeName { get; }
}
