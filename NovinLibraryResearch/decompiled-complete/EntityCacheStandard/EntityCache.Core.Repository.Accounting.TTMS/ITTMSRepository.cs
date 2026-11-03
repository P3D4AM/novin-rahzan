using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.TTMS;

namespace EntityCache.Core.Repository.Accounting.TTMS;

public interface ITTMSRepository : IRepository<TTMSBusiness>, INoGuidRepository<TTMSBusiness>, IDisposable
{
	Task<IEnumerable<TTMSReportBusiness>> GetAllReportAsync(CancellationToken token, string tranName);
}
