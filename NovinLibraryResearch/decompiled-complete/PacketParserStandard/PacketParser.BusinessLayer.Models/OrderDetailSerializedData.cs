using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class OrderDetailSerializedData
{
	public PercentageMode DiscountMode { get; set; }

	public PercentageMode Visitor1Mode { get; set; }

	public PercentageMode Visitor2Mode { get; set; }

	public PercentageMode Visitor3Mode { get; set; }

	public PercentageMode Visitor4Mode { get; set; }

	public PercentageMode TaxMode { get; set; }

	public PercentageMode AvarezMode { get; set; }

	public decimal FeePercent { get; set; }

	public EnFeeType FeeType { get; set; }

	public decimal ProfitPercent { get; set; }

	public EnFeeType ProfitType { get; set; }

	public decimal ProductTaxPercent { get; set; }

	public decimal ProductTaxPrice { get; set; }

	public decimal ProfitTaxPercent { get; set; }

	public decimal ProfitTaxPrice { get; set; }

	public decimal FeeTaxPercent { get; set; }

	public decimal FeeTaxPrice { get; set; }

	public decimal SellPrice { get; set; }

	public decimal NetWeight { get; set; }

	public decimal GrossWeight { get; set; }

	public decimal PackingLength { get; set; }

	public decimal PackingWidth { get; set; }

	public decimal PackingHeight { get; set; }
}
