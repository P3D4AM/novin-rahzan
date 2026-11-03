using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_DibalSaleRepository : IRepository<Scale_DibalSaleBusiness>, INoGuidRepository<Scale_DibalSaleBusiness>, IDisposable
{
}
