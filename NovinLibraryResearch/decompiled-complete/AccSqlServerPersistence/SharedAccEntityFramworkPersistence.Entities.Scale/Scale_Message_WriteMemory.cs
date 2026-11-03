using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_Message_WriteMemory")]
public class Scale_Message_WriteMemory : IScale_Message_WriteMemory, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	[MaxLength(250)]
	public string Code { get; set; }

	[MaxLength(250)]
	public string Barcode { get; set; }

	[MaxLength(250)]
	public string Name { get; set; }

	public decimal UnitPrice { get; set; }

	public DateTime DateM { get; set; }

	public DateTime Expiredate { get; set; }

	public short ST { get; set; }

	public bool Vazni { get; set; }

	public short NextTry { get; set; }

	public Guid ScaleGuid { get; set; }

	public short TryCount { get; set; }

	public short MaxTryCount { get; set; }

	[MaxLength(250)]
	public string Parametr1 { get; set; }

	[MaxLength(250)]
	public string Parametr2 { get; set; }

	[MaxLength(250)]
	public string Parametr3 { get; set; }

	[MaxLength(250)]
	public string Parametr4 { get; set; }

	public int MemoryNumber { get; set; }

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
