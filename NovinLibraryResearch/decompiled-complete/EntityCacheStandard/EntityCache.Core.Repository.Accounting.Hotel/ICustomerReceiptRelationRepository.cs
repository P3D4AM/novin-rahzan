using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;

namespace EntityCache.Core.Repository.Accounting.Hotel;

public interface ICustomerReceiptRelationRepository : INoGuidRepository<CustomerReceiptRelationBusiness>, IDisposable
{
	Task<IEnumerable<CustomerReceiptRelationBusiness>> GetAllAsync(Guid receiptGuid, CancellationToken token, string tranName);
}
