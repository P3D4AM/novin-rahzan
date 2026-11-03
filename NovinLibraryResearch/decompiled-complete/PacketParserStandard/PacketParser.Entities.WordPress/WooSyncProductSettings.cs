using System.ComponentModel;

namespace PacketParser.Entities.WordPress;

public class WooSyncProductSettings : WooSyncBaseProductSettings
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("سینک کالاها فعال باشد")]
	public bool SyncAll { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف کالا")]
	public bool Add { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش کالا")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف کالا")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر نام")]
	public bool ChangeName { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر گروه")]
	public bool ChangeGroup { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر Type")]
	public bool ChangeType { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت انتشار")]
	public bool ChangePublishStatus { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر عنوان سئو")]
	public bool ChangeSeoTitle { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر توضیحات سئو")]
	public bool ChangeSeoDescription { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر کلمه کلیدی اصلی")]
	public bool ChangeSeoFocusKeyword { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تنوع پیش فرض برای کالای متغیر")]
	public bool ChangeDefaultVariation { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ارسال چندواحدی کالا به سایت به عنوان تنوع")]
	public bool SendMultivahedTowpAsVariation { get; set; } = true;


	public WooSyncProductSettings Clone()
	{
		return (WooSyncProductSettings)MemberwiseClone();
	}
}
