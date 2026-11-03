using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Accounting;

public interface ICheckedSaveRepository : IRepository<CheckedSaveBusiness>, INoGuidRepository<CheckedSaveBusiness>, IDisposable
{
	Task<IEnumerable<CheckedSaveBusiness>> GetAllAsync(checkedTablReportType reportType, Guid hesabGuid, bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> DeleteAsync(checkedTablReportType reportType, Guid TafsilGuid, DateTime d1, DateTime d2, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> DeleteAsync(checkedTablReportType reportType, Guid hesabGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> DeleteAsync(checkedTablReportType reportType, Guid hesabGuid, Guid detailGuid, short tryCount, CancellationToken token, string tranName);
}
