namespace PacketParser.Entities;

public enum EnumNotification : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None = 0,
	[PersianNameAttribute.PersianName("از دست رفته")]
	MissedCall = 1,
	[PersianNameAttribute.PersianName("مزاحم نشوید")]
	Dnd = 2,
	[PersianNameAttribute.PersianName("زنگ خوردن")]
	IncommingCall = 3,
	[PersianNameAttribute.PersianName("در جریان")]
	ConversationStarted = 4,
	[PersianNameAttribute.PersianName("پایان یافته")]
	ConversationFinished = 5,
	[PersianNameAttribute.PersianName("مشغول")]
	Busy = 6,
	[PersianNameAttribute.PersianName("برقراری ارتباط با مودم")]
	Connected = 8,
	[PersianNameAttribute.PersianName("قطع ارتباط از با مودم")]
	Disconnected = 9,
	[PersianNameAttribute.PersianName("قطع ارتباط خط متصل")]
	LineDisconnect = 10,
	[PersianNameAttribute.PersianName("حذف شده")]
	Deleted = 11
}
