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

public class clsSanadStatusReport : IEntity<WebSanadStatusReport>
{
	private class NestedSanadStatus
	{
		internal static readonly clsSanadStatusReport Instance;

		static NestedSanadStatus()
		{
			Instance = new clsSanadStatusReport();
		}
	}

	public static clsSanadStatusReport Instance => NestedSanadStatus.Instance;

	public Task<List<WebSanadStatusReport>> ApplyAccessAsync(IEnumerable<WebSanadStatusReport> lst, IClsConnectionData connection)
	{
		try
		{
			if (!connection.IsAccountingFamily)
			{
				return Task.FromResult<List<WebSanadStatusReport>>(null);
			}
			if (connection.Source != ENSource.Accounting)
			{
				lst?.Count();
				lst = lst?.Where((WebSanadStatusReport p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
			}
			return Task.FromResult(lst?.ToList());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 39);
			throw;
		}
	}

	internal async Task OnSaveRequestAsync(object sender, ConnectionEventArgs<WebSanadStatusReport> e)
	{
		using (new JaegerMonitor("OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs"))
		{
			_ = 2;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.SanadStatusReport, 1, isReceivedPacket: true);
				ConnectionEventArgs<WebSanadStatusReport> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				List<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.SanadStatusReport.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
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
					WebErrorLog.Instance.StartLog(ex2, "", "OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 63);
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection != null)
					{
						await connection.SendPacketAsync(WebSanadStatusReport.GenerateSaveResponse(ret), Priority.High);
					}
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				IEnumerable<WebSanadStatusReport> truSavedInRet = e.ConnectionData.Where((WebSanadStatusReport p) => trueSaved.Contains(p.Guid));
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				ConnectionEventArgs<WebSanadStatusReport> connectionEventArgs2 = e;
				if ((connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting) && (truSavedInRet?.Any() ?? false))
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion >= 23)
					{
						Task.Run(() => accconnection.SendPacketAsync(WebSanadStatusReport.GenerateSaveRequest(truSavedInRet, e.PartitionId), Priority.Medium));
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.IsAccountingFamily && p.Source != ENSource.Accounting && p.ClientVersion >= 23);
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (ClsConnectionData Connection in enumerable)
				{
					List<WebSanadStatusReport> dataForSend = await ApplyAccessAsync(truSavedInRet, Connection);
					if (dataForSend?.Any() ?? false)
					{
						Task.Run(() => Connection.SendPacketAsync(WebSanadStatusReport.GenerateReadResponse(dataForSend, isSync: false, e.PartitionId), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 99);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SyncToClientAsync(ClsConnectionData connection, short maxParalel = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (connection.ClientVersion < 23 || connection.Source == ENSource.WebsiteWordPress)
		{
			return ret;
		}
		try
		{
			if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 107);
			}
			if (await connection.GetSyncInfoAsync() == null)
			{
				await ret.AddReturnedValueAsync(ReturnedState.Error, "connection?.SyncInfo == null", "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 111);
				return ret;
			}
			List<short> totalPartitions = new List<short>();
			List<short> list = await Server.Core.Assistance.Initializer.Dal.SanadStatusReport.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
			if (list != null && list.Any())
			{
				totalPartitions.AddRange(list);
				while (list != null && list.Any())
				{
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						await ret.AddReturnedValueAsync(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 124);
						return ret;
					}
					await SyncPartitionsAsync(list, connection, maxParalel);
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						await ret.AddReturnedValueAsync(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 130);
						return ret;
					}
					if (connection == null || connection.IsDisposed)
					{
						return ret;
					}
					list = (await Server.Core.Assistance.Initializer.Dal.SanadStatusReport.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => !totalPartitions.Contains(x))?.ToList();
				}
			}
		}
		catch (OperationCanceledException exception)
		{
			await ret.AddReturnedValueAsync(exception, "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 139);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 142);
			await ret.AddReturnedValueAsync(ex, "", "SyncToClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 143);
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
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 168);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection.ClientVersion < 23)
			{
				throw new Exception("version" + connection.Version + " is less than 23!");
			}
			DateTime syncing = DateTime.Now;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 178);
			}
			List<WebSanadStatusReport> lst = await Server.Core.Assistance.Initializer.Dal.SanadStatusReport.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
			using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.SanadStatusReport, 300000, connection.cancellationToken);
			if (lst?.Any() ?? false)
			{
				Task.Run(() => connection.SendPacketAsync(WebSanadStatusReport.GenerateReadResponse(lst, isSync: true, partition), Priority.Low));
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					await ret.AddReturnedValueAsync(ReturnedState.Error, "waiter time out", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 187);
					return ret;
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 190);
				}
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				await returnedSaveFuncInfo.AddReturnedValueAsync(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.SanadStatusReport, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 192);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 195);
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user"))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 199);
			await ret.AddReturnedValueAsync(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 200);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection.cancellationToken, connection));
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection.Source != ENSource.Accounting || connection.ClientVersion < 23)
				{
					return ret;
				}
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.SanadStatusReport, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 213);
				}
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 218);
						}
						WebSanadStatusReport webSanadStatusReport = await Server.Core.Assistance.Initializer.Dal.SanadStatusReport.GetAsync(item, connection.DepId);
						if (webSanadStatusReport != null)
						{
							await connection.SendPacketAsync(WebSanadStatusReport.GenerateSaveRequest(new List<WebSanadStatusReport> { webSanadStatusReport }, 0), Priority.Medium);
						}
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				await ret.AddReturnedValueAsync(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 227);
				throw;
			}
			catch (Exception ex)
			{
				await ret.AddReturnedValueAsync(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 232);
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsSanadStatusReport.cs", 233);
			}
			return ret;
		}
	}
}
