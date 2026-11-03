using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_PrdGroupCacheRepository : CacheRepository<Scale_PrdGroupBusiness>, IScale_PrdGroupRepository, IRepository<Scale_PrdGroupBusiness>, INoGuidRepository<Scale_PrdGroupBusiness>, IDisposable
{
	private IScale_PrdGroupRepository _dbRepository;

	public override IItemAsyncMapper<Scale_PrdGroupBusiness, Scale_PrdGroupBusiness> Mapper => Scale_PrdGroupMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_PrdGroupBusiness> TimingCache { get; set; }

	public Scale_PrdGroupCacheRepository(IScale_PrdGroupRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_PrdGroupBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_PrdGroupBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
