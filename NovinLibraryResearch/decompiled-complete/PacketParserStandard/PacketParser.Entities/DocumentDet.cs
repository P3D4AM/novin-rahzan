namespace PacketParser.Entities;

public enum DocumentDet
{
	[PersianNameAttribute.PersianName("دریافت نقدی")]
	DaryaftN = 9101,
	[PersianNameAttribute.PersianName("دریافت چک")]
	DaryaftCh = 9102,
	[PersianNameAttribute.PersianName("دریافت حواله")]
	DaryaftH = 9103,
	[PersianNameAttribute.PersianName("انتقال در دریافت")]
	DaryaftEnteghal = 9150,
	[PersianNameAttribute.PersianName("پرداخت نقدی")]
	PardakhtN = 9201,
	[PersianNameAttribute.PersianName("پرداخت چک شخصی")]
	PardakhtChSh = 9202,
	[PersianNameAttribute.PersianName("پرداخت چک مشتری")]
	PardakhtChAshkhas = 9203,
	[PersianNameAttribute.PersianName("پرداخت حواله")]
	PardakhtBank = 9204,
	[PersianNameAttribute.PersianName("انتقال در پرداخت")]
	PardakhtEnteghal = 9250,
	[PersianNameAttribute.PersianName("فروش")]
	SellPrd = 9300,
	[PersianNameAttribute.PersianName("بستانکار خدمات (فروش)")]
	SellService = 9301,
	[PersianNameAttribute.PersianName("تخفیف (فروش)")]
	SellDiscount = 9302,
	[PersianNameAttribute.PersianName("کرایه (فروش)")]
	SellKeraye = 9303,
	[PersianNameAttribute.PersianName("ویزیتور (فروش)")]
	SellBazaryab = 9304,
	[PersianNameAttribute.PersianName("ارزش افزوده (فروش)")]
	SellMaliyat = 9305,
	[PersianNameAttribute.PersianName("ارزش افزوده (فروش)")]
	SellTax = 9307,
	[PersianNameAttribute.PersianName("عوارض قانونی (فروش)")]
	SellAvarez = 9308,
	[PersianNameAttribute.PersianName("سند متصل (فروش)")]
	SellRelatedSanad = 9350,
	[PersianNameAttribute.PersianName("خرید")]
	BuyPrd = 9400,
	[PersianNameAttribute.PersianName("تخفیف (خرید)")]
	BuyDiscount = 9402,
	[PersianNameAttribute.PersianName("کرایه (خرید)")]
	BuyKeraye = 9403,
	[PersianNameAttribute.PersianName("ارزش افزوده (خرید)")]
	BuyMaliyat = 9405,
	[PersianNameAttribute.PersianName("هزینه های خرید")]
	BuyHFactorBuy = 9406,
	[PersianNameAttribute.PersianName("ارزش افزوده (خرید)")]
	BuyTax = 9407,
	[PersianNameAttribute.PersianName("عوارض قانونی (خرید)")]
	BuyAvarez = 9408,
	[PersianNameAttribute.PersianName("سند متصل (خرید)")]
	BuyRelatedSanad = 9450,
	[PersianNameAttribute.PersianName("HDarAval2Re")]
	HDarAval2Re = 9501,
	[PersianNameAttribute.PersianName("HParAval2Re")]
	HParAval2Re = 9502,
	[PersianNameAttribute.PersianName("Aval2ReBank")]
	Aval2ReBank = 9503,
	[PersianNameAttribute.PersianName("Aval2ReSandogh")]
	Aval2ReSandogh = 9504,
	[PersianNameAttribute.PersianName("ChParAval2Re")]
	ChParAval2Re = 9505,
	[PersianNameAttribute.PersianName("ChDarAval2Re")]
	ChDarAval2Re = 9506,
	[PersianNameAttribute.PersianName("MojodiAval2Re")]
	MojodiAval2Re = 9507,
	[PersianNameAttribute.PersianName("BargashtChDarAval2Re")]
	BargashtChDarAval2Re = 9601,
	[PersianNameAttribute.PersianName("BargashtChdarTol2Re")]
	BargashtChdarTol2Re = 9602,
	[PersianNameAttribute.PersianName("برگشت از خرید")]
	BargashtBuyPrd = 9700,
	[PersianNameAttribute.PersianName("تخفیف (ب از خرید)")]
	BargashtBuyDiscount = 9702,
	[PersianNameAttribute.PersianName("کرایه (ب از خرید)")]
	BargashtBuyKeraye = 9703,
	[PersianNameAttribute.PersianName("ارزش افزوده (ب از خرید)")]
	BargashtBuyTax = 9707,
	[PersianNameAttribute.PersianName("عوارض قانونی (ب از خرید)")]
	BargashtBuyAvarez = 9708,
	[PersianNameAttribute.PersianName("سند متصل (ب از خرید)")]
	BargashtBuyRelatedSanad = 9750,
	[PersianNameAttribute.PersianName("برگشت از فروش")]
	BargashtSellPrd = 9800,
	[PersianNameAttribute.PersianName("تخفیف (ب از فروش)")]
	BargashtSellDiscount = 9802,
	[PersianNameAttribute.PersianName("کرایه (ب از فروش)")]
	BargashtSellKeraye = 9803,
	[PersianNameAttribute.PersianName("ویزیتور (ب از فروش)")]
	BargashtSellBazaryab = 9804,
	[PersianNameAttribute.PersianName("ارزش افزوده (ب از فروش)")]
	BargashtSellTax = 9805,
	[PersianNameAttribute.PersianName("عوارش قانونی (ب از فروش)")]
	BargashtSellAvarez = 9808,
	[PersianNameAttribute.PersianName("سند متصل (ب از فروش)")]
	BargashtSellRelatedSanad = 9850,
	[PersianNameAttribute.PersianName("پیش فاکتور")]
	PishFactorPrd = 10,
	[PersianNameAttribute.PersianName("سند متصل (پیش فاکتور)")]
	PishFactorRelatedSanad = 1050,
	[PersianNameAttribute.PersianName("ChShBargashtiAval2Re")]
	ChShBargashtiAval2Re = 9901,
	[PersianNameAttribute.PersianName("ChShBargashtiTol2Re")]
	ChShBargashtiTol2Re = 9902,
	[PersianNameAttribute.PersianName("پرداخت چک شخصی")]
	PardakhtCheckSh = 1100,
	[PersianNameAttribute.PersianName("VajheCheckN")]
	VajheCheckN = 1200,
	[PersianNameAttribute.PersianName("انتقال وجه بین بانکها")]
	EnteghalVajhBeinBank = 1300,
	[PersianNameAttribute.PersianName("انتقال وجه بین صندوق ها")]
	EnteghalVajhBeinSandogh = 1400,
	[PersianNameAttribute.PersianName("کسر کارمزد")]
	KasrKarmozdAzBank = 1500,
	[PersianNameAttribute.PersianName("واریز وجه به بانک")]
	VarizVajheNBeBank = 1600,
	[PersianNameAttribute.PersianName("واریز سود به بانک")]
	VarizSoodBeBank = 1700,
	[PersianNameAttribute.PersianName("برداشت وجه نقد از بانک")]
	BardasshtNAzBank = 1800,
	[PersianNameAttribute.PersianName("اعلام وصول چک شخصی")]
	ElamVosolCh = 1900,
	[PersianNameAttribute.PersianName("سند حسابداری")]
	SanadHesabdari = 5000,
	[PersianNameAttribute.PersianName("تولید")]
	Tolid = 2000,
	[PersianNameAttribute.PersianName("انتقال بین انبارها")]
	DepEnteghal = 2200,
	[PersianNameAttribute.PersianName("فاکتور ضایعات")]
	Zayeat = 2100,
	[PersianNameAttribute.PersianName("سند متصل (ضایعات)")]
	ZayeatRelatedSanad = 2150,
	[PersianNameAttribute.PersianName("فاکتور امانی وارده")]
	AmaniVorod = 26,
	[PersianNameAttribute.PersianName("سند متصل (انامی وارده)")]
	AmaniVorodRelatedSanad = 2650,
	[PersianNameAttribute.PersianName("فاکتور امانی صادره")]
	AmaniKhoroj = 27,
	[PersianNameAttribute.PersianName("سند متصل (امانی صادره)")]
	AmaniKhorojRelatedSanad = 2750,
	[PersianNameAttribute.PersianName("انبارگردانی")]
	AnbarGardani = 2400,
	[PersianNameAttribute.PersianName("سند چک ها")]
	ChequeSanad = 60,
	[PersianNameAttribute.PersianName("NONE")]
	None = 0,
	[PersianNameAttribute.PersianName("درخواست خرید")]
	PreBuyPrd = 206,
	[PersianNameAttribute.PersianName("اقساط دریافتنی")]
	AghsatM = 215
}
