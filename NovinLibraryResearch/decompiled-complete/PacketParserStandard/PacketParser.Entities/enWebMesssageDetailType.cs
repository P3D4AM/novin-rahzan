namespace PacketParser.Entities;

public enum enWebMesssageDetailType : short
{
	[PersianNameAttribute.PersianName("پیام")]
	Message,
	[PersianNameAttribute.PersianName("فایل")]
	File,
	[PersianNameAttribute.PersianName("آدرس اینترنتی")]
	Url,
	[PersianNameAttribute.PersianName("نامشخص")]
	None
}
