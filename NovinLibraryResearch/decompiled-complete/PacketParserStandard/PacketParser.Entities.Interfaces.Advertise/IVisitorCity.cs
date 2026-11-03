using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IVisitorCity : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid VisitorGuid { get; set; }

	Guid CityGuid { get; set; }
}
