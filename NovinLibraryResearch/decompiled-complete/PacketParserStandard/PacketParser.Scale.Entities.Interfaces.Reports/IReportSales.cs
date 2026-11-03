using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Scale.Entities.Interfaces.Reports;

public interface IReportSales
{
	int AccFactorNo { get; set; }

	decimal AccountingDiscount { get; set; }

	decimal AccountingSum { get; set; }

	decimal AccountingTax { get; set; }

	decimal AccountingTotalSum { get; set; }

	Guid AccRegisterGuid { get; set; }

	DateTime AccRegisterTime { get; set; }

	decimal AghlamSumCalced { get; }

	int ClerkNo { get; set; }

	short Count { get; set; }

	string CustFullName { get; set; }

	DateTime DateM { get; set; }

	string DateSh { get; }

	decimal DiscountOnScale { get; set; }

	int FactorNo { get; set; }

	int Id { get; set; }

	List<IReportSaleDet> List { get; set; }

	int MemId { get; set; }

	string PortIp { get; set; }

	DateTime ReadDateM { get; set; }

	string ReadDateSh { get; }

	ReadStatus ReadSt { get; set; }

	int ScaleId { get; set; }

	string ScaleName { get; set; }

	string ScaleNo { get; set; }

	decimal SumOnScale { get; set; }

	decimal TaxOnScale { get; set; }

	string Time { get; }

	string TotalBarcode { get; set; }

	decimal TotalSumOnScale { get; set; }
}
