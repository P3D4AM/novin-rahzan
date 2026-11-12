using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;

namespace Server.Core.Connection;

public interface IConnectionManager
{
	List<string> GetGuids(short DepId, int hashId);

	List<string> GetGuids(short DepId);

	ConnectionData GetConnection(string connectionUniqueId);

	Task SendPacketAsync(ConnectionData connection, Packet packet, Priority priority, int partitionId, CancellationToken token);
}
