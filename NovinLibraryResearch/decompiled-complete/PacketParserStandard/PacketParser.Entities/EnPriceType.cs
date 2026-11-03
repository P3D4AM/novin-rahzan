namespace PacketParser.Entities;

public enum EnPriceType : short
{
	[PersianNameAttribute.PersianName("بدون تغییر")]
	none = 0,
	[PersianNameAttribute.PersianName("سطح فروش یک")]
	sell1 = 1,
	[PersianNameAttribute.PersianName("سطح فروش دو")]
	sell2 = 2,
	[PersianNameAttribute.PersianName("سطح فروش سه")]
	sell3 = 3,
	[PersianNameAttribute.PersianName("سطح فروش چهار")]
	sell4 = 4,
	[PersianNameAttribute.PersianName("سطح فروش پنج")]
	sell5 = 5,
	[PersianNameAttribute.PersianName("سطح فروش شش")]
	sell6 = 6,
	[PersianNameAttribute.PersianName("سطح فروش هفت")]
	sell7 = 7,
	[PersianNameAttribute.PersianName("سطح فروش هشت")]
	sell8 = 8,
	[PersianNameAttribute.PersianName("آخرین فروش به تمام مشتریان")]
	LastSellToAll = 9,
	[PersianNameAttribute.PersianName("آخرین فروش به این مشتری")]
	LastSellToCus = 10,
	[PersianNameAttribute.PersianName("آخرین خرید از همه مشتریان")]
	BuyLastFromAll = 11,
	[PersianNameAttribute.PersianName("آخرین خرید از مشتری")]
	BuyLastFromCus = 12,
	[PersianNameAttribute.PersianName("فی ابتدای دوره")]
	Avale2re = 13,
	[PersianNameAttribute.PersianName("میانگین خرید")]
	BuyAverage = 14,
	[PersianNameAttribute.PersianName("سطح خرید یک")]
	Buy1 = 15,
	[PersianNameAttribute.PersianName("سطح خرید دو")]
	Buy2 = 16,
	[PersianNameAttribute.PersianName("سطح خرید سه")]
	Buy3 = 17,
	[PersianNameAttribute.PersianName("آخرین خرید")]
	BuyLast = 19,
	[PersianNameAttribute.PersianName("ثبت شده در آخرین ویرایش")]
	CurrentSaved = 18,
	[PersianNameAttribute.PersianName("قیمت مصرف کننده")]
	Regular = 20
}
