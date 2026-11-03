using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class AmaniSadereAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور فاکتور امانی ما نزد دیگران")]
	public bool AmaniSadere_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور امانی ما نزد دیگران (کلیه کاربران)")]
	public bool AmaniSadere_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور امانی ما نزد دیگران (کلیه کاربران)")]
	public bool AmaniSadere_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور امانی ما نزد دیگران (فقط خودش)")]
	public bool AmaniSadere_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور امانی ما نزد دیگران (فقط خودش)")]
	public bool AmaniSadere_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور امانی ما نزد دیگران (کلیه کاربران)")]
	public bool AmaniSadere_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور امانی ما نزد دیگران (فقط خودش)")]
	public bool AmaniSadere_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتورهای امانی ما نزد دیگران (کلیه کاربران)")]
	public bool AmaniSadere_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتورهای امانی ما نزد دیگران (فقط خودش)")]
	public bool AmaniSadere_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده فی کاردکس ")]
	public bool AmaniSadere_SeePrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام")]
	public bool EditPrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند فاکتور امانی ما نزد دیگران")]
	public bool AmaniSadere_ChangeStatusSanad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دسترسی به فاکتورهایی که تیک کاردکس دارند")]
	public bool AccessToKardexAffective { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده مبالغ فاکتور")]
	public bool ViewInvoiceAmounts { get; set; } = true;


	public List<OrderAmaniKhorojSanadStatusAccess> SanadStatusAccess { get; set; } = new List<OrderAmaniKhorojSanadStatusAccess>();


	public OrderAmaniKhorojDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new OrderAmaniKhorojDefaultSanadStatusAccess();

}
