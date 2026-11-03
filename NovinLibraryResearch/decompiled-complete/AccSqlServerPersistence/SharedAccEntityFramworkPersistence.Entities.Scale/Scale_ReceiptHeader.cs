using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_ReceiptHeader")]
public class Scale_ReceiptHeader : IScale_ReceiptHeader, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	[MaxLength(250)]
	public string WorkstationIp { get; set; }

	public int SerialNumber { get; set; }

	public int ReceiptNumber { get; set; }

	public DateTime ReceiptTime { get; set; }

	[MaxLength(250)]
	public string GhorfehName { get; set; }

	public short WorkstationNumber { get; set; }

	[MaxLength(250)]
	public string UserName { get; set; }

	public short InvoiceType { get; set; }

	public int DiscountSupercharge { get; set; }

	public int TotalAmounts { get; set; }

	public int TotalWeights { get; set; }

	public int TotalItems { get; set; }

	public int TotalTax { get; set; }

	public short Tax { get; set; }

	public int TotalItemFromPlu { get; set; }

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
