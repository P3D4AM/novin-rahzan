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
using PacketParser.Mapping;
using PacketParser.Records.Sanad;

namespace EntityCache.Cache.Repository.Accounting;

public class ViewSanadDetailCacheRepository : SyncCacheRepository<ViewSanadDetailBusiness>, IViewSanadDetailRepository, ISyncRepository<ViewSanadDetailBusiness>, IEditableDelivery, IRepository<ViewSanadDetailBusiness>, INoGuidRepository<ViewSanadDetailBusiness>, IDisposable
{
	private IViewSanadDetailRepository _dbRepository;

	public override IItemAsyncMapper<ViewSanadDetailBusiness, ViewSanadDetailBusiness> Mapper => ViewSanadDetailMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, ViewSanadDetailBusiness> TimingCache { get; set; }

	public ViewSanadDetailCacheRepository(IViewSanadDetailRepository dbRepo, int CachingTime)
		: base((ISyncRepository<ViewSanadDetailBusiness>)dbRepo, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepo;
		TimingCache = new TimingCache<Guid, ViewSanadDetailBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Sanad\\ViewSanadDetailCacheRepository.cs", 39);
		}
	}

	public Task<IEnumerable<ViewSanadDetailBusiness>> GetAllSanadOutOfFinancialPeriodAsync(DateTime startDate, DateTime endDate, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllSanadOutOfFinancialPeriodAsync(startDate, endDate, token, tranName);
	}

	public Task<IEnumerable<ViewSanadDetailBusiness>> GetAllAsync(int sanadNo, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(sanadNo, token, tranName);
	}

	public Task<int> GetSanadNoAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetSanadNoAsync(guid, token, tranName);
	}

	public Task<int> GetFutureSanadCountAsync(DateTime lastDate, CancellationToken token, string tranName)
	{
		return _dbRepository.GetFutureSanadCountAsync(lastDate, token, tranName);
	}

	public Task<List<ViewSanadExistRecord>> GetAllExistRecordAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllExistRecordAsync(token, tranName);
	}
}
