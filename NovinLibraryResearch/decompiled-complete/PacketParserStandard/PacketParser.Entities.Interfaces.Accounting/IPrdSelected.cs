using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IPrdSelected : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Code { get; set; }

	string Title { get; set; }

	short? Type { get; set; }

	Guid UserGuid { get; set; }

	string Description { get; set; }
}
