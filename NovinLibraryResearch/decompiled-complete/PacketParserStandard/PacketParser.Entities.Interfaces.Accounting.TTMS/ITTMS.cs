using System;

namespace PacketParser.Entities.Interfaces.Accounting.TTMS;

public interface ITTMS : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string FilePath { get; set; }

	DateTime DateM { get; set; }

	EnSeason Priod { get; set; }

	short Year { get; set; }

	short State { get; set; }
}
