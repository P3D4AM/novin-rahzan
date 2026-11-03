using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting;

public interface IResidRepository : IRepository<ResidBusiness>, INoGuidRepository<ResidBusiness>, IDisposable
{
	Task<ResidBusiness> GetAsync(long Id2, CancellationToken token, string tranName);

	Task<IEnumerable<ResidCompactBusiness>> GetAllCompactAsync(ResidFilter filter, CancellationToken token, string tranName);

	Task<long> GetNextId2Async(int? statusId, CancellationToken token, string tranName);

	Task<ResidBusiness> GetByOrderGuidAsync(Guid guid, CancellationToken token, string tranName);

	Task<ResidBusiness> GetByReceptionGuidAsync(Guid guid, CancellationToken token, string tranName);

	Task<List<string>> GetSuggestionListAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName);
}
