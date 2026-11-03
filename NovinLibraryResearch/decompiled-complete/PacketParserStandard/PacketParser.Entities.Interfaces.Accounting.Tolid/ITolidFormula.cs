using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid;

public interface ITolidFormula : IHasJustGuid, IHasModified
{
	DateTime DateM { get; set; }

	string Title { get; set; }

	string Description { get; set; }
}
