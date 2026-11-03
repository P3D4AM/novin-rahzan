using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_DateSH_HCacheRepository : CacheRepository<Scale_DateSH_HBusiness>, IScale_DateSH_HRepository, IRepository<Scale_DateSH_HBusiness>, INoGuidRepository<Scale_DateSH_HBusiness>, IDisposable
{
	private IScale_DateSH_HRepository _dbRepository;

	public override IItemAsyncMapper<Scale_DateSH_HBusiness, Scale_DateSH_HBusiness> Mapper => Scale_DateSH_HMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_DateSH_HBusiness> TimingCache { get; set; }

	public Scale_DateSH_HCacheRepository(IScale_DateSH_HRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_DateSH_HBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_DateSH_HBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
