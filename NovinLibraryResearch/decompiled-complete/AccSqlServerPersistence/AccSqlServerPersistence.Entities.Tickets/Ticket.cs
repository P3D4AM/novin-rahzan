using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Tickets;

[Table("Global_Ticket")]
public class Ticket : Serializable<ITicket>, ITicket, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	private DateTime _dateM = DateTime.Now;

	private DateTime _modified = DateTime.Now;

	private EnTargetType _sourceType;

	[Key]
	public Guid Guid { get; set; }

	public int Number { get; set; }

	[Required]
	[MaxLength(500)]
	public string Title { get; set; }

	public DateTime DateM
	{
		get
		{
			return _dateM;
		}
		set
		{
			DateTime dateM = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_dateM = dateM;
		}
	}

	public EnTargetType TargetType { get; set; }

	public EnTargetType SourceType
	{
		get
		{
			return _sourceType;
		}
		set
		{
			if (value == EnTargetType.None)
			{
				throw new NotImplementedException();
			}
			_sourceType = value;
		}
	}

	public Guid TargetGuid { get; set; }

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

	public string? MetaData { get; set; }

	public string? Description { get; set; }

	public Guid? ReferenceGuid { get; set; }

	public Guid SourceGuid { get; set; }

	public short StatusId { get; set; }

	public Priority Priority { get; set; }

	public bool Status { get; set; }

	public virtual Ticket MasterTicket { get; set; }

	public ICollection<TicketDet> TicketDets { get; set; }

	public ICollection<Ticket> Refrences { get; set; }
}
