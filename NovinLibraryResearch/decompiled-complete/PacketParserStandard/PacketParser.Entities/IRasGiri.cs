using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IRasGiri : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime DateM { get; }

	DateTime BaseDateM { get; set; }

	decimal Sood_Percent { get; set; }

	Guid TafsilGuid { get; set; }

	Guid SanadGuid { get; set; }

	SanadSt SanadType { get; set; }
}
