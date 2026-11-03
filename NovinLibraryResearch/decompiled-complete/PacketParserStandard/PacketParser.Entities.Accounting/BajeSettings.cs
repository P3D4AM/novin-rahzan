using System.ComponentModel;

namespace PacketParser.Entities.Accounting;

public class BajeSettings
{
	[DefaultValue(BajeLevel.Level1)]
	[PersianNameAttribute.PersianName("سطح باجه")]
	public BajeLevel BajeLevel { get; set; }

	[PersianNameAttribute.PersianName("تولید")]
	public bool Tolid { get; set; }

	[PersianNameAttribute.PersianName("بارکد")]
	public bool Barcode { get; set; }

	public BajeSettings Clone()
	{
		return (BajeSettings)MemberwiseClone();
	}
}
