namespace PacketParser.Entities;

public enum EnPlacementStatus : short
{
	[PersianNameAttribute.PersianName("خالی")]
	Free,
	[PersianNameAttribute.PersianName("پر")]
	Full,
	[PersianNameAttribute.PersianName("رزرو")]
	Reserve
}
