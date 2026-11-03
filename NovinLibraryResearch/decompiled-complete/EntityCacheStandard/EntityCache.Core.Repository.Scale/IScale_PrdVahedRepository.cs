using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_PrdVahedRepository : IRepository<Scale_PrdVahedBusiness>, INoGuidRepository<Scale_PrdVahedBusiness>, IDisposable
{
}
