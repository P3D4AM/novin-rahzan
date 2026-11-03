using System;
using EntityCache.Business;

namespace EntityCache.Core.Repository.Accounting.Details;

public interface IPardakhtBankRepository : IPersistenceMasterRepository<PardakhtBankBusiness>, IDisposable
{
}
