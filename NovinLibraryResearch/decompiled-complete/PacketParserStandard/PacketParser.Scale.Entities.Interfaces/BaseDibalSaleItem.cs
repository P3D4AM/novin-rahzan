using System;
using PacketParser.Scale.Entities.Interfaces.Reports;

namespace PacketParser.Scale.Entities.Interfaces;

public class BaseDibalSaleItem : IDibalSaleItem
{
	public string Raw { get; set; }

	public bool Parsed { get; set; }

	public short Group { get; set; }

	public char Key2 { get; set; }

	public string Key { get; set; }

	public int FactorNo { get; set; }

	public int ClientKey { get; set; }

	public short SectionNo { get; set; }

	public short SectionMessageNo { get; set; }

	public Sign TotalSumSign { get; set; }

	public int TotalSum { get; set; }

	public int Seller { get; set; }

	public short TotalDiscount { get; set; }

	public Sign RoundingSign { get; set; }

	public short Rounding { get; set; }

	public short AnimalNumber1 { get; set; }

	public short AnimalNumber2 { get; set; }

	public int Code1 { get; set; }

	public float Weight1 { get; set; }

	public Sign AmountSign1 { get; set; }

	public double Amount1 { get; set; }

	public float Price1 { get; set; }

	public short OperationType1 { get; set; }

	public short VatType1 { get; set; }

	public short Discount1 { get; set; }

	public short AnimalLink1 { get; set; }

	public int Code2 { get; set; }

	public float Weight2 { get; set; }

	public Sign AmountSign2 { get; set; }

	public double Amount2 { get; set; }

	public float Price2 { get; set; }

	public short OperationType2 { get; set; }

	public short VatType2 { get; set; }

	public short Discount2 { get; set; }

	public short AnimalLink2 { get; set; }

	public string RawDate { get; set; }

	public string RawTime { get; set; }

	public DateTime Date { get; set; }
}
