using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Tickets;

public interface ITicketDetRepository : IRepository<TicketDetBusiness>, INoGuidRepository<TicketDetBusiness>, IDisposable
{
	Task<IEnumerable<TicketReportBusiness>> GetAllAsync(TicketDetFilter filter, CancellationToken token, string tranName);
}
