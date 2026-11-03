using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Filters;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel;

public interface IGardeshTafsilSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebGardeshTafsili>>> GetAllAsync(GardeshTafsiliFilter filter, EnSystem system, string tranName, CancellationToken token);
}
