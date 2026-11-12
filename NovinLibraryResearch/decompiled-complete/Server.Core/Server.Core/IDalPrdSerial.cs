using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalPrdSerial
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashid, ENSource source, List<WebPrdSerial> lst, CancellationToken token);

	Task<WebPrdSerial> LoadDataAsync(short DepId, Guid guid);

	Task<List<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);

	Task<List<WebPrdSerial>> GetAllAsync(short depId, long hashId, short partition, CancellationToken cancellationToken);
}
