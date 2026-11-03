using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_MemCacheRepository : CacheRepository<Scale_MemBusiness>, IScale_MemRepository, IRepository<Scale_MemBusiness>, INoGuidRepository<Scale_MemBusiness>, IDisposable
{
	private IScale_MemRepository _dbRepository;

	public override IItemAsyncMapper<Scale_MemBusiness, Scale_MemBusiness> Mapper => Scale_MemMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_MemBusiness> TimingCache { get; set; }

	public Scale_MemCacheRepository(IScale_MemRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_MemBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_MemBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
