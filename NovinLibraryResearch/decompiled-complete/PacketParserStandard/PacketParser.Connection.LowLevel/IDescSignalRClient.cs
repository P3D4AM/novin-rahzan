using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel;

public interface IDescSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebDesc>>> GetAllAsync(bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebDesc>>> GetAllAsync(DescSection section, bool? status, CancellationToken token, string tranName);
}
