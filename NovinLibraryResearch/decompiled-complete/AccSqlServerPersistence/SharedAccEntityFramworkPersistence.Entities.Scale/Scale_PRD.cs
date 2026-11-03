using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_PRD")]
public class Scale_PRD : IScale_PRD, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public short MemoryNumber { get; set; }

	[MaxLength(250)]
	public string MemoryName { get; set; }

	public decimal MemoryPrice { get; set; }

	public bool Vazni { get; set; }

	public decimal TotalQT { get; set; }

	public decimal TotalPrice { get; set; }

	public Guid ProductGuid { get; set; }

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
