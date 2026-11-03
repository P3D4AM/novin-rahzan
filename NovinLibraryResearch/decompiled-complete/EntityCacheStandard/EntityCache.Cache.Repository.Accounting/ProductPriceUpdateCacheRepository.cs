using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class ProductPriceUpdateCacheRepository : CacheRepository<ProductPriceUpdateBusiness>, IProductPriceUpdateRepository, IRepository<ProductPriceUpdateBusiness>, INoGuidRepository<ProductPriceUpdateBusiness>, IDisposable
{
	private IProductPriceUpdateRepository _dbRepository;

	public override IItemAsyncMapper<ProductPriceUpdateBusiness, ProductPriceUpdateBusiness> Mapper => ProductPriceUpdateMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => true;

	protected override TimingCache<Guid, ProductPriceUpdateBusiness> TimingCache { get; set; }

	public ProductPriceUpdateCacheRepository(IProductPriceUpdateRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<ProductPriceUpdateBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ProductPriceUpdateBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<ReturnedSaveFuncInfo> InsertBatchAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.InsertBatchAsync(token, tranName);
	}

	public Task<IEnumerable<ProductPriceUpdateModel>> GetAllModelAsync(Guid productGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllModelAsync(productGuid, token, tranName);
	}
}
