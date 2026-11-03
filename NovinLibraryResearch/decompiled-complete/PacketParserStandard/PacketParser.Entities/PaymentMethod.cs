namespace PacketParser.Entities;

public enum PaymentMethod
{
	[PersianNameAttribute.PersianName("افزایش اعتبار")]
	None,
	[PersianNameAttribute.PersianName("انلاین")]
	Online,
	[PersianNameAttribute.PersianName("کسر از اعتبار")]
	Credit,
	[PersianNameAttribute.PersianName("در محل")]
	InPlace
}
