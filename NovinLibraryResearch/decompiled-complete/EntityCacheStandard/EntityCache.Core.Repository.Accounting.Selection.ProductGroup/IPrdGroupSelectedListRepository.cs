using System;
using EntityCache.Business.Accounting.Selection.ProductGroup;

namespace EntityCache.Core.Repository.Accounting.Selection.ProductGroup;

public interface IPrdGroupSelectedListRepository : IRepository<PrdGroupSelectedListBusiness>, INoGuidRepository<PrdGroupSelectedListBusiness>, IDisposable
{
}
