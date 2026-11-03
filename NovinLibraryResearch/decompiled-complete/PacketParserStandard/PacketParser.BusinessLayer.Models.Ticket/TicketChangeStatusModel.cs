using System;

namespace PacketParser.BusinessLayer.Models.Ticket;

public class TicketChangeStatusModel
{
	public Guid Guid { get; set; }

	public int StatusId { get; set; }
}
