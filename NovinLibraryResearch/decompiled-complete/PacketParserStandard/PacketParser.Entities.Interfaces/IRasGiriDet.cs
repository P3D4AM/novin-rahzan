using System;

namespace PacketParser.Entities.Interfaces;

public interface IRasGiriDet : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	decimal Price { get; set; }

	string Description { get; set; }

	DateTime DateM { get; set; }
}
