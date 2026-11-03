using System.ComponentModel;

namespace PacketParser.Access;

public class DepotReceiptAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور رسید انبار")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف رسید انبار (کلیه کاربران) ")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش رسید انبار (کلیه کاربران) ")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش رسید انبار (فقط خودش) ")]
	public bool Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف رسید انبار (فقط خودش) ")]
	public bool Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ رسید انبار")]
	public bool Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری رسید انبار (کلیه کاربران) ")]
	public bool Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری رسید انبار (فقط خودش) ")]
	public bool ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل رسید انبار به فاکتور خرید")]
	public bool ChangeToOrder { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل رسید انبار به فاکتور برگشت از فروش")]
	public bool ChangeToOrderBackSell { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل رسید انبار به فاکتور امانی دیگران نزد ما")]
	public bool ChangeToOrderAmaniVorod { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("نمایش لیست رسید انبار ها")]
	public bool ShowForm { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ذخیره شدن رسید انبار درصورت عدم تطبیق کالا با فاکتور مرجع")]
	public bool AllowSaveWithoutInvoiceMatch { get; set; } = true;

}
