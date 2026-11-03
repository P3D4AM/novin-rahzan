using System;

namespace PacketParser.Scale.Entities.Interfaces.Reports;

public interface IDibalSaleItem
{
	string Raw { get; set; }

	bool Parsed { get; set; }

	int FactorNo { get; set; }

	int ClientKey { get; set; }

	short SectionNo { get; set; }

	short SectionMessageNo { get; set; }

	Sign TotalSumSign { get; set; }

	int TotalSum { get; set; }

	int Seller { get; set; }

	short TotalDiscount { get; set; }

	Sign RoundingSign { get; set; }

	short Rounding { get; set; }

	short AnimalNumber1 { get; set; }

	short AnimalNumber2 { get; set; }

	int Code1 { get; set; }

	float Weight1 { get; set; }

	Sign AmountSign1 { get; set; }

	double Amount1 { get; set; }

	float Price1 { get; set; }

	short OperationType1 { get; set; }

	short VatType1 { get; set; }

	short Discount1 { get; set; }

	short AnimalLink1 { get; set; }

	int Code2 { get; set; }

	float Weight2 { get; set; }

	Sign AmountSign2 { get; set; }

	double Amount2 { get; set; }

	float Price2 { get; set; }

	short OperationType2 { get; set; }

	short VatType2 { get; set; }

	short Discount2 { get; set; }

	short AnimalLink2 { get; set; }

	string RawDate { get; set; }

	string RawTime { get; set; }

	DateTime Date { get; set; }

	short Group { get; set; }

	char Key2 { get; set; }

	string Key { get; set; }
}
