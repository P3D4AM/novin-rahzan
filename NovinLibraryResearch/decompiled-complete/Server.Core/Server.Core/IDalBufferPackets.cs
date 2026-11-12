using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalBufferPackets
{
	Task InsertAsync(BufferPacket BufferPacket);

	Task DeleteAsync(int Hashid);

	Task<List<BufferPacket>> GetAllAsync(short HashId);
}
