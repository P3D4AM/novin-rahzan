using System;
using EntityCache.Business.Global.Tickets;

namespace EntityCache.Core.Repository.Global.Ticket;

public interface ITicketPartDetailRepository : INoGuidRepository<TicketPartDetailBusiness>, IDisposable
{
}
