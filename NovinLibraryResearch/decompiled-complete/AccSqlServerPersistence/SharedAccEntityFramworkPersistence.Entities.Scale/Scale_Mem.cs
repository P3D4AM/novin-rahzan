using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_Mem")]
public class Scale_Mem : IScale_Mem, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid ScaleGuid { get; set; }

	public Guid ProductGuid { get; set; }

	public int MemoryNumber { get; set; }

	public DateTime Date { get; set; }

	public bool ISSaved { get; set; }

	public decimal PriceOnScale { get; set; }

	[MaxLength(250)]
	public string CodeOnScale { get; set; }

	[MaxLength(250)]
	public string NameScale { get; set; }

	[MaxLength(250)]
	public string BarCode { get; set; }

	[MaxLength(250)]
	public string Name { get; set; }

	public decimal Price { get; set; }

	public bool Vazni { get; set; }

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
