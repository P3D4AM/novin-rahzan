namespace PacketParser.Entities;

public enum EnSadadPacketType
{
	Stx = 2,
	Etx = 3,
	Eot = 4,
	Ack = 6,
	Nack = 21,
	Request = 25,
	Response = 26,
	Unknown = 27,
	Inm = 241
}
