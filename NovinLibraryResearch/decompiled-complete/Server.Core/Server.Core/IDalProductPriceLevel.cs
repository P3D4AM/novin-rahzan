using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalProductPriceLevel
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashid, ENSource source, List<WebProductPriceLevel> lst, CancellationToken token);

	Task<List<WebProductPriceLevel>> GetAllAsync(short depId, long hashId, int partitionId, CancellationToken cancellationToken);

	Task<WebProductPriceLevel> GetAsync(Guid guid, short depId);

	Task<List<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);
}
