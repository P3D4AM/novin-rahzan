using System.ComponentModel;

namespace PacketParser.Access;

public class EvaluationAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور تجدید ارزیابی")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف تجدید ارزیابی (کلیه کاربران) ")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش تجدید ارزیابی (کلیه کاربران) ")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش تجدید ارزیابی (فقط خودش) ")]
	public bool Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف تجدید ارزیابی (فقط خودش) ")]
	public bool Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری تجدید ارزیابی (کلیه کاربران) ")]
	public bool Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری تجدید ارزیابی (فقط خودش) ")]
	public bool ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("نمایش لیست تجدید ارزیابی ها")]
	public bool ShowForm { get; set; } = true;

}
