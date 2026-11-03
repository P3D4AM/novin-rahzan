using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class ProductSettings
{
	private string sell1Name;

	private string sell2Name;

	private string sell3Name;

	private string sell4Name;

	private string sell5Name;

	private string sell6Name;

	private string sell7Name;

	private string sell8Name;

	private string buy1Name;

	private string buy2Name;

	private string buy3Name;

	private string technicalInfo1Name;

	private string technicalInfo2Name;

	private string technicalInfo3Name;

	private string technicalInfo4Name;

	private string technicalInfo5Name;

	private string technicalInfo6Name;

	private string technicalInfo7Name;

	private string technicalInfo8Name;

	private string serial_caption;

	private List<ProductsSpecialSettings> customListPrdForAndroid;

	private List<ProductsSpecialSettings> customList;

	private EnPriceType pRDSite_DefPrice;

	private bool loadBarcodeInsteadOfCode;

	[Obsolete("moved to WebUser", true)]
	public string PRD_MessageNoPrice { get; set; } = "";


	[Obsolete("moved to WebUser", true)]
	public bool PRDSite_JustMojood { get; set; }

	public List<ProductsSpecialSettings> CustomListPrdForAndroid
	{
		get
		{
			return customListPrdForAndroid?.ToList();
		}
		set
		{
			customListPrdForAndroid = value;
		}
	}

	public List<ProductsSpecialSettings> CustomList
	{
		get
		{
			return customList?.ToList();
		}
		set
		{
			customList = value;
		}
	}

	[DefaultValue(EnSuggestionType.FillGaps)]
	public EnSuggestionType SuggestionType { get; set; } = EnSuggestionType.FillGaps;


	[DefaultValue(0)]
	[Obsolete("Use RegularPrice in Product", true)]
	public EnPriceType PRDSite_DefPrice
	{
		get
		{
			try
			{
				if (WebsitePriceDiscounted == null)
				{
					WebsitePriceDiscount websitePriceDiscount2 = (WebsitePriceDiscounted = new WebsitePriceDiscount());
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PRDSite_DefPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\ProductSettings.cs", 37);
			}
			return pRDSite_DefPrice;
		}
		set
		{
			try
			{
				pRDSite_DefPrice = value;
				if (WebsitePriceDiscounted == null)
				{
					WebsitePriceDiscounted = new WebsitePriceDiscount();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PRDSite_DefPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\ProductSettings.cs", 48);
			}
		}
	}

	[Obsolete]
	[DefaultValue(typeof(decimal), "1")]
	public decimal Sell_Discount_1 { get; set; } = 1m;


	[Obsolete]
	[DefaultValue(typeof(decimal), "1")]
	public decimal Sell_Discount_2 { get; set; } = 1m;


	[Obsolete]
	[DefaultValue(typeof(decimal), "1")]
	public decimal Sell_Discount_3 { get; set; } = 1m;


	[Obsolete]
	[DefaultValue(typeof(decimal), "1")]
	public decimal Sell_Discount_4 { get; set; } = 1m;


	[Obsolete]
	[DefaultValue(typeof(decimal), "1")]
	public decimal Sell_Discount_5 { get; set; } = 1m;


	[Obsolete]
	[DefaultValue(typeof(decimal), "1")]
	public decimal Sell_Discount_6 { get; set; } = 1m;


	[Obsolete]
	[DefaultValue(typeof(decimal), "1")]
	public decimal Sell_Discount_7 { get; set; } = 1m;


	[Obsolete]
	[DefaultValue(typeof(decimal), "1")]
	public decimal Sell_Discount_8 { get; set; } = 1m;


	public string TechnicalInfo1Name
	{
		get
		{
			if (!string.IsNullOrEmpty(technicalInfo1Name))
			{
				return technicalInfo1Name;
			}
			return "مشخصه فنی یک";
		}
		set
		{
			technicalInfo1Name = value;
		}
	}

	public string TechnicalInfo2Name
	{
		get
		{
			if (!string.IsNullOrEmpty(technicalInfo2Name))
			{
				return technicalInfo2Name;
			}
			return "مشخصه فنی دو";
		}
		set
		{
			technicalInfo2Name = value;
		}
	}

	public string TechnicalInfo3Name
	{
		get
		{
			if (!string.IsNullOrEmpty(technicalInfo3Name))
			{
				return technicalInfo3Name;
			}
			return "مشخصه فنی سه";
		}
		set
		{
			technicalInfo3Name = value;
		}
	}

	public string TechnicalInfo4Name
	{
		get
		{
			if (!string.IsNullOrEmpty(technicalInfo4Name))
			{
				return technicalInfo4Name;
			}
			return "مشخصه فنی چهار";
		}
		set
		{
			technicalInfo4Name = value;
		}
	}

	public string TechnicalInfo5Name
	{
		get
		{
			if (!string.IsNullOrEmpty(technicalInfo5Name))
			{
				return technicalInfo5Name;
			}
			return "مشخصه فنی پنج";
		}
		set
		{
			technicalInfo5Name = value;
		}
	}

	public string TechnicalInfo6Name
	{
		get
		{
			if (!string.IsNullOrEmpty(technicalInfo6Name))
			{
				return technicalInfo6Name;
			}
			return "مشخصه فنی شش";
		}
		set
		{
			technicalInfo6Name = value;
		}
	}

	public string TechnicalInfo7Name
	{
		get
		{
			if (!string.IsNullOrEmpty(technicalInfo7Name))
			{
				return technicalInfo7Name;
			}
			return "مشخصه فنی هفت";
		}
		set
		{
			technicalInfo7Name = value;
		}
	}

	public string TechnicalInfo8Name
	{
		get
		{
			if (!string.IsNullOrEmpty(technicalInfo8Name))
			{
				return technicalInfo8Name;
			}
			return "مشخصه فنی هشت";
		}
		set
		{
			technicalInfo8Name = value;
		}
	}

	[Obsolete]
	public string Sell1Name
	{
		get
		{
			if (!string.IsNullOrEmpty(sell1Name))
			{
				return sell1Name;
			}
			return "خرده فروشی".GetTranslate();
		}
		set
		{
			sell1Name = value;
		}
	}

	[Obsolete]
	public string Sell2Name
	{
		get
		{
			if (!string.IsNullOrEmpty(sell2Name))
			{
				return sell2Name;
			}
			return "جزیی".GetTranslate();
		}
		set
		{
			sell2Name = value;
		}
	}

	[Obsolete]
	public string Sell3Name
	{
		get
		{
			if (!string.IsNullOrEmpty(sell3Name))
			{
				return sell3Name;
			}
			return "کلی".GetTranslate();
		}
		set
		{
			sell3Name = value;
		}
	}

	[Obsolete]
	public string Sell4Name
	{
		get
		{
			if (!string.IsNullOrEmpty(sell4Name))
			{
				return sell4Name;
			}
			return "همکار درجه1".GetTranslate();
		}
		set
		{
			sell4Name = value;
		}
	}

	[Obsolete]
	public string Sell5Name
	{
		get
		{
			if (!string.IsNullOrEmpty(sell5Name))
			{
				return sell5Name;
			}
			return "همکار درجه2".GetTranslate();
		}
		set
		{
			sell5Name = value;
		}
	}

	[Obsolete]
	public string Sell6Name
	{
		get
		{
			if (!string.IsNullOrEmpty(sell6Name))
			{
				return sell6Name;
			}
			return "همکار درجه3".GetTranslate();
		}
		set
		{
			sell6Name = value;
		}
	}

	[Obsolete]
	public string Sell7Name
	{
		get
		{
			if (!string.IsNullOrEmpty(sell7Name))
			{
				return sell7Name;
			}
			return "فصلی".GetTranslate();
		}
		set
		{
			sell7Name = value;
		}
	}

	[Obsolete]
	public string Sell8Name
	{
		get
		{
			if (!string.IsNullOrEmpty(sell8Name))
			{
				return sell8Name;
			}
			return "نمایشگاه".GetTranslate();
		}
		set
		{
			sell8Name = value;
		}
	}

	[Obsolete]
	public string Buy1Name
	{
		get
		{
			if (!string.IsNullOrEmpty(buy1Name))
			{
				return buy1Name;
			}
			return "نقدی".GetTranslate();
		}
		set
		{
			buy1Name = value;
		}
	}

	[Obsolete]
	public string Buy2Name
	{
		get
		{
			if (!string.IsNullOrEmpty(buy2Name))
			{
				return buy2Name;
			}
			return "یک ماهه".GetTranslate();
		}
		set
		{
			buy2Name = value;
		}
	}

	[Obsolete]
	public string Buy3Name
	{
		get
		{
			if (!string.IsNullOrEmpty(buy3Name))
			{
				return buy3Name;
			}
			return "سه ماهه".GetTranslate();
		}
		set
		{
			buy3Name = value;
		}
	}

	public BarCodeSettings BarcodeSettings { get; set; } = new BarCodeSettings();


	public string Serial_Caption
	{
		get
		{
			if (!string.IsNullOrEmpty(serial_caption))
			{
				return serial_caption;
			}
			return "سریال".GetTranslate();
		}
		set
		{
			serial_caption = value;
		}
	}

	[Obsolete]
	public bool Sell1PRDGrid { get; set; } = true;


	[Obsolete]
	public bool Sell2PRDGrid { get; set; } = true;


	[Obsolete]
	public bool Sell3PRDGrid { get; set; }

	[Obsolete]
	public bool Sell4PRDGrid { get; set; }

	[Obsolete]
	public bool Sell5PRDGrid { get; set; }

	[Obsolete]
	public bool Sell6PRDGrid { get; set; }

	[Obsolete]
	public bool Sell7PRDGrid { get; set; }

	[Obsolete]
	public bool Sell8PRDGrid { get; set; }

	public bool Sell1PRDFactor { get; set; }

	public bool Sell2PRDFactor { get; set; }

	[DefaultValue(true)]
	public bool Sell1AnbarGrid { get; set; } = true;


	[DefaultValue(true)]
	public bool Sell2AnbarGrid { get; set; } = true;


	public bool Sell3AnbarGrid { get; set; }

	public bool Sell4AnbarGrid { get; set; }

	public bool Sell5AnbarGrid { get; set; }

	public bool Sell6AnbarGrid { get; set; }

	public bool Sell7AnbarGrid { get; set; }

	public bool Sell8AnbarGrid { get; set; }

	public bool Buy1ShowPRDGrid { get; set; }

	public bool Buy2ShowPRDGrid { get; set; }

	public bool Buy3ShowPRDGrid { get; set; }

	public bool T1Enable { get; set; }

	public bool T2Enable { get; set; }

	public bool T3Enable { get; set; }

	public bool T4Enable { get; set; }

	public bool T5Enable { get; set; }

	public bool T6Enable { get; set; }

	public bool T7Enable { get; set; }

	public bool T8Enable { get; set; }

	public bool TSEnable { get; set; }

	public bool TSShow { get; set; }

	[Obsolete("", true)]
	public bool TSAutoTakmil { get; set; }

	[DefaultValue(true)]
	public bool PicEnable { get; set; } = true;


	[DefaultValue(true)]
	public bool FullScreen { get; set; } = true;


	[DefaultValue(800)]
	public short ProductsMainW { get; set; } = 800;


	[DefaultValue(600)]
	public short ProductsMainH { get; set; } = 600;


	public bool PRDCode { get; set; }

	public bool AutoSellAfterBuy { get; set; }

	public bool ChandVahedi { get; set; }

	[Obsolete("", true)]
	public int ProductsMainTop { get; set; }

	[DefaultValue("مدل]")]
	public string ResidT1Name { get; set; } = "[مدل]".GetTranslate();


	[DefaultValue("رنگ")]
	public string ResidT2Name { get; set; } = "رنگ".GetTranslate();


	[DefaultValue("سریال")]
	public string ResidT3Name { get; set; } = "[سریال]".GetTranslate();


	[DefaultValue("معایب")]
	public string ResidT4Name { get; set; } = "[معایب]".GetTranslate();


	[DefaultValue("درخواست ها")]
	public string ResidT5Name { get; set; } = "[درخواست ها]".GetTranslate();


	[Obsolete("moved to WebUser", true)]
	public WebsitePriceDiscount WebsitePriceDiscounted { get; set; } = new WebsitePriceDiscount();


	public ProductReportSettings Report { get; set; } = new ProductReportSettings();


	public ProductSerialSettings Serial { get; set; } = new ProductSerialSettings();


	public ProductDisplaySettings Display { get; set; } = new ProductDisplaySettings();


	[DefaultValue(true)]
	public bool SaveLastInterfaceSettings { get; set; } = true;


	[DefaultValue(true)]
	public bool PriceAndRPTLeft { get; set; } = true;


	public Guid DefaultProductGuid { get; set; }

	public Guid? DefaultGroupGuid { get; set; }

	public Guid? DefaultUnitGuid { get; set; }

	[DefaultValue(false)]
	public bool LoadBarcodeInsteadOfCode
	{
		get
		{
			return loadBarcodeInsteadOfCode;
		}
		set
		{
			loadBarcodeInsteadOfCode = value;
		}
	}

	public Guid? DefaultFeeProductGuid { get; set; }

	public Guid? DefaultProfitProductGuid { get; set; }

	[DefaultValue(100)]
	public int CountForIndexing { get; set; } = 100;


	[DefaultValue(true)]
	public bool ShowProductGroupNameInList { get; set; } = true;


	public bool DefaultSpecial { get; set; }

	[DefaultValue(true)]
	public bool DefaultAvailableSellToOnlineShop { get; set; } = true;


	public decimal Sell_Discount(EnPriceType level)
	{
		using (new JaegerMonitor("Sell_Discount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\ProductSettings.cs"))
		{
			try
			{
				return level switch
				{
					EnPriceType.sell2 => Sell_Discount_2, 
					EnPriceType.sell3 => Sell_Discount_3, 
					EnPriceType.sell4 => Sell_Discount_4, 
					EnPriceType.sell5 => Sell_Discount_5, 
					EnPriceType.sell6 => Sell_Discount_6, 
					EnPriceType.sell7 => Sell_Discount_7, 
					EnPriceType.sell8 => Sell_Discount_8, 
					_ => Sell_Discount_1, 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sell_Discount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\ProductSettings.cs", 140);
				return 0m;
			}
		}
	}
}
