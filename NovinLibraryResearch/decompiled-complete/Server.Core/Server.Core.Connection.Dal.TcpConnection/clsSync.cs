using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core.Connection.Dal.TcpConnection;

public class clsSync : IDalSync
{
	private class NestedSync
	{
		internal static readonly clsSync Instance;

		static NestedSync()
		{
			Instance = new clsSync();
		}
	}

	public static clsSync Instance => NestedSync.Instance;

	public Task<int> GetHashIdAsync(byte[] hashData, CancellationToken token)
	{
		return Task.FromResult(0);
	}

	public Task<DateTime> GetLastSyncTime(long hashId, EntityPart part, int partitionId, CancellationToken token)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> InsertFisrtSyncAsync(long HashId, short depId, int userId, short userAcc, long lkSerial, long androidIme, Guid dbGuid, CancellationToken token)
	{
		return Task.FromResult(new ReturnedSaveFuncInfo());
	}

	public Task<int> InsertHashAsync(byte[] hashData, CancellationToken token)
	{
		return Task.FromResult(0);
	}

	public Task<ReturnedSaveFuncInfoWithValue<Sync>> LoadDataAsync(long HashId, CancellationToken token)
	{
		return Task.FromResult(new ReturnedSaveFuncInfoWithValue<Sync>
		{
			value = GetDefaultData()
		});
	}

	public Task<ReturnedSaveFuncInfoWithValue<Sync>> LoadDataAsync(Guid HashGuid, CancellationToken token)
	{
		return Task.FromResult(new ReturnedSaveFuncInfoWithValue<Sync>
		{
			value = GetDefaultData()
		});
	}

	public Task<ReturnedSaveFuncInfo> ResetSyncsAsync(short DepId, List<UserAccessChanges> result, CancellationToken token)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveSyncedAsync(int PartitionId, EntityPart SyncPart, DateTime Modified, IClsConnectionData connection)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSyncDoneAsync(short depId, long hashId, EntityPart part, CancellationToken token)
	{
		throw new NotImplementedException();
	}

	public Task UpdateLastSyncAsync(int hashId, DateTime minSync, CancellationToken token)
	{
		throw new NotImplementedException();
	}

	private Sync GetDefaultData()
	{
		return new Sync();
	}
}
