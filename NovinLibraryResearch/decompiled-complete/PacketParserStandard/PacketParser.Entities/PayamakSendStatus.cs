namespace PacketParser.Entities;

public enum PayamakSendStatus : short
{
	[PersianNameAttribute.PersianName("نمایش فرم ارسال")]
	ShowSendForm,
	[PersianNameAttribute.PersianName("ارسال فوری")]
	ImedSend,
	[PersianNameAttribute.PersianName("غیرفعال")]
	Disable
}
