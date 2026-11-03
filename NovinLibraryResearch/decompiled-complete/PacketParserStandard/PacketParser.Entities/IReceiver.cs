using System.Threading.Tasks;

namespace PacketParser.Entities;

internal interface IReceiver
{
	Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection);

	string GetPacketActionName(short action);

	string GetPacketActionEnumName(short action);
}
