using System.Drawing;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;
using PacketParser.LkService;

namespace FormCustom.LockService.Class;

public class PackageContainerForDisplay
{
	public EnLK_SpecialFacilities Facility { get; set; }

	public string ActivationCode { get; set; }

	public string FacilityForDisplay => Facility.GetDisplay().GetTranslate();

	public Image Image
	{
		get
		{
			if (!string.IsNullOrEmpty(ActivationCode))
			{
				return (Image)(object)FormCustom.Properties.Resources.NP_CHEK;
			}
			return (Image)(object)FormCustom.Properties.Resources.NP_close;
		}
	}

	public short Ordinal { get; set; }

	public PackageContainerForDisplay(PackageContainer data)
	{
		Facility = data.Facility;
		ActivationCode = data.ActivationCode;
	}
}
