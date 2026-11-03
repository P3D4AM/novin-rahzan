using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ITicketDet : IHasMasterGuid, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime? SeenTime { get; set; }

	bool IsSeen { get; set; }

	Guid SourceGuid { get; set; }

	EnTargetType SourceType { get; set; }

	string Description { get; set; }
}
