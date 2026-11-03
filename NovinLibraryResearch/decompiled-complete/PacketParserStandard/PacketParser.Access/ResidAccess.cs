using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class ResidAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور رسید تحویل کالا")]
	public bool Resid_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف رسید تحویل کالا (کلیه کاربران) ")]
	public bool Resid_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش رسید تحویل کالا (کلیه کاربران) ")]
	public bool Resid_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش رسید تحویل کالا (فقط خودش) ")]
	public bool Resid_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف رسید تحویل کالا (فقط خودش) ")]
	public bool Resid_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ رسید تحویل کالا (کلیه کاربران) ")]
	public bool Resid_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ رسید تحویل کالا (فقط خودش) ")]
	public bool Resid_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری از رسید ها (کلیه کاربران) ")]
	public bool Resid_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری از رسید ها (فقط خودش) ")]
	public bool Resid_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("اعلام تحویل به شخص (کلیه کاربران) ")]
	public bool Resid_Tahvil { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("اعلام تحویل به شخص (فقط خودش) ")]
	public bool Resid_Tahvil_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند رسید تحویل")]
	public bool Resid_Tahvil_ChangeStatusSanad { get; set; } = true;


	public List<ResidSanadStatusAccess> SanadStatusAccess { get; set; } = new List<ResidSanadStatusAccess>();


	public ResidDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new ResidDefaultSanadStatusAccess();

}
