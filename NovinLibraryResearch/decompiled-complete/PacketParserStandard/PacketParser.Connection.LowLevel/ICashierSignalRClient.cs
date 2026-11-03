using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel;

public interface ICashierSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebCashier>>> GetAllAsync(bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebCashier>> GetAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebCashier>> GetByUsernameAsync(string username, CancellationToken token, string tranName);
}
