using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalCallingLog
{
	Task<IEnumerable<SaveResponse>> SaveAsync(short DepId, long hashId, IEnumerable<WebCallingLog> lst, ENSource Source, CancellationToken token);

	Task<IEnumerable<WebCallingLog>> GetAllAsync(short DepId, DateTime Modified, CancellationToken cancellationToken);

	Task<WebCallingLog> GetAsync(Guid guid);

	Task<IEnumerable<WebCallingLog>> GetAllAsync(short DepId, CancellationToken cancellationToken);

	Task<IEnumerable<WebCallingLog>> GetAllAsync(short DepId, long HashId, short Partition, CancellationToken cancellationToken);

	Task<IEnumerable<short>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);

	Task<IEnumerable<WebCallingLog>> GetallFromClients_ListForServerAsync(short DepId, CancellationToken cancellationToken);

	Task<WebCallingLog> GetAsync(Guid guid, short depId);
}
