using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_Info")]
public class Scale_Info : IScale_Info, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid ScaleGuid { get; set; }

	public int PortNumber { get; set; }

	public int TimerInterval { get; set; }

	[MaxLength(250)]
	public string Name { get; set; }

	public bool Deleted { get; set; }

	public bool SellJamandeh { get; set; }

	[MaxLength(20)]
	public string IP { get; set; }

	public string EndMessage { get; set; }

	[MaxLength(250)]
	public string ShopName { get; set; }

	public bool AccountingMemory { get; set; }

	public bool AccountingDate { get; set; }

	public bool AccountingDivision { get; set; }

	public bool AccountingID { get; set; }

	public bool AccountingIP { get; set; }

	public bool MNGLock { get; set; }

	public short FactorNumber { get; set; }

	public short ConnectionType { get; set; }

	[MaxLength(250)]
	public string Model { get; set; }

	[MaxLength(250)]
	public string Serial { get; set; }

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
