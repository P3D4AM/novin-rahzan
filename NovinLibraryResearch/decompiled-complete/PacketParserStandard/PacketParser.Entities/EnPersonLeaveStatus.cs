namespace PacketParser.Entities;

public enum EnPersonLeaveStatus : short
{
	[PersianNameAttribute.PersianName("در انتظار تایید مدیر")]
	Pending,
	[PersianNameAttribute.PersianName("رد شده")]
	Failed,
	[PersianNameAttribute.PersianName("پذیرفته شده")]
	Accepted,
	[PersianNameAttribute.PersianName("حذف شده")]
	Deleted,
	[PersianNameAttribute.PersianName("در انتظار تایید مدیر واحد")]
	PendingForAdsmin,
	[PersianNameAttribute.PersianName("تایید شده توسط مدیر واحد")]
	AcceptedByAdmin,
	[PersianNameAttribute.PersianName("رد شده توسط مدیر واحد")]
	RejectedByAdmin
}
