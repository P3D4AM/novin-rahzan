namespace PacketParser.Entities;

public enum EnProductKind : short
{
	[PersianNameAttribute.PersianName("کالای عمومی (ساده)")]
	SimpleProduct,
	[PersianNameAttribute.PersianName("خدمات")]
	Service,
	[PersianNameAttribute.PersianName("کالای مرکب")]
	CompositeProduct,
	[PersianNameAttribute.PersianName("کالای تنوع دار")]
	Variation,
	[PersianNameAttribute.PersianName("کالای وابسته به ارز")]
	Decimal
}
