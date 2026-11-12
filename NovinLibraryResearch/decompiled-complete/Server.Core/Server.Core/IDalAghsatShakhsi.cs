using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalAghsatShakhsi
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashId, List<WebAghsatShakhsi> lst, ENSource Source, CancellationToken token);

	Task<List<WebAghsatShakhsi>> GetAllAsync(short DepId, DateTime Modified, CancellationToken cancellationToken);

	Task<List<WebAghsatShakhsi>> GetAllAsync(short DepId, CancellationToken cancellationToken);

	Task<List<WebAghsatShakhsi>> GetAllAsync(short DepId, long HashId, short Partition, CancellationToken cancellationToken);

	Task<List<short>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);

	Task<List<WebAghsatShakhsi>> GetallFromClients_ListForServerAsync(short DepId, CancellationToken cancellationToken);

	Task<WebAghsatShakhsi> GetAsync(Guid guid, short depId);
}
