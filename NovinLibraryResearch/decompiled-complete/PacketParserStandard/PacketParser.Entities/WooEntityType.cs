namespace PacketParser.Entities;

public enum WooEntityType
{
	[PersianNameAttribute.PersianName("گروه کالا")]
	PrdGroup = 0,
	[PersianNameAttribute.PersianName("واحد کالا")]
	PrdUnit = 1,
	[PersianNameAttribute.PersianName("شخص")]
	Customer = 3,
	[PersianNameAttribute.PersianName("کالاهای مرتبط")]
	ProductVariation = 5,
	[PersianNameAttribute.PersianName("پست")]
	Post = 6
}
