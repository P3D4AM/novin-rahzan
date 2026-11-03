using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Models.Accounting.Prd;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;
using PacketParser.Records.Kardex;

namespace EntityCache.Cache.Repository.Accounting;

public class KardexCacheRepository : CacheRepositoryNoGuiId<KardexBusiness, string>, IKardexRepository, INoGuidRepository<KardexBusiness>, IDisposable
{
	private IKardexRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<string, KardexBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<KardexBusiness, KardexBusiness> Mapper => KardexMapper.Instance;

	public KardexCacheRepository(INoGuidRepository<KardexBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = (IKardexRepository)dbRepository;
		TimingCache = new TimingCache<string, KardexBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<decimal> LastVorodPriceAvgAsync(Guid prdGuid, DateTime? Date, Guid? depotGuid, bool? kardexAffective, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.LastVorodPriceAvgAsync(prdGuid, Date, depotGuid, kardexAffective, token, tranName);
	}

	public Task<IEnumerable<KardexBusiness>> GetAllAsync(KardexFilter filter, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<ReturnedSaveFuncInfoWithValue<IEnumerable<KardexInvalidatedModel>>> GetAllInvalidated_PrdId_DepotId_Async(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllInvalidated_PrdId_DepotId_Async(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ProgressRemAsync(Guid prdGuid, Guid dptGuid, CancellationToken token, string tranName)
	{
		return dbRepository.ProgressRemAsync(prdGuid, dptGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> FillAsync(Guid? sanadGuid, Document? sanadType, DateTime? newDate, CancellationToken token, string tranName)
	{
		return dbRepository.FillAsync(sanadGuid, sanadType, newDate, token, tranName);
	}

	public Task<List<KardexCustomerModel>> GetAllCustomerGardeshBetweenAsync(CancellationToken token, DateTime date1, DateTime date2, bool? kardexAffective, string tranName)
	{
		return dbRepository.GetAllCustomerGardeshBetweenAsync(token, date1, date2, kardexAffective, tranName);
	}

	public Task<List<KardexTajmieReportBusiness>> GetAllTajmieProductAsync(CancellationToken token, TajmieeProductFilter filter, string tranName)
	{
		return dbRepository.GetAllTajmieProductAsync(token, filter, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\KardexCacheRepository.cs", 62);
		}
	}

	public Task<KardexSumModel> GetSummaryInfoAsync(KardexSummaryFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetSummaryInfoAsync(filter, token, tranName);
	}

	public Task<List<KardexViewModel>> GetAllViewModelAsync(CancellationToken token, DateTime? date1, DateTime? date2, bool GardeshManfi, bool NoghteSefaresh, bool? kardexAffective, string tranName)
	{
		return dbRepository.GetAllViewModelAsync(token, date1, date2, GardeshManfi, NoghteSefaresh, kardexAffective, tranName);
	}

	public Task<double> GetPrdAmountAsync(Guid prdGuid, Guid depotGuid, DateTime date, bool? kardexAffective, CancellationToken token, string tranName)
	{
		return dbRepository.GetPrdAmountAsync(prdGuid, depotGuid, date, kardexAffective, token, tranName);
	}

	public Task<List<KardexTajmieReportBusiness>> GetAllGardeshProductAsync(DateTime date1, DateTime date2, bool? kardexAffective, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllGardeshProductAsync(date1, date2, kardexAffective, token, tranName);
	}

	public Task<List<OrderMoghayeratModel>> GetAllOrderMoghayeratAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllOrderMoghayeratAsync(token, tranName);
	}

	public Task<DateTime?> GetLastRecordDateAsync(bool? kardexAffective, CancellationToken token, string tranName)
	{
		return dbRepository.GetLastRecordDateAsync(kardexAffective, token, tranName);
	}

	public Task<IEnumerable<KardexLinkedOrder>> GetAllLinkSellAsync(Guid tafsilGuid, Guid productGuid, DateTime dateM, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllLinkSellAsync(tafsilGuid, productGuid, dateM, token, tranName);
	}

	public Task<KardexBusiness> GetAsync(Guid productGuid, Guid depotGuid, DateTime date, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(productGuid, depotGuid, date, token, tranName);
	}

	public Task<IEnumerable<KardexConflictRecord>> GetAllConflictsAsync(MaliYearSanadType sanadType, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllConflictsAsync(sanadType, token, tranName);
	}

	public Task<KardexBusiness> GetLastRecordsByDateAsync(DateTime date, Guid prdGuid, Guid depotGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetLastRecordsByDateAsync(date, prdGuid, depotGuid, token, tranName);
	}

	public Task<KardexBusiness> GetLastEnteredRecordAsync(Guid productGuid, Guid? depotGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetLastEnteredRecordAsync(productGuid, depotGuid, token, tranName);
	}
}
