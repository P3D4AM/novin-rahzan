using System.Collections.Generic;
using PacketParser.Access.Orders;
using PacketParser.Access.SanadStatusAccess;

namespace PacketParser.Access.Ticket;

public class TicketAccess
{
	public List<TicketSanadStatusAccess> SanadStatusAccess { get; set; } = new List<TicketSanadStatusAccess>();


	public TicketDefaultSanadStatusAccess DefaultSanadStatusAccess { get; set; } = new TicketDefaultSanadStatusAccess();

}
