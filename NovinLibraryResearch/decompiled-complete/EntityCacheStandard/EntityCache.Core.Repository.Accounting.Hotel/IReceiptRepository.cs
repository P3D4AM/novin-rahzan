using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Business.Accounting.Hotel.ReportsT;
using PacketParser.Filters.Hotel;

namespace EntityCache.Core.Repository.Accounting.Hotel;

public interface IReceiptRepository : IRepository<ReceiptBusiness>, INoGuidRepository<ReceiptBusiness>, IDisposable
{
	Task<int> GetNextNumberRecipt(CancellationToken token, string tranName);

	Task<IEnumerable<ReceiptReportBusiness>> GetAllAsync(ReceiptFilter filter, CancellationToken token, string tranName);
}
