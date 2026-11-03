using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface IStateRepository : IRepository<StateBusiness>, INoGuidRepository<StateBusiness>, IDisposable
{
	Task<List<StateBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName);
}
