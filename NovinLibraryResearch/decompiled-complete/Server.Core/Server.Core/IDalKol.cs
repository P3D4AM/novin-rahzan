using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalKol
{
	Task<List<SaveResponse>> SaveAsync(short depId, long hashId, ENSource source, List<WebKol> lst, CancellationToken token);

	Task<List<WebKol>> GetAllAsync(short depId, DateTime modified, CancellationToken cancellationToken, bool loadGardeshHesab);

	Task<List<WebKol>> GetAllAsync(short depId, long hashId, int partitionId, CancellationToken cancellationToken, bool loadGardeshHesab);

	Task<List<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);

	Task<WebKol> GetAsync(Guid guid, short depId, bool loadGardeshHesab);
}
