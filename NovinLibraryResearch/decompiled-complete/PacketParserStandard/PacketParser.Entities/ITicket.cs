using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ITicket : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int Number { get; set; }

	Guid SourceGuid { get; set; }

	EnTargetType SourceType { get; set; }

	string Title { get; set; }

	DateTime DateM { get; set; }

	string Description { get; set; }

	EnTargetType TargetType { get; set; }

	Guid TargetGuid { get; set; }

	short StatusId { get; set; }

	Guid? ReferenceGuid { get; set; }

	Priority Priority { get; set; }
}
