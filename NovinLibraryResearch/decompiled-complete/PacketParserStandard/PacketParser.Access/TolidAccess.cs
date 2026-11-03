using System.ComponentModel;

namespace PacketParser.Access;

public class TolidAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور رسید تولید")]
	public bool Tolid_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف رسید تولید (کلیه کاربران) ")]
	public bool Tolid_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف رسید تولید (فقط خودش) ")]
	public bool Tolid_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش رسید تولید (کلیه کاربران) ")]
	public bool Tolid_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش رسید تولید (فقط خودش) ")]
	public bool Tolid_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری رسیدهای تولید (کلیه کاربران) ")]
	public bool Tolid_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری رسیدهای تولید (فقط خودش) ")]
	public bool Tolid_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن فرمول تولید کالا")]
	public bool Tolid_Formula_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف فرمول تولید کالا")]
	public bool Tolid_Formula_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش فرمول تولید کالا")]
	public bool Tolid_Formula_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری فرمول های تولید کالا")]
	public bool Tolid_Formula_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام محصول")]
	public bool EditPriceMahsul { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام مصرفی")]
	public bool EditPriceMavad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند تولید")]
	public bool Tolid_ChangeStatusSanad { get; set; } = true;


	[DefaultValue(false)]
	[PersianNameAttribute.PersianName("تغییر فی واحد")]
	public bool Tolid_ChangeFiVahed { get; set; }

	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ لیست تولید")]
	public bool Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دسترسی به اسناد تولید موثر در کاردکس")]
	public bool KardexAffective { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دسترسی به اسناد تولید دارای سند حسابداری")]
	public bool SanadAffective { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("نمایش مبالغ اسناد تولید")]
	public bool ShowAmounts { get; set; } = true;

}
