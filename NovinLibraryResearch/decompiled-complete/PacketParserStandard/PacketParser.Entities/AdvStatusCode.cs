namespace PacketParser.Entities;

public enum AdvStatusCode : short
{
	[PersianNameAttribute.PersianName("در صف انتشار")]
	InPublishQueue = 1,
	[PersianNameAttribute.PersianName("منتشر شده")]
	Published = 2,
	[PersianNameAttribute.PersianName("نیاز به اصلاح")]
	EditNeeded = 3,
	[PersianNameAttribute.PersianName("منتظر پرداخت")]
	WaitForPayment = 4,
	[PersianNameAttribute.PersianName("رد شده")]
	Failed = 5,
	[PersianNameAttribute.PersianName("حذف شده")]
	Deleted = 6,
	[PersianNameAttribute.PersianName("منقضی شده")]
	Expired = 7,
	[PersianNameAttribute.PersianName("خطای درج")]
	InsertError = 8,
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown = -1,
	[PersianNameAttribute.PersianName("منتظر پرداخت")]
	None = 0
}
