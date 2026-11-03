using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_SalesCacheRepository : CacheRepository<Scale_SalesBusiness>, IScale_SalesRepository, IRepository<Scale_SalesBusiness>, INoGuidRepository<Scale_SalesBusiness>, IDisposable
{
	private IScale_SalesRepository _dbRepository;

	public override IItemAsyncMapper<Scale_SalesBusiness, Scale_SalesBusiness> Mapper => Scale_SalesMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_SalesBusiness> TimingCache { get; set; }

	public Scale_SalesCacheRepository(IScale_SalesRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_SalesBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_SalesBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
