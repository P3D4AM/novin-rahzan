using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Ticket.TicketPart;

public class AccessibleDepartmentAndUser
{
	public string Name { get; set; }

	public Guid Guid { get; set; }

	public EnTargetType TargetType { get; set; }
}
