using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Models.Accounting.Prd;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Records.Kardex;

namespace EntityCache.Core.Repository.Accounting;

public interface IKardexRepository : INoGuidRepository<KardexBusiness>, IDisposable
{
	Task<decimal> LastVorodPriceAvgAsync(Guid prdGuid, DateTime? Date, Guid? depotGuid, bool? kardexAffective, CancellationToken token, string tranName);

	Task<IEnumerable<KardexBusiness>> GetAllAsync(KardexFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<IEnumerable<KardexInvalidatedModel>>> GetAllInvalidated_PrdId_DepotId_Async(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ProgressRemAsync(Guid prdGuid, Guid dptGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> FillAsync(Guid? sanadGuid, Document? sanadType, DateTime? newDate, CancellationToken token, string tranName);

	Task<KardexSumModel> GetSummaryInfoAsync(KardexSummaryFilter filter, CancellationToken token, string tranName);

	Task<List<KardexViewModel>> GetAllViewModelAsync(CancellationToken token, DateTime? date1, DateTime? date2, bool GardeshManfi, bool NoghteSefaresh, bool? kardexAffective, string tranName);

	Task<List<KardexCustomerModel>> GetAllCustomerGardeshBetweenAsync(CancellationToken token, DateTime date1, DateTime date2, bool? kardexAffective, string tranName);

	Task<List<KardexTajmieReportBusiness>> GetAllTajmieProductAsync(CancellationToken token, TajmieeProductFilter filter, string tranName);

	Task<double> GetPrdAmountAsync(Guid prdGuid, Guid depotGuid, DateTime date, bool? kardexAffective, CancellationToken token, string tranName);

	Task<List<KardexTajmieReportBusiness>> GetAllGardeshProductAsync(DateTime date1, DateTime date2, bool? kardexAffective, CancellationToken token, string tranName);

	Task<List<OrderMoghayeratModel>> GetAllOrderMoghayeratAsync(CancellationToken token, string tranName);

	Task<DateTime?> GetLastRecordDateAsync(bool? kardexAffective, CancellationToken token, string tranName);

	Task<IEnumerable<KardexLinkedOrder>> GetAllLinkSellAsync(Guid tafsilGuid, Guid productGuid, DateTime dateM, CancellationToken token, string tranName);

	Task<KardexBusiness> GetAsync(Guid productGuid, Guid depotGuid, DateTime date, CancellationToken token, string tranName);

	Task<IEnumerable<KardexConflictRecord>> GetAllConflictsAsync(MaliYearSanadType sanadType, CancellationToken token, string tranName);

	Task<KardexBusiness> GetLastRecordsByDateAsync(DateTime date, Guid prdGuid, Guid depotGuid, CancellationToken token, string tranName);

	Task<KardexBusiness> GetLastEnteredRecordAsync(Guid productGuid, Guid? depotGuid, CancellationToken token, string tranName);
}
