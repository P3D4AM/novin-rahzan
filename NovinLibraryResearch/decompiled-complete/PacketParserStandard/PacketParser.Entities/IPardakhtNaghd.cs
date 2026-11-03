using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IPardakhtNaghd : IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid TafsilGuid { get; set; }

	Guid MoeinGuid { get; set; }

	Guid MasterGuid { get; set; }

	DateTime DateM { get; set; }

	DateTime? DateUtc { get; set; }
}
