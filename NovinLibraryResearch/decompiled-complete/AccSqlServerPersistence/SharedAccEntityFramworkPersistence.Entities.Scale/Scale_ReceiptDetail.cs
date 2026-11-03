using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_ReceiptDetail")]
public class Scale_ReceiptDetail : IScale_ReceiptDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid HeaderRowGuid { get; set; }

	public short ItemNumber { get; set; }

	public Guid ProductGuid { get; set; }

	[MaxLength(250)]
	public string Name { get; set; }

	public int Price { get; set; }

	public int Discount { get; set; }

	public int TotalDiscount { get; set; }

	public int PluTax { get; set; }

	public int TotalDetailTax { get; set; }

	public int WeightQuantity { get; set; }

	[MaxLength(250)]
	public string Unit { get; set; }

	public int TotalPrice { get; set; }

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
