using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_MemoryCacheRepository : CacheRepository<Scale_MemoryBusiness>, IScale_MemoryRepository, IRepository<Scale_MemoryBusiness>, INoGuidRepository<Scale_MemoryBusiness>, IDisposable
{
	private IScale_MemoryRepository _dbRepository;

	public override IItemAsyncMapper<Scale_MemoryBusiness, Scale_MemoryBusiness> Mapper => Scale_MemoryMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_MemoryBusiness> TimingCache { get; set; }

	public Scale_MemoryCacheRepository(IScale_MemoryRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_MemoryBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_MemoryBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
