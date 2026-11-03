namespace PacketParser.Entities;

public enum EnConversation : short
{
	[PersianNameAttribute.PersianName("None")]
	None,
	[PersianNameAttribute.PersianName("بدون پاسخ")]
	MissedCall,
	[PersianNameAttribute.PersianName("تماس ورودی در حال مکالمه")]
	IncomingCallAnswered,
	[PersianNameAttribute.PersianName("تماس ورودی پایان یافته")]
	IncomingCallCompleted,
	[PersianNameAttribute.PersianName("تماس ورودی در حال زنگ خوردن")]
	Ringing,
	[PersianNameAttribute.PersianName("تماس خروجی درحال مکالمه")]
	OutCominigCallAnswered,
	[PersianNameAttribute.PersianName("تماس خروجی پایان یافته")]
	OutCominigCallCompleted
}
