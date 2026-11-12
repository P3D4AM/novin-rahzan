using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.CompositeKeys;
using PacketParser.EventArgs;

namespace Server.Core;

public interface ILastPriceRepository
{
	Task<ReturnedSaveFuncInfoWithValue<WebLastPrice>> GetLastPriceAsync(short depId, Guid prdGuid, Guid tafsilGuid);

	Task<List<SaveResponse<LastPriceCompositeKey>>> SaveAllAsync(IEnumerable<WebLastPrice> lst, short depId, CancellationToken token, bool flush, short tryCount, ENSource source);

	Task<IEnumerable<WebLastPrice>> GetAllAsync(short depId, long hashId, short partition, CancellationToken cancellationToken);

	Task<IEnumerable<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);
}
