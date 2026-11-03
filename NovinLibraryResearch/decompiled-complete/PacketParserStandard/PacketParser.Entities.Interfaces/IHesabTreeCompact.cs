using System;

namespace PacketParser.Entities.Interfaces;

public interface IHesabTreeCompact
{
	Guid Guid { get; set; }

	string Name { get; set; }

	decimal Account { get; set; }

	bool Status { get; set; }

	HesabType HType { get; set; }
}
