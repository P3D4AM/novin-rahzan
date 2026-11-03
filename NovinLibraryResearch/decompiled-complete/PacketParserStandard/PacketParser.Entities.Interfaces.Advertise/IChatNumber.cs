using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IChatNumber : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Number { get; set; }

	DateTime DateM { get; set; }

	string City { get; set; }

	string Category { get; set; }

	bool IsUsed { get; set; }
}
