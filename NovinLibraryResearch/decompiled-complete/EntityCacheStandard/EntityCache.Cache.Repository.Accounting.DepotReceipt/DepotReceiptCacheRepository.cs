using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DepotReceipt;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.DepotReceipt;
using EntityCache.Mapping.BusinessMappers.Accounting.DepotReceipt;
using EntityCache.Models.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.DepotReceipt;

public class DepotReceiptCacheRepository : CacheRepository<DepotReceiptBusiness>, IDepotReceiptRepository, IRepository<DepotReceiptBusiness>, INoGuidRepository<DepotReceiptBusiness>, IDisposable
{
	private IDepotReceiptRepository _dbRepository;

	public override IItemAsyncMapper<DepotReceiptBusiness, DepotReceiptBusiness> Mapper => DepotReceiptMapper.GetInstance();

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, DepotReceiptBusiness> TimingCache { get; set; }

	public DepotReceiptCacheRepository(IDepotReceiptRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<DepotReceiptBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, DepotReceiptBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\DepotReceipt\\DepotReceiptCacheRepository.cs", 40);
		}
	}

	public Task<int> GetNextNumberAsync(EnDepotReceiptType type, CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextNumberAsync(type, token, tranName);
	}

	public Task<Guid> GetGuidById2Async(EnDepotReceiptType type, int id2, CancellationToken token, string tranName)
	{
		return _dbRepository.GetGuidById2Async(type, id2, token, tranName);
	}

	public Task<IEnumerable<DepotReceiptReportBusiness>> GetAllAsync(DepotReceiptFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateRefrenceGuidAsync(List<SanadAttchmentModel> model, CancellationToken token, string tranName)
	{
		return _dbRepository.UpdateRefrenceGuidAsync(model, token, tranName);
	}

	public Task<List<Guid>> GetAllByOrderGuidAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllByOrderGuidAsync(orderGuid, token, tranName);
	}
}
