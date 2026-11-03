using System;

namespace PacketParser.Entities.Interfaces.Accounting.Hotel;

public interface IPlacementFacilitiesRelation
{
	Guid FacilitiesGuid { get; set; }

	Guid PlacementGuid { get; set; }
}
