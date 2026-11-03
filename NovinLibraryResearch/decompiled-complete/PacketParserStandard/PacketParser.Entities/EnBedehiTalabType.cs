namespace PacketParser.Entities;

public enum EnBedehiTalabType
{
	[PersianNameAttribute.PersianName("سند دریافتنی")]
	CheckM = 1,
	[PersianNameAttribute.PersianName("سند پرداختنی")]
	CheckSh,
	[PersianNameAttribute.PersianName("قسط دریافتنی")]
	AghsatM,
	[PersianNameAttribute.PersianName("قسط پرداختنی")]
	AghsatSh
}
