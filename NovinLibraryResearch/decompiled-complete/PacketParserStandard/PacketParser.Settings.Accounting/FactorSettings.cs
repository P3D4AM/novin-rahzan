using System;
using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class FactorSettings
{
	public static string FishNumberLastResetTimeKey { get; } = "FishNumberLastResetTime";


	public ReturnedState Sell_LowerThanBahayeTamamShodeh { get; set; } = ReturnedState.Warning;


	public bool ShowBuyHazine { get; set; }

	public bool DefSell_Resturan { get; set; }

	[DefaultValue(true)]
	public bool Sell_Resturan_ReceptionNaghdForm { get; set; } = true;


	public bool Sell_Resturan_ShowMaliatCol { get; set; }

	public bool Show_BazaryabForm { get; set; }

	public bool Enable_BazaryabZarib { get; set; }

	public bool NewFactorAfterSave { get; set; }

	public bool PRDJozee { get; set; }

	public bool ViewBankNumber { get; set; }

	[DefaultValue(ReturnedState.Warning)]
	public ReturnedState Sell_LowerThanBahayeTamamShodeh_Med { get; set; } = ReturnedState.Warning;


	public ReturnedState PrdTekrari { get; set; } = ReturnedState.Warning;


	public bool NewSerial { get; set; }

	[DefaultValue(true)]
	public bool ShowTasviyeForm { get; set; } = true;


	[DefaultValue(true)]
	public bool FactorFotroshgahiShowKala { get; set; } = true;


	public bool FactorFotroshgahiSearchCust { get; set; }

	public decimal KasreRial { get; set; }

	public decimal AddHazine { get; set; }

	public bool KasrRialAuto { get; set; }

	public bool AddHazineAuto { get; set; }

	public bool Tasviye { get; set; }

	[DefaultValue(10)]
	public int TasviyeDay { get; set; } = 10;


	[DefaultValue(3)]
	public int TasviyeYadavari { get; set; } = 3;


	[DefaultValue(1)]
	[Obsolete("", true)]
	public int SellType { get; set; } = 1;


	public Guid? SellTypeGuid { get; set; }

	public bool Res_Enable { get; set; }

	[DefaultValue("پیک")]
	public string Res_String1Title { get; set; } = "پیک";


	[DefaultValue("سالن")]
	public string Res_String2Title { get; set; } = "سالن";


	[DefaultValue("بیرون")]
	public string Res_String3Title { get; set; } = "بیرون";


	[DefaultValue(true)]
	public bool Res_Tajmi { get; set; } = true;


	[DefaultValue(400)]
	public int PRDWidth { get; set; } = 400;


	[DefaultValue(1)]
	public int ReceptionDef { get; set; } = 1;


	public bool BackSell2Sell { get; set; }

	public bool LotteryEnable { get; set; }

	[DefaultValue(typeof(decimal), "-1")]
	public decimal LotteryPrice { get; set; } = -1m;


	[DefaultValue(typeof(decimal), "-1")]
	public decimal LotteryReward { get; set; } = -1m;


	public bool LotteryRound { get; set; }

	public bool SayerVisible { get; set; }

	public bool BazaryabVisible { get; set; }

	public bool DiscountVisible { get; set; }

	[DefaultValue(true)]
	public bool PicMojodi { get; set; } = true;


	public bool Bazaryab2Visible { get; set; }

	public bool MaliatVisible { get; set; }

	[DefaultValue(true)]
	public bool Service { get; set; } = true;


	[DefaultValue(true)]
	public bool Product { get; set; } = true;


	[DefaultValue(1)]
	public double Percent { get; set; } = 1.0;


	[DefaultValue(false)]
	public bool ForceCreditLimit { get; set; }

	public bool InsertPRDQT { get; set; }

	public bool ViewPRDForm { get; set; }

	[DefaultValue(false)]
	public bool FactorFotroshgahiShowIsChecked { get; set; }

	[DefaultValue(ReturnedState.Warning)]
	public ReturnedState Outstanding_Order { get; set; } = ReturnedState.Warning;


	[DefaultValue(ReturnedState.None)]
	public ReturnedState BackSell_WithoutDesc { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Sell_WithoutDesc { get; set; }

	[DefaultValue(false)]
	public bool InvoiceOrderIntroductionMethod { get; set; }

	[DefaultValue(false)]
	public bool OrderSellIntroductionMethod { get; set; }

	[DefaultValue(false)]
	public bool OrderSellShowOtherInformations { get; set; }

	public List<TimeSpan> FishNoResetTimeList { get; set; }

	[DefaultValue(1)]
	public int FishNoStartIndex { get; set; } = 1;


	public Guid ExcessServiceProductGuid { get; set; }

	public bool ExcessServiceStaticPrice { get; set; }

	public bool ExcessServiceDynamicPrice { get; set; }

	[DefaultValue(true)]
	public bool ShowOrderShortCutKey { get; set; } = true;


	[DefaultValue(5000)]
	public decimal VolumetricWeight { get; set; } = 5000m;


	[DefaultValue(false)]
	public bool IsTaxImpactOnCost { get; set; }
}
