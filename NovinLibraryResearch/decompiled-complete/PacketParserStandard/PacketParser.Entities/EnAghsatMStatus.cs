namespace PacketParser.Entities;

public enum EnAghsatMStatus
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("دریافت شده")]
	Recieved,
	[PersianNameAttribute.PersianName("دریافت نشده")]
	NotRecieved,
	[PersianNameAttribute.PersianName("باطل شده")]
	Removed
}
