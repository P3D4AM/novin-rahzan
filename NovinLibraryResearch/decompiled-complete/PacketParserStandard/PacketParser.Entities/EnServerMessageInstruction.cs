namespace PacketParser.Entities;

public enum EnServerMessageInstruction
{
	none,
	ping,
	pongExists,
	pongNotExists,
	publicKey,
	symmetricKey,
	setting,
	IsSecured,
	ByteData
}
