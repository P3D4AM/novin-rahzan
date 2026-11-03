using System;

namespace PacketParser.Filters.Global;

public class TicketPartFilter
{
	public bool? Status { get; set; } = true;


	public bool IsLoadFullName { get; set; }

	public Guid? CashierGuid { get; set; }
}
