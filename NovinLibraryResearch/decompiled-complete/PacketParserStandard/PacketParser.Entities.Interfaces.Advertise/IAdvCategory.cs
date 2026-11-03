using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IAdvCategory : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	Guid ParentGuid { get; set; }

	AdvertiseType Type { get; set; }
}
