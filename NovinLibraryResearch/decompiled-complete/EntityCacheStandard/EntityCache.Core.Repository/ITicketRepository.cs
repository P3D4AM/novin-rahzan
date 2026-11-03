using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Ticket;
using PacketParser.Filters;

namespace EntityCache.Core.Repository;

public interface ITicketRepository : IRepository<TicketBusiness>, INoGuidRepository<TicketBusiness>, IDisposable
{
	Task<IEnumerable<TicketReportBusiness>> GetAllAsync(TicketFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeStatusAsync(Guid guid, short statusId, CancellationToken token, string tranName);

	Task<List<TicketTitleSuggest>> GetSuggestTitelsAsync(CancellationToken token, string tranName);

	Task<TicketBusiness> GetByNumberAsync(int number, CancellationToken token, string tranName);

	Task<int> NextNumberAsync(CancellationToken token, string tranName);
}
