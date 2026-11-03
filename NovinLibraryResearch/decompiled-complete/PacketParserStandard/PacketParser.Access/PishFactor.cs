using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class PishFactor
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور پیش فاکتور فروش")]
	public bool PishFactor_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف پیش فاکتور فروش (کلیه کاربران) ")]
	public bool PishFactor_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش پیش فاکتور فروش (کلیه کاربران) ")]
	public bool PishFactor_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش پیش فاکتور فروش (فقط خودش) ")]
	public bool PishFactor_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف پیش فاکتور فروش (فقط خودش) ")]
	public bool PishFactor_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ پیش فاکتور فروش (کلیه کاربران) ")]
	public bool PishFactor_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ پیش فاکتور فروش (فقط خودش) ")]
	public bool PishFactor_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری پیش فاکتورهای فروش (کلیه کاربران) ")]
	public bool PishFactor_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری پیش فاکتورهای فروش (فقط خودش) ")]
	public bool PishFactor_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری از پیش فاکتورهای فروش روز های قبل")]
	public bool PishFactor_ReportPreviuseDay { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل پیش فاکتور به فاکتور فروش")]
	public bool PishFactor_Tabdil { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام فاکتور")]
	public bool PishFactor_EditPrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف فاکتور")]
	public bool PishFactor_EditDiscount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف اقلام فاکتور")]
	public bool PishFactor_EditAghlamDiscount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد مالیات فاکتور")]
	public bool PishFactor_EditTax { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد عوارض فاکتور")]
	public bool PishFactor_EditAvarez { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد پورسانت بازاریاب")]
	public bool EditVisitorPercent { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت پیش فاکتور فروش")]
	public bool PishFactor_ChangeStatusSanad { get; set; } = true;


	public List<OrderPreSellSanadStatusAccess> SanadStatusAccess { get; set; } = new List<OrderPreSellSanadStatusAccess>();


	public OrderPreSellDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new OrderPreSellDefaultSanadStatusAccess();

}
