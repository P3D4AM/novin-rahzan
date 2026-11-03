using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Filters.Customer;

namespace EntityCache.Core.Repository;

public interface ICustomerCompactRepository : ICompactRepository<CusCompactBusiness>, IDisposable
{
	Task<CusCompactBusiness> GetAsync(long code, List<Guid> lstMoeinGuids, CancellationToken token, string tranName);

	Task<int> CountWithThisTitleAsync(Guid titleGuid, CancellationToken token, string tranName);

	void ResetCache(CancellationToken token, string tranName);

	Task UpdateAccountAsync(Guid guid, decimal account, CancellationToken token, string tranName);

	Task UpdateSumAghsatAsync(Guid guid, decimal account, CancellationToken token, string tranName);

	Task<IEnumerable<CusCompactBusiness>> GetAllAsync(CustomerCompactFilter filter, CancellationToken token, string tranName);

	Task<List<CusCompactBusiness>> GetAllAsync(List<Guid> lstMoeinGuids, bool? status, CancellationToken token, string tranName);

	Task<CusCompactBusiness> GetAsync(Guid guid, List<Guid> lstMoeinGuids, CancellationToken token, string tranName);

	CusCompactBusiness Get(Guid guid, List<Guid> lstMoeinGuids, CancellationToken token, string tranName);

	Task<IEnumerable<CusCompactBusiness>> GetAllForSendToServerSniftsAsync(CancellationToken token, string tranName);
}
