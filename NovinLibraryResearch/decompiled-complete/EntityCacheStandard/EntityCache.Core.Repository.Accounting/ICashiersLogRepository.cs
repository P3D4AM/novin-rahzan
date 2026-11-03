using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting;

public interface ICashiersLogRepository : ISyncRepository<CashiersLogBusiness>, IEditableDelivery, IRepository<CashiersLogBusiness>, INoGuidRepository<CashiersLogBusiness>, IDisposable
{
	Task<List<CashiersLogBusiness>> GetAllAsync(CashierLogFilter filter, CancellationToken token, string tranName);
}
