using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Crm;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.Crm;

public interface ICallingLogSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebCallingLogReport>>> GetAllAsync(CallingLogFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebCallingLog>> GetAsync(string uuid, CancellationToken token, string tranName);
}
