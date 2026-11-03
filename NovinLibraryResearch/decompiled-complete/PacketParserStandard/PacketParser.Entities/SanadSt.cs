namespace PacketParser.Entities;

public enum SanadSt : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("موقت")]
	Movaghat,
	[PersianNameAttribute.PersianName("دائم")]
	Daemi
}
