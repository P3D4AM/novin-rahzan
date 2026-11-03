namespace PacketParser.Entities;

public enum EnProductTab : short
{
	[PersianNameAttribute.PersianName("مشخصات عمومی")]
	PublicInfo,
	[PersianNameAttribute.PersianName("سطوح قیمت ها")]
	PriceInfo,
	[PersianNameAttribute.PersianName("مشخصات اختصاصی")]
	SpecialInfo,
	[PersianNameAttribute.PersianName("چند واحدی")]
	MultiUnit,
	[PersianNameAttribute.PersianName("سایر اطلاعات")]
	OtherInfo,
	[PersianNameAttribute.PersianName("اطلاعات مالیاتی")]
	TaxInfo,
	[PersianNameAttribute.PersianName("بارکد")]
	BarcodeInfo,
	[PersianNameAttribute.PersianName("فروشگاه اینترنتی")]
	OnlineShop,
	[PersianNameAttribute.PersianName("ویژگی های کالا")]
	AttributeInfo,
	[PersianNameAttribute.PersianName("مشخصات ارزی")]
	DecimalInfo,
	[PersianNameAttribute.PersianName("فرمول تولید")]
	TolidFormula,
	[PersianNameAttribute.PersianName("برچسب ها")]
	Tag
}
