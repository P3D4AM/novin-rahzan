namespace PacketParser.Entities;

public enum WebEntityLog : short
{
	[PersianNameAttribute.PersianName("None")]
	None = 0,
	[PersianNameAttribute.PersianName("کالاها")]
	Product = 1,
	[PersianNameAttribute.PersianName("گروه کالاها")]
	ProductGroup = 2,
	[PersianNameAttribute.PersianName("واحد کالاها")]
	ProductVahed = 3,
	[PersianNameAttribute.PersianName("اشخاص")]
	Customer = 4,
	[PersianNameAttribute.PersianName("گروه اشخاص")]
	CustomerGroup = 5,
	[PersianNameAttribute.PersianName("عناوین اشخاص")]
	CustomerTitle = 6,
	[PersianNameAttribute.PersianName("انبارها")]
	Depot = 7,
	[PersianNameAttribute.PersianName("رابطه کالا و انبار")]
	ProductAnbarRelation = 8,
	[PersianNameAttribute.PersianName("کاربران")]
	Cashier = 9,
	[PersianNameAttribute.PersianName("کاربران")]
	User = 10,
	[PersianNameAttribute.PersianName("گردش تفصیلی")]
	GardeshTafsili = 11,
	[PersianNameAttribute.PersianName("تنظیمات")]
	Settings = 12,
	[PersianNameAttribute.PersianName("ارتباط با ما")]
	ContactUs = 14,
	[PersianNameAttribute.PersianName("HyperLink")]
	HyperLink = 15,
	[PersianNameAttribute.PersianName("اخبار")]
	News = 16,
	[PersianNameAttribute.PersianName("FileInfo")]
	FileInformations = 17,
	[PersianNameAttribute.PersianName("سند دریافت")]
	Reception = 18,
	[PersianNameAttribute.PersianName("سند پرداخت")]
	Pardakht = 19,
	[PersianNameAttribute.PersianName("فاکتورها")]
	Order = 20,
	[PersianNameAttribute.PersianName("Delivery")]
	Delivery = 21,
	[PersianNameAttribute.PersianName("بارکد")]
	Barcode = 22,
	[PersianNameAttribute.PersianName("GPS")]
	Gps = 23,
	[PersianNameAttribute.PersianName("حساب های کل")]
	Kol = 24,
	[PersianNameAttribute.PersianName("حساب های معین")]
	Moein = 25,
	[PersianNameAttribute.PersianName("حساب های تفصیلی")]
	Tafsil = 26,
	[PersianNameAttribute.PersianName("فاکتور فروش")]
	OrderSell = 27,
	[PersianNameAttribute.PersianName("فاکتور خرید")]
	OrderBuy = 28,
	[PersianNameAttribute.PersianName("افتتاح حساب")]
	EftetaheHesab = 29,
	[PersianNameAttribute.PersianName("برگشت چک مشتریان")]
	CheckAshkhasBargasht = 30,
	[PersianNameAttribute.PersianName("فاکتور برگشت از خرید")]
	OrderBackBuy = 31,
	[PersianNameAttribute.PersianName("فاکتور برگشت از فروش")]
	OrderBackSell = 32,
	[PersianNameAttribute.PersianName("برگشت چک پرداختنی")]
	CheckShakhsiBargasht = 33,
	[PersianNameAttribute.PersianName("پیش فاکتور فروش")]
	PishFactor = 34,
	[PersianNameAttribute.PersianName("اعلام پرداخت چک پرداختنی")]
	CheckShakhshiElameVosol = 35,
	[PersianNameAttribute.PersianName("نقد کردن چک مشتریان")]
	CheckAshkhasNaghdKardan = 36,
	[PersianNameAttribute.PersianName("انتقال وجه بین بانک ها")]
	BankBankEnteghal = 37,
	[PersianNameAttribute.PersianName("انتقال وجه بین صندوقها")]
	SandoghSandoghEnteghal = 38,
	[PersianNameAttribute.PersianName("کسر کارمزد بانک")]
	BankKasreKarmozd = 39,
	[PersianNameAttribute.PersianName("واریز نقد به بانک")]
	BankVarizeNaghd = 40,
	[PersianNameAttribute.PersianName("واریز سود به بانک")]
	BankVarizeSood = 41,
	[PersianNameAttribute.PersianName("واریز نقد به بانک")]
	BankDaryafteNaghd = 42,
	[PersianNameAttribute.PersianName("وصول چک مشتریان")]
	CheckAshkhasVosol = 43,
	[PersianNameAttribute.PersianName("تولید")]
	Tolid = 44,
	[PersianNameAttribute.PersianName("ضایعات")]
	Zayeat = 45,
	[PersianNameAttribute.PersianName("انتقال بین انبارها")]
	EnteghalBeyneAnbar = 46,
	[PersianNameAttribute.PersianName("یادداشت")]
	Note = 47,
	[PersianNameAttribute.PersianName("انبارگردانی")]
	AnbarGardani = 48,
	[PersianNameAttribute.PersianName("لیست توزیع")]
	TozieeList = 49,
	[PersianNameAttribute.PersianName("کالای امانی دیگران نزد ما")]
	AmaniVorod = 50,
	[PersianNameAttribute.PersianName("کالای امانی ما نزد دیگران")]
	AmaniKhoroj = 51,
	[PersianNameAttribute.PersianName("سیستم بایگانی فایل")]
	FileManagement = 52,
	[PersianNameAttribute.PersianName("هزینه های تولید")]
	Tolid_Hazine = 53,
	[PersianNameAttribute.PersianName("کالای تولید شده")]
	Tolid_Mahsul = 54,
	[PersianNameAttribute.PersianName("مواد اولیه مصرف شده")]
	Tolid_Mavad = 55,
	[PersianNameAttribute.PersianName("عملیات چک")]
	CheckM = 56,
	[PersianNameAttribute.PersianName("سند حسابداری")]
	Sanad = 57,
	[PersianNameAttribute.PersianName("سند افتتاحیه")]
	SanadEftetahie = 58,
	[PersianNameAttribute.PersianName("سند اختتامیه")]
	SanadEkhtetamie = 59,
	[PersianNameAttribute.PersianName("سند بستن حسابهای موقت")]
	SanadBastaneMovaghat = 60,
	[PersianNameAttribute.PersianName("سند یادداشت")]
	SanadNote = 61,
	[PersianNameAttribute.PersianName("سند چکها")]
	CheckSanad = 62,
	[PersianNameAttribute.PersianName("قفل-اکتیویشین")]
	LkInfo = 63,
	[PersianNameAttribute.PersianName("ذخیره و ویرایش کاربر")]
	SaveUser = 64
}
