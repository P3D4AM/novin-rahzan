using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Crm;

namespace PacketParser.Connection.LowLevel;

public interface ICallingLogDetailSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebCallingLogDetail>>> GetAllAsync(CallingLogDetailFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebCallingLogDetail>> GetAsync(string uuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebCallingLogDetail callDet, short tryCount, CancellationToken token, string tranName);
}
