using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_ProductRepository : IRepository<Scale_ProductBusiness>, INoGuidRepository<Scale_ProductBusiness>, IDisposable
{
}
