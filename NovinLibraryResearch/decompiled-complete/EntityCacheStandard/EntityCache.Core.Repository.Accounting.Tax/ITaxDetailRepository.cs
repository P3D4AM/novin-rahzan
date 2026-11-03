using System;
using EntityCache.Business.Accounting.Tax;

namespace EntityCache.Core.Repository.Accounting.Tax;

public interface ITaxDetailRepository : IPersistenceMasterRepository<TaxDetailBusiness>, IDisposable
{
}
