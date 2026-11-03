namespace PacketParser.Entities;

public enum EnCouponGardesh : short
{
	[PersianNameAttribute.PersianName("یک هفته")]
	Week,
	[PersianNameAttribute.PersianName("یک ماه")]
	OneMonth,
	[PersianNameAttribute.PersianName("سه ماه")]
	ThreeMonth,
	[PersianNameAttribute.PersianName("شش ماه")]
	SixMonth,
	[PersianNameAttribute.PersianName("نه ماه")]
	NineMonth,
	[PersianNameAttribute.PersianName("یک سال")]
	OneYear
}
