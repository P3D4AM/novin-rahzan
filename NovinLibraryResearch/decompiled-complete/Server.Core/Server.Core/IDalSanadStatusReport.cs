using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalSanadStatusReport
{
	Task<List<SaveResponse>> SaveAsync(short depId, long hashid, ENSource source, List<WebSanadStatusReport> lst, CancellationToken token);

	Task<List<WebSanadStatusReport>> GetAllAsync(short depId, long hashId, int partitionId, CancellationToken cancellationToken);

	Task<WebSanadStatusReport> GetAsync(Guid guid, short depId);

	Task<List<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);
}
