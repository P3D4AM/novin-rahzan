using System;

namespace PacketParser.Entities.Interfaces.Global.Tickets;

public interface ITicketPartDetail : IHasMasterGuid
{
	EnTargetType TargetType { get; set; }

	Guid TargetGuid { get; set; }
}
