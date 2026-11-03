namespace PacketParser.Entities;

public enum enMessageSender : short
{
	[PersianNameAttribute.PersianName("در صف ارسال")]
	Pending,
	[PersianNameAttribute.PersianName("ارسال شده")]
	Sended,
	[PersianNameAttribute.PersianName("ارسال نشده")]
	Faild,
	[PersianNameAttribute.PersianName("وضعیت نامشخص")]
	Unknown
}
