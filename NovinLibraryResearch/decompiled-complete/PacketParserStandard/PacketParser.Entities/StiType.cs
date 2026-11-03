namespace PacketParser.Entities;

public enum StiType : short
{
	[PersianNameAttribute.PersianName(" فاکتور فروش (تکی)")]
	SellOne = 1,
	[PersianNameAttribute.PersianName("فاکتور فروش 2 (تکی)")]
	SellOne2 = 32101,
	[PersianNameAttribute.PersianName("فاکتور فروش 3 (تکی)")]
	SellOne3 = 32102,
	[PersianNameAttribute.PersianName("فاکتور فروش 3 (تکی)")]
	SellOne4 = 32103,
	[PersianNameAttribute.PersianName("فاکتور فروش 3 (تکی)")]
	SellOne5 = 32104,
	[PersianNameAttribute.PersianName("فاکتور فروش 3 (تکی)")]
	SellOne6 = 32105,
	[PersianNameAttribute.PersianName("فاکتور فروش 3 (تکی)")]
	SellOne7 = 32106,
	[PersianNameAttribute.PersianName("فاکتور فروش 3 (تکی)")]
	SellOne8 = 32107,
	[PersianNameAttribute.PersianName("فاکتور فروش 3 (تکی)")]
	SellOne9 = 32108,
	[PersianNameAttribute.PersianName("فاکتور فروش 3 (تکی)")]
	SellOne10 = 32109,
	[PersianNameAttribute.PersianName("فاکتور فروش (لیست)")]
	SellList = 2,
	[PersianNameAttribute.PersianName("فاکتور برگشت از فروش (تکی)")]
	BackSellOne = 3,
	[PersianNameAttribute.PersianName("فاکتور برگشت از فروش (لیست)")]
	BackSellList = 4,
	[PersianNameAttribute.PersianName("فاکتور خرید (تکی)")]
	BuyOne = 5,
	[PersianNameAttribute.PersianName("فاکتور خرید (لیست)")]
	BuyList = 6,
	[PersianNameAttribute.PersianName("فاکتور برگشت از خرید (تکی)")]
	BackBuyOne = 7,
	[PersianNameAttribute.PersianName("فاکتور برگشت از خرید (لیست)")]
	BackBuyList = 8,
	[PersianNameAttribute.PersianName("دریافت (تکی)")]
	ReceptionOne = 9,
	[PersianNameAttribute.PersianName("دریافت (لیست)")]
	ReceptionList = 10,
	[PersianNameAttribute.PersianName("پرداخت (تکی)")]
	PardakhtOne = 11,
	[PersianNameAttribute.PersianName("پرداخت (لیست)")]
	PardakhtList = 12,
	[PersianNameAttribute.PersianName("اسناد دریافتنی (تکی)")]
	CheckVaredeOne = 13,
	[PersianNameAttribute.PersianName("اسناد دریافتنی (لیست)")]
	CheckVaredeList = 14,
	[PersianNameAttribute.PersianName("اسناد پرداختنی (تکی)")]
	CheckSaredeOne = 150,
	[PersianNameAttribute.PersianName("اسناد پرداختنی (لیست)")]
	CheckSadereList = 160,
	[PersianNameAttribute.PersianName("RptProductsKoli")]
	RptProductsKoli = 15,
	[PersianNameAttribute.PersianName("RptProductsJozi")]
	RptProductsJozi = 16,
	[PersianNameAttribute.PersianName("پیش فاکتور فروش (تکی)")]
	PishFactorOne = 17,
	[PersianNameAttribute.PersianName("پیش فاکتور فروش (لیست)")]
	PishFactorList = 18,
	[PersianNameAttribute.PersianName("کالاهای زیر نقطه سفارش")]
	ProductPointOrder = 19,
	[PersianNameAttribute.PersianName("اشخاص (لیست)")]
	CustomerList = 20,
	[PersianNameAttribute.PersianName("تولید (تکی)")]
	TolidOne = 21,
	[PersianNameAttribute.PersianName("تولید (لیست)")]
	TolidList = 22,
	[PersianNameAttribute.PersianName("رسیدهای صادره (تکی)")]
	ResidOne = 23,
	[PersianNameAttribute.PersianName("ریز عملکرد کالا (کارکس)")]
	ProductCardex = 24,
	[PersianNameAttribute.PersianName("کالاها (لیست)")]
	ProductSell = 25,
	[PersianNameAttribute.PersianName("ProductSellPic")]
	ProductSellPic = 26,
	[PersianNameAttribute.PersianName("GardeshCustomer")]
	GardeshCustomer = 27,
	[PersianNameAttribute.PersianName("GardeshBank")]
	GardeshBank = 28,
	[PersianNameAttribute.PersianName("GardeshSandogh")]
	GardeshSandogh = 29,
	[PersianNameAttribute.PersianName("GardeshCustTafsil")]
	GardeshCustTafsil = 30,
	[PersianNameAttribute.PersianName("گزارش کالا و طرف حساب")]
	ProductCustomer = 31,
	[PersianNameAttribute.PersianName("کالاها (کاتالوگ محصولات 3)")]
	ProductCatalog3 = 32,
	[PersianNameAttribute.PersianName("ریز عملکرد کالا (کاردکس ریالی و تعدادی)")]
	ProductCardexNew = 33,
	[PersianNameAttribute.PersianName("نامه اداری")]
	NameEdari = 34,
	[PersianNameAttribute.PersianName("سود ماهیانه")]
	SoodMonthlyNew = 35,
	[PersianNameAttribute.PersianName("دفتر معین")]
	DaftareMoein = 36,
	[PersianNameAttribute.PersianName("گزارش ماهیانه چکها")]
	CheckMonthly = 37,
	[PersianNameAttribute.PersianName("سند حسابداری")]
	Sanad = 38,
	[PersianNameAttribute.PersianName("لیست اسناد حسابداری")]
	SanadList = 39,
	[PersianNameAttribute.PersianName("تراز حساب کل")]
	TarazHesab_kol = 42,
	[PersianNameAttribute.PersianName("تراز حساب معین")]
	TarazHesab_moein = 43,
	[PersianNameAttribute.PersianName("تراز حساب تفصیلی به تفکیک معین")]
	TarazHesab_Tafsil_Moein = 44,
	[PersianNameAttribute.PersianName("تراز حساب تفصیلی")]
	TarazHesab_Tafsil = 45,
	[PersianNameAttribute.PersianName("تراز حساب ---")]
	TarazHesab_r1 = 46,
	[PersianNameAttribute.PersianName("تراز حساب ---")]
	TarazHesab_r2 = 48,
	[PersianNameAttribute.PersianName("دفتر روزنامه")]
	Rozname = 49,
	[PersianNameAttribute.PersianName("ترازنامه")]
	TarazName = 50,
	[PersianNameAttribute.PersianName("صورت سود و زیان")]
	SodoZian = 51,
	[PersianNameAttribute.PersianName("تراز آزمایشی")]
	TarazAzmayeshi = 52,
	[PersianNameAttribute.PersianName("راس گیری")]
	RasGiri = 53,
	[PersianNameAttribute.PersianName("تجمیع فروش")]
	RptprdSumSell = 60,
	[PersianNameAttribute.PersianName("تجمیع برگشت از فروش")]
	RptprdSumBSell = 61,
	[PersianNameAttribute.PersianName("تجمیع خرید")]
	RptprdSumBuy = 62,
	[PersianNameAttribute.PersianName("تجمیع برگشت از خرید")]
	RptprdSumBBuy = 63,
	[PersianNameAttribute.PersianName("تجمیع ضایعات")]
	RptprdSumZayeat = 64,
	[PersianNameAttribute.PersianName("تجمیع تولید")]
	RptprdSumTolid = 65,
	[PersianNameAttribute.PersianName("تجمیع انتقال کالا بین انبارها")]
	RptprdSumDptEnteghal = 66,
	[PersianNameAttribute.PersianName("ریز عملکرد کالاها (کارکس چند واحدی)")]
	ProductCardexNewPrintMultiVahed = 67,
	[PersianNameAttribute.PersianName("لیست کالاهای انتخابی")]
	RptSelectionPrd = 68,
	[PersianNameAttribute.PersianName("لیست اشخاص انتخابی")]
	RptSelectionCust = 69,
	[PersianNameAttribute.PersianName("بارکد رسید اشخاص")]
	ResidBarcode = 70,
	[PersianNameAttribute.PersianName("انتقال کالا بین انبارها")]
	RptdptEnteghal = 71,
	[PersianNameAttribute.PersianName("تجمیع پیش فاکتور")]
	RptprdSumPishfactor = 72,
	[PersianNameAttribute.PersianName("ElmiPrint")]
	ElmiPrint = 73,
	[PersianNameAttribute.PersianName("دفتر کل")]
	DaftrarKol = 74,
	[PersianNameAttribute.PersianName("DaftarMoein")]
	DaftarMoein = 75,
	[PersianNameAttribute.PersianName("دفتر تفصیلی")]
	DaftarTafsil = 76,
	[PersianNameAttribute.PersianName("اقساط دریافتنی")]
	AghsatAshkhas = 77,
	[PersianNameAttribute.PersianName("RptCustSellQt")]
	RptCustSellQt = 78,
	[PersianNameAttribute.PersianName("RptCustSellDec")]
	RptCustSellDec = 79,
	[PersianNameAttribute.PersianName("RptCustBuyQt")]
	RptCustBuyQt = 80,
	[PersianNameAttribute.PersianName("RptCustBuyDec")]
	RptCustBuyDec = 81,
	[PersianNameAttribute.PersianName("RptCustBBuyQt")]
	RptCustBBuyQt = 82,
	[PersianNameAttribute.PersianName("RptCustBBuyDec")]
	RptCustBBuyDec = 83,
	[PersianNameAttribute.PersianName("RptCustBSellQt")]
	RptCustBSellQt = 84,
	[PersianNameAttribute.PersianName("RptCustBSellDec")]
	RptCustBSellDec = 85,
	[PersianNameAttribute.PersianName("RptCustSood")]
	RptCustSood = 86,
	[PersianNameAttribute.PersianName("اقساط دریافتنی (لیست)")]
	RptAghsatMList = 87,
	[PersianNameAttribute.PersianName("RptAghsatMPrint")]
	RptAghsatMPrint = 91,
	[PersianNameAttribute.PersianName("RptAghsatMSum")]
	RptAghsatMSum = 92,
	[PersianNameAttribute.PersianName("RptCustLast")]
	RptCustLast = 88,
	[PersianNameAttribute.PersianName("سن بدهی اشخاص")]
	RptBedehiGeneration = 89,
	[PersianNameAttribute.PersianName("انتقال کالا بین انبارها (لیست)")]
	RptdptEnteghalList = 90,
	[PersianNameAttribute.PersianName("RptPezeshki")]
	RptPezeshki = 93,
	[PersianNameAttribute.PersianName("رسیدهای صادره (لیست)")]
	RptResidList = 94,
	[PersianNameAttribute.PersianName("RptGlasses")]
	RptGlasses = 95,
	[PersianNameAttribute.PersianName("فاکتور ضایعات (تکی)")]
	ZayeatOne = 96,
	[PersianNameAttribute.PersianName("بارکد MPL8")]
	Mpl8 = 908,
	[PersianNameAttribute.PersianName("بارکد MPL12")]
	Mpl12 = 912,
	[PersianNameAttribute.PersianName("بارکد MPL16")]
	Mpl16 = 916,
	[PersianNameAttribute.PersianName("بارکد MPL21")]
	Mpl21 = 921,
	[PersianNameAttribute.PersianName("بارکد MPL24")]
	Mpl24 = 924,
	[PersianNameAttribute.PersianName("بارکد MPL30")]
	Mpl30 = 930,
	[PersianNameAttribute.PersianName("بارکد MPL40")]
	Mpl40 = 940,
	[PersianNameAttribute.PersianName("بارکد MPL65")]
	Mpl65 = 965,
	[PersianNameAttribute.PersianName("بارکد MPL84")]
	Mpl84 = 984,
	[PersianNameAttribute.PersianName("RptAdvariBazaryab")]
	RptAdvariBazaryab = 97,
	[PersianNameAttribute.PersianName("RptAdvariSellDec")]
	RptAdvariSellDec = 98,
	[PersianNameAttribute.PersianName("RptAdvariSellQt")]
	RptAdvariSellQt = 99,
	[PersianNameAttribute.PersianName("RptAdvariBuyDec")]
	RptAdvariBuyDec = 100,
	[PersianNameAttribute.PersianName("RptAdvariBuyQt")]
	RptAdvariBuyQt = 101,
	[PersianNameAttribute.PersianName("RptAdvariBSellDec")]
	RptAdvariBSellDec = 102,
	[PersianNameAttribute.PersianName("RptAdvariBSellQt")]
	RptAdvariBSellQt = 103,
	[PersianNameAttribute.PersianName("RptAdvariBBuyDec")]
	RptAdvariBBuyDec = 104,
	[PersianNameAttribute.PersianName("RptAdvariBBuyQt")]
	RptAdvariBBuyQt = 105,
	[PersianNameAttribute.PersianName("TTMS")]
	TTMS = 106,
	[PersianNameAttribute.PersianName("ProductCardexNew_2Re")]
	ProductCardexNew_2Re = 112,
	[PersianNameAttribute.PersianName("ProductCardexNewPrintMultiVahed_2Re")]
	ProductCardexNewPrintMultiVahed_2Re = 113,
	[PersianNameAttribute.PersianName("گزارش حساب معین در تفصیلی")]
	Moein2Tafsils = 114,
	[PersianNameAttribute.PersianName("گزارش حساب تفصیلی در معین")]
	Tafsil2Moeins = 115,
	[PersianNameAttribute.PersianName("TajmiTafsili")]
	TajmiTafsili = 116,
	[PersianNameAttribute.PersianName("دفتر روزنامه در سطح کل")]
	DaftarRoznameKol = 117,
	[PersianNameAttribute.PersianName("لیست توزیع")]
	Toziee = 118,
	[PersianNameAttribute.PersianName("انبارگردانی")]
	AnbarGardani = 119,
	[PersianNameAttribute.PersianName("گروه تفصیلی")]
	TafsilLevel = 120,
	[PersianNameAttribute.PersianName("TafsilMoeins")]
	TafsilMoeins = 121,
	[PersianNameAttribute.PersianName("HesabList")]
	HesabList = 122,
	[PersianNameAttribute.PersianName("گردش تفصیلی")]
	GardeshTafsil = 123,
	[PersianNameAttribute.PersianName("گردش تجمیعی")]
	GardeshTajmiee = 124,
	[PersianNameAttribute.PersianName("Check")]
	Check = 125,
	[PersianNameAttribute.PersianName("CheckM")]
	CheckM = 126,
	[PersianNameAttribute.PersianName("CheckSh")]
	CheckSh = 127,
	[PersianNameAttribute.PersianName("یادداشت")]
	NoteOne = 128,
	[PersianNameAttribute.PersianName("حساب های تفصیلی (لیست)")]
	TafsilList = 129,
	[PersianNameAttribute.PersianName("حساب های معین (لیست)")]
	MoeinList = 130,
	[PersianNameAttribute.PersianName("حساب های کل (لیست)")]
	KolList = 131,
	[PersianNameAttribute.PersianName("ProductCatalog4")]
	ProductCatalog4 = 132,
	[PersianNameAttribute.PersianName("فاکتور امانی دیگران نزد ما (تکی)")]
	AmaniVarede = 133,
	[PersianNameAttribute.PersianName("فاکتور امانی ما نزد دیگران (تکی)")]
	AmaniSadere = 134,
	[PersianNameAttribute.PersianName("فاکتور امانی (لیست)")]
	AmaniList = 135,
	[PersianNameAttribute.PersianName("گردش ارزش اشخاص")]
	TafsilCurrencyGardesh = 136,
	[PersianNameAttribute.PersianName("کیوسک سفارش گیر")]
	Kiosk = 137,
	[PersianNameAttribute.PersianName("چاپ سند حسابداری در سطح کل")]
	SanadKol = 138,
	[PersianNameAttribute.PersianName("چاپ سند حسابداری در سطح معین")]
	SanadMoein = 139,
	[PersianNameAttribute.PersianName("چاپ سند حسابداری به تجمیع کل")]
	SanadTajmieeKol = 140,
	[PersianNameAttribute.PersianName("اسناد و اقساط در راه فاکتور")]
	OrderChequeQest = 141,
	[PersianNameAttribute.PersianName("آخرین تراکنش مشتریان")]
	CustomerLastTransaction = 142,
	[PersianNameAttribute.PersianName("درخواست خرید (تکی)")]
	OrderPreBuy = 143,
	[PersianNameAttribute.PersianName("فرمول تولید")]
	TolidFormula = 144,
	[PersianNameAttribute.PersianName("ریز عملکرد کالا")]
	ProductCardexSubDetails = 145,
	[PersianNameAttribute.PersianName("دفتر کل تجمیعی")]
	SumKol = 146,
	[PersianNameAttribute.PersianName("تجمیع درخواست خرید")]
	RptprdSumPreBuy = 147,
	[PersianNameAttribute.PersianName("تجمیع امانی دیگران نزد ما")]
	RptprdSumAmaniVorod = 148,
	[PersianNameAttribute.PersianName("تجمیع امانی ما نزد دیگران")]
	RptprdSumAmaniKhoroj = 149,
	[PersianNameAttribute.PersianName("چاپ برچسب پستی")]
	PostalTag = 150,
	[PersianNameAttribute.PersianName("یادداشت(لیست)")]
	NoteList = 151,
	[PersianNameAttribute.PersianName("گروه حساب")]
	HesabGroup = 152,
	[PersianNameAttribute.PersianName("چک پرینت")]
	CheckPrint = 153,
	[PersianNameAttribute.PersianName("سریال MPL24")]
	Serial_Mpl24 = 154,
	[PersianNameAttribute.PersianName("سریال MPL30")]
	Serial_Mpl30 = 155,
	[PersianNameAttribute.PersianName("سریال MPL65")]
	Serial_Mpl65 = 156,
	[PersianNameAttribute.PersianName("بارکد کوپن MPL24")]
	CouponBarcode_Mpl24 = 157,
	[PersianNameAttribute.PersianName("بارکد کوپن MPL30")]
	CouponBarcode_Mpl30 = 158,
	[PersianNameAttribute.PersianName("بارکد کوپن MPL65")]
	CouponBarcode_Mpl65 = 159,
	[PersianNameAttribute.PersianName("رسید انبار - تکی")]
	DepotReceipt_One = 160,
	[PersianNameAttribute.PersianName("حواله انبار - تکی")]
	DepotHavale_One = 161,
	[PersianNameAttribute.PersianName("رسید انبار - لیست")]
	DepotReceipt_List = 162,
	[PersianNameAttribute.PersianName("حواله انبار - لیست")]
	DepotHavale_List = 163,
	[PersianNameAttribute.PersianName("سرنخ های فروش - لیست")]
	SarNakh_List = 164,
	[PersianNameAttribute.PersianName("گزارش نقدینگی بانک ها")]
	BankFinance = 165,
	[PersianNameAttribute.PersianName("گزارش صورت حساب سامانه مودیان")]
	TaxOrderLog = 166,
	[PersianNameAttribute.PersianName("گزارش بدهی و طلب")]
	BedehiTalab = 167,
	[PersianNameAttribute.PersianName("گزارش عملکرد اشخاص")]
	CustomerPerformance = 168,
	[PersianNameAttribute.PersianName("گزارش اقساط دارای تاخیر")]
	DellayedInstallments = 169,
	[PersianNameAttribute.PersianName("گزارش عملکرد کاربران")]
	CashierLog = 170,
	[PersianNameAttribute.PersianName("لیست بانک ها")]
	BankList = 171,
	[PersianNameAttribute.PersianName("لیست کوپن ها")]
	CouponList = 172,
	[PersianNameAttribute.PersianName("تجمیع کالاها به تفکیک تاریخ")]
	AggregateProductSum = 173,
	[PersianNameAttribute.PersianName("سریال قفل نوع 1")]
	LkSerialType1 = 174,
	[PersianNameAttribute.PersianName("سریال قفل نوع 2")]
	LkSerialType2 = 175,
	[PersianNameAttribute.PersianName("سریال قفل نوع 3")]
	LkSerialType3 = 176,
	[PersianNameAttribute.PersianName("دفتر روزنامه ویژه دفاتر الکترونیک")]
	RoznameElectronic = 177,
	[PersianNameAttribute.PersianName("لیست توزیع")]
	TozieeList = 178,
	[PersianNameAttribute.PersianName("گزارش دوره ویزیت مشتریان")]
	VisitPriod = 179,
	[PersianNameAttribute.PersianName("بیومتریک")]
	BioMetric = 999
}
