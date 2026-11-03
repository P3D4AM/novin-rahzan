namespace PacketParser.Entities;

public enum AdvertiseType : short
{
	[PersianNameAttribute.PersianName("دیوار")]
	Divar = 0,
	[PersianNameAttribute.PersianName("شیپور")]
	Sheypoor = 1,
	[PersianNameAttribute.PersianName("نیازکده")]
	NiazKade = 2,
	[PersianNameAttribute.PersianName("چت دیوار")]
	DivarChat = 3,
	[PersianNameAttribute.PersianName("نیازمندیها")]
	NiazmandyHa = 4,
	[PersianNameAttribute.PersianName("همه")]
	All = 100
}
