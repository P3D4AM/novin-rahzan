using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel;

public interface IContextHolderSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfo> RollBackTransactionAsync(EnCacheConfig config, string tranName, bool disposeContext);

	Task<ReturnedSaveFuncInfo> BeginTransactionAsync(EnCacheConfig config, string tranName, int timeout);

	Task<ReturnedSaveFuncInfo> CommitTransactionAsync(EnCacheConfig config, string tranName, bool disposedContext);

	Task<ReturnedSaveFuncInfoWithValue<bool>> ExistsAsync(EnCacheConfig config, string tranName);
}
