using System;
using System.ComponentModel;
using PacketParser.Settings.Accounting.Orders.OrderDefaults;

namespace PacketParser.Settings.Accounting;

public class SellSettings : OrderDefaultSettingBase
{
	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomer { get; set; } = true;


	[DefaultValue(true)]
	public bool SelectDefMotefareghe { get; set; } = true;


	public bool FactorByCode { get; set; }

	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomerExceptionMotafareghe { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomerSumOfSell { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomerAvgSell3Month { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomerSumOfCheckBargashti { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomerCountOfCheckBargashti { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomerCountOfCheckPasNashode { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomerSumOfCheckPasShode { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomerSumOfCheckPasNaShode { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomerCountOfCheckPasShode { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomerDateAndLastSell { get; set; } = true;


	public bool SegestSellPriceQT { get; set; }

	public bool SegestSellPrice { get; set; }

	public decimal Sell1MinSegest { get; set; }

	public decimal Sell1MaxSegest { get; set; }

	public decimal Sell2MinSegest { get; set; }

	public decimal Sell2MaxSegest { get; set; }

	public decimal Sell3MinSegest { get; set; }

	public decimal Sell3MaxSegest { get; set; }

	public decimal Sell4MinSegest { get; set; }

	public decimal Sell4MaxSegest { get; set; }

	public decimal Sell5MinSegest { get; set; }

	public decimal Sell5MaxSegest { get; set; }

	public decimal Sell6MinSegest { get; set; }

	public decimal Sell6MaxSegest { get; set; }

	public decimal Sell7MinSegest { get; set; }

	public decimal Sell7MaxSegest { get; set; }

	public decimal Sell8MinSegest { get; set; }

	public decimal Sell8MaxSegest { get; set; }

	[DefaultValue(typeof(decimal), "1")]
	public decimal OrderSellDefaultQT { get; set; } = 1m;


	[Obsolete("", true)]
	public Guid? TaxTafsilGuid { get; set; }

	[Obsolete("", true)]
	public Guid? TaxMoeinGuid { get; set; }

	[DefaultValue(true)]
	public bool CustomerTaxEnable { get; set; } = true;


	[Obsolete("", true)]
	public Guid? AvarezTafsilGuid { get; set; }

	[Obsolete("", true)]
	public Guid? AvarezMoeinGuid { get; set; }

	public Guid? KerayeMoeinGuid { get; set; }

	public Guid? KerayeTafsilGuid { get; set; }

	[DefaultValue(true)]
	public bool Sell_Resturan_AlbaghiVajh { get; set; } = true;


	[DefaultValue(false)]
	public bool AddTolidAfterNegativeOrder { get; set; }

	[DefaultValue(false)]
	public bool AddOriginalPriceToOrder { get; set; }
}
