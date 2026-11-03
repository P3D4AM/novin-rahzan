using System;

namespace PacketParser.Scale.Entities.Interfaces.Reports;

public interface IReportSaleDet
{
	decimal AccountingDiscount { get; set; }

	decimal AccountingTax { get; set; }

	decimal AccountingTotalPrice { get; set; }

	decimal AccountingUnitPrice { get; set; }

	decimal DiscountOnScale { get; set; }

	decimal DiscountPercent { get; }

	int Id { get; set; }

	int IM { get; set; }

	short Order { get; set; }

	int PluNo { get; set; }

	string PrdCode { get; set; }

	Guid PrdGuid { get; set; }

	string PrdName { get; set; }

	double Qt { get; set; }

	string QtString { get; }

	int ScaleId { get; set; }

	decimal TaxOnScale { get; set; }

	decimal TotalPriceOnScale { get; set; }

	string UnitName { get; set; }

	decimal UnitPriceOnScale { get; set; }
}
