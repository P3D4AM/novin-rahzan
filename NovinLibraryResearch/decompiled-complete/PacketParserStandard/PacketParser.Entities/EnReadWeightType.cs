namespace PacketParser.Entities;

public enum EnReadWeightType : short
{
	[PersianNameAttribute.PersianName("به در خواست کاربر")]
	ByRequest,
	[PersianNameAttribute.PersianName("به صورت پیوسته")]
	Continuously
}
