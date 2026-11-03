using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class AmaniVaredeAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور فاکتور امانی دیگران نزد ما")]
	public bool AmaniVarede_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور امانی دیگران نزد ما (کلیه کاربران)")]
	public bool AmaniVarede_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور امانی دیگران نزد ما (کلیه کاربران)")]
	public bool AmaniVarede_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور امانی دیگران نزد ما (فقط خودش)")]
	public bool AmaniVarede_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور امانی دیگران نزد ما (فقط خودش)")]
	public bool AmaniVarede_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور امانی دیگران نزد ما (کلیه کاربران)")]
	public bool AmaniVarede_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور امانی دیگران نزد ما (فقط خودش)")]
	public bool AmaniVarede_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتورهای امانی دیگران نزد ما (کلیه کاربران)")]
	public bool AmaniVarede_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتورهای امانی دیگران نزد ما (فقط خودش)")]
	public bool AmaniVarede_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده فی کاردکس")]
	public bool AmaniVarede_SeePrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام")]
	public bool EditPrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت فاکتور امانی دیگران نزد ما")]
	public bool AmaniVarede_ChangeStatusSanad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دسترسی به فاکتورهایی که تیک کاردکس دارند")]
	public bool AccessToKardexAffective { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده مبالغ فاکتور")]
	public bool ViewInvoiceAmounts { get; set; } = true;


	public List<OrderAmaniVorodSanadStatusAccess> SanadStatusAccess { get; set; } = new List<OrderAmaniVorodSanadStatusAccess>();


	public OrderAmaniVorodDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new OrderAmaniVorodDefaultSanadStatusAccess();

}
