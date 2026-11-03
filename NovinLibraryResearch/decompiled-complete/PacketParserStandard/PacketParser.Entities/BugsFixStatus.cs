namespace PacketParser.Entities;

public enum BugsFixStatus : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("در صف بررسی")]
	InPublishQueue,
	[PersianNameAttribute.PersianName("پذیرفته شده")]
	Accept,
	[PersianNameAttribute.PersianName("رد شده")]
	Failed,
	[PersianNameAttribute.PersianName("حذف شده")]
	Deleted,
	[PersianNameAttribute.PersianName("منقضی شده")]
	Expired,
	[PersianNameAttribute.PersianName("نیاز به اصلاح مجدد")]
	EditNeeded,
	[PersianNameAttribute.PersianName("اصلاح شده")]
	TestOk
}
