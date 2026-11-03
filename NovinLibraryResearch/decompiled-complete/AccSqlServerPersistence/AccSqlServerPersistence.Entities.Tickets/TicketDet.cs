using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Tickets;

[Table("Global_TicketDetail")]
public class TicketDet : Serializable<ITicketDet>, ITicketDet, IHasMasterGuid, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	private EnTargetType _sourceType;

	[Key]
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public DateTime DateM { get; set; }

	public string? MetaData { get; set; }

	public string? Description { get; set; }

	public Guid SourceGuid { get; set; }

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

	public bool IsSeen { get; set; }

	public DateTime? SeenTime { get; set; }

	[NotMapped]
	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public virtual Ticket Ticket { get; set; }
}
