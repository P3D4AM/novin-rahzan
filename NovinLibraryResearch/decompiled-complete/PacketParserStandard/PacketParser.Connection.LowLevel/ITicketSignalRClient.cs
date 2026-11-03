using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace PacketParser.Connection.LowLevel;

public interface ITicketSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebTicket item, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebTicketReport>>> GetAllAsync(TicketFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebTicket>> GetAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeStatusAsync(Guid guid, short statusId, CancellationToken token, string tranName);
}
