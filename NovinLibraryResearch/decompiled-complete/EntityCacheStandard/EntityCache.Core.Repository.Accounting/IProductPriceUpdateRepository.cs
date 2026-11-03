using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;

namespace EntityCache.Core.Repository.Accounting;

public interface IProductPriceUpdateRepository : IRepository<ProductPriceUpdateBusiness>, INoGuidRepository<ProductPriceUpdateBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> InsertBatchAsync(CancellationToken token, string tranName);

	Task<IEnumerable<ProductPriceUpdateModel>> GetAllModelAsync(Guid productGuid, CancellationToken token, string tranName);
}
