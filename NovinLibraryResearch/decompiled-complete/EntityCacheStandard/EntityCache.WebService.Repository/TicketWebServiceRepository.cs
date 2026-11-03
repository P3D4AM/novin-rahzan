using System;
using System.Threading.Tasks;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class TicketWebServiceRepository : WebServiceRepository<TicketBusiness>, IDisposable
{
	public TicketWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<TicketBusiness>)cache.CacheRepository.Ticket, cache)
	{
	}

	public override void Dispose()
	{
	}

	private async Task TicketInstance_OnSaveResponse(object arg1, SaveResponseEventArgs e)
	{
	}

	private async Task SyncInstance_OnSyncReset(object sender, SyncResetEventArgs e)
	{
	}

	private async Task TicketInstance_OnSync(object sender, ConnectionEventArgs<WebTicket> e)
	{
	}

	private async Task TicketInstance_OnReadResponse(object sender, ConnectionEventArgs<WebTicket> e)
	{
	}
}
