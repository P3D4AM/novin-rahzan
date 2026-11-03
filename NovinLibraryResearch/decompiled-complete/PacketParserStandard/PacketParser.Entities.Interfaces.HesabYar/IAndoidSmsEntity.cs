using System;

namespace PacketParser.Entities.Interfaces.HesabYar;

public interface IAndoidSmsEntity
{
	Guid Guid { get; set; }

	string Sender { get; set; }

	string Message { get; set; }
}
