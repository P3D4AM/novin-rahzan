using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalTafsil
{
	Task<List<SaveResponse>> SaveAsync(short depId, long hashId, ENSource source, IEnumerable<WebTafsil> lst, CancellationToken token);

	Task<List<WebTafsil>> GetAllAsync(short depId, DateTime modified, CancellationToken cancellationToken, bool loadGardeshHesab);

	Task<List<WebTafsil>> GetAllAsync(short depId, long hashId, int partitionId, CancellationToken cancellationToken, bool loadGardeshHesab);

	Task<List<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);

	Task<WebTafsil> GetAsync(Guid guid, short depId, bool loadGardeshHesab, CancellationToken token);

	Task<List<WebTafsil>> GetTafsilFromAllDepartments(Guid guid, CancellationToken token);
}
