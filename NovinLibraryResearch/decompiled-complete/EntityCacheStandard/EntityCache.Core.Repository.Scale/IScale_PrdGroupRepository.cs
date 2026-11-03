using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_PrdGroupRepository : IRepository<Scale_PrdGroupBusiness>, INoGuidRepository<Scale_PrdGroupBusiness>, IDisposable
{
}
