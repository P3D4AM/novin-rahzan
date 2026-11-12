using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalAghsatAshkhas
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashId, List<WebAghsatAshkhas> lst, ENSource Source, CancellationToken token);

	Task<List<WebAghsatAshkhas>> GetAllAsync(short DepId, DateTime Modified, CancellationToken cancellationToken);

	Task<List<WebAghsatAshkhas>> GetAllAsync(short DepId, Guid TafsilGuid, DateTime start, DateTime end, CancellationToken cancellationToken);

	Task<List<WebAghsatAshkhas>> GetAllAsync(short DepId, CancellationToken cancellationToken);

	Task<List<WebAghsatAshkhas>> GetAllAsync(short DepId, long HashId, short Partition, CancellationToken cancellationToken);

	Task<List<short>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);

	Task<List<WebAghsatAshkhas>> GetallFromClients_ListForServerAsync(short DepId, CancellationToken cancellationToken);

	Task<WebAghsatAshkhas> GetAsync(Guid guid, short depId);
}
