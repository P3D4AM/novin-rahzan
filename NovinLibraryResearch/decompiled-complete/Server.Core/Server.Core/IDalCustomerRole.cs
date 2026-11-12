using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.WordPress;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalCustomerRole
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashId, ENSource source, List<WebCustomerRole> lst, CancellationToken token);

	Task<List<WebCustomerRole>> GetAllAsync(short DepId, CancellationToken cancellationToken);

	Task<List<WebCustomerRole>> GetAllAsync(short depId, long hashId, int partitionId, CancellationToken cancellationToken);

	Task<WebCustomerRole> GetAsync(short depId, Guid guid);

	Task<List<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);
}
