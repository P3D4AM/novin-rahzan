using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IRegion : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid CityGuid { get; set; }

	string Name { get; set; }

	AdvertiseType Type { get; set; }
}
