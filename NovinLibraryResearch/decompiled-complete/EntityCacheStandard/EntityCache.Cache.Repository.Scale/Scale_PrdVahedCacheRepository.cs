using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_PrdVahedCacheRepository : CacheRepository<Scale_PrdVahedBusiness>, IScale_PrdVahedRepository, IRepository<Scale_PrdVahedBusiness>, INoGuidRepository<Scale_PrdVahedBusiness>, IDisposable
{
	private IScale_PrdVahedRepository _dbRepository;

	public override IItemAsyncMapper<Scale_PrdVahedBusiness, Scale_PrdVahedBusiness> Mapper => Scale_PrdVahedMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_PrdVahedBusiness> TimingCache { get; set; }

	public Scale_PrdVahedCacheRepository(IScale_PrdVahedRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_PrdVahedBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_PrdVahedBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
