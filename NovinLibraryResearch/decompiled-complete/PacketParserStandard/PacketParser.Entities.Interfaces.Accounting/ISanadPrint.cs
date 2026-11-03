using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ISanadPrint : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid AsGuid { get; set; }

	Document AsType { get; set; }

	string Name { get; set; }

	string Tel { get; set; }

	string Address { get; set; }

	string Mob { get; set; }

	DateTime DateM { get; set; }

	long MobileLong { get; set; }
}
