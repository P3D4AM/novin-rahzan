using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_SaleDetailRepository : IRepository<Scale_SaleDetailBusiness>, INoGuidRepository<Scale_SaleDetailBusiness>, IDisposable
{
}
