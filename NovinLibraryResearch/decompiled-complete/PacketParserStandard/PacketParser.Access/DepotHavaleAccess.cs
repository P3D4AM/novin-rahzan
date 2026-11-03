using System.ComponentModel;

namespace PacketParser.Access;

public class DepotHavaleAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور حواله انبار")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف حواله انبار (کلیه کاربران) ")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش حواله انبار (کلیه کاربران) ")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش حواله انبار (فقط خودش) ")]
	public bool Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف حواله انبار (فقط خودش) ")]
	public bool Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ حواله انبار")]
	public bool Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری حواله انبار (کلیه کاربران) ")]
	public bool Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری حواله انبار (فقط خودش) ")]
	public bool ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل حواله انبار به فاکتور فروش")]
	public bool ChangeToOrder { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل حواله انبار به فاکتور برگشت از خرید")]
	public bool ChangeToOrderBackBuy { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل حواله انبار به فاکتور امانی ما نزد دیگران")]
	public bool ChangeToOrderAmaniKhoroj { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("نمایش لیست حواله انبار ها")]
	public bool ShowForm { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ذخیره شدن حواله انبار درصورت عدم تطبیق کالا با فاکتور مرجع")]
	public bool AllowSaveWithoutInvoiceMatch { get; set; } = true;

}
