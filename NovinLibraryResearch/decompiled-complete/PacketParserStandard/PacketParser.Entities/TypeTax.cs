namespace PacketParser.Entities;

public enum TypeTax : short
{
	[PersianNameAttribute.PersianName("تعیین نشده")]
	None,
	[PersianNameAttribute.PersianName("غیرفعال")]
	Disable,
	[PersianNameAttribute.PersianName("فعال")]
	Enable
}
