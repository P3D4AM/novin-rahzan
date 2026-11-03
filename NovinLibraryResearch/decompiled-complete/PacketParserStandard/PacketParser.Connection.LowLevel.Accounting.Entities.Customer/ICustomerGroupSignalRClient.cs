using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.Customer;

public interface ICustomerGroupSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebCusGroup>>> GetAllAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebCusGroup>> GetAsync(Guid guid, CancellationToken token, string tranName);
}
