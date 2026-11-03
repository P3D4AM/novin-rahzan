namespace PacketParser.Entities;

public enum TypeAction : short
{
	[PersianNameAttribute.PersianName("لاگ عملکرد")]
	none,
	[PersianNameAttribute.PersianName("حذف")]
	Delete,
	[PersianNameAttribute.PersianName("فزودن")]
	Add,
	[PersianNameAttribute.PersianName("ویرایش")]
	Edit,
	[PersianNameAttribute.PersianName("ساده")]
	acc_Sade,
	[PersianNameAttribute.PersianName("متوسط")]
	Acc_Motavaset,
	[PersianNameAttribute.PersianName("پیشرفته")]
	Acc_Pishrafteh,
	[PersianNameAttribute.PersianName("جامع")]
	Acc_Jame,
	[PersianNameAttribute.PersianName("ترازو")]
	Scale,
	[PersianNameAttribute.PersianName("تولید")]
	Tolid,
	[PersianNameAttribute.PersianName("بارکد")]
	Barcode,
	[PersianNameAttribute.PersianName("کالر آی دی")]
	CallerID,
	[PersianNameAttribute.PersianName("تست")]
	Test,
	[PersianNameAttribute.PersianName("کلمه عبور")]
	Password,
	[PersianNameAttribute.PersianName("سطح 2")]
	Hozor_Level2,
	[PersianNameAttribute.PersianName("سطح 1")]
	Hozor_Level1,
	[PersianNameAttribute.PersianName("سطح 3")]
	Hozor_Level3,
	[PersianNameAttribute.PersianName("فعالسازی")]
	Activation,
	[PersianNameAttribute.PersianName("گزارش")]
	Report,
	[PersianNameAttribute.PersianName("سطح 4")]
	Hozor_Level4,
	[PersianNameAttribute.PersianName("سطح 5")]
	Hozor_Level5
}
