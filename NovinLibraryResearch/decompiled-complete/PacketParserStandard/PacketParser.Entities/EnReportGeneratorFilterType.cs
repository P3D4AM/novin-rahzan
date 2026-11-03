namespace PacketParser.Entities;

public enum EnReportGeneratorFilterType : short
{
	[PersianNameAttribute.PersianName("نا مشخص")]
	None,
	[PersianNameAttribute.PersianName("تاریخ")]
	Date,
	[PersianNameAttribute.PersianName("شماره")]
	Number,
	[PersianNameAttribute.PersianName("کالا")]
	Product,
	[PersianNameAttribute.PersianName("گروه کالا")]
	ProductGroup,
	[PersianNameAttribute.PersianName("لیست انتخابی کالاها")]
	ProductList,
	[PersianNameAttribute.PersianName("طرف حساب")]
	Customer,
	[PersianNameAttribute.PersianName("گروه اشخاص")]
	CustomerGroup,
	[PersianNameAttribute.PersianName("لیست انتخابی اشخاص")]
	CustomerList,
	[PersianNameAttribute.PersianName("کاربر")]
	User,
	[PersianNameAttribute.PersianName("انبار")]
	Depot,
	[PersianNameAttribute.PersianName("مبلغ")]
	Price,
	[PersianNameAttribute.PersianName("مبلغ بدهکار")]
	DebitPrice,
	[PersianNameAttribute.PersianName("مبلغ بستانکار")]
	CreditPrice,
	[PersianNameAttribute.PersianName("شرح سند")]
	SanadDesc,
	[PersianNameAttribute.PersianName("کد تفصیل")]
	TafsilCode,
	[PersianNameAttribute.PersianName("کد معین")]
	MoeinCode,
	[PersianNameAttribute.PersianName("کد کل")]
	KolCode,
	[PersianNameAttribute.PersianName("سریال")]
	Serial,
	[PersianNameAttribute.PersianName("کد گارانتی")]
	Garanti,
	[PersianNameAttribute.PersianName("مسیر")]
	Masir,
	[PersianNameAttribute.PersianName("لاگ گیری")]
	Log,
	[PersianNameAttribute.PersianName("درصد مشارکت")]
	Mosharekat
}
