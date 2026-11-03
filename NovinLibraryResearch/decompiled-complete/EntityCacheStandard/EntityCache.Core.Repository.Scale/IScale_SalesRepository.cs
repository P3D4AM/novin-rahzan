using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_SalesRepository : IRepository<Scale_SalesBusiness>, INoGuidRepository<Scale_SalesBusiness>, IDisposable
{
}
