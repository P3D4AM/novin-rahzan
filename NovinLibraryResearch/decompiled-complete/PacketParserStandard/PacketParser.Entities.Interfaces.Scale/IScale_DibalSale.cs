using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_DibalSale : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Raw { get; set; }

	DateTime Date { get; set; }

	string RawDate { get; set; }

	string RawTime { get; set; }

	decimal Amount1 { get; set; }

	decimal Amount2 { get; set; }

	decimal Price1 { get; set; }

	decimal Price2 { get; set; }

	decimal Discount1 { get; set; }

	decimal Discount2 { get; set; }

	short AmountSign1 { get; set; }

	short AmountSign2 { get; set; }

	short AnimalLink1 { get; set; }

	short AnimalLink2 { get; set; }

	short AnimalNumber1 { get; set; }

	short AnimalNumber2 { get; set; }

	int ClientKey { get; set; }

	short FactorNumber { get; set; }

	short OperationType1 { get; set; }

	short OperationType2 { get; set; }

	bool Parsed { get; set; }

	short Rounding { get; set; }

	short RoundingSign { get; set; }

	short SectionMessageNumber { get; set; }

	short SectionNumber { get; set; }

	int Seller { get; set; }

	short TotalDiscount { get; set; }

	short TotalSum { get; set; }

	short TotalSumSign { get; set; }

	short VatType1 { get; set; }

	short VatType2 { get; set; }

	decimal Weight1 { get; set; }

	decimal Weight2 { get; set; }

	string Key { get; set; }

	string Key2 { get; set; }

	short Group { get; set; }

	int Code1 { get; set; }

	int Code2 { get; set; }
}
