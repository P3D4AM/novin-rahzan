using System;

namespace PacketParser.Entities.Interfaces.Global;

public interface ISaveTemp : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid AsGuid { get; set; }

	Document AsType { get; set; }

	DateTime DateM { get; set; }

	int Id2 { get; set; }

	string Description { get; set; }

	byte[] SerialedData { get; set; }
}
