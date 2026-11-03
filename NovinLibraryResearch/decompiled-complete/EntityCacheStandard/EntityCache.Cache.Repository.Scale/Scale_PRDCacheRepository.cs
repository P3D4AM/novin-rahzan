using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_PRDCacheRepository : CacheRepository<Scale_PRDBusiness>, IScale_PRDRepository, IRepository<Scale_PRDBusiness>, INoGuidRepository<Scale_PRDBusiness>, IDisposable
{
	private IScale_PRDRepository _dbRepository;

	public override IItemAsyncMapper<Scale_PRDBusiness, Scale_PRDBusiness> Mapper => Scale_PRDMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_PRDBusiness> TimingCache { get; set; }

	public Scale_PRDCacheRepository(IScale_PRDRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_PRDBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_PRDBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
