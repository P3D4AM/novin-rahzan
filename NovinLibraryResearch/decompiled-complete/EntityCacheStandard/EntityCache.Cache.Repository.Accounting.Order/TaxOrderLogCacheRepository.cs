using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Order;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Order;

public class TaxOrderLogCacheRepository : SyncCacheRepository<TaxOrderLogBusiness>, ITaxOrderLogRepository, ISyncRepository<TaxOrderLogBusiness>, IEditableDelivery, IRepository<TaxOrderLogBusiness>, INoGuidRepository<TaxOrderLogBusiness>, IDisposable
{
	private ITaxOrderLogRepository _dbRepository;

	public override IItemAsyncMapper<TaxOrderLogBusiness, TaxOrderLogBusiness> Mapper => TaxOrderLogMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<Guid, TaxOrderLogBusiness> TimingCache { get; set; }

	public TaxOrderLogCacheRepository(ITaxOrderLogRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<TaxOrderLogBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TaxOrderLogBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\TaxOrderLogCacheRepository.cs", 40);
		}
	}

	public Task<IEnumerable<TaxOrderLogBusiness>> GetAllAsync(TaxOrderLogFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<IEnumerable<TaxOrderLogReportBusiness>> GetAllReportAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllReportAsync(orderGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangAsync(List<Guid> lstGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveRangAsync(lstGuid, token, tranName);
	}

	public Task<short> GetNextOrdinalAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextOrdinalAsync(orderGuid, token, tranName);
	}
}
