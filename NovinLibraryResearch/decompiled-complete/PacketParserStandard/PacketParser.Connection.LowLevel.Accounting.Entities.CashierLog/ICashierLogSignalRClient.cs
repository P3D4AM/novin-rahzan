using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.CashierLog;

public interface ICashierLogSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<WebCashierLog>> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebCashierLog item, short tryCount, CancellationToken token, string tranName);
}
