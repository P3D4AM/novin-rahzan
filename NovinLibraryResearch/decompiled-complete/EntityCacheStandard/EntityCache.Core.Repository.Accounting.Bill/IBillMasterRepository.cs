using System;
using EntityCache.Business.Accounting.Bill;

namespace EntityCache.Core.Repository.Accounting.Bill;

public interface IBillMasterRepository : IRepository<BillMasterBusiness>, INoGuidRepository<BillMasterBusiness>, IDisposable
{
}
