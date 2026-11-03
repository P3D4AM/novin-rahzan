using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting.Order;

public interface ITaxOrderLogRepository : ISyncRepository<TaxOrderLogBusiness>, IEditableDelivery, IRepository<TaxOrderLogBusiness>, INoGuidRepository<TaxOrderLogBusiness>, IDisposable
{
	Task<IEnumerable<TaxOrderLogBusiness>> GetAllAsync(TaxOrderLogFilter filter, CancellationToken token, string tranName);

	Task<IEnumerable<TaxOrderLogReportBusiness>> GetAllReportAsync(Guid orderGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangAsync(List<Guid> lstGuid, CancellationToken token, string tranName);

	Task<short> GetNextOrdinalAsync(Guid orderGuid, CancellationToken token, string tranName);
}
