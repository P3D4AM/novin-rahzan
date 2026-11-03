using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Entities.Interfaces;
using PacketParser.LkService;

namespace PacketParser.Connection.LowLevel.Accounting.Lock;

public interface ILkEnvironmentSignalR : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<LKInfo>> GetLkInfoAsync(CancellationToken token);

	Task<ReturnedSaveFuncInfo> SetLkInfoAsync(LKInfo lk, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<long>> GetLkSerialAsync(CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<LockInformation>> GetLockInformationAsync(Package package, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SetLockInformationAsync(LockInformation lk, Package package, CancellationToken token);

	Task<ReturnedSaveFuncInfo> CleanActivationCodesAsync(Package package, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SetLkSerialAsync(long value, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SetSerialNumberAsync(long value, Package package, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<bool>> ShakingHandAsync(CancellationToken token);
}
