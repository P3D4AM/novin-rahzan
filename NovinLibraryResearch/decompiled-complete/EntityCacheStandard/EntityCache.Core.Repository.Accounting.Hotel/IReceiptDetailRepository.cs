using System;
using EntityCache.Business.Accounting.Hotel;

namespace EntityCache.Core.Repository.Accounting.Hotel;

public interface IReceiptDetailRepository : IPersistenceMasterRepository<ReceiptDetailBusiness>, IDisposable
{
}
