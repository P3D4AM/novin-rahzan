using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Khazane;

namespace PacketParser.Entities;

public interface IPardakhtBank : IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasWage
{
	DateTime DateM { get; set; }

	string Number { get; set; }

	Guid TafsilGuid { get; set; }

	Guid MoeinGuid { get; set; }

	Guid? DecimalGuid { get; set; }

	Guid MasterGuid { get; set; }

	DateTime? DateUtc { get; set; }
}
