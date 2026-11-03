using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IAghsatShakhsi : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int ID2 { get; set; }

	Guid TafsilGuid { get; set; }

	Guid MoeinGuid { get; set; }

	string Description { get; set; }

	DateTime DateSarResid { get; set; }

	DateTime DateM { get; set; }

	Guid UserGuid { get; set; }

	Guid? PardakhtGuid { get; set; }

	DateTime? PardakhtDateM { get; set; }

	decimal PardakhtSum { get; set; }

	decimal Price { get; set; }

	EntityChangeSt ChangeStatus { get; set; }

	ENSource Source { get; set; }

	DateTime? DateUtc { get; set; }
}
