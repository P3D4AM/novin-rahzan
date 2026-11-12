using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalAccount
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashid, ENSource source, List<WebAccount> lst, CancellationToken token);

	Task<List<WebAccount>> GetAllAsync(short DepId, long HashId, int partitionId, CancellationToken cancellationToken);

	Task<List<WebAccount>> GetAllByTafsilGuidAsync(short DepId, Guid tafsilGuid, CancellationToken cancellationToken);

	Task<WebAccount> GetAsync(Guid guid, short depId);

	Task<List<short>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);
}
