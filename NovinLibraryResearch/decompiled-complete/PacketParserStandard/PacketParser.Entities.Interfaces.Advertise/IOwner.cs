using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IOwner : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	string Family { get; set; }

	Guid VisitorGuid { get; set; }
}
