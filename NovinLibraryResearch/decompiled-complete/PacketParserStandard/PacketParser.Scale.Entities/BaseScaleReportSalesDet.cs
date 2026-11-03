using System;
using Newtonsoft.Json;
using PacketParser.Scale.Entities.Interfaces.Reports;

namespace PacketParser.Scale.Entities;

public class BaseScaleReportSalesDet : IReportSaleDet
{
	public decimal AccountingDiscount { get; set; }

	public decimal AccountingTax { get; set; }

	public decimal AccountingTotalPrice { get; set; }

	public decimal AccountingUnitPrice { get; set; }

	public decimal DiscountOnScale { get; set; }

	public decimal DiscountPercent
	{
		get
		{
			if (Qt * (double)UnitPriceOnScale <= 0.0)
			{
				return 0m;
			}
			return 100m * Math.Round(DiscountOnScale / (decimal)(Qt * (double)UnitPriceOnScale), DigitForRound);
		}
	}

	public int Id { get; set; }

	public int IM { get; set; }

	public short Order { get; set; }

	public int PluNo { get; set; }

	public string PrdCode { get; set; }

	public Guid PrdGuid { get; set; }

	public string PrdName { get; set; }

	public double Qt { get; set; }

	public string QtString => Qt.ToString().Replace(".", "/");

	public int ScaleId { get; set; }

	public decimal TaxOnScale { get; set; }

	public decimal TotalPriceOnScale { get; set; }

	public string UnitName { get; set; }

	public decimal UnitPriceOnScale { get; set; }

	[JsonIgnore]
	public short DigitForRound { get; }

	public BaseScaleReportSalesDet()
	{
		DigitForRound = 3;
	}
}
