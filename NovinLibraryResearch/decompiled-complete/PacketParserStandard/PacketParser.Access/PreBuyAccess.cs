using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class PreBuyAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور درخواست خرید")]
	public bool PreBuy_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف درخواست خرید (کلیه کاربران) ")]
	public bool PreBuy_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش درخواست خرید (کلیه کاربران) ")]
	public bool PreBuy_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش درخواست خرید (فقط خودش) ")]
	public bool PreBuy_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف درخواست خرید (فقط خودش) ")]
	public bool PreBuy_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ درخواست خرید (کلیه کاربران) ")]
	public bool PreBuy_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ درخواست خرید (فقط خودش) ")]
	public bool PreBuy_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری درخواست های خرید (کلیه کاربران) ")]
	public bool PreBuy_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری درخواست های خرید (فقط خودش) ")]
	public bool PreBuy_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری از درخواست های خرید روز های قبل")]
	public bool PreBuy_ReportPreviuseDay { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل درخواست خرید به فاکتور خرید")]
	public bool PreBuy_Tabdil { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام فاکتور")]
	public bool PreBuy_EditPrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف فاکتور")]
	public bool PreBuy_EditDiscount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف اقلام فاکتور")]
	public bool PreBuy_EditAghlamDiscount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد مالیات فاکتور")]
	public bool PreBuy_EditTax { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد عوارض فاکتور")]
	public bool PreBuy_EditAvarez { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت درخواست خرید")]
	public bool PreBuy_ChangeStatusSanad { get; set; } = true;


	public List<OrderPreBuySanadStatusAccess> SanadStatusAccess { get; set; } = new List<OrderPreBuySanadStatusAccess>();


	public OrderPreBuyDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new OrderPreBuyDefaultSanadStatusAccess();

}
