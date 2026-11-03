namespace PacketParser.Entities;

public enum EnTaxType : short
{
	[PersianNameAttribute.PersianName("مشمول")]
	Contains = 1,
	[PersianNameAttribute.PersianName("معاف")]
	NotContains
}
