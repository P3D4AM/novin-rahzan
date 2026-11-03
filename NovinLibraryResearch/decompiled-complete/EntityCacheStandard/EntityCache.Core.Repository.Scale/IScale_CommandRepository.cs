using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_CommandRepository : IRepository<Scale_CommandBusiness>, INoGuidRepository<Scale_CommandBusiness>, IDisposable
{
}
