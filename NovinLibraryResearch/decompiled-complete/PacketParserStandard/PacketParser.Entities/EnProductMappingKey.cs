namespace PacketParser.Entities;

public enum EnProductMappingKey : short
{
	[PersianNameAttribute.PersianName("مشخص نشده")]
	None,
	[PersianNameAttribute.PersianName("MappedId")]
	MappedId,
	[PersianNameAttribute.PersianName("Sku")]
	Sku,
	[PersianNameAttribute.PersianName("کد کالا")]
	Code,
	[PersianNameAttribute.PersianName("شناسه کالا")]
	Guid
}
