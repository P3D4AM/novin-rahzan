using System;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Coding;
using EntityCache.Mapping.BusinessMappers.Accounting.Coding;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Coding;

public class AccountLogCacheRepository : CacheRepository<AccountLogBusiness>, IAccountLogRepository, IRepository<AccountLogBusiness>, INoGuidRepository<AccountLogBusiness>, IDisposable
{
	private IAccountLogRepository _dbRepository;

	public override IItemAsyncMapper<AccountLogBusiness, AccountLogBusiness> Mapper => AccountLogMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => true;

	protected override TimingCache<Guid, AccountLogBusiness> TimingCache { get; set; }

	public AccountLogCacheRepository(IAccountLogRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<AccountLogBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AccountLogBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
