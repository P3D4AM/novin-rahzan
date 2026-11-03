using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid.Summary;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting.Tolid;

public interface ITolidSummaryRepository : IRepository<TolidSummaryBusiness>, INoGuidRepository<TolidSummaryBusiness>, IDisposable
{
	Task<IEnumerable<TolidSummaryReportBusiness>> GetAllAsync(TolidSummaryFilter filter, CancellationToken token, string tranName);

	Task<int> NextNumberAsync(CancellationToken token, string tranName);

	Task<List<TolidSummaryDetailBusiness>> GetAllAsync(TolidSummaryBusiness master, DateTime dateM, CancellationToken token, string tranName);
}
