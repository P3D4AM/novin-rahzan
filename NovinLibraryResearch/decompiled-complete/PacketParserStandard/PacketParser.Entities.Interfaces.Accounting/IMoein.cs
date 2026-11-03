using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IMoein : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	Guid KolGuid { get; set; }

	long Code { get; set; }

	string Description { get; set; }

	ENMahiat Mahiat { get; set; }

	bool System { get; set; }

	bool Peygiri { get; set; }

	bool Tedadi { get; set; }

	decimal Account { get; set; }

	DateTime DateM { get; set; }

	byte[] GardeshHesab { get; set; }
}
