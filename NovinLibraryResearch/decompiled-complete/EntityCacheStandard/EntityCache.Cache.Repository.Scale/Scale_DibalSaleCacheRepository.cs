using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_DibalSaleCacheRepository : CacheRepository<Scale_DibalSaleBusiness>, IScale_DibalSaleRepository, IRepository<Scale_DibalSaleBusiness>, INoGuidRepository<Scale_DibalSaleBusiness>, IDisposable
{
	private IScale_DibalSaleRepository _dbRepository;

	public override IItemAsyncMapper<Scale_DibalSaleBusiness, Scale_DibalSaleBusiness> Mapper => Scale_DibalSaleMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_DibalSaleBusiness> TimingCache { get; set; }

	public Scale_DibalSaleCacheRepository(IScale_DibalSaleRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_DibalSaleBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_DibalSaleBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
