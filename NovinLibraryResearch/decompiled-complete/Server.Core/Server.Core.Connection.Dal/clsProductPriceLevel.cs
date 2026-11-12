using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class clsProductPriceLevel : IEntity<WebProductPriceLevel>
{
	private class NestedProductPriceLevel
	{
		internal static readonly clsProductPriceLevel Instance;

		static NestedProductPriceLevel()
		{
			Instance = new clsProductPriceLevel();
		}
	}

	public static clsProductPriceLevel Instance => NestedProductPriceLevel.Instance;

	public Task<List<WebProductPriceLevel>> ApplyAccessAsync(IEnumerable<WebProductPriceLevel> lst, IClsConnectionData connection)
	{
		try
		{
			if (!connection.IsAccountingFamily)
			{
				return Task.FromResult<List<WebProductPriceLevel>>(null);
			}
			if (connection.Source != ENSource.Accounting)
			{
				lst?.Count();
				lst = lst?.Where((WebProductPriceLevel p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
			}
			return Task.FromResult(lst?.ToList());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 39);
			throw;
		}
	}

	internal async Task OnSaveRequestAsync(object sender, ConnectionEventArgs<WebProductPriceLevel> e)
	{
		using (new JaegerMonitor("OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs"))
		{
			_ = 2;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.ProductPriceLevel, 1, isReceivedPacket: true);
				ConnectionEventArgs<WebProductPriceLevel> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				List<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.ProductPriceLevel.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					foreach (SaveResponse item in ret)
					{
						item.PartitionId = e.PartitionId;
					}
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex2)
				{
					ret = ret.Append(new SaveResponse
					{
						SaveResultStatus = false
					})?.ToList();
					result = false;
					WebErrorLog.Instance.StartLog(ex2, "", "OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 63);
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection != null)
					{
						await connection.SendPacketAsync(WebProductPriceLevel.GenerateSaveResponse(ret), Priority.High);
					}
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				IEnumerable<WebProductPriceLevel> truSavedInRet = e.ConnectionData.Where((WebProductPriceLevel p) => trueSaved.Contains(p.Guid));
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				ConnectionEventArgs<WebProductPriceLevel> connectionEventArgs2 = e;
				if ((connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting) && (truSavedInRet?.Any() ?? false))
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion >= 22)
					{
						Task.Run(() => accconnection.SendPacketAsync(WebProductPriceLevel.GenerateSaveRequest(truSavedInRet, e.PartitionId), Priority.Medium));
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.IsAccountingFamily && p.Source != ENSource.Accounting && p.ClientVersion >= 22);
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (ClsConnectionData Connection in enumerable)
				{
					List<WebProductPriceLevel> dataForSend = await ApplyAccessAsync(truSavedInRet, Connection);
					if (dataForSend?.Any() ?? false)
					{
						Task.Run(() => Connection.SendPacketAsync(WebProductPriceLevel.GenerateReadResponse(dataForSend, isSync: false, e.PartitionId), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 99);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SyncToClientAsync(ClsConnectionData connection, short maxParalel = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (connection.ClientVersion < 22 || connection.Source == ENSource.WebsiteWordPress)
		{
			return ret;
		}
		try
		{
			if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 107);
			}
			if (await connection.GetSyncInfoAsync() == null)
			{
				await ret.AddReturnedValueAsync(ReturnedState.Error, "connection?.SyncInfo == null", "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 111);
				return ret;
			}
			List<short> totalPartitions = new List<short>();
			List<short> list = await Server.Core.Assistance.Initializer.Dal.ProductPriceLevel.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
			if (list != null && list.Any())
			{
				totalPartitions.AddRange(list);
				while (list != null && list.Any())
				{
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						await ret.AddReturnedValueAsync(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 124);
						return ret;
					}
					await SyncPartitionsAsync(list, connection, maxParalel);
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						await ret.AddReturnedValueAsync(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 130);
						return ret;
					}
					if (connection == null || connection.IsDisposed)
					{
						return ret;
					}
					list = (await Server.Core.Assistance.Initializer.Dal.ProductPriceLevel.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => !totalPartitions.Contains(x))?.ToList();
				}
			}
		}
		catch (OperationCanceledException exception)
		{
			await ret.AddReturnedValueAsync(exception, "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 139);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 142);
			await ret.AddReturnedValueAsync(ex, "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 143);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
		}
		return ret;
	}

	private async Task<bool> SyncPartitionsAsync(List<short> partitions, ClsConnectionData connection, short maxParalel)
	{
		_ = 1;
		try
		{
			new CancellationTokenSource();
			if (partitions != null)
			{
				if (partitions == null || partitions.Count > 0)
				{
					List<Task> tasks = new List<Task>();
					foreach (short partition in partitions)
					{
						tasks.Add(SyncPartitionAsync(partition, connection));
						if (tasks.Count >= maxParalel)
						{
							List<Task> list = tasks;
							list.Remove(await Task.WhenAny(tasks));
						}
					}
					await Task.WhenAll(tasks);
					return true;
				}
			}
			return true;
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 168);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection.ClientVersion < 22)
			{
				throw new Exception("version" + connection.Version + " is less than 22!");
			}
			DateTime syncing = DateTime.Now;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 178);
			}
			List<WebProductPriceLevel> lst = await Server.Core.Assistance.Initializer.Dal.ProductPriceLevel.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
			using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.ProductPriceLevel, 300000, connection.cancellationToken);
			if (lst?.Any() ?? false)
			{
				Task.Run(() => connection.SendPacketAsync(WebProductPriceLevel.GenerateReadResponse(lst, isSync: true, partition), Priority.Low));
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					await ret.AddReturnedValueAsync(ReturnedState.Error, "waiter time out", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 187);
					return ret;
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 190);
				}
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				await returnedSaveFuncInfo.AddReturnedValueAsync(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.ProductPriceLevel, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 192);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 195);
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user"))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 199);
			await ret.AddReturnedValueAsync(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 200);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection.cancellationToken, connection));
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection.Source != ENSource.Accounting || connection.ClientVersion < 22)
				{
					return ret;
				}
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.ProductPriceLevel, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 213);
				}
				if (list != null && list.Any())
				{
					foreach (Guid guid in list)
					{
						await ret.AddInformationAsync($"sending tafsilGroup({guid}) to accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 218);
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 219);
						}
						WebProductPriceLevel webProductPriceLevel = await Server.Core.Assistance.Initializer.Dal.ProductPriceLevel.GetAsync(guid, connection.DepId);
						if (webProductPriceLevel != null)
						{
							await connection.SendPacketAsync(WebProductPriceLevel.GenerateSaveRequest(new List<WebProductPriceLevel> { webProductPriceLevel }, 0), Priority.Medium);
						}
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				await ret.AddReturnedValueAsync(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 228);
				throw;
			}
			catch (Exception ex)
			{
				await ret.AddReturnedValueAsync(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 233);
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsProductPriceLevel.cs", 234);
			}
			return ret;
		}
	}
}
