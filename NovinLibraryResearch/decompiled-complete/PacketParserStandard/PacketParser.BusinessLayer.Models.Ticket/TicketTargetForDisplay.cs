using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Ticket;

public class TicketTargetForDisplay
{
	public EnTargetType TicketTarget { get; set; }

	public string TicketTargetEnumForDisplay => TicketTarget.GetDisplay().GetTranslate();

	public TicketTargetForDisplay(EnTargetType ticketTarget)
	{
		TicketTarget = ticketTarget;
	}
}
