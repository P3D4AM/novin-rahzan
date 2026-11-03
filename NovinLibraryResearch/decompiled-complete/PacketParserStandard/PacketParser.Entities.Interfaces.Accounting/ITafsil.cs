using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ITafsil : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	long Code { get; set; }

	string Description { get; set; }

	bool System { get; set; }

	Guid TafsilGroupGuid { get; set; }

	HesabType HType { get; set; }

	DateTime DateM { get; set; }

	string SerializedData { get; set; }

	decimal SumAghsat { get; set; }

	int Version { get; set; }

	bool IsSpecial { get; set; }
}
