namespace PacketParser.Settings.Accounting;

public class AccAutoDescriptionSettings
{
	private string _sellDescription;

	private string _sellDetailProductDescription;

	private string _sellDetailServiceDescription;

	private string _sellDiscountDescription;

	private string _sellFareDescription;

	private string _sellTaxDescription;

	private string _sellAvarezDescription;

	private string _sellV1Description;

	private string _sellV2Description;

	private string _sellV3Description;

	private string _sellV4Description;

	private string _buyDescription;

	private string _buyDetailDescription;

	private string _buyDiscountDescription;

	private string _buyFareDescription;

	private string _buyTaxDescription;

	private string _buyAvarezDescription;

	private string _buyHazine1Description;

	private string _buyHazine2Description;

	private string _buyHazine3Description;

	private string _buyHazine4Description;

	private string _buyHazine5Description;

	private string _backSellDescription;

	private string _backSellDetailDescription;

	private string _backSellDiscountDescription;

	private string _backSellTaxDescription;

	private string _backSellAvarezDescription;

	private string _backSellFareDescription;

	private string _backSellV1Description;

	private string _backSellV2Description;

	private string _backSellV3Description;

	private string _backSellV4Description;

	private string _backBuyDescription;

	private string _backBuyDetailDescription;

	private string _backBuyDiscountDescription;

	private string _backBuyTaxDescription;

	private string _backBuyAvarezDescription;

	private string _backBuyFareDescription;

	private string _receptionDescription;

	private string _receptionNaghdDescription;

	private string _receptionCheckDescription;

	private string _receptionHavaleDescription;

	private string _receptionEnteghalDescription;

	private string _pardakhtDescription;

	private string _pardakhtNaghdDescription;

	private string _pardakhtCheckMDescription;

	private string _pardakhtCheckShDescription;

	private string _pardakhtHavaleDescription;

	private string _pardakhtEnteghalDescription;

	private string _sanadDescription;

	private string _sanadDetailDescription;

	public string SellDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_sellDescription))
			{
				_sellDescription = "فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _sellDescription;
		}
		set
		{
			_sellDescription = value;
		}
	}

	public string SellDetailProductDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_sellDetailProductDescription))
			{
				_sellDetailProductDescription = "فروش شماره : ([شماره]) ([نام کالا] - [کد کالا]) [شرح فاکتور] [شرح سطر]";
			}
			return _sellDetailProductDescription;
		}
		set
		{
			_sellDetailProductDescription = value;
		}
	}

	public string SellDetailServiceDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_sellDetailServiceDescription))
			{
				_sellDetailServiceDescription = "فروش شماره : ([شماره]) ([نام سرویس] - [کد سرویس]) [شرح فاکتور] [شرح سطر]";
			}
			return _sellDetailServiceDescription;
		}
		set
		{
			_sellDetailServiceDescription = value;
		}
	}

	public string SellDiscountDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_sellDiscountDescription))
			{
				_sellDiscountDescription = "تخفیف فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _sellDiscountDescription;
		}
		set
		{
			_sellDiscountDescription = value;
		}
	}

	public string SellFareDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_sellFareDescription))
			{
				_sellFareDescription = "کرایه حمل فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _sellFareDescription;
		}
		set
		{
			_sellFareDescription = value;
		}
	}

	public string SellTaxDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_sellTaxDescription))
			{
				_sellTaxDescription = "ارزش افزوده فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _sellTaxDescription;
		}
		set
		{
			_sellTaxDescription = value;
		}
	}

	public string SellAvarezDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_sellAvarezDescription))
			{
				_sellAvarezDescription = "عوارض قانونی فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _sellAvarezDescription;
		}
		set
		{
			_sellAvarezDescription = value;
		}
	}

	public string SellVisitor1Description
	{
		get
		{
			if (string.IsNullOrEmpty(_sellV1Description))
			{
				_sellV1Description = "هزینه ویزیتوری فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _sellV1Description;
		}
		set
		{
			_sellV1Description = value;
		}
	}

	public string SellVisitor2Description
	{
		get
		{
			if (string.IsNullOrEmpty(_sellV2Description))
			{
				_sellV2Description = "هزینه ویزیتوری سطح 2 فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _sellV2Description;
		}
		set
		{
			_sellV2Description = value;
		}
	}

	public string SellVisitor3Description
	{
		get
		{
			if (string.IsNullOrEmpty(_sellV3Description))
			{
				_sellV3Description = "هزینه ویزیتوری سطح 3 فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _sellV3Description;
		}
		set
		{
			_sellV3Description = value;
		}
	}

	public string SellVisitor4Description
	{
		get
		{
			if (string.IsNullOrEmpty(_sellV4Description))
			{
				_sellV4Description = "هزینه ویزیتوری سطح 4 فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _sellV4Description;
		}
		set
		{
			_sellV4Description = value;
		}
	}

	public string BuyDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_buyDescription))
			{
				_buyDescription = "فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _buyDescription;
		}
		set
		{
			_buyDescription = value;
		}
	}

	public string BuyDetailDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_buyDetailDescription))
			{
				_buyDetailDescription = "خرید شماره : ([شماره]) ([نام کالا] - [کد کالا]) [شرح فاکتور] [شرح سطر])";
			}
			return _buyDetailDescription;
		}
		set
		{
			_buyDetailDescription = value;
		}
	}

	public string BuyDiscountDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_buyDiscountDescription))
			{
				_buyDiscountDescription = "تخفیف فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _buyDiscountDescription;
		}
		set
		{
			_buyDiscountDescription = value;
		}
	}

	public string BuyFareDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_buyDetailDescription))
			{
				_buyFareDescription = "کرایه حمل فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _buyFareDescription;
		}
		set
		{
			_buyFareDescription = value;
		}
	}

	public string BuyTaxDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_buyTaxDescription))
			{
				_buyTaxDescription = "ارزش افزوده فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _buyTaxDescription;
		}
		set
		{
			_buyTaxDescription = value;
		}
	}

	public string BuyAvarezDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_buyAvarezDescription))
			{
				_buyAvarezDescription = "عوارض قانونی فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _buyAvarezDescription;
		}
		set
		{
			_buyAvarezDescription = value;
		}
	}

	public string BuyHazine1Description
	{
		get
		{
			if (string.IsNullOrEmpty(_buyHazine1Description))
			{
				_buyHazine1Description = "هزینه فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _buyHazine1Description;
		}
		set
		{
			_buyHazine1Description = value;
		}
	}

	public string BuyHazine2Description
	{
		get
		{
			if (string.IsNullOrEmpty(_buyHazine2Description))
			{
				_buyHazine2Description = "هزینه فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _buyHazine2Description;
		}
		set
		{
			_buyHazine2Description = value;
		}
	}

	public string BuyHazine3Description
	{
		get
		{
			if (string.IsNullOrEmpty(_buyHazine3Description))
			{
				_buyHazine3Description = "هزینه فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _buyHazine3Description;
		}
		set
		{
			_buyHazine3Description = value;
		}
	}

	public string BuyHazine4Description
	{
		get
		{
			if (string.IsNullOrEmpty(_buyHazine4Description))
			{
				_buyHazine4Description = "هزینه فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _buyHazine4Description;
		}
		set
		{
			_buyHazine4Description = value;
		}
	}

	public string BuyHazine5Description
	{
		get
		{
			if (string.IsNullOrEmpty(_buyHazine5Description))
			{
				_buyHazine5Description = "هزینه فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _buyHazine5Description;
		}
		set
		{
			_buyHazine5Description = value;
		}
	}

	public string BackSellDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backSellDescription))
			{
				_backSellDescription = "فاکتور برگشت از فروش شماره : [شماره] [شرح فاکتور]";
			}
			return _backSellDescription;
		}
		set
		{
			_backSellDescription = value;
		}
	}

	public string BackSellDetailDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backSellDetailDescription))
			{
				_backSellDetailDescription = "برگشت از فروش شماره : [شماره] ([نام کالا] - [کد کالا]) [شرح فاکتور] [شرح سطر])";
			}
			return _backSellDetailDescription;
		}
		set
		{
			_backSellDetailDescription = value;
		}
	}

	public string BackSellDiscountDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backSellDiscountDescription))
			{
				_backSellDiscountDescription = "تخفیف فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backSellDiscountDescription;
		}
		set
		{
			_backSellDiscountDescription = value;
		}
	}

	public string BackSellTaxDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backSellTaxDescription))
			{
				_backSellTaxDescription = "ارزش افزوده فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backSellTaxDescription;
		}
		set
		{
			_backSellTaxDescription = value;
		}
	}

	public string BackSellAvarezDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backSellAvarezDescription))
			{
				_backSellAvarezDescription = "عوارض قانونی فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backSellAvarezDescription;
		}
		set
		{
			_backSellAvarezDescription = value;
		}
	}

	public string BackSellFareDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backSellFareDescription))
			{
				_backSellFareDescription = "کرایه حمل فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backSellFareDescription;
		}
		set
		{
			_backSellFareDescription = value;
		}
	}

	public string BackSellVisitor1Description
	{
		get
		{
			if (string.IsNullOrEmpty(_backSellV1Description))
			{
				_backSellV1Description = "هزینه ویزیتوری فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backSellV1Description;
		}
		set
		{
			_backSellV1Description = value;
		}
	}

	public string BackSellVisitor2Description
	{
		get
		{
			if (string.IsNullOrEmpty(_backSellV2Description))
			{
				_backSellV2Description = "هزینه ویزیتوری سطح 2 فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backSellV2Description;
		}
		set
		{
			_backSellV2Description = value;
		}
	}

	public string BackSellVisitor3Description
	{
		get
		{
			if (string.IsNullOrEmpty(_backSellV3Description))
			{
				_backSellV3Description = "هزینه ویزیتوری سطح 3 فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backSellV3Description;
		}
		set
		{
			_backSellV3Description = value;
		}
	}

	public string BackSellVisitor4Description
	{
		get
		{
			if (string.IsNullOrEmpty(_backSellV4Description))
			{
				_backSellV4Description = "هزینه ویزیتوری سطح 4 فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backSellV4Description;
		}
		set
		{
			_backSellV4Description = value;
		}
	}

	public string BackBuyDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backBuyDescription))
			{
				_backBuyDescription = "فاکتور برگشت از خرید شماره : [شماره] [شرح فاکتور]";
			}
			return _backBuyDescription;
		}
		set
		{
			_backBuyDescription = value;
		}
	}

	public string BackBuyDetailDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backBuyDetailDescription))
			{
				_backBuyDetailDescription = "برگشت از خرید شماره : [شماره] ([نام کالا] - [کد کالا]) [شرح فاکتور] [شرح سطر])";
			}
			return _backBuyDetailDescription;
		}
		set
		{
			_backBuyDetailDescription = value;
		}
	}

	public string BackBuyDiscountDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backBuyDiscountDescription))
			{
				_backBuyDiscountDescription = "تخفیف فاکتور برگشت از خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backBuyDiscountDescription;
		}
		set
		{
			_backBuyDiscountDescription = value;
		}
	}

	public string BackBuyFareDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backBuyFareDescription))
			{
				_backBuyFareDescription = "کرایه حمل فاکتور برگشت از خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backBuyFareDescription;
		}
		set
		{
			_backBuyFareDescription = value;
		}
	}

	public string BackBuyTaxDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backBuyTaxDescription))
			{
				_backBuyTaxDescription = "ارزش افزوده فاکتور برگشت از خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backBuyTaxDescription;
		}
		set
		{
			_backBuyTaxDescription = value;
		}
	}

	public string BackBuyAvarezDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_backBuyAvarezDescription))
			{
				_backBuyAvarezDescription = "عوارض قانونی فاکتور برگشت از خرید شماره : ([شماره]) [شرح فاکتور]";
			}
			return _backBuyAvarezDescription;
		}
		set
		{
			_backBuyAvarezDescription = value;
		}
	}

	public string ReceptionDescrption
	{
		get
		{
			if (string.IsNullOrEmpty(_receptionDescription))
			{
				_receptionDescription = "سند دریافت شماره : [شماره] [شرح دریافت]";
			}
			return _receptionDescription;
		}
		set
		{
			_receptionDescription = value;
		}
	}

	public string ReceptionNaghdDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_receptionNaghdDescription))
			{
				_receptionNaghdDescription = "دریافت نقدی طی برگه شماره : [شماره] بابت : [شرح دریافت] [شرح سطر]";
			}
			return _receptionNaghdDescription;
		}
		set
		{
			_receptionNaghdDescription = value;
		}
	}

	public string ReceptionCheckDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_receptionCheckDescription))
			{
				_receptionCheckDescription = "دریافت چک شماره : [شماره چک] [نام بانک] به تاریخ : [تاریخ چک] بابت : [شرح دریافت] [شرح سطر]";
			}
			return _receptionCheckDescription;
		}
		set
		{
			_receptionCheckDescription = value;
		}
	}

	public string ReceptionHavaleDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_receptionHavaleDescription))
			{
				_receptionHavaleDescription = "دریافت حواله شماره : [شماره حواله] بابت : [شرح دریافت] [شرح سطر]";
			}
			return _receptionHavaleDescription;
		}
		set
		{
			_receptionHavaleDescription = value;
		}
	}

	public string ReceptionEnteghalDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_receptionEnteghalDescription))
			{
				_receptionEnteghalDescription = " دریافت : ([شماره]) [شرح دریافت] [شرح سطر]";
			}
			return _receptionEnteghalDescription;
		}
		set
		{
			_receptionEnteghalDescription = value;
		}
	}

	public string PardakhtDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_pardakhtDescription))
			{
				_pardakhtDescription = "سند پرداخت شماره : [شماره] [شرح پرداخت]";
			}
			return _pardakhtDescription;
		}
		set
		{
			_pardakhtDescription = value;
		}
	}

	public string PardakhtNaghdDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_pardakhtNaghdDescription))
			{
				_pardakhtNaghdDescription = "پرداخت نقدی طی برگه شماره : [شماره] بابت : [شرح پرداخت] [شرح سطر]";
			}
			return _pardakhtNaghdDescription;
		}
		set
		{
			_pardakhtNaghdDescription = value;
		}
	}

	public string PardakhtCheckMDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_pardakhtCheckMDescription))
			{
				_pardakhtCheckMDescription = "پرداخت چک وارده شماره : [شماره چک] بابت: [شرح پرداخت] [شرح سطر]";
			}
			return _pardakhtCheckMDescription;
		}
		set
		{
			_pardakhtCheckMDescription = value;
		}
	}

	public string PardakhtCheckSHDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_pardakhtCheckShDescription))
			{
				_pardakhtCheckShDescription = "پرداخت چک شخصی شماره: [شماره چک] به تاریخ : [تاریخ چک] بابت [شرح پرداخت] [شرح سطر]";
			}
			return _pardakhtCheckShDescription;
		}
		set
		{
			_pardakhtCheckShDescription = value;
		}
	}

	public string PardakhtHavaleDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_pardakhtHavaleDescription))
			{
				_pardakhtHavaleDescription = "پرداخت حواله شماره : [شماره حواله] بابت : [شرح پرداخت] [شرح سطر]";
			}
			return _pardakhtHavaleDescription;
		}
		set
		{
			_pardakhtHavaleDescription = value;
		}
	}

	public string PardakhtEnteghalDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_pardakhtEnteghalDescription))
			{
				_pardakhtEnteghalDescription = " پرداخت : ([شماره]) [شرح پرداخت]";
			}
			return _pardakhtEnteghalDescription;
		}
		set
		{
			_pardakhtEnteghalDescription = value;
		}
	}

	public string SanadDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_sanadDescription))
			{
				_sanadDescription = "سند حسابداری شماره : [شماره] [شرح سند]";
			}
			return _sanadDescription;
		}
		set
		{
			_sanadDescription = value;
		}
	}

	public string SanadAghlamDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_sanadDetailDescription))
			{
				_sanadDetailDescription = "سند حسابداری شماره : [شماره] [شرح سطر] [شرح سند]";
			}
			return _sanadDetailDescription;
		}
		set
		{
			_sanadDetailDescription = value;
		}
	}
}
