using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.Order;

public interface IOrderSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<WebOrderReport>> GetLastOrderByTafsilGuidAsync(Guid tafsilGuid, Document orderType, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebOrderReport>>> GetAllAsync(OrderFilter filter, CancellationToken token, string tranName);
}
