using System;
using EntityCache.Business.Accounting.DepotReceipt;

namespace EntityCache.Core.Repository.Accounting.DepotReceipt;

public interface IDepotReceiptDetailRepository : IPersistenceMasterRepository<DepotReceiptDetailBusiness>, IDisposable
{
}
