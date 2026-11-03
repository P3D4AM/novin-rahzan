namespace PacketParser.Entities;

public class WebTelegramConfig : ITelegramConfig
{
	[PersianNameAttribute.PersianName("کلید ربات تلگرام")]
	public string ApiKey { get; set; }

	[PersianNameAttribute.PersianName("مشخصات ارتباط")]
	public string ContactInfo { get; set; }

	[PersianNameAttribute.PersianName("نوع کاربری")]
	public EnTelegramType contactType { get; set; }
}
