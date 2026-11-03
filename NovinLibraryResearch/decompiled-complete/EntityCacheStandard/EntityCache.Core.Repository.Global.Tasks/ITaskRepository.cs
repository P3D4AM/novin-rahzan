using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tasks;

namespace EntityCache.Core.Repository.Global.Tasks;

public interface ITaskRepository : IRepository<TaskBusiness>, INoGuidRepository<TaskBusiness>, IDisposable
{
	Task<int> GetNextId2Async(CancellationToken token, string tranName);
}
