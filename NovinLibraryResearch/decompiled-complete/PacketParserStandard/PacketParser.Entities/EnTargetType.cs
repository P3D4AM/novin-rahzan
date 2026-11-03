namespace PacketParser.Entities;

public enum EnTargetType : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("تفصیل")]
	Tafsil,
	[PersianNameAttribute.PersianName("کاربر")]
	Cashier,
	[PersianNameAttribute.PersianName("دپارتمان")]
	Department
}
