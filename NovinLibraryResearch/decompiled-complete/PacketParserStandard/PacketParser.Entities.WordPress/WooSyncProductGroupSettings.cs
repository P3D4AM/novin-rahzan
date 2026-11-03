using System.ComponentModel;

namespace PacketParser.Entities.WordPress;

public class WooSyncProductGroupSettings
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("سینک گروه های کالا فعال باشد")]
	public bool SyncAll { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف گروه کالا")]
	public bool Add { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف گروه کالا")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش گروه کالا")]
	public bool Edit { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر کد")]
	public bool ChangeCode { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر نام")]
	public bool ChangeName { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر والد")]
	public bool ChangeParent { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تصویر")]
	public bool ChangePicture { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر توضیحات")]
	public bool ChangeDescription { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر SLUG")]
	public bool ChangeSlug { get; set; } = true;


	public WooSyncProductGroupSettings Clone()
	{
		return (WooSyncProductGroupSettings)MemberwiseClone();
	}
}
