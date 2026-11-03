namespace PacketParser.Entities;

public enum EnLK_SpecialFacilities : short
{
	[PersianNameAttribute.PersianName("بدون امکانات ویژه")]
	None = 1,
	[PersianNameAttribute.PersianName("زیربسته چک و خزانه پیشرفته")]
	Cheque = 2,
	[PersianNameAttribute.PersianName("زیربسته قسط و وام پیشرفته")]
	Aghsat = 4,
	[PersianNameAttribute.PersianName("زیربسته مالی پیشرفته")]
	AdvancedFinance = 8,
	[PersianNameAttribute.PersianName("زیربسته انبار پیشرفته")]
	Depot = 0x10,
	[PersianNameAttribute.PersianName("زیربسته سرویس و خدمات پیشرفته")]
	Service = 0x20,
	[PersianNameAttribute.PersianName("زیربسته سامانه امور مالیاتی")]
	Tax = 0x40,
	[PersianNameAttribute.PersianName("زیربسته باشگاه مشتریان")]
	CustomerClub = 0x80,
	[PersianNameAttribute.PersianName("زیربسته ارتباط با کالر ایدی")]
	CallerId = 0x100,
	[PersianNameAttribute.PersianName("زیربسته کالای سریال دار")]
	ProductSerial = 0x200,
	[PersianNameAttribute.PersianName("زیربسته چند ارزی")]
	MultiCurrency = 0x400,
	[PersianNameAttribute.PersianName("زیربسته فراخوان صوتی")]
	VoiceCall = 0x800,
	[PersianNameAttribute.PersianName("زیربسته تولید و بهای تمام شده")]
	Tolid = 0x1000,
	[PersianNameAttribute.PersianName("زیربسته مدیریت بارکد")]
	Barcode = 0x2000,
	[PersianNameAttribute.PersianName("زیربسته ارتباط با ترازو")]
	Scale = 0x4000
}
