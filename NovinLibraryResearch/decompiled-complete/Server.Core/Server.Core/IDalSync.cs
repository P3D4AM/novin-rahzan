using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Server.Core.Connection;

namespace Server.Core;

public interface IDalSync
{
	Task<ReturnedSaveFuncInfoWithValue<Sync>> LoadDataAsync(long HashId, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<Sync>> LoadDataAsync(Guid HashGuid, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SaveSyncedAsync(int PartitionId, EntityPart SyncPart, DateTime Modified, IClsConnectionData connection);

	Task<ReturnedSaveFuncInfo> ResetSyncsAsync(short DepId, List<UserAccessChanges> result, CancellationToken token);

	Task<int> GetHashIdAsync(byte[] hashData, CancellationToken token);

	Task<int> InsertHashAsync(byte[] hashData, CancellationToken token);

	Task<ReturnedSaveFuncInfo> InsertFisrtSyncAsync(long HashId, short depId, int userId, short userAcc, long lkSerial, long androidIme, Guid dbGuid, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SetSyncDoneAsync(short depId, long hashId, EntityPart part, CancellationToken token);

	Task UpdateLastSyncAsync(int hashId, DateTime minSync, CancellationToken token);

	Task<DateTime> GetLastSyncTime(long hashId, EntityPart part, int partitionId, CancellationToken token);
}
