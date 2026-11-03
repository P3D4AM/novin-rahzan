using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface ICity : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string CityName { get; set; }

	Guid StateGuid { get; set; }

	int Weight { get; set; }

	bool isDivarCity { get; set; }

	bool isSheypoorCity { get; set; }

	bool isNiazKadeCity { get; set; }

	bool isNiazmandyHaCity { get; set; }
}
