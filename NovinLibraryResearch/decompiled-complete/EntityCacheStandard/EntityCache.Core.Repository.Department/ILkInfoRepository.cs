using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Department;
using PacketParser.DepartmentApiModel;

namespace EntityCache.Core.Repository.Department;

public interface ILkInfoRepository : INoGuidRepository<LkInfoBusiness>, IDisposable
{
	Task<LkInfoBusiness> GetAsync(long lkSerial, CancellationToken token, string transaction);

	Task<List<LkInfoBusiness>> GetAllAsync(LkModel filters, CancellationToken token, string tranName);

	Task<int> GetAllGuidsDontLock();

	Task<int> GetAllGuidsHasLock();

	Task<List<long>> GetAllNotSentAsync(string tranName);
}
