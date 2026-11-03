using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.Customer;

public interface ICustomerTitelSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebCusTitle>>> GetAllAsync(bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebCusTitle>> GetAsync(Guid guid, CancellationToken token, string tranName);
}
