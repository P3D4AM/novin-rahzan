namespace PacketParser.Entities;

public enum EnTypeChose : short
{
	[PersianNameAttribute.PersianName("برای هر کالا ،هدیه خاص")]
	GiftPackageForProduct,
	[PersianNameAttribute.PersianName("انتخاب هدیه به عهده خریدار")]
	SelectGiftByCustomer
}
