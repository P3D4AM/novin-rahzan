using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalSanadPrint
{
	Task<List<SaveResponse>> SaveAsync(short depId, long hashid, ENSource source, List<WebSanadPrint> lst, CancellationToken token);

	Task<List<WebSanadPrint>> GetAllAsync(short depId, long hashId, int partitionId, CancellationToken cancellationToken);

	Task<WebSanadPrint> GetAsync(Guid guid, short depId);

	Task<List<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);
}
