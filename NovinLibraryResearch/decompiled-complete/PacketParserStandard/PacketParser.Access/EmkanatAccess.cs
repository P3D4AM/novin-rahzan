using System.ComponentModel;

namespace PacketParser.Access;

public class EmkanatAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تنظیمات")]
	public bool Emkanat_Tanzimat { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("پشتیبان گیری از اطلاعات")]
	public bool Emkanat_Backup { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("بازگردانی اطلاعات")]
	public bool Emkanat_Restore { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("بازسازی اطلاعات")]
	public bool Emkanat_Bazsazi { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تنظیم تاریخ و ساعت سیستم")]
	public bool Emkanat_DateTime { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مرتب سازی اسناد بر اساس شماره")]
	public bool Emkanat_SanadSort { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("اجرای کوئری SQL")]
	public bool Emkanat_Query { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("بازیابی اسناد و فاکتورها")]
	public bool Bazyabi { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("بازنگری اطلاعات")]
	public bool DocumentReview { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مدیریت ویژه")]
	public bool SpecialManagement { get; set; } = true;

}
