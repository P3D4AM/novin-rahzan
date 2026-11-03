using System;
using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class SellAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور فاکتور فروش")]
	public bool Sell_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور فروش (کلیه کاربران) ")]
	public bool Sell_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور فروش (کلیه کاربران) ")]
	public bool Sell_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور فروش (فقط خودش) ")]
	public bool Sell_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور فروش (فقط خودش) ")]
	public bool Sell_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور فروش (کلیه کاربران) ")]
	public bool Sell_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور فروش (فقط خودش) ")]
	public bool Sell_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتور های فروش (کلیه کاربران) ")]
	public bool Sell_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتور های فروش (فقط خودش) ")]
	public bool Sell_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری از فاکتور های فروش روز های قبل")]
	public bool Sell_ReportPreviuseDay { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام فروش")]
	public bool Sell_EditPrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("محاسبه سود فاکتور فروش")]
	public bool Sell_CalcSood { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده آخرین معاملات کالا (کلیه فاکتورها)")]
	public bool Sell_SeeLastMoamelat { get; set; } = true;


	[Obsolete("", true)]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("معامله کالا در کلیه انبارها (کلیه فاکتورها)")]
	public bool Sell_EditDepote { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور فاکتور فروش فروشگاهی")]
	public bool Sell_Restorani { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تاریخ فاکتور فروش")]
	public bool Sell_EditDate { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر شماره فاکتور فروش")]
	public bool Sell_EditFactorNO { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چک ها و اقساط در راه")]
	public bool Sell_CheckAghsat { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("قسط بندی فاکتور فروش")]
	public bool Sell_GhestBandi { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتورهای فروش روزهای قبل")]
	public bool Sell_Update_NODate { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف فاکتور فروش")]
	public bool Sell_Discount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("کرایه فاکتور فروش")]
	public bool Sell_Keraye { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف اقلام فاکتور")]
	public bool Sell_EditAghlamDiscount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد مالیات فاکتور")]
	public bool Sell_EditTax { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد عوارض فاکتور")]
	public bool Sell_EditAvarez { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر درصد پورسانت بازاریاب")]
	public bool EditVisitorPercent { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن بستانکار کرایه جدید")]
	public bool Sell_AddFareOrderCredit { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف بستانکار کرایه")]
	public bool Sell_RemoveFareOrderCredit { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف نحوه آشنایی")]
	public bool Sell_InsertIntroductionMethod { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش نحوه آشنایی")]
	public bool Sell_InsertIntroductionMethodEdit { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف نحوه آشنایی")]
	public bool Sell_InsertIntroductionMethodDelete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند فاکتور فروش")]
	public bool Sell_ChangeStatusSanad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دسترسی به فاکتورهایی که تیک کاردکس دارند")]
	public bool AccessToKardexAffective { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده مبالغ فاکتور")]
	public bool ViewInvoiceAmounts { get; set; } = true;


	public List<OrderSellSanadStatusAccess> SanadStatusAccess { get; set; } = new List<OrderSellSanadStatusAccess>();


	public OrderSellDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new OrderSellDefaultSanadStatusAccess();

}
