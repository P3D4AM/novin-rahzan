namespace PacketParser.Entities;

public enum StatusLock
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("دارای اعتبار")]
	Normal,
	[PersianNameAttribute.PersianName("مسدود")]
	Blocked
}
