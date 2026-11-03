using System.ComponentModel;

namespace PacketParser.Entities.WordPress;

public class WooSyncProductPropertySettings
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("سینک ویژگی های کالا فعال باشد")]
	public bool SyncAll { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن ویژگی کالا")]
	public bool Add { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش ویژگی")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف ویژگی")]
	public bool Delete { get; set; } = true;


	public WooSyncProductPropertySettings Clone()
	{
		return (WooSyncProductPropertySettings)MemberwiseClone();
	}
}
