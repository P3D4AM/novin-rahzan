using PacketParser.LogPacket;

namespace Server.Core.Log;

public interface IDalPacketLog
{
	void Insert(WebLogPacket webLogPacket, bool isSentPacket, short tryCount = 10);
}
