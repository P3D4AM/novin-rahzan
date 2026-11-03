using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_MemRepository : IRepository<Scale_MemBusiness>, INoGuidRepository<Scale_MemBusiness>, IDisposable
{
}
