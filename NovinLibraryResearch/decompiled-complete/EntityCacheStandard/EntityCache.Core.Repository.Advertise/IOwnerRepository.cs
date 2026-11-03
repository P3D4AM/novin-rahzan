using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface IOwnerRepository : IRepository<OwnerBusiness>, INoGuidRepository<OwnerBusiness>, IDisposable
{
	Task<IEnumerable<OwnerBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName);

	Task<int> DeleteCountAsync(string tranName);

	Task<bool> CheckNameAsync(string name, Guid guid, string tranName);
}
