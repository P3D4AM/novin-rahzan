using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class ManualSanadAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور سند حسابداری")]
	public bool ManualSanad_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف سند حسابداری (کلیه کاربران) ")]
	public bool ManualSanad_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش سند حسابداری (کلیه کاربران) ")]
	public bool ManualSanad_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش سند حسابداری (فقط خودش) ")]
	public bool ManualSanad_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف سند حسابداری (فقط خودش) ")]
	public bool ManualSanad_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ سند حسابداری (کلیه کاربران) ")]
	public bool ManualSanad_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ سند حسابداری (فقط خودش) ")]
	public bool ManualSanad_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری اسناد حسابداری (کلیه کاربران) ")]
	public bool ManualSanad_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری اسناد حسابداری (فقط خودش) ")]
	public bool ManualSanad_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل سند دائم به موقت")]
	public bool manualSanad_Permanent2temp { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل سند موقت به دائم")]
	public bool manualSanad_Temp2Permanent { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تجمیع و تغییر شماره اسناد")]
	public bool manualSanad_AggSanad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور سند حسابداری یک سطری")]
	public bool ManualSanad_Insert_Simple { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده عطف سند")]
	public bool ManualSanad_ShowId { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند حسابداری")]
	public bool ManualSanad_ChangeStatusSanad { get; set; } = true;


	public List<ManualSanadSanadStatusAccess> SanadStatusAccess { get; set; } = new List<ManualSanadSanadStatusAccess>();


	public SanadDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new SanadDefaultSanadStatusAccess();

}
