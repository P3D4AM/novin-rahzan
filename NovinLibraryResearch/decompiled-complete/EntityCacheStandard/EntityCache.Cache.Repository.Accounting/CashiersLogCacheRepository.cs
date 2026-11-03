using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class CashiersLogCacheRepository : SyncCacheRepository<CashiersLogBusiness>, ICashiersLogRepository, ISyncRepository<CashiersLogBusiness>, IEditableDelivery, IRepository<CashiersLogBusiness>, INoGuidRepository<CashiersLogBusiness>, IDisposable
{
	private ICashiersLogRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, CashiersLogBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<CashiersLogBusiness, CashiersLogBusiness> Mapper => CashierLogMapper.Instance;

	public new bool IsDisposed { get; private set; }

	public CashiersLogCacheRepository(ICashiersLogRepository _dbRepository, int CachingTime)
		: base((ISyncRepository<CashiersLogBusiness>)_dbRepository, CachingTime, cachingEnable: true)
	{
		dbRepository = _dbRepository;
		TimingCache = new TimingCache<Guid, CashiersLogBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\CashiersLogCacheRepository.cs", 40);
		}
	}

	public Task<List<CashiersLogBusiness>> GetAllAsync(CashierLogFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(filter, token, tranName);
	}
}
