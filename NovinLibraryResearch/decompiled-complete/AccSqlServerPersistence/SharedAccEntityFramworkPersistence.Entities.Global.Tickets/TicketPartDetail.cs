using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Tickets;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global.Tickets;

namespace SharedAccEntityFramworkPersistence.Entities.Global.Tickets;

[Table("Global_TicketPartDetail")]
public class TicketPartDetail : ITicketPartDetail, IHasMasterGuid
{
	[Key]
	public Guid MasterGuid { get; set; }

	[Key]
	public Guid TargetGuid { get; set; }

	[Key]
	public EnTargetType TargetType { get; set; }

	public TicketPart TicketPart { get; set; }
}
