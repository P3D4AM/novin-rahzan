using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_DibalSale")]
public class Scale_DibalSale : IScale_DibalSale, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	[MaxLength(250)]
	public string Raw { get; set; }

	public DateTime Date { get; set; }

	[MaxLength(250)]
	public string RawDate { get; set; }

	[MaxLength(250)]
	public string RawTime { get; set; }

	public decimal Amount1 { get; set; }

	public decimal Amount2 { get; set; }

	public decimal Price1 { get; set; }

	public decimal Price2 { get; set; }

	public decimal Discount1 { get; set; }

	public decimal Discount2 { get; set; }

	public short AmountSign1 { get; set; }

	public short AmountSign2 { get; set; }

	public short AnimalLink1 { get; set; }

	public short AnimalLink2 { get; set; }

	public short AnimalNumber1 { get; set; }

	public short AnimalNumber2 { get; set; }

	public int ClientKey { get; set; }

	public short FactorNumber { get; set; }

	public short OperationType1 { get; set; }

	public short OperationType2 { get; set; }

	public bool Parsed { get; set; }

	public short Rounding { get; set; }

	public short RoundingSign { get; set; }

	public short SectionMessageNumber { get; set; }

	public short SectionNumber { get; set; }

	public int Seller { get; set; }

	public short TotalDiscount { get; set; }

	public short TotalSum { get; set; }

	public short TotalSumSign { get; set; }

	public short VatType1 { get; set; }

	public short VatType2 { get; set; }

	public decimal Weight1 { get; set; }

	public decimal Weight2 { get; set; }

	[MaxLength(250)]
	public string Key { get; set; }

	[MaxLength(250)]
	public string Key2 { get; set; }

	public short Group { get; set; }

	public int Code1 { get; set; }

	public int Code2 { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	public bool Status { get; set; }
}
