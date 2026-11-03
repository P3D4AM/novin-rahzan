namespace PacketParser.Entities;

public enum ProductMojodiSt
{
	[PersianNameAttribute.PersianName("بر اساس تنظیمات انبار")]
	DepotDefault = 1,
	[PersianNameAttribute.PersianName("غیر فعال")]
	DisableMojodiManfi,
	[PersianNameAttribute.PersianName("فعال")]
	EnableMojodiManfi
}
