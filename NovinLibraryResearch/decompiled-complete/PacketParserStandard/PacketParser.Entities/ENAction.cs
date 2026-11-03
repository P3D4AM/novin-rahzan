namespace PacketParser.Entities;

public enum ENAction : short
{
	[PersianNameAttribute.PersianName("نا مشخص")]
	None,
	[PersianNameAttribute.PersianName("اضافه")]
	Add,
	[PersianNameAttribute.PersianName("ویرایش")]
	Edit,
	[PersianNameAttribute.PersianName("حذف")]
	Delete,
	[PersianNameAttribute.PersianName("گزارش")]
	Report,
	[PersianNameAttribute.PersianName("ورود")]
	Login,
	[PersianNameAttribute.PersianName("خروج")]
	Logout,
	[PersianNameAttribute.PersianName("مشاهده")]
	See,
	[PersianNameAttribute.PersianName("ذخیره")]
	Save,
	[PersianNameAttribute.PersianName("مشاهده")]
	Cancel,
	[PersianNameAttribute.PersianName("مشاهده")]
	Print,
	[PersianNameAttribute.PersianName("خطا در ثبت داده")]
	Error
}
