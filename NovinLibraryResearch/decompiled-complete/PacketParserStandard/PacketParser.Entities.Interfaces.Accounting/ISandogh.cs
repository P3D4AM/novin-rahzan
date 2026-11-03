using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ISandogh : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	long Code { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	bool Manfi { get; set; }

	Guid TafsilGroupGuid { get; set; }
}
