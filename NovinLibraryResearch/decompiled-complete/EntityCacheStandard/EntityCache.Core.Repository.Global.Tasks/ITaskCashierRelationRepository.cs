using System;
using EntityCache.Business.Global.Tasks;

namespace EntityCache.Core.Repository.Global.Tasks;

public interface ITaskCashierRelationRepository : INoGuidRepository<TaskCashierRelationBusiness>, IDisposable
{
}
