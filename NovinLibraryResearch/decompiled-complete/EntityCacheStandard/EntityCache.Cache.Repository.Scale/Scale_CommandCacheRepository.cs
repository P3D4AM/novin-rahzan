using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_CommandCacheRepository : CacheRepository<Scale_CommandBusiness>, IScale_CommandRepository, IRepository<Scale_CommandBusiness>, INoGuidRepository<Scale_CommandBusiness>, IDisposable
{
	private IScale_CommandRepository _dbRepository;

	public override IItemAsyncMapper<Scale_CommandBusiness, Scale_CommandBusiness> Mapper => Scale_CommandMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_CommandBusiness> TimingCache { get; set; }

	public Scale_CommandCacheRepository(IScale_CommandRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_CommandBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_CommandBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
