using System;

namespace PacketParser.Payamak.Interfaces.Entities;

public interface IBaseSms
{
	int ID { get; set; }

	string Number { get; set; }

	string Message { get; set; }

	DateTime DateM { get; set; }

	int CustomerId { get; set; }

	bool ST { get; set; }
}
