using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ITicketPart : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	Guid? ParentGuid { get; set; }

	string Description { get; set; }

	short Ordinal { get; set; }
}
