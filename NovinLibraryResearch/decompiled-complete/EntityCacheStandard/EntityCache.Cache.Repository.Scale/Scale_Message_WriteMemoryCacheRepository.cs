using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_Message_WriteMemoryCacheRepository : CacheRepository<Scale_Message_WriteMemoryBusiness>, IScale_Message_WriteMemoryRepository, IRepository<Scale_Message_WriteMemoryBusiness>, INoGuidRepository<Scale_Message_WriteMemoryBusiness>, IDisposable
{
	private IScale_Message_WriteMemoryRepository _dbRepository;

	public override IItemAsyncMapper<Scale_Message_WriteMemoryBusiness, Scale_Message_WriteMemoryBusiness> Mapper => Scale_Message_WriteMemoryMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_Message_WriteMemoryBusiness> TimingCache { get; set; }

	public Scale_Message_WriteMemoryCacheRepository(IScale_Message_WriteMemoryRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_Message_WriteMemoryBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_Message_WriteMemoryBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
