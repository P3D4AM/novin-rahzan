using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_InfoRepository : IRepository<Scale_InfoBusiness>, INoGuidRepository<Scale_InfoBusiness>, IDisposable
{
}
