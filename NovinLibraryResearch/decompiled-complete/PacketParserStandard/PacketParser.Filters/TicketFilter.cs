using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class TicketFilter
{
	public Guid? TargetGuid { get; set; }

	public EnTargetType TargetType { get; set; }

	public List<short> StatusId { get; set; }

	public List<Priority> Priorities { get; set; }

	public DateTime? DateTimeFrom { get; set; }

	public DateTime? DateTimeTo { get; set; }
}
