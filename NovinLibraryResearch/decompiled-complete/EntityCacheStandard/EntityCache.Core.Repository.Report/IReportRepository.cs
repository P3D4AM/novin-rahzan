using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Report;
using EntityCache.Models.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.DepartmentEntitySummery;
using PacketParser.Filters;
using PacketParser.Records;
using PacketParser.Records.Kardex;
using PacketParser.Records.OccasionCalendar;

namespace EntityCache.Core.Repository.Report;

public interface IReportRepository : IDisposable
{
	Task<IEnumerable<BedehiTalabModel>> GetBedehiTalabReportAsync(EnBedehiTalabType type, DateTime? date1, DateTime? date2, short status, CancellationToken token, string tranName);

	Task<List<TarazHesabBusiness>> GetAllTarazHesabAsync(CancellationToken token, TarazHesabFilter filter, string tranName);

	Task<IEnumerable<AccountTreeModel>> GetAllAccountTreeAsync(CancellationToken token, string tranName);

	Task<PayamakManage> GetPayamakManageAsync(DateTime d1, DateTime d2, CancellationToken token, string tranName);

	Task<IEnumerable<SumKolBusiness>> GetSumKolListAsync(SumKolFilter filter, CancellationToken token, string tranName);

	Task<WebDepartmentEntitySummery> GetDepartmentEntitySummeryAsync(CancellationToken token, string tranName);

	Task<List<OccasionCalendarRecord>> GetOccasionCalendarListAsync(CancellationToken token, string tranName);

	Task<List<ServerStatusReportModel>> GetServerStatusAsync(ENSource source, List<ServerStatusForClient> serverStatuses, CancellationToken token, string tranName);

	Task<IEnumerable<KardexAndAccountConflictRecord>> GetAllKardexAndAccountConflictAsync(Guid moeinGuid, CancellationToken token, string tranName);

	Task<IEnumerable<DocumentRoundMismatchRecord>> GetAllDocumentRoundMismatchAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(List<ServerStatusReportModel> list, CancellationToken token, string tranName);
}
