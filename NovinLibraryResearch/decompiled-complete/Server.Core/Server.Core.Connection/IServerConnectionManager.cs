using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;

namespace Server.Core.Connection;

public interface IServerConnectionManager
{
	string ServerFriendlyName { get; }

	string ConnectionId { get; }

	WebContextHolder ContextHolder { get; }

	Task GenerateHandleConnectionAsync(ConnectionData connection);

	Task GenerateUnHandleConnectionAsync(string connectionUniqueId);

	Task GenerateHandleConnectionsAsync(List<ConnectionData> connections, string ServerUniqueId);

	Task GenerateSendPacketAsync(Packet pack, string connectionUniqueId, Priority priority);
}
