using System;
using EntityCache.Business.Accounting.TozieList;

namespace EntityCache.Core.Repository.Accounting.TozieList;

public interface ITozieListDetailRepository : IPersistenceMasterRepository<TozieListDetailBusiness>, IDisposable
{
}
