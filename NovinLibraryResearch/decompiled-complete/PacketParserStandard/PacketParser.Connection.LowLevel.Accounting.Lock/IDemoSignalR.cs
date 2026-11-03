using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Lock;

public interface IDemoSignalR : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<string>> CheckLockAsync(string version, Package package, bool ignoreHttpExceptions, CancellationToken token);
}
