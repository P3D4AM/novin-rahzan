using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class CashierLogFilter
{
	public Guid? SanadGuid { get; set; }

	public Guid? CashierGuid { get; set; }

	public string Description { get; set; } = "";


	public DateTime? Date1 { get; set; }

	public DateTime? Date2 { get; set; }

	public EnLog? SanadType { get; set; }
}
