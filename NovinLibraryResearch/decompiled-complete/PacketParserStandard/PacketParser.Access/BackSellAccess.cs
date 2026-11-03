using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class BackSellAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور فاکتور برگشت از فروش")]
	public bool BackSell_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور برگشت از فروش(کلیه کاربران)")]
	public bool BackSell_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور برگشت از فروش(کلیه کاربران)")]
	public bool BackSell_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور برگشت از فروش(فقط خودش)")]
	public bool BackSell_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور برگشت از فروش(فقط خودش)")]
	public bool BackSell_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور برگشت از فروش(کلیه کاربران)")]
	public bool BackSell_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور برگشت از فروش(فقط خودش)")]
	public bool BackSell_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتورهای برگشت از فروش(کلیه کاربران)")]
	public bool BackSell_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتورهای برگشت از فروش(فقط خودش)")]
	public bool BackSell_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری از فاکتورهای برگشت از فروش روز های قبل")]
	public bool BackSell_ReportPreviuseDay { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام فاکتور")]
	public bool BackSell_EditPrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف فاکتور")]
	public bool BackSell_EditDiscount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف اقلام فاکتور")]
	public bool BackSell_EditAghlamDiscount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد مالیات فاکتور")]
	public bool BackSell_EditTax { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد عوارض فاکتور")]
	public bool BackSell_EditAvarez { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد پورسانت بازاریاب")]
	public bool EditVisitorPercent { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند فاکتور برگشت از خرید")]
	public bool BackSell_ChangeStatusSanad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دسترسی به فاکتورهایی که تیک کاردکس دارند")]
	public bool AccessToKardexAffective { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده مبالغ فاکتور")]
	public bool ViewInvoiceAmounts { get; set; } = true;


	public List<OrderBackSellSanadStatusAccess> SanadStatusAccess { get; set; } = new List<OrderBackSellSanadStatusAccess>();


	public OrderBackSellDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new OrderBackSellDefaultSanadStatusAccess();

}
