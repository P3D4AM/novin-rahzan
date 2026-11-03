namespace PacketParser.Entities;

public enum SanadMode : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None = 0,
	[PersianNameAttribute.PersianName("اتومات")]
	Automatic = 1,
	[PersianNameAttribute.PersianName("عادی")]
	Manual = 2,
	[PersianNameAttribute.PersianName("مرکب")]
	Mixed = 2
}
