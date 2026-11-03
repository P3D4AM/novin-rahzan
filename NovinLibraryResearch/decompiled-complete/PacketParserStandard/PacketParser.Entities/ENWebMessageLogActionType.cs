namespace PacketParser.Entities;

public enum ENWebMessageLogActionType : short
{
	[PersianNameAttribute.PersianName("ارسال پیام")]
	SendMessage,
	[PersianNameAttribute.PersianName("بررسی وضعیت")]
	CheckStatus,
	[PersianNameAttribute.PersianName("شروع لاگین")]
	StartLogin,
	[PersianNameAttribute.PersianName("احراز هویت")]
	VerifyLogin,
	[PersianNameAttribute.PersianName("حذف سشن")]
	DeleteSession
}
