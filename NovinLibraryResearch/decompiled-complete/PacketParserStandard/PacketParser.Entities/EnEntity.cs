namespace PacketParser.Entities;

public enum EnEntity : short
{
	[PersianNameAttribute.PersianName("گروه اشخاص")]
	CustomerGroup,
	[PersianNameAttribute.PersianName("عنوان اشخاص")]
	CustomerTitle,
	[PersianNameAttribute.PersianName("حساب تفصیلی")]
	Tafsil,
	[PersianNameAttribute.PersianName("بانک ها")]
	Bank,
	[PersianNameAttribute.PersianName("خدمات")]
	Service,
	[PersianNameAttribute.PersianName("کالاها")]
	Product,
	[PersianNameAttribute.PersianName("گروه کالا")]
	ProductGroup,
	[PersianNameAttribute.PersianName("واحد کالا")]
	ProductUnit,
	[PersianNameAttribute.PersianName("گروه کالای انتخابی")]
	ProductSelectedGroup,
	[PersianNameAttribute.PersianName("حساب معین")]
	Moein
}
