using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalMoein
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashid, ENSource source, List<WebMoein> lst, CancellationToken token);

	Task<List<WebMoein>> GetAllAsync(short DepId, DateTime Modified, CancellationToken cancellationToken, bool loadGardeshHesab);

	Task<List<WebMoein>> GetAllAsync(short DepId, long HashId, int partitionId, CancellationToken cancellationToken, bool loadGardeshHesab);

	Task<List<short>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);

	Task<WebMoein> GetAsync(Guid guid, short depId, bool loadGardeshHesab);
}
