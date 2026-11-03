using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.LkService;

public class PackageContainer
{
	public EnLK_SpecialFacilities Facility { get; set; }

	[JsonIgnore]
	public string FacilityForDisplay => Facility.GetDisplay();

	public string ActivationCode { get; set; }

	public bool IsActive { get; set; }

	public PackageContainer(EnLK_SpecialFacilities facilities, bool isActive = false, string activationCode = "")
	{
		Facility = facilities;
		IsActive = isActive;
		ActivationCode = activationCode;
	}
}
