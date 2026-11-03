using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_MemoryHistoryCacheRepository : CacheRepository<Scale_MemoryHistoryBusiness>, IScale_MemoryHistoryRepository, IRepository<Scale_MemoryHistoryBusiness>, INoGuidRepository<Scale_MemoryHistoryBusiness>, IDisposable
{
	private IScale_MemoryHistoryRepository _dbRepository;

	public override IItemAsyncMapper<Scale_MemoryHistoryBusiness, Scale_MemoryHistoryBusiness> Mapper => Scale_MemoryHistoryMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_MemoryHistoryBusiness> TimingCache { get; set; }

	public Scale_MemoryHistoryCacheRepository(IScale_MemoryHistoryRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_MemoryHistoryBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_MemoryHistoryBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
