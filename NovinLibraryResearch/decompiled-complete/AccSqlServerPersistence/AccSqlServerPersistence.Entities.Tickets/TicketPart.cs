using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SharedAccEntityFramworkPersistence.Entities.Global.Tickets;

namespace AccSqlServerPersistence.Entities.Tickets;

[Table("Global_TicketPart")]
public class TicketPart : Serializable<ITicketPart>, ITicketPart, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	private DateTime _modified = DateTime.Now;

	[Key]
	public Guid Guid { get; set; }

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

	[Required]
	[MaxLength(500)]
	public string Name { get; set; }

	public Guid? ParentGuid { get; set; }

	public bool Status { get; set; }

	public string? MetaData { get; set; }

	public string? Description { get; set; }

	public short Ordinal { get; set; }

	public ICollection<TicketPartDetail> TicketPartDetails { get; set; }
}
