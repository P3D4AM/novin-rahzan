using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Customer;

namespace PacketParser.Connection.LowLevel;

public interface ICustomerCompactSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<WebCusCompact>> GetAsync(Guid guid, List<Guid> lstMoeinGuids, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebCusCompact>>> GetAllAsync(List<Guid> lstMoeinGuids, bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebCusCompact>>> GetAllAsync(CustomerCompactFilter filter, CancellationToken token, string tranName);
}
