using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class ReceptionAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور سند دریافت")]
	public bool Reception_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف سند دریافت (کلیه کاربران) ")]
	public bool Reception_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش سند دریافت (کلیه کاربران) ")]
	public bool Reception_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش سند دریافت (فقط خودش) ")]
	public bool Reception_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف سند دریافت (فقط خودش) ")]
	public bool Reception_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ سند دریافت (کلیه کاربران) ")]
	public bool Reception_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ سند دریافت (فقط خودش) ")]
	public bool Reception_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری اسناد دریافت (کلیه کاربران) ")]
	public bool Reception_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری اسناد دریافت (فقط خودش) ")]
	public bool Reception_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری از اسناد دریافت روز های قبل")]
	public bool Reception_ReportPreviuseDay { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش سندهای دریافت روزهای قبل")]
	public bool Reception_Update_NODate { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند دریافت")]
	public bool Reception_ChangeStatusSanad { get; set; } = true;


	public List<ReceptionSanadStatusAccess> SanadStatusAccess { get; set; } = new List<ReceptionSanadStatusAccess>();


	public ReceptionDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new ReceptionDefaultSanadStatusAccess();

}
