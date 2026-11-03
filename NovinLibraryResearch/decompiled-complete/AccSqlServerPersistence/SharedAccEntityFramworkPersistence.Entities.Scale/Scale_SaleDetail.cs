using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_SaleDetail")]
public class Scale_SaleDetail : IScale_SaleDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid ScaleGuid { get; set; }

	public Guid ProductGuid { get; set; }

	[MaxLength(250)]
	public string ProductCode { get; set; }

	[MaxLength(250)]
	public string ProductName { get; set; }

	public int PluNumber { get; set; }

	[MaxLength(250)]
	public string UnitName { get; set; }

	public decimal UnitPrice { get; set; }

	public decimal Discount { get; set; }

	public decimal Tax { get; set; }

	public decimal TotalPrice { get; set; }

	public decimal QT { get; set; }

	public int IM { get; set; }

	public short Order { get; set; }

	public decimal AccountingUnitPrice { get; set; }

	public decimal AccountingDiscount { get; set; }

	public int MyProperty { get; set; }

	public decimal AccountingTax { get; set; }

	public decimal AccountingTotalPrice { get; set; }

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
