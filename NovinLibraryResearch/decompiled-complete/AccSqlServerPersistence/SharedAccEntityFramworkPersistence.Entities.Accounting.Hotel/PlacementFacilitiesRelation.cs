using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces.Accounting.Hotel;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

[Table("Acc_PlacementFacilitiesRelation")]
public class PlacementFacilitiesRelation : IPlacementFacilitiesRelation
{
	public Guid PlacementGuid { get; set; }

	public Guid FacilitiesGuid { get; set; }

	public Placement Room { get; set; }

	public Facilities Facility { get; set; }
}
