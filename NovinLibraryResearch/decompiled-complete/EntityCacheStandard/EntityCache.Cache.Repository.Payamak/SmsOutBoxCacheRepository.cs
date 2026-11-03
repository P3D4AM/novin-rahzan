using System;
using EntityCache.Business.Accounting.Payamak;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using EntityCache.Mapping.BusinessMappers.Payamak;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Payamak;

public class SmsOutBoxCacheRepository : CacheRepository<SmsOutBoxBusiness>, ISmsOutBoxRepository, IRepository<SmsOutBoxBusiness>, INoGuidRepository<SmsOutBoxBusiness>, IDisposable
{
	private ISmsOutBoxRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<SmsOutBoxBusiness, SmsOutBoxBusiness> Mapper => SmsOutBoxMapper.Instance;

	protected override TimingCache<Guid, SmsOutBoxBusiness> TimingCache { get; set; }

	public SmsOutBoxCacheRepository(ISmsOutBoxRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<SmsOutBoxBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, SmsOutBoxBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Payamak\\SmsOutBoxCacheRepository.cs", 37);
		}
	}
}
