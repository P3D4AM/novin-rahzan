namespace PacketParser.Entities;

public enum ServerStatusForClient : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("ارسال شده")]
	Sent,
	[PersianNameAttribute.PersianName("تحویل شده")]
	Delivered,
	[PersianNameAttribute.PersianName("خطا در ارسال")]
	SendError,
	[PersianNameAttribute.PersianName("خطا در تحویل")]
	DeliveryError,
	[PersianNameAttribute.PersianName("تحویل مستقیم")]
	DirectDelivery,
	[PersianNameAttribute.PersianName("متصل به فاکتور")]
	LinkToOrder,
	[PersianNameAttribute.PersianName("بررسی شده")]
	Seen
}
