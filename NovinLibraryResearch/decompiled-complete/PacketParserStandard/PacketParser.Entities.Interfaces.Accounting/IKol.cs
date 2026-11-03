using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IKol : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	Guid GroupGuid { get; }

	ENMahiat Mahiat { get; set; }

	int Code { get; set; }

	string Description { get; set; }

	bool System { get; set; }

	decimal Account { get; set; }

	DateTime DateM { get; set; }

	byte[] GardeshHesab { get; set; }
}
