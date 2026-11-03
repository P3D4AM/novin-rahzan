using System;
using EntityCache.Business.Accounting.Factor;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Factor;
using EntityCache.Mapping.BusinessMappers.Accounting.Factor;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Factor;

public class SanadStatusLogCacheRepository : CacheRepository<SanadStatusLogBusiness>, ISanadStatusLogRepository, IRepository<SanadStatusLogBusiness>, INoGuidRepository<SanadStatusLogBusiness>, IDisposable
{
	public ISanadStatusLogRepository _dbRepository;

	public override IItemAsyncMapper<SanadStatusLogBusiness, SanadStatusLogBusiness> Mapper => SanadStatusLogMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => true;

	protected override TimingCache<Guid, SanadStatusLogBusiness> TimingCache { get; set; }

	public SanadStatusLogCacheRepository(IRepository<SanadStatusLogBusiness> dbRepository, int CachingTime, bool cachingEnable = true)
		: base(dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = (ISanadStatusLogRepository)dbRepository;
		TimingCache = new TimingCache<Guid, SanadStatusLogBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
