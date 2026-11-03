using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class BackBuyAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور فاکتور برگشت از خرید")]
	public bool BackBuy_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور برگشت از خرید(کلیه کاربران)")]
	public bool BackBuy_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور برگشت از خرید(کلیه کاربران)")]
	public bool BackBuy_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور برگشت از خرید(فقط خودش)")]
	public bool BackBuy_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور برگشت از خرید(فقط خودش)")]
	public bool BackBuy_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور برگشت از خرید(کلیه کاربران)")]
	public bool BackBuy_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور برگشت از خرید(فقط خودش)")]
	public bool BackBuy_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتورهای برگشت از خرید(کلیه کاربران)")]
	public bool BackBuy_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتورهای برگشت از خرید(فقط خودش)")]
	public bool BackBuy_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری از فاکتورهای برگشت از خرید روز های قبل")]
	public bool BackBuy_ReportPreviuseDay { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام فاکتور")]
	public bool BackBuy_EditPrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف فاکتور")]
	public bool BackBuy_EditDiscount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف اقلام فاکتور")]
	public bool BackBuy_EditAghlamDiscount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد مالیات فاکتور")]
	public bool BackBuy_EditTax { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد عوارض فاکتور")]
	public bool BackBuy_EditAvarez { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند فاکتور برگشت از خرید")]
	public bool BackBuy_ChangeStatusSanad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دسترسی به فاکتورهایی که تیک کاردکس دارند")]
	public bool AccessToKardexAffective { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده مبالغ فاکتور")]
	public bool ViewInvoiceAmounts { get; set; } = true;


	public List<OrderBackBuySanadStatusAccess> SanadStatusAccess { get; set; } = new List<OrderBackBuySanadStatusAccess>();


	public OrderBackBuyDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new OrderBackBuyDefaultSanadStatusAccess();

}
