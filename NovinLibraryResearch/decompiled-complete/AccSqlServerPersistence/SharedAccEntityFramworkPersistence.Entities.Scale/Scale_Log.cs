using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_Log")]
public class Scale_Log : IScale_Log, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

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
