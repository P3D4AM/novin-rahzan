using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;

namespace EntityCache.Business.Accounting.Hotel;

public class PlacementFacilitiesRelationBusiness : IPlacementFacilitiesRelation, IHasContext
{
	public Guid PlacementGuid { get; set; }

	public Guid FacilitiesGuid { get; set; }

	public string ContextGuid { get; set; }

	public PlacementFacilitiesRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
