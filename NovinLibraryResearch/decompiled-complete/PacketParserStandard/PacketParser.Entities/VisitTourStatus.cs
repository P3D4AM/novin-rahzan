namespace PacketParser.Entities;

public enum VisitTourStatus : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown = 0,
	[PersianNameAttribute.PersianName("تور آینده")]
	FutureVisit = 1,
	[PersianNameAttribute.PersianName("شروع ویزیتور")]
	VisitorStarted = 2,
	[PersianNameAttribute.PersianName("پایان ویزیتور")]
	VisitorFinished = 3,
	[PersianNameAttribute.PersianName("بستن مدیریت")]
	AdminClosed = 15
}
