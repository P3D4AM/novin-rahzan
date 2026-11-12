using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalDepot
{
	Task<bool> SaveAsync(short DepId, long hashId, IEnumerable<WebDepot> lst, ENSource source, CancellationToken token);

	Task<WebDepot> LoadDataAsync(short DepId, short Id);

	Task<WebDepot> LoadDataAsync(short DepId, Guid guid);

	Task<IEnumerable<WebDepot>> GetAllAsync(short DepId, DateTime Modified, CancellationToken cancellationToken);
}
