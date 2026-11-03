namespace PacketParser.Entities;

public enum EnSanadType : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	none = 0,
	[PersianNameAttribute.PersianName("سند دریافت")]
	Reception = 1,
	[PersianNameAttribute.PersianName("سند پرداخت")]
	Pardakht = 2,
	[PersianNameAttribute.PersianName("فاکتور فروش")]
	OrderSell = 3,
	[PersianNameAttribute.PersianName("فاکتور خرید")]
	OrderBuy = 4,
	[PersianNameAttribute.PersianName("افتتاح حساب")]
	EftetaheHesab = 5,
	[PersianNameAttribute.PersianName("فاکتور برگشت از خرید")]
	OrderBackBuy = 6,
	[PersianNameAttribute.PersianName("فاکتور برگشت از فروش")]
	OrderBackSell = 7,
	[PersianNameAttribute.PersianName("انتقال وجه بین بانکها")]
	BankBankEnteghal = 8,
	[PersianNameAttribute.PersianName("انتقال وجه بین صندوقها")]
	SandoghSandoghEnteghal = 9,
	[PersianNameAttribute.PersianName("کسر کارمزد از بانک")]
	BankKasreKarmozd = 10,
	[PersianNameAttribute.PersianName("واریز نقد به بانک")]
	BankVarizeNaghd = 11,
	[PersianNameAttribute.PersianName("واریز سود به بانک")]
	BankVarizeSood = 12,
	[PersianNameAttribute.PersianName("دریافت وجه نقد از بانک")]
	BankDaryafteNaghd = 13,
	[PersianNameAttribute.PersianName("پیش فاکتور")]
	PishFactor = 14,
	[PersianNameAttribute.PersianName("رسید تولید")]
	Tolid = 15,
	[PersianNameAttribute.PersianName("فاکتور ضایعات")]
	Zayeat = 16,
	[PersianNameAttribute.PersianName("انتقال کالا بین انبارها")]
	enteghalBeyneAnbar = 17,
	[PersianNameAttribute.PersianName("چک دریافتنی")]
	CheckM = 60,
	[PersianNameAttribute.PersianName("برگشت چک شخصی")]
	CheckShakhsiBargasht = 34,
	[PersianNameAttribute.PersianName("اعلام وصول چک شخصی")]
	CheckShakhshiElameVosol = 35,
	[PersianNameAttribute.PersianName("سند حسابداری")]
	ManualSanad = 50,
	[PersianNameAttribute.PersianName("ورود و خروج کاربر")]
	Main = 51
}
