using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_SellDetail")]
public class Scale_SellDetail : IScale_SellDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public int PortNumber { get; set; }

	public Guid ScaleGuid { get; set; }

	public int FactorNumber { get; set; }

	public short MemoryNumber { get; set; }

	public decimal Quantity { get; set; }

	public decimal UnitPrice { get; set; }

	public bool Type_Weight { get; set; }

	public DateTime DateM { get; set; }

	[MaxLength(250)]
	public string DateSH_H { get; set; }

	public DateTime DateUpdate { get; set; }

	[MaxLength(250)]
	public string ComputerName { get; set; }

	public bool IsRead { get; set; }

	public short Ghorfe { get; set; }

	public decimal RowSum { get; set; }

	public int RInt1 { get; set; }

	public int RInt2 { get; set; }

	[MaxLength(250)]
	public string RText1 { get; set; }

	public int RText2 { get; set; }

	public decimal RDbl1 { get; set; }

	public decimal RDbl2 { get; set; }

	public string IP { get; set; }

	public Guid SC_Guid { get; set; }

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
