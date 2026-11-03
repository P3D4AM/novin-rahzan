using System.ComponentModel;

namespace PacketParser.Entities.WordPress;

public class WooSyncCustomerSettings
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("سینک اشخاص فعال باشد")]
	public bool SyncAll { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف شخص در وردپرس")]
	public bool Add { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر کد")]
	public bool ChangeCode { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر نام")]
	public bool ChangeName { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر نام خانوادگی")]
	public bool ChangeFamily { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تصویر چهره")]
	public bool ChangePicFace { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تصویر امضا")]
	public bool ChangePicSign { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر توضیحات")]
	public bool ChangeDescription { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر نام کاربری")]
	public bool ChangeUsername { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("نام رمز عبور")]
	public bool ChangePassword { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر ایمیل")]
	public bool ChangeEmail { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر نقش کاربری")]
	public bool ChangeRole { get; set; } = true;


	public WooSyncCustomerSettings Clone()
	{
		return (WooSyncCustomerSettings)MemberwiseClone();
	}
}
