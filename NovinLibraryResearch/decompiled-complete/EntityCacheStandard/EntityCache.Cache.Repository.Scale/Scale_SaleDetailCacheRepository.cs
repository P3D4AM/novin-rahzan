using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_SaleDetailCacheRepository : CacheRepository<Scale_SaleDetailBusiness>, IScale_SaleDetailRepository, IRepository<Scale_SaleDetailBusiness>, INoGuidRepository<Scale_SaleDetailBusiness>, IDisposable
{
	private IScale_SaleDetailRepository _dbRepository;

	public override IItemAsyncMapper<Scale_SaleDetailBusiness, Scale_SaleDetailBusiness> Mapper => Scale_SaleDetailMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_SaleDetailBusiness> TimingCache { get; set; }

	public Scale_SaleDetailCacheRepository(IScale_SaleDetailRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_SaleDetailBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_SaleDetailBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
