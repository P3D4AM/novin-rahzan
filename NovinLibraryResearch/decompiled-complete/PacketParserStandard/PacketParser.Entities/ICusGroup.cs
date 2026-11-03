using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ICusGroup : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Description { get; set; }

	string Name { get; set; }

	bool Special { get; set; }

	Guid MoeinDefaultGuid { get; set; }

	Guid TafsilGroupGuid { get; set; }

	ServerStatusForClient ServerStatus { get; set; }
}
