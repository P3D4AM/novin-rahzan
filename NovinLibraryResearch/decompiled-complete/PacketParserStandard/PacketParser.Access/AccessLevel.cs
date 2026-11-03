using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PacketParser.Access.Orders;
using PacketParser.Access.Ticket;
using PacketParser.Entities;

namespace PacketParser.Access;

public class AccessLevel : Serializable<AccessLevel>
{
	private List<short> tafsilGroupList;

	[PersianNameAttribute.PersianName("دفتر اقساط")]
	public AghsatAccess Aghsat { get; set; } = new AghsatAccess();


	[PersianNameAttribute.PersianName("فاکتور امانی ما نزد دیگران")]
	public AmaniSadereAccess AmaniSadere { get; set; } = new AmaniSadereAccess();


	[PersianNameAttribute.PersianName("فاکتور امانی دیگران نزد ما")]
	public AmaniVaredeAccess AmaniVarede { get; set; } = new AmaniVaredeAccess();


	[PersianNameAttribute.PersianName("بارکد")]
	public BarCodeAccess Barcode { get; set; } = new BarCodeAccess();


	[PersianNameAttribute.PersianName("انبارگردانی")]
	public AnbarGardaniAccess AnbarGardani { get; set; } = new AnbarGardaniAccess();


	[PersianNameAttribute.PersianName("فاکتور برگشت از خرید")]
	public BackBuyAccess BackBuy { get; set; } = new BackBuyAccess();


	[PersianNameAttribute.PersianName("فاکتور برگشت از فروش")]
	public BackSellAccess BackSell { get; set; } = new BackSellAccess();


	[PersianNameAttribute.PersianName("بانک")]
	public BankAccess Bank { get; set; } = new BankAccess();


	[PersianNameAttribute.PersianName("برداشت")]
	public BardashtAccess Bardasht { get; set; } = new BardashtAccess();


	[PersianNameAttribute.PersianName("فاکتور خرید")]
	public BuyAccess Buy { get; set; } = new BuyAccess();


	[PersianNameAttribute.PersianName("عملیات چک")]
	public CheckAccess Check { get; set; } = new CheckAccess();


	[PersianNameAttribute.PersianName("اشخاص")]
	public CustomerAccess Customers { get; set; } = new CustomerAccess();


	[PersianNameAttribute.PersianName("دسته چک")]
	public DasteCheckAccess DasteCheck { get; set; } = new DasteCheckAccess();


	[PersianNameAttribute.PersianName("انبار")]
	public DepotAccess Depot { get; set; } = new DepotAccess();


	[PersianNameAttribute.PersianName("امکانات")]
	public EmkanatAccess Emkanat { get; set; } = new EmkanatAccess();


	[PersianNameAttribute.PersianName("فروشگاه اینترنتی من")]
	public ForoshgahInternetiAccess ForoshgahInterneti { get; set; } = new ForoshgahInternetiAccess();


	[PersianNameAttribute.PersianName("Crm")]
	public CrmAccess Crm { get; set; } = new CrmAccess();


	[PersianNameAttribute.PersianName("گزارشات پویا")]
	public Gozaresh_PoyaAccess Gozaresh_Poya { get; set; } = new Gozaresh_PoyaAccess();


	[PersianNameAttribute.PersianName("حواله بین انبارها")]
	public HavaleBeyneAnbarAccess HavaleBeyneAnbar { get; set; } = new HavaleBeyneAnbarAccess();


	[PersianNameAttribute.PersianName("هزینه")]
	public HazineAccess Hazine { get; set; } = new HazineAccess();


	[PersianNameAttribute.PersianName("کدینگ حسابها")]
	public HesabAccess Hesab { get; set; } = new HesabAccess();


	[PersianNameAttribute.PersianName("گروه تفصیلی")]
	public TafsilLevelAcess TafsilLevel { get; set; } = new TafsilLevelAcess();


	[PersianNameAttribute.PersianName("سند حسابداری")]
	public ManualSanadAccess ManualSanad { get; set; } = new ManualSanadAccess();


	[PersianNameAttribute.PersianName("سند پرداخت")]
	public PardakhtAccess Pardakht { get; set; } = new PardakhtAccess();


	[PersianNameAttribute.PersianName("پیامک")]
	public PayamakAccess Payamak { get; set; } = new PayamakAccess();


	[PersianNameAttribute.PersianName("پیش فاکتور فروش")]
	public PishFactor PishFacotr { get; set; } = new PishFactor();


	[PersianNameAttribute.PersianName("درخواست خرید")]
	public PreBuyAccess PreBuy { get; set; } = new PreBuyAccess();


	[PersianNameAttribute.PersianName("کالاها")]
	public ProductAccess Product { get; set; } = new ProductAccess();


	[PersianNameAttribute.PersianName("گروه کالاها")]
	public ProductGroupAccess ProductGroup { get; set; } = new ProductGroupAccess();


	[PersianNameAttribute.PersianName("سند دریافت")]
	public ReceptionAccess Reception { get; set; } = new ReceptionAccess();


	[PersianNameAttribute.PersianName("گزارشات حسابداری")]
	public ReportsAccess Reports { get; set; } = new ReportsAccess();


	[PersianNameAttribute.PersianName("رسید تحویل کالا")]
	public ResidAccess Resid { get; set; } = new ResidAccess();


	[PersianNameAttribute.PersianName("صندوق")]
	public SandoghAccess Sandogh { get; set; } = new SandoghAccess();


	[PersianNameAttribute.PersianName("فاکتور فروش")]
	public SellAccess Sell { get; set; } = new SellAccess();


	[PersianNameAttribute.PersianName("خدمات")]
	public ServicesAccess Services { get; set; } = new ServicesAccess();


	[PersianNameAttribute.PersianName("رسید تولید")]
	public TolidAccess Tolid { get; set; } = new TolidAccess();


	[PersianNameAttribute.PersianName("ابزارها")]
	public ToolsAccess Tools { get; set; } = new ToolsAccess();


	[PersianNameAttribute.PersianName("کاربران")]
	public UserAccess User { get; set; } = new UserAccess();


	[PersianNameAttribute.PersianName("فاکتور ضایعات")]
	public ZayeatAccess Zayeat { get; set; } = new ZayeatAccess();


	[PersianNameAttribute.PersianName("ارزها")]
	public DecimalAccess Decimal { get; set; } = new DecimalAccess();


	[PersianNameAttribute.PersianName("ویزیتور و تور")]
	public VisitAccess Visit { get; set; } = new VisitAccess();


	[PersianNameAttribute.PersianName("وضعیت اسناد")]
	public SanadStatusReportAccess SanadStatus { get; set; } = new SanadStatusReportAccess();


	[PersianNameAttribute.PersianName("کارمزد های بانکی")]
	public WageAccess Wage { get; set; } = new WageAccess();


	[PersianNameAttribute.PersianName("نقش اشخاص")]
	public CustomerRoleAccess CustomerRole { get; set; } = new CustomerRoleAccess();


	[PersianNameAttribute.PersianName("رسید ورود به انبار")]
	public DepotReceiptAccess DepotReceipt { get; set; } = new DepotReceiptAccess();


	[PersianNameAttribute.PersianName("حواله خروج از انبار")]
	public DepotHavaleAccess DepotHavale { get; set; } = new DepotHavaleAccess();


	[PersianNameAttribute.PersianName("تنوع ها و ویژگی های کالا")]
	public ProductVariationAccess ProductVariation { get; set; } = new ProductVariationAccess();


	[PersianNameAttribute.PersianName("سامانه مودیان")]
	public TaxServiceAccess TaxService { get; set; } = new TaxServiceAccess();


	[PersianNameAttribute.PersianName("سرنخ های فروش")]
	public SarNakhAccess SarNakh { get; set; } = new SarNakhAccess();


	[PersianNameAttribute.PersianName("تیکت ها")]
	public TicketAccess TicketAccess { get; set; } = new TicketAccess();


	[PersianNameAttribute.PersianName("مالیات کالاها")]
	public ProductTaxAccess ProductTax { get; set; } = new ProductTaxAccess();


	[PersianNameAttribute.PersianName("کوپن تخفیف")]
	public CouponAccess Coupon { get; set; } = new CouponAccess();


	[PersianNameAttribute.PersianName("تخفیفات حجمی")]
	public DiscountAccess Discount { get; set; } = new DiscountAccess();


	[PersianNameAttribute.PersianName("تقویم مناسبت ها")]
	public CalendarAccess Calendar { get; set; } = new CalendarAccess();


	[PersianNameAttribute.PersianName("(اتاق-میز)فضاها")]
	public PlacementAccess Placement { get; set; } = new PlacementAccess();


	[PersianNameAttribute.PersianName("شعبه ها")]
	public BranchAccess Branch { get; set; } = new BranchAccess();


	[PersianNameAttribute.PersianName("سطوح قیمت کالا")]
	public ProductPriceLevelAccess ProductPriceLevel { get; set; } = new ProductPriceLevelAccess();


	[PersianNameAttribute.PersianName("تجدید ارزیابی")]
	public EvaluationAccess Evaluation { get; set; } = new EvaluationAccess();


	[PersianNameAttribute.PersianName("اشانتیون ها")]
	public GiftAccess Gift { get; set; } = new GiftAccess();


	[Obsolete("please use PrdGroupListGuid", false)]
	public List<short> PrdGroupList { get; set; }

	[Obsolete("Use TafsilGroupGuids", false)]
	public List<short> TafsilGroupList
	{
		get
		{
			return tafsilGroupList?.Distinct()?.ToList();
		}
		set
		{
			tafsilGroupList = value;
		}
	}

	public List<Guid> PrdGroupListGuid { get; set; } = new List<Guid>();


	public List<Guid> PrdPriceLevelListGuid { get; set; } = new List<Guid>();


	public List<Guid> TafsilGroupGuid { get; set; } = new List<Guid>();


	public List<Guid> TafsilGroupJustListGuid { get; set; } = new List<Guid>();


	public List<clsKardexAccess> KardexAccessList { get; set; } = new List<clsKardexAccess>();


	public bool DashboardAccess { get; set; }

	public bool CurrencyDashboardAccess { get; set; }

	public DecimalSettings DecimalSetting { get; set; } = new DecimalSettings();


	[DefaultValue(true)]
	public bool MainNotificationAccess { get; set; } = true;


	[DefaultValue(false)]
	public bool GoldAndCurrencyAccess { get; set; }

	[DefaultValue(ReturnedState.Warning)]
	public ReturnedState DisableProductInOrderSell { get; set; } = ReturnedState.Warning;

}
