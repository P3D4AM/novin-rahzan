using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IReceptionNaghd : IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid TafsilGuid { get; set; }

	Guid MoeinGuid { get; set; }

	Guid? DecimalGuid { get; set; }

	DateTime DateM { get; set; }

	Guid MasterGuid { get; set; }

	DateTime? DateUtc { get; set; }
}
