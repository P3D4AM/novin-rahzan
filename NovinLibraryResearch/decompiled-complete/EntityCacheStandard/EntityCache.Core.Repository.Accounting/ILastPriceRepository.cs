using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.CompositeKeys;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace EntityCache.Core.Repository.Accounting;

public interface ILastPriceRepository : IHasContext, INoGuidRepository<LastPriceBusiness>, IDisposable
{
	Task<LastPriceBusiness> GetAsync(LastPriceCompositeKey key, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAllAsync(List<LastPriceBusiness> lst, bool flush, short tryCount, CancellationToken token, string tranName);

	Task<IEnumerable<LastPriceBusiness>> GetAllAsync(Guid productGuid, Guid tafsilGuid, Document asType, CancellationToken token, string tranName);

	Task<IEnumerable<LastPriceBusiness>> GetAllAsync(Guid orderGuid, Document asType, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse<LastPriceCompositeKey> item, CancellationToken token, short tryCount, string tranName);

	Task<List<LastPriceCompositeKey>> GetAllNotSentAsync(CancellationToken token, string tranName);
}
