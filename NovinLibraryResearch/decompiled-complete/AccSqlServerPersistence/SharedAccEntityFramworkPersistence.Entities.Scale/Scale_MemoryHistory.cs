using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_MemoryHistory")]
public class Scale_MemoryHistory : IScale_MemoryHistory, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid ScaleGuid { get; set; }

	[MaxLength(250)]
	public string ScaleName { get; set; }

	[MaxLength(250)]
	public string ProductCode { get; set; }

	[MaxLength(250)]
	public string BarCode { get; set; }

	[MaxLength(250)]
	public string ProductName { get; set; }

	public int MemoryNumber { get; set; }

	public DateTime Date { get; set; }

	public bool Vazni { get; set; }

	public decimal UnitPrice { get; set; }

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
