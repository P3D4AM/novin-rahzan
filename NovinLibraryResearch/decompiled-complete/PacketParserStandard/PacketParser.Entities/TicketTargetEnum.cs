namespace PacketParser.Entities;

public enum TicketTargetEnum : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None = 0,
	[PersianNameAttribute.PersianName("کاربر")]
	Cashier = 1,
	[PersianNameAttribute.PersianName("مشتری")]
	Tafsil = 3
}
