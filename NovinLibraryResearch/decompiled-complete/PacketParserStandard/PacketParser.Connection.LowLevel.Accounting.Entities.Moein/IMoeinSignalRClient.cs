using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.Moein;

public interface IMoeinSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<WebMoein>> GetAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebMoein moein, bool updateAccount, short tryCount, CancellationToken token, string tranName);
}
