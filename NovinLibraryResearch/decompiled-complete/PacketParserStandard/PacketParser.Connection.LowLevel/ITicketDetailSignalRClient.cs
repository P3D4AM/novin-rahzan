using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace PacketParser.Connection.LowLevel;

public interface ITicketDetailSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebTicketDet item, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebTicketDet>>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebTicketReport>>> GetAllAsync(TicketDetFilter filter, CancellationToken token, string tranName);
}
