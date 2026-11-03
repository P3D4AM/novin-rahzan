using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_InfoCacheRepository : CacheRepository<Scale_InfoBusiness>, IScale_InfoRepository, IRepository<Scale_InfoBusiness>, INoGuidRepository<Scale_InfoBusiness>, IDisposable
{
	private IScale_InfoRepository _dbRepository;

	public override IItemAsyncMapper<Scale_InfoBusiness, Scale_InfoBusiness> Mapper => Scale_InfoMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_InfoBusiness> TimingCache { get; set; }

	public Scale_InfoCacheRepository(IScale_InfoRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_InfoBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_InfoBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
