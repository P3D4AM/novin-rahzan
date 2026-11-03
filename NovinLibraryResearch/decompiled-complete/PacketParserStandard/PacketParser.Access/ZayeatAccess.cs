using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class ZayeatAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور فاکتور ضایعات")]
	public bool Zayeat_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور ضایعات (کلیه کاربران) ")]
	public bool Zayeat_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور ضایعات (کلیه کاربران) ")]
	public bool Zayeat_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فاکتور ضایعات (فقط خودش) ")]
	public bool Zayeat_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فاکتور ضایعات (فقط خودش) ")]
	public bool Zayeat_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور ضایعات (کلیه کاربران) ")]
	public bool Zayeat_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ فاکتور ضایعات (فقط خودش) ")]
	public bool Zayeat_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتور های ضایعات  (کلیه کاربران) ")]
	public bool Zayeat_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فاکتور های ضایعات  (فقط خودش) ")]
	public bool Zayeat_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری از فاکتور های ضایعات روز های قبل")]
	public bool Zayeat_ReportPreviuseDay { get; set; } = true;


	[DefaultValue(false)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام ضایعات ")]
	public bool Zayeat_EditPrice { get; set; }

	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند فاکتور ضایعات")]
	public bool Zayeat_ChangeStatusSanad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دسترسی به فاکتورهایی که تیک کاردکس دارند")]
	public bool AccessToKardexAffective { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده مبالغ فاکتور")]
	public bool ViewInvoiceAmounts { get; set; } = true;


	public List<OrderZayeatSanadStatusAccess> SanadStatusAccess { get; set; } = new List<OrderZayeatSanadStatusAccess>();


	public OrderZayeatDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new OrderZayeatDefaultSanadStatusAccess();

}
