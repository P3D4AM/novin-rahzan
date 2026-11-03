using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_MemoryHistoryRepository : IRepository<Scale_MemoryHistoryBusiness>, INoGuidRepository<Scale_MemoryHistoryBusiness>, IDisposable
{
}
