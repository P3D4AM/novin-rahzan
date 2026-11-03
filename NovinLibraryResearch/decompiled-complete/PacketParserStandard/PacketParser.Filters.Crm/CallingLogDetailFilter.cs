using System;
using System.Collections.Generic;

namespace PacketParser.Filters.Crm;

public class CallingLogDetailFilter
{
	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public Guid? TicketGuid { get; set; }

	public string Callfrom { get; set; }

	public string LinkedId { get; set; }

	public Guid? CashierGuid { get; set; }

	public List<string> NumberList { get; set; }
}
