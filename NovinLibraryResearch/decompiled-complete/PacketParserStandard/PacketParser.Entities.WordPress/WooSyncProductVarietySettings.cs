using System.ComponentModel;

namespace PacketParser.Entities.WordPress;

public class WooSyncProductVarietySettings : WooSyncBaseProductSettings
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("سینک تنوع های کالایی فعال باشد")]
	public bool SyncAll { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف تنوع کالا در وردپرس")]
	public bool Add { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش تنوع کالا")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف تنوع کالا")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر نام")]
	public bool ChangeName { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت انتشار")]
	public bool ChangePublishStatus { get; set; } = true;


	public WooSyncProductVarietySettings Clone()
	{
		return (WooSyncProductVarietySettings)MemberwiseClone();
	}
}
