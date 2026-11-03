using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_ProductCacheRepository : CacheRepository<Scale_ProductBusiness>, IScale_ProductRepository, IRepository<Scale_ProductBusiness>, INoGuidRepository<Scale_ProductBusiness>, IDisposable
{
	private IScale_ProductRepository _dbRepository;

	public override IItemAsyncMapper<Scale_ProductBusiness, Scale_ProductBusiness> Mapper => Scale_ProductMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_ProductBusiness> TimingCache { get; set; }

	public Scale_ProductCacheRepository(IScale_ProductRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_ProductBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_ProductBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
