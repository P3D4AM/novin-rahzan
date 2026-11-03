using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_SellDetailRepository : IRepository<Scale_SellDetailBusiness>, INoGuidRepository<Scale_SellDetailBusiness>, IDisposable
{
}
