using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class PardakhtAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور سند پرداخت")]
	public bool Pardakht_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف سند پرداخت (کلیه کاربران) ")]
	public bool Pardakht_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش سند پرداخت (کلیه کاربران) ")]
	public bool Pardakht_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش سند پرداخت (فقط خودش) ")]
	public bool Pardakht_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف سند پرداخت (فقط خودش) ")]
	public bool Pardakht_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ سند پرداخت (کلیه کاربران) ")]
	public bool Pardakht_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ سند پرداخت (فقط خودش) ")]
	public bool Pardakht_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری اسناد پرداخت (کلیه کاربران) ")]
	public bool Pardakht_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری اسناد پرداخت (فقط خودش) ")]
	public bool Pardakht_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری از اسناد پرداخت روز های قبل")]
	public bool Pardakht_ReportPreviuseDay { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند پرداخت")]
	public bool Pardakht_ChangeStatusSanad { get; set; } = true;


	public List<PardakhtSanadStatusAccess> SanadStatusAccess { get; set; } = new List<PardakhtSanadStatusAccess>();


	public PardakhtDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new PardakhtDefaultSanadStatusAccess();

}
