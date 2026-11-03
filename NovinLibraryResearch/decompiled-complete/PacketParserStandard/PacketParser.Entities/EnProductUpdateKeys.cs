namespace PacketParser.Entities;

public enum EnProductUpdateKeys : short
{
	[PersianNameAttribute.PersianName("ویزیتور 1")]
	Visitor1Percent = 1,
	[PersianNameAttribute.PersianName("ویزیتور 2")]
	Visitor2Percent,
	[PersianNameAttribute.PersianName("ویزیتور 3")]
	Visitor3Percent,
	[PersianNameAttribute.PersianName("ویزیتور 4")]
	Visitor4Percent,
	[PersianNameAttribute.PersianName("درصد تخفیف")]
	DiscountPercent,
	[PersianNameAttribute.PersianName("تاریخ آغاز اعمال تخفیف")]
	DiscountStartDate,
	[PersianNameAttribute.PersianName("تاریخ پایان اعمال تخفیف")]
	DiscountEndDate,
	[PersianNameAttribute.PersianName("ساختار مالیاتی")]
	TaxClassGuid,
	[PersianNameAttribute.PersianName("گروه کالا")]
	GroupGuid,
	[PersianNameAttribute.PersianName("کالای خاص")]
	Special,
	[PersianNameAttribute.PersianName("مشخصه فنی")]
	TechnicalInfo
}
