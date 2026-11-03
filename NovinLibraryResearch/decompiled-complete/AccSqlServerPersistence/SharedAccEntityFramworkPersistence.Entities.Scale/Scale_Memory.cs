using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_Memory")]
public class Scale_Memory : IScale_Memory, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid ScaleGuid { get; set; }

	public int MyProperty { get; set; }

	public Guid ProductGuid { get; set; }

	public short MemoryNumber { get; set; }

	[MaxLength(250)]
	public string MemoryName { get; set; }

	public decimal MemoryPrice { get; set; }

	public DateTime Date { get; set; }

	public bool Taeed { get; set; }

	public bool Vazni { get; set; }

	public decimal TotalQT { get; set; }

	public decimal TotalPrice { get; set; }

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

	public string Description { get; set; }
}
