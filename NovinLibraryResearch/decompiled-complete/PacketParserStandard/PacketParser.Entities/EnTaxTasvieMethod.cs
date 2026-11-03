namespace PacketParser.Entities;

public enum EnTaxTasvieMethod
{
	[PersianNameAttribute.PersianName("نقد")]
	Cash = 1,
	[PersianNameAttribute.PersianName("نسیه")]
	Etebar,
	[PersianNameAttribute.PersianName("نقد/نسیه")]
	Cash_Etebar
}
