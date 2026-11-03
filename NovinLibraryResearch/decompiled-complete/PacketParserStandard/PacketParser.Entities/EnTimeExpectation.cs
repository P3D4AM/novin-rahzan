namespace PacketParser.Entities;

public enum EnTimeExpectation : short
{
	[PersianNameAttribute.PersianName("کوتاه")]
	Kotah,
	[PersianNameAttribute.PersianName("متوسط")]
	Motarf,
	[PersianNameAttribute.PersianName("طولانی")]
	Tolani
}
