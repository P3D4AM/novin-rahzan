using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser;

namespace Server.Core;

public interface IDalSignalRLogger
{
	Task SlogSent_InsertAsync(Packet Message, List<string> ConnectionUniqueIds);

	Task SlogReceviedMsg_InsertAsync(byte[] Message, string ConnectionUniqueId);

	Task SlogSendReq_InsertAsync(Packet Message, short Source, short DepId, int UserId, List<string> ConnectionUniqueId);
}
