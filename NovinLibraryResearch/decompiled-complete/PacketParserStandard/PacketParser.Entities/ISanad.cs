using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ISanad : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo
{
	int Id { get; set; }

	DateTime DateM { get; set; }

	string Description { get; set; }

	SanadSt SanadSt { get; set; }

	Guid UserGuid { get; set; }

	decimal Debit { get; }

	decimal Credit { get; }

	ENSource Source { get; set; }

	SanadType SanadType { get; set; }

	short StatusId { get; set; }

	bool IsTemp { get; set; }
}
