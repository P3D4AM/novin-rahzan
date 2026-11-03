using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Coding.ReportsT;
using PacketParser.Filters.Hotel;

namespace EntityCache.Core.Repository.Accounting;

public interface IHesabGroupRepository : IRepository<HesabGroupBusiness>, INoGuidRepository<HesabGroupBusiness>, IDisposable
{
	Task<IEnumerable<HesabGroupBusiness>> GetAllAsync(Guid? hesabTypeGuid, bool? status, CancellationToken token, string tranName);

	Task<int> GetNextCodeHesabGroupAsync(Guid typeGuid, CancellationToken token, string tranName);

	Task<IEnumerable<HesabGroupReportBusiness>> GetAllAsync(HesabGroupFilter filter, CancellationToken token = default(CancellationToken), string tranName = "");
}
