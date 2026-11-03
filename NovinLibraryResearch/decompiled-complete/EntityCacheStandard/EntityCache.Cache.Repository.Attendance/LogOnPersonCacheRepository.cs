using System;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository.Attendance;
using EntityCache.Mapping.BusinessMappers.Attendance;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Attendance;

internal class LogOnPersonCacheRepository : CacheRepository<LogOnPersonBusiness>, ILogOnPersonRepository, IRepository<LogOnPersonBusiness>, INoGuidRepository<LogOnPersonBusiness>, IDisposable
{
	private ILogOnPersonRepository _dbRepository;

	public override IItemAsyncMapper<LogOnPersonBusiness, LogOnPersonBusiness> Mapper => LogOnPersonMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, LogOnPersonBusiness> TimingCache { get; set; }

	public LogOnPersonCacheRepository(ILogOnPersonRepository dbRepository, int CachingTime)
		: base((IRepository<LogOnPersonBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, LogOnPersonBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
