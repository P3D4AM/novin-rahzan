namespace PacketParser.Entities;

public enum EnLog : short
{
	[PersianNameAttribute.PersianName(" ")]
	None = 0,
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
	[PersianNameAttribute.PersianName("برگشت چک مشتریان")]
	CheckAshkhasBargasht = 6,
	[PersianNameAttribute.PersianName("فاکتور برگشت از خرید")]
	OrderBackBuy = 7,
	[PersianNameAttribute.PersianName("فاکتور برگشت از فروش")]
	OrderBackSell = 8,
	[PersianNameAttribute.PersianName("برگشت چک پرداختنی")]
	CheckShakhsiBargasht = 9,
	[PersianNameAttribute.PersianName("پیش فاکتور فروش")]
	PishFactor = 10,
	[PersianNameAttribute.PersianName("اعلام پرداخت چک پرداختنی")]
	CheckShakhshiElameVosol = 11,
	[PersianNameAttribute.PersianName("نقد کردن چک مشتریان")]
	CheckAshkhasNaghdKardan = 12,
	[PersianNameAttribute.PersianName("انتقال وجه بین بانک ها")]
	BankBankEnteghal = 13,
	[PersianNameAttribute.PersianName("انتقال وجه بین صندوقها")]
	SandoghSandoghEnteghal = 14,
	[PersianNameAttribute.PersianName("کسر کارمزد بانک")]
	BankKasreKarmozd = 15,
	[PersianNameAttribute.PersianName("واریز نقد به بانک")]
	BankVarizeNaghd = 16,
	[PersianNameAttribute.PersianName("واریز سود به بانک")]
	BankVarizeSood = 17,
	[PersianNameAttribute.PersianName("برداشت نقد از بانک")]
	BankDaryafteNaghd = 18,
	[PersianNameAttribute.PersianName("وصول چک مشتریان")]
	CheckAshkhasVosol = 19,
	[PersianNameAttribute.PersianName("تولید")]
	Tolid = 20,
	[PersianNameAttribute.PersianName("ضایعات")]
	Zayeat = 21,
	[PersianNameAttribute.PersianName("انتقال بین انبارها")]
	EnteghalBeyneAnbar = 22,
	[PersianNameAttribute.PersianName("یادداشت")]
	Note = 23,
	[PersianNameAttribute.PersianName("انبارگردانی")]
	AnbarGardani = 24,
	[PersianNameAttribute.PersianName("لیست توزیع")]
	TozieeList = 25,
	[PersianNameAttribute.PersianName("کالای امانی دیگران نزد ما")]
	AmaniVorod = 26,
	[PersianNameAttribute.PersianName("کالای امانی ما نزد دیگران")]
	AmaniKhoroj = 27,
	[PersianNameAttribute.PersianName("سیستم بایگانی فایل")]
	FileManagement = 28,
	[PersianNameAttribute.PersianName("Sell_SelectionListCustomer")]
	Sell_SelectionListCustomer = 31,
	[PersianNameAttribute.PersianName("Sell_SelectionListBazaryab")]
	Sell_SelectionListBazaryab = 32,
	[PersianNameAttribute.PersianName("هزینه های تولید")]
	Tolid_Hazine = 43,
	[PersianNameAttribute.PersianName("کالای تولید شده")]
	Tolid_Mahsul = 44,
	[PersianNameAttribute.PersianName("مواد اولیه مصرف شده")]
	Tolid_Mavad = 45,
	[PersianNameAttribute.PersianName("گزارش فروش لیستی از اشخاص")]
	RPTSellSelectionListCustomer = 46,
	[PersianNameAttribute.PersianName("گزارش فروش بازاریاب ها")]
	RPTSellSelectionListVisitor = 47,
	[PersianNameAttribute.PersianName("گزارش وضعیت پرسنل")]
	Main = 48,
	[PersianNameAttribute.PersianName("اسناد دریافتنی")]
	CheckM = 49,
	[PersianNameAttribute.PersianName("سند حسابداری")]
	Sanad = 50,
	[PersianNameAttribute.PersianName("سند چکها")]
	CheckSanad = 60,
	[PersianNameAttribute.PersianName("اسناد پرداختنی")]
	CheckSh = 102,
	[PersianNameAttribute.PersianName("اشخاص")]
	CustomerLog = 200,
	[PersianNameAttribute.PersianName("کالاها")]
	ProductLog = 201,
	[PersianNameAttribute.PersianName("قفل-اکتیویشین")]
	LkInfo = 202,
	[PersianNameAttribute.PersianName("رسید تحویل کالا")]
	Resid = 203,
	[PersianNameAttribute.PersianName("عملیات اسناد دریافتنی")]
	ChequeMOperation = 204,
	[PersianNameAttribute.PersianName("عملیات اسناد پرداختنی")]
	ChequeShOperation = 205,
	[PersianNameAttribute.PersianName("درخواست خرید")]
	OrderPreBuy = 206,
	[PersianNameAttribute.PersianName("فرمول تولید")]
	TolidFormula = 207,
	[PersianNameAttribute.PersianName("پشتیبانی")]
	Ticket = 208,
	[PersianNameAttribute.PersianName("رسید انبار (ورود کالا)")]
	DepotReceipt = 209,
	[PersianNameAttribute.PersianName("حواله انبار (خروج کالا)")]
	DepotHavale = 210,
	[PersianNameAttribute.PersianName("اقساط دریافتنی")]
	AghsatM = 211,
	[PersianNameAttribute.PersianName("اقساط پرداختنی")]
	AghsatSh = 212,
	[PersianNameAttribute.PersianName("سرنخ های فروش")]
	SarNakh = 213,
	[PersianNameAttribute.PersianName("ذخیره و ویرایش کاربر")]
	SaveUser = 214,
	[PersianNameAttribute.PersianName("اقساط دریافتنی")]
	AghsatMProfit = 215,
	[PersianNameAttribute.PersianName("صورتحساب")]
	Billing = 216,
	[PersianNameAttribute.PersianName("واحد کالا")]
	ProductVahed = 217,
	[PersianNameAttribute.PersianName("گروه کالا")]
	ProductGroup = 218,
	[PersianNameAttribute.PersianName("حساب تفصیلی")]
	Tafsil = 219,
	[PersianNameAttribute.PersianName("حساب معین")]
	Moein = 220,
	[PersianNameAttribute.PersianName("حساب کل")]
	Kol = 221,
	[PersianNameAttribute.PersianName("گروه اشخاص")]
	CustomerGroup = 222,
	[PersianNameAttribute.PersianName("عناوین اشخاص")]
	CustomerTitle = 223,
	[PersianNameAttribute.PersianName("گروه تفصیلی")]
	TafsilGroup = 224,
	[PersianNameAttribute.PersianName("بیومتریک")]
	BioMetric = 999
}
