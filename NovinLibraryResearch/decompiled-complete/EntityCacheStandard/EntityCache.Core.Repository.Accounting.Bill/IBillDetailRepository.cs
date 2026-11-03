using System;
using EntityCache.Business.Accounting.Bill;

namespace EntityCache.Core.Repository.Accounting.Bill;

public interface IBillDetailRepository : IPersistenceMasterRepository<BillDetailBusiness>, IDisposable
{
}
