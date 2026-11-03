using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_Sales")]
public class Scale_Sales : IScale_Sales, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	[MaxLength(250)]
	public string TotalBarcode { get; set; }

	public Guid MemoryGuid { get; set; }

	public DateTime DateM { get; set; }

	[MaxLength(250)]
	public string DateSH { get; set; }

	public DateTime ReadDateM { get; set; }

	[MaxLength(250)]
	public string MyProperty { get; set; }

	[MaxLength(250)]
	public string ReadDateSH { get; set; }

	[MaxLength(250)]
	public string PortIP { get; set; }

	public short Count { get; set; }

	public decimal Sum { get; set; }

	public decimal Tax { get; set; }

	public decimal Discount { get; set; }

	public decimal TotalSum { get; set; }

	[MaxLength(250)]
	public string ScaleNumber { get; set; }

	public Guid ScaleGuid { get; set; }

	public int FactorNumber { get; set; }

	public int ClerkNumber { get; set; }

	[MaxLength(250)]
	public string ScaleName { get; set; }

	public decimal AccountingSum { get; set; }

	public decimal AccountingDiscount { get; set; }

	public decimal AccountingTax { get; set; }

	public decimal AccountingTotalSum { get; set; }

	public Guid AccountingRegisterGuid { get; set; }

	public DateTime AccountingRegisterTime { get; set; }

	[MaxLength(250)]
	public string CustomerFullName { get; set; }

	public int AccountingFactorNumber { get; set; }

	public bool AccountingRegisterStatus { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }
}
