using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IDesc : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DescSection Section { get; set; }

	string Description { get; set; }

	TicketTargetEnum SourceType { get; set; }

	string Source { get; set; }

	TicketTargetEnum DestinationType { get; set; }

	string Destination { get; set; }
}
