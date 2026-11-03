using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel;

public interface ITicketPartSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebTicketPart>>> GetAllAsync(bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebTicketPart>> GetAsync(Guid guid, CancellationToken token, string tranName);
}
