using System;
using EntityCache.Business.Accounting.Bill;

namespace EntityCache.Core.Repository.Accounting.Bill;

public interface ITempBillRepository : INoGuidRepository<TempBillBusiness>, IDisposable
{
}
