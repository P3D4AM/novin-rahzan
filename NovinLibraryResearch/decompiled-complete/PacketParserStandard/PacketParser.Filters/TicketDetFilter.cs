using System;

namespace PacketParser.Filters;

public class TicketDetFilter
{
	public Guid? TafsilGuid { get; set; }

	public Guid? CashierGuid { get; set; }

	public Guid? MasterGuid { get; set; }
}
