using System.ComponentModel;

namespace PacketParser.Entities.Accounting;

public class HesabyarSettings
{
	[DefaultValue(HesabyarLevel.Level1)]
	[PersianNameAttribute.PersianName("سطح باجه")]
	public HesabyarLevel BajeLevel { get; set; }

	public HesabyarSettings Clone()
	{
		return (HesabyarSettings)MemberwiseClone();
	}
}
