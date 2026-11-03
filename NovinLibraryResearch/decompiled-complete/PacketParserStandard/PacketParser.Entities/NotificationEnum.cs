namespace PacketParser.Entities;

public enum NotificationEnum : short
{
	[PersianNameAttribute.PersianName("بسیار خوب")]
	Ok = 1,
	[PersianNameAttribute.PersianName("لغو")]
	Cancel,
	[PersianNameAttribute.PersianName("اطلاعات")]
	Information,
	[PersianNameAttribute.PersianName("خطا")]
	Error,
	[PersianNameAttribute.PersianName("اخطار")]
	Exclamtion,
	[PersianNameAttribute.PersianName("سوال")]
	question,
	[PersianNameAttribute.PersianName("پایان سینک")]
	EndSync,
	[PersianNameAttribute.PersianName("در حال سینک")]
	Syncing,
	[PersianNameAttribute.PersianName("شروع سینک")]
	StartSync,
	[PersianNameAttribute.PersianName("بافر")]
	Buffer
}
