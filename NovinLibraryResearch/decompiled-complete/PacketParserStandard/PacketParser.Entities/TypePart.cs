namespace PacketParser.Entities;

public enum TypePart : short
{
	[PersianNameAttribute.PersianName("هیچکدام")]
	none,
	[PersianNameAttribute.PersianName("کدفعاسازی")]
	ActivationCode,
	[PersianNameAttribute.PersianName("ارسال مجدد اطلاعات به سرور")]
	ResendData2Server,
	[PersianNameAttribute.PersianName("اشخاص")]
	Customer,
	[PersianNameAttribute.PersianName("هیچکدام")]
	EnterActivationApp,
	[PersianNameAttribute.PersianName("صفحه تعریف قفل")]
	LK,
	[PersianNameAttribute.PersianName("دپارتمان")]
	Department,
	[PersianNameAttribute.PersianName("لیست قفل های منقضی شده")]
	ExpireList,
	[PersianNameAttribute.PersianName("لاگ عملکرد کاربران")]
	Log_User_Activation,
	[PersianNameAttribute.PersianName("کاربران اکتیویشن")]
	User_Activaion,
	[PersianNameAttribute.PersianName("صفحه اصلی اکتیویشن")]
	ActivationMain,
	[PersianNameAttribute.PersianName("SMS")]
	SMS
}
