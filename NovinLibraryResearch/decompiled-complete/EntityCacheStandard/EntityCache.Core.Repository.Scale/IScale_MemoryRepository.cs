using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_MemoryRepository : IRepository<Scale_MemoryBusiness>, INoGuidRepository<Scale_MemoryBusiness>, IDisposable
{
}
