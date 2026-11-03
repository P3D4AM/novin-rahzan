using System;
using System.Threading.Tasks;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository.Tickets;

public class TicketDetWebServiceRepository : WebServiceRepository<TicketDetBusiness>, IDisposable
{
	public TicketDetWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<TicketDetBusiness>)cache.CacheRepository.TicketDet, cache)
	{
	}

	private async Task WebTicketDet_OnDeliverdRequest(object arg1, ConnectionEventArgs<Guid> arg2)
	{
	}

	public override void Dispose()
	{
	}

	private async Task SyncInstance_OnSyncReset(object sender, SyncResetEventArgs e)
	{
		using (new JaegerMonitor("SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketDetWebServiceRepository.cs"))
		{
		}
	}

	private async Task TicketDetInstance_OnSync(object sender, ConnectionEventArgs<WebTicketDet> e)
	{
	}

	private async Task TicketDetInstance_OnReadResponse(object sender, ConnectionEventArgs<WebTicketDet> e)
	{
	}

	private async Task TicketDetInstance_OnSaveResponse(object arg1, SaveResponseEventArgs e)
	{
	}
}
